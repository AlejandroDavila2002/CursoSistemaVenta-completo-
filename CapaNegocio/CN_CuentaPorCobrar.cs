using CapaDatos;
using CapaEntidad;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaNegocio
{
    public class CN_CuentaPorCobrar
    {
        private CD_CuentaPorCobrar objcd_cuenta = new CD_CuentaPorCobrar();

        public List<CuentaPorCobrar> Listar()
        {
            return objcd_cuenta.Listar();
        }

        public bool RegistrarAbono(int idCuenta, decimal monto, out string Mensaje)
        {
            return objcd_cuenta.RegistrarAbono(idCuenta, monto, out Mensaje);
        }

        public bool Eliminar(int idCuenta, out string Mensaje)
        {
            return objcd_cuenta.Eliminar(idCuenta, out Mensaje);
        }

        public List<Abono> ListarAbonos(int idCuenta)
        {
            return objcd_cuenta.ListarAbonos(idCuenta);
        }


    }
}