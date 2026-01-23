using CapaDatos;
using CapaEntidad;
using System.Collections.Generic;
using System.Data;

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


     
        public Dictionary<string, decimal> ObtenerResumen(string fechaInicio, string fechaFin)
        {
            return objcd.ObtenerResumenFinanciero(fechaInicio, fechaFin);
        }





        // --- MÉTODOS PARA CATEGORIAS ---
        public List<CategoriaGasto> ListarCategorias()
        {
            return objcd.ListarCategorias();
        }

        public bool RegistrarCategoria(CategoriaGasto obj, out string Mensaje)
        {
            Mensaje = string.Empty;
            if (string.IsNullOrEmpty(obj.Descripcion) || string.IsNullOrWhiteSpace(obj.Descripcion))
            {
                Mensaje = "La descripción de la categoría no puede ser vacía.";
                return false;
            }
            return objcd.RegistrarCategoria(obj, out Mensaje);
        }

        public bool EliminarCategoria(int id, out string Mensaje)
        {
            return objcd.EliminarCategoria(id, out Mensaje);
        }



        // --- MÉTODOS PARA FORMAS DE PAGO ---
        public List<FormaPago> ListarFormasPago()
        {
            return objcd.ListarFormasPago();
        }

        public bool RegistrarFormaPago(FormaPago obj, out string Mensaje)
        {
            Mensaje = string.Empty;
            if (string.IsNullOrEmpty(obj.Descripcion) || string.IsNullOrWhiteSpace(obj.Descripcion))
            {
                Mensaje = "La descripción no puede ser vacía.";
                return false;
            }
            return objcd.RegistrarFormaPago(obj, out Mensaje);
        }

        public bool EliminarFormaPago(int id, out string Mensaje)
        {
            return objcd.EliminarFormaPago(id, out Mensaje);
        }




        // Metodos para CIERRE DE CAJA

        public DataTable ObtenerDetalleCierre(string fecha, out decimal saldoFinal, out bool hayPendientes)
        {
            return objcd.ObtenerDetalleCierre(fecha, out saldoFinal, out hayPendientes);
        }

        public bool RegistrarCierre(CierreCaja obj, DataTable dtDetalle, out string Mensaje)
        {
            return objcd.RegistrarCierre(obj, dtDetalle, out Mensaje);
        }


        public bool ValidarCierreExistente(string fecha, out string mensaje)
        {
            return objcd.ValidarCierreExistente(fecha, out mensaje);
        }
    }
}