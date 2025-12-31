using CapaDatos;
using CapaEntidad;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaNegocio
{
    public class CN_Venta
    {
        private CD_Ventas objcd_Venta = new CapaDatos.CD_Ventas();

        public bool RestarStock(int idProducto, int cantidad)
        {
             return objcd_Venta.RestarStock(idProducto, cantidad);
        }

        public bool sumarStock(int IdProducto, int Cantidad)
        {
            return objcd_Venta.sumarStock(IdProducto, Cantidad);
        }


        public int ObtenerCorrelativo()
        {
            return objcd_Venta.ObtenerCorrelativo();
        }

        public bool RegistrarVenta(Venta obj, DataTable DetalleVenta, out string Mensaje)
        {
            return objcd_Venta.RegistrarVenta(obj, DetalleVenta, out Mensaje);
        }


        public Venta ObtenerVenta(string NumeroDocumento)
        {
            Venta oVenta = objcd_Venta.ObtenerVenta(NumeroDocumento);

            if (oVenta.IdVenta != 0)
            {
                List<Detalle_Venta> oListaDetalleVenta = objcd_Venta.ObtenerDetalleVenta(oVenta.IdVenta);
                oVenta.oDetalleVenta = oListaDetalleVenta;
            }

            return oVenta;
        }

        public List<Venta> ListarVentasResumen()
        {
            return objcd_Venta.ListarVentasResumen();
        }


    }
}
