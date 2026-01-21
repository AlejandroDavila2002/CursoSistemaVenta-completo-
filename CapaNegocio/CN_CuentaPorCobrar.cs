using CapaDatos;
using CapaEntidad;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CapaNegocio
{
    public class CN_CuentaPorCobrar
    {
        private CD_CuentaPorCobrar objCD_Cuenta = new CD_CuentaPorCobrar();
        private CD_Cuota objCD_Cuota = new CD_Cuota();

        public List<CuentaPorCobrar> Listar()
        {
            return objCD_Cuenta.Listar();
        }

        // --- MÉTODO CON LA LÓGICA DE CASCADA ---
        public bool RegistrarAbono(int idCuenta, int idVenta, decimal montoAbono, out string Mensaje)
        {
            // 1. Validar monto
            if (montoAbono <= 0)
            {
                Mensaje = "El abono debe ser mayor a 0";
                return false;
            }

            // 2. Registrar el dinero (Baja la deuda global)
            // Usamos el método que ya tienes en CD_CuentaPorCobrar
            bool abonoExitoso = objCD_Cuenta.RegistrarAbono(idCuenta, montoAbono, out Mensaje);

            if (!abonoExitoso) return false;

            // --- INICIO LÓGICA AUTOMÁTICA DE CUOTAS (C#) ---
            try
            {
                // A. Obtenemos el nuevo estado de la cuenta (para saber cuánto se ha pagado en total)
                // Buscamos la cuenta específica en la lista
                CuentaPorCobrar cuentaActual = objCD_Cuenta.Listar().FirstOrDefault(c => c.IdCuentaPorCobrar == idCuenta);

                if (cuentaActual != null)
                {
                    decimal totalPagadoAcumulado = cuentaActual.MontoPagado;

                    // B. Obtenemos las cuotas de esa venta (ordenadas por fecha idealmente)
                    // Nota: Asegúrate que Listar devuelva las cuotas ordenadas por NumeroCuota o Fecha
                    List<Cuota> listaCuotas = objCD_Cuota.Listar(idVenta);

                    decimal acumuladoCalculo = 0;

                    foreach (Cuota c in listaCuotas)
                    {
                        // Vamos sumando el monto de las cuotas para ver hasta dónde alcanza el dinero
                        acumuladoCalculo += c.MontoCuota;

                        // Si el dinero que ha pagado el cliente CUBRE esta cuota...
                        if (totalPagadoAcumulado >= acumuladoCalculo)
                        {
                            // Si la cuota NO estaba pagada, la marcamos como PAGADA
                            if (c.Estado != "Pagada")
                            {
                                objCD_Cuota.ActualizarEstadoCuota(c.IdCuota, "Pagada", DateTime.Now.ToString("yyyy-MM-dd"));
                            }
                        }
                        else
                        {
                            // El dinero no alcanza para cubrir esta cuota completa
                            // La dejamos Pendiente (o podrías implementar lógica de 'Parcial' si quisieras)
                            if (c.Estado == "Pagada")
                            {
                                // Caso raro: Si anulamos un abono, podríamos revertir a Pendiente aquí
                                objCD_Cuota.ActualizarEstadoCuota(c.IdCuota, "Pendiente", "");
                            }
                        }
                    }
                }

                Mensaje = "Abono registrado y cuotas actualizadas correctamente.";
                return true;
            }
            catch (Exception ex)
            {
                // Si falla la lógica de cuotas, al menos el dinero sí se registró.
                Mensaje = "Abono registrado, pero hubo error al actualizar cuotas: " + ex.Message;
                return true;
            }
        }

        public bool Eliminar(int id, out string Mensaje)
        {
            return objCD_Cuenta.Eliminar(id, out Mensaje);
        }

        public List<Abono> ListarAbonosPorVenta(int idVenta)
        {
            return objCD_Cuenta.ListarAbonosPorVenta(idVenta);
        }
    }
}