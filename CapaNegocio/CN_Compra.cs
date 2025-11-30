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
    public class CN_Compra
    {
        private CD_Compra objcd_Compra = new CD_Compra();

        public int ObtenerCorrelativo()
        {

            return objcd_Compra.ObtenerCorrelativo();   
        }
         
        public bool RegistrarComprar(Compra obj, DataTable DetalleCompra, out string Mensaje)
        {
           return objcd_Compra.RegistrarCompra(obj, DetalleCompra, out Mensaje);  
        }

       public Compra ObtenerCompra(string NumeroDocumento)
        {
            Compra oCompra = objcd_Compra.ObtenerCompra(NumeroDocumento);

            if (oCompra != null && oCompra.IdCompra != 0)
            {
                List<Detalle_Compra> oDetalle_Compras = objcd_Compra.ObtenerDetalleCompra(oCompra.IdCompra);

                oCompra.oDetalleCommpra = oDetalle_Compras;
            }
            
                return oCompra;

       }


    }
}



