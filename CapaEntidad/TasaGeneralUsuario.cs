using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaEntidad
{
    public class TasaGeneralUsuario
    {
        // La abreviación de la moneda preferida (ej: "USD" o "EUR")
        public string MonedaAbreviacion { get; set; }

        // El valor específico de la tasa (ej: 38.50)
        public decimal Valor { get; set; }
    }
}
