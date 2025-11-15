using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using HtmlAgilityPack; // Requiere instalación vía NuGet
using CapaEntidad;
using CapaDatos;       // Necesario para usar CD_TasaCambio

namespace CapaNegocio
{
    // Clase estática para manejar la lógica de obtención, caché y persistencia de datos del BCV
    public static class BcvScraper
    {
        private const string URL_BCV = "https://www.bcv.org.ve/";

        // Intervalo de actualización: 1 hora
        private static readonly TimeSpan IntervaloActualizacion = TimeSpan.FromHours(1);

        // Instancia de la Capa de Datos para interactuar con SQL Server
        private static CD_TasaCambio _dataManager = new CD_TasaCambio();

        /// <summary>
        /// Obtiene las tasas de cambio. Si la última tasa en BD tiene más de 1 hora, realiza Web Scraping y registra los nuevos valores.
        /// </summary>
        /// <returns>Diccionario con las últimas TasaCambio (obtenidas de la DB o del web scraping), clave por abreviación de moneda.</returns>
        public static async Task<Dictionary<string, TasaCambio>> ObtenerTasasAsync()
        {
            // 1. OBTENER LAS ÚLTIMAS TASAS DE LA BASE DE DATOS
            Dictionary<string, TasaCambio> tasasActualesDB = _dataManager.ObtenerUltimasTasas();

            // Si hay datos en la DB y el dato más reciente es menor a 1 hora, devuelve los datos de la DB
            if (tasasActualesDB.Any())
            {
                // Busca la fecha más reciente de las tasas encontradas
                DateTime fechaMaximaDB = tasasActualesDB.Values.Max(t => t.FechaRegistro);

                if (DateTime.Now - fechaMaximaDB < IntervaloActualizacion)
                {
                    // Devolver datos de la BD (son recientes)
                    return tasasActualesDB;
                }
            }

            // 2. SI NO SON RECIENTES O NO HAY DATOS, HACER WEB SCRAPING
            try
            {
                var web = new HtmlWeb();

                // Cargar el HTML de forma asíncrona
                HtmlDocument oDoc = await Task.Run(() => web.Load(URL_BCV));

                // CORRECCIÓN 1: Usar SelectSingleNode con XPath en lugar de CssSelect (requiere paquete adicional)
                // Buscamos el nodo 'body' para obtener todo el texto.
                HtmlNode bodyNode = oDoc.DocumentNode.SelectSingleNode("//body");

                if (bodyNode == null)
                {
                    // Manejo de error si el cuerpo HTML no se encuentra
                    throw new Exception("No se pudo cargar el cuerpo del documento HTML.");
                }

                string bodyText = bodyNode.InnerText;

                // Definición de Regex
                string patronMonedas = @"(EUR|CNY|TRY|RUB|USD)\s+(\d{1,3}(?:,\d{3})*,\d+)";
                string patronFecha = @"Fecha Valor:\s*(.+)";

                Match matchFecha = Regex.Match(bodyText, patronFecha);
                string fechaValor = matchFecha.Success ? matchFecha.Groups[1].Value.Trim() : "Fecha no disponible.";

                MatchCollection matchesMonedas = Regex.Matches(bodyText, patronMonedas);
                var nuevasTasasWeb = new Dictionary<string, TasaCambio>();
                DateTime fechaRegistro = DateTime.Now;

                foreach (Match match in matchesMonedas)
                {
                    string abreviacion = match.Groups[1].Value;
                    string valorString = match.Groups[2].Value;

                    // Sustituir la coma (,) por punto (.) para que el TryParse funcione correctamente
                    if (decimal.TryParse(valorString.Replace(",", "."), System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, out decimal valorDecimal))
                    {
                        var nuevaTasa = new TasaCambio
                        {
                            MonedaAbreviacion = abreviacion,
                            NombreCompleto = ObtenerNombreCompleto(abreviacion),
                            Valor = valorDecimal,
                            FechaValor = fechaValor,
                            FechaRegistro = fechaRegistro
                        };
                        nuevasTasasWeb[abreviacion] = nuevaTasa;

                        // 3. REGISTRAR EL NUEVO DATO EN LA BASE DE DATOS (TASA_CAMBIO)
                        _dataManager.RegistrarTasa(nuevaTasa);
                    }
                }

                // 4. Devolver las tasas recién extraídas (y guardadas en DB)
                return nuevasTasasWeb;
            }
            catch (Exception ex)
            {
                // Si falla el scraping, devolvemos las tasas viejas de la DB como fallback.
                // En una aplicación real, se debe loggear este error.
                Console.WriteLine($"Error al hacer Web Scraping, usando datos antiguos de la DB: {ex.Message}");
                return tasasActualesDB;
            }
        }

        // CORRECCIÓN 2: Usar la estructura tradicional switch-case (compatible con C# 7.3)
        // Método de ayuda para obtener el nombre completo
        private static string ObtenerNombreCompleto(string abreviacion)
        {
            switch (abreviacion)
            {
                case "EUR":
                    return "Euro";
                case "CNY":
                    return "Yuan Chino";
                case "TRY":
                    return "Lira Turca";
                case "RUB":
                    return "Rublo Ruso";
                case "USD":
                    return "Dólar Estadounidense";
                default:
                    return abreviacion;
            }
        }
    }
}