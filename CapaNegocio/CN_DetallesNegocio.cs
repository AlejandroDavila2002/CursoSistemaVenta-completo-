using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using CapaDatos;
using CapaEntidad;

namespace CapaNegocio
{
    public class CN_DetallesNegocio
    {
        private CD_DetallesNegocio objDetallesNegocio = new CD_DetallesNegocio();


        public Detalles_Negocio ObtenerDetallesNegocio()
        {
            return objDetallesNegocio.ObtenerDetallesNegocio();
        }

        public bool GuardarDatos(Detalles_Negocio obj, out string Mensaje)
        {
            Mensaje = string.Empty;

            if(obj.Nombre == "")
            {
                Mensaje = "El nombre del negocio no puede estar vacío.";
                return false;
            }

            if(obj.RUC == "")
            {
                Mensaje = "El RUC del negocio no puede estar vacío.";
                return false;
            }

            if (obj.Direccion == "")
            {
                Mensaje = "La dirección del negocio no puede estar vacía.";
                return false;
            }

            if(Mensaje != string.Empty)
            {
                return false;
            }
            else
            {
                return objDetallesNegocio.GuardarDatos(obj);
            }
        }

        public byte[] obtenerLogo(out bool obtenido)
        {
            return objDetallesNegocio.ObtenerLogo(out obtenido);
        }

        public bool ActualizarLogo(byte[] imagen, out string Mensaje)
        {
            return objDetallesNegocio.ActualizarLogo(imagen, out Mensaje);
        }

        public Detalles_Negocio ObtenerDatos()
        {
            return objDetallesNegocio.ObtenerDetallesNegocio();
        }
    }
}
 