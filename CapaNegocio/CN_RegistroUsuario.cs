using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks; // <-- AÑADIDO
using CapaEntidad;
using CapaDatos;

namespace CapaNegocio
{
    public class CN_RegistroUsuario
    {
        
        private CD_TasaCambio oCD_TasaCambio = new CD_TasaCambio();

        
        public async Task<Dictionary<string, TasaCambio>> ObtenerTasasActualizadasAsync()
        {
            // Esta es la llamada correcta que invoca al scraper
            return await BcvScraper.ObtenerTasasAsync();
        }

        /// <summary>
        /// Obtiene un diccionario de las últimas tasas de cambio registradas en la BD (SIN SCRAPING).
        /// </summary>
        /// <returns>Diccionario con la MonedaAbreviacion como clave y el objeto TasaCambio como valor.</returns>
        public Dictionary<string, TasaCambio> ObtenerTasasDisponibles()
        {
            return oCD_TasaCambio.ObtenerUltimasTasas();
        }

        // ==============================================================================
        // MÉTODOS DE REGISTRO
        // ==============================================================================

        /// <summary>
        /// Guarda un registro individual de operación de tasa de cambio asociada a un usuario.
        /// </summary>
        public bool GuardarRegistroIndividual(Usuario oUsuario, TasaCambio tasa, decimal montoOperacion)
        {
            // Simple validación de negocio antes de llamar a la Capa de Datos
            if (oUsuario == null || oUsuario.IdUsuario <= 0)
            {
                throw new ArgumentException("El usuario de la sesión es inválido.");
            }

            if (montoOperacion <= 0)
            {
                throw new ArgumentException("El monto de la operación debe ser un valor positivo.");
            }

            return oCD_TasaCambio.RegistrarRegistroUsuario(oUsuario, tasa, montoOperacion);
        }

        // ==============================================================================
        // MÉTODOS DE HISTORIAL Y TASA GENERAL
        // ==============================================================================

        /// <summary>
        /// Obtiene el historial de todas las operaciones de tasas registradas 
        /// previamente por un usuario.
        /// </summary>
        public List<object[]> ObtenerHistorialOperacionesPorUsuario(int idUsuario)
        {
            if (idUsuario <= 0)
            {
                return new List<object[]>();
            }

            return oCD_TasaCambio.ObtenerHistorialUsuario(idUsuario);
        }

        /// <summary>
                /// Establece una tasa de cambio específica (Moneda y Monto) como 
                /// la 'Tasa General' o predeterminada del sistema para este usuario.
                /// </summary>

        public bool EstablecerTasaGeneralUsuario(int idUsuario, TasaCambio tasa, decimal montoPersonalizado)
        {
            // 1. Validaciones de Negocio
            if (idUsuario <= 0) return false;

            if (tasa == null || string.IsNullOrEmpty(tasa.MonedaAbreviacion))
                throw new ArgumentException("La moneda seleccionada no es válida.");

            if (montoPersonalizado <= 0)
                throw new ArgumentException("El monto de la tasa general debe ser mayor a cero.");

            // 2. Llamada a la Capa de Datos
            CD_TasaCambio objData = new CD_TasaCambio();
            bool resultado = objData.GuardarTasaGeneralUsuario(idUsuario, tasa.MonedaAbreviacion, montoPersonalizado);

            // 3. Lógica de Sincronización Global
            if (resultado)
            {
                // Actualizamos el objeto de sesión si es necesario (se profundizará en el Punto 4)
                // Por ahora, devolvemos el éxito del registro en DB
                return true;
            }

            return false;
        }

        
        
        public TasaGeneralUsuario ObtenerTasaGeneral(int idUsuario)
        {
            if (idUsuario <= 0)
            {
                return null;
            }
            return oCD_TasaCambio.ObtenerTasaGeneralUsuario(idUsuario);
        }
    }
}