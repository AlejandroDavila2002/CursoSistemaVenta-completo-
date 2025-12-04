using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CapaDatos;
using CapaEntidad;

namespace CapaNegocio
{
    public class CN_Reporte
    {
        private CD_Reportes objReporte = new CD_Reportes();

        public List<ReporteCompras> compras(string FechaInicio, string fechaFin, int IdProveedor)
        {
            return objReporte.compras(FechaInicio, fechaFin, IdProveedor);
        }

        public List<ReporteVenta> Ventas(string FechaInicio, string FechaFin)
        {
            return objReporte.Venta(FechaInicio, FechaFin);
        }
    }
}
