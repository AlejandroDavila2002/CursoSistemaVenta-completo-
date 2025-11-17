using CapaDatos;
using CapaEntidad;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaNegocio
{
    public class CN_Proveedor
    {
        private CD_Proveedor objcd_Proveedor = new CD_Proveedor();

        public List<Proveedor> listar()
        {

            return objcd_Proveedor.listar();
        }


        public int Registrar(Proveedor obj, out string Mensaje)
        {
            Mensaje = string.Empty;

            if (obj.Documento == "")
            {
                Mensaje += "Es necesario el numero de documento\n";
            }

            if (obj.RazonSocial == "")
            {
                Mensaje += "Es necesario el nombre completo\n";
            }

            if (obj.Correo == "")
            {
                Mensaje += "Es necesario el Correo\n";
            }

            if (obj.Telefono == "")
            {
                Mensaje += "Es necesario el numero de telefono\n";
            }

            if (Mensaje != string.Empty)
            {
                return 0;
            }
            else
            {
                return objcd_Proveedor.Registrar(obj, out Mensaje);
            }
        }



        public bool Editar(Proveedor obj, out string Mensaje)
        {
            Mensaje = string.Empty;

            if (obj.Documento == "")
            {
                Mensaje += "Es necesario el numero de documento\n";
            }

            if (obj.RazonSocial == "")
            {
                Mensaje += "Es necesario el nombre completo\n";
            }

            if (obj.Correo == "")
            {
                Mensaje += "Es necesario el Correo\n";
            }

            if (obj.Telefono == "")
            {
                Mensaje += "Es necesario el numero de telefono\n";
            }

            if (Mensaje != string.Empty)
            {
                return false;
            }
            else
            {
                return objcd_Proveedor.Editar(obj, out Mensaje);
            }

        }

        public bool Eliminar(Proveedor obj, out string Mensaje)
        {

            return objcd_Proveedor.Eliminar(obj, out Mensaje);
        }
    }
}
