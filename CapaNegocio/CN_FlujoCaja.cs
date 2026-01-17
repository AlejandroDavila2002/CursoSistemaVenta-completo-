using CapaDatos;
using CapaEntidad;
using System.Collections.Generic;

namespace CapaNegocio
{
    public class CN_FlujoCaja
    {
        private CD_FlujoCaja objcd = new CD_FlujoCaja();

        // Validar y Registrar
        public bool Registrar(Gasto obj, out string Mensaje)
        {
            Mensaje = string.Empty;

            // Validaciones de Negocio
            if (obj.oCategoriaGasto.IdCategoriaGasto == 0)
            {
                Mensaje = "Debe seleccionar una categoría de gasto.";
                return false;
            }

            if (string.IsNullOrEmpty(obj.Descripcion) || string.IsNullOrWhiteSpace(obj.Descripcion))
            {
                Mensaje = "La descripción del gasto no puede estar vacía.";
                return false;
            }

            if (obj.Monto <= 0)
            {
                Mensaje = "El monto debe ser mayor a 0.";
                return false;
            }

            return objcd.RegistrarGasto(obj, out Mensaje);
        }

        // Listar
        public List<Gasto> Listar(string fechaInicio, string fechaFin)
        {
            return objcd.ListarGastos(fechaInicio, fechaFin);
        }

        // Eliminar
        public bool Eliminar(int id, out string Mensaje)
        {
            return objcd.EliminarGasto(id, out Mensaje);
        }
    }
}