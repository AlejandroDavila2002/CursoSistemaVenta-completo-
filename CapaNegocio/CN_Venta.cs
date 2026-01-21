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
            // VALIDACIÓN: Evitar restar stock negativo o cero
            if (cantidad <= 0) return false;
            return objcd_Venta.RestarStock(idProducto, cantidad);
        }

        public bool sumarStock(int IdProducto, int Cantidad)
        {
            // VALIDACIÓN: Evitar sumar stock negativo o cero
            if (Cantidad <= 0) return false;
            return objcd_Venta.sumarStock(IdProducto, Cantidad);
        }


        public int ObtenerCorrelativo()
        {
            return objcd_Venta.ObtenerCorrelativo();
        }

        public bool RegistrarVenta(Venta obj, DataTable DetalleVenta, CuentaPorCobrar oCuenta, DataTable dtCuotas, out string Mensaje)
        {
            return objcd_Venta.RegistrarVenta(obj, DetalleVenta, oCuenta, out Mensaje);
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

        // En CN_Venta.cs
        public int ObtenerIdVenta(string numeroDocumento)
        {
            return objcd_Venta.ObtenerIdVenta(numeroDocumento);
        }



        public List<Venta> ListarVentasContado()
        {
            return objcd_Venta.ListarVentasContado();
        }


        // Obtener Detalle Venta de CD_Ventas
        public List<Detalle_Venta> ObtenerDetalleVenta(int idVenta)
        {
            return objcd_Venta.ObtenerDetalleVenta(idVenta);
        }




    }


}
