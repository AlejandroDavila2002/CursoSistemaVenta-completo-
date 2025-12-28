using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaEntidad
{
    public class TasaGeneralUsuario
    {
        public int idUsuario { get; set; } // Clave primaria del usuario
        // La abreviación de la moneda preferida (ej: "USD" o "EUR")
        public string MonedaAbreviacion { get; set; }

        // El valor específico de la tasa (ej: 38.50)
        public decimal Valor { get; set; }

        // Propiedad calculada para mostrar en etiquetas de la interfaz (ej: "Tasa Actual: 1 USD = 36.50")
        public string DisplayTasa => $"1 {MonedaAbreviacion} = {Valor:N2}";
    }
}
