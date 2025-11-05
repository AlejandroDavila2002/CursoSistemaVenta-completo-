using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaEntidad
{
    public class TasaCambio
    {
        public int IdTasaCambio { get; set; }        // Clave primaria
        public string MonedaAbreviacion { get; set; } // EUR, USD, CNY, etc.
        public string NombreCompleto { get; set; }     // Euro, Dólar Estadounidense, etc.
        public decimal Valor { get; set; }             // El valor numérico de la tasa
        public string FechaValor { get; set; }         // La fecha en que fue publicada la tasa (Ej: "03 de Noviembre de 2025")
        public DateTime FechaRegistro { get; set; }    // Fecha y hora en que se extrajo/registró el dato

        // Propiedad de solo lectura para mostrar en el ComboBox de la interfaz
        public string Display => $"{NombreCompleto} ({MonedaAbreviacion})";
    }
}
