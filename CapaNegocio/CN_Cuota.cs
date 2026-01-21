using CapaDatos;
using CapaEntidad;
using System.Collections.Generic;

namespace CapaNegocio
{
    public class CN_Cuota
    {
        private CD_Cuota objcd_cuota = new CD_Cuota();

        public List<Cuota> Listar(int idVenta)
        {
            return objcd_cuota.Listar(idVenta);
        }

        public bool Registrar(Cuota obj, out string Mensaje)
        {
            return objcd_cuota.Registrar(obj, out Mensaje);
        }
    }
}