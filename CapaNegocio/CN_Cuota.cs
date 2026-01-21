using CapaDatos;
using CapaEntidad;
using System.Collections.Generic;

namespace CapaNegocio
{
    public class CN_Cuota
    {
        private CD_Cuota objCD_Cuota = new CD_Cuota();

        // 1. Método existente (si lo tenías)
        public bool Registrar(Cuota obj, out string Mensaje)
        {
            return objCD_Cuota.Registrar(obj, out Mensaje);
        }

        // 2. NUEVO MÉTODO PUENTE (El que faltaba)
        // EN CapaNegocio -> CN_Cuota.cs

        public List<Cuota> ListarPorVenta(int idVenta)
        {
            return objCD_Cuota.ListarPorVenta(idVenta);
        }

        // 3. Método para la lógica de cascada (Opcional si lo usas en CN_CuentaPorCobrar)
        public bool ActualizarEstadoCuota(int idCuota, string estado, string fechaPago)
        {
            return objCD_Cuota.ActualizarEstadoCuota(idCuota, estado, fechaPago);
        }
    }
}