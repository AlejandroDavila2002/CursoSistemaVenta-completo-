using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaEntidad
{
    public class CuentaPorCobrar
    {
        public int IdCuentaPorCobrar { get; set; }
        public Venta oVenta { get; set; }
        public Cliente oCliente { get; set; }
        public decimal MontoTotal { get; set; }
        public decimal MontoPagado { get; set; }
        public decimal SaldoPendiente { get; set; }
        public bool Estado { get; set; }
        public string FechaRegistro { get; set; }

        public string FechaVencimiento { get; set; }
        public string DescripcionPlan { get; set; }
        public decimal PorcentajeMora { get; set; }
    }
}