using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CapaEntidad;
using CapaDatos;

namespace CapaNegocio
{
    public class CN_RegistroUsuario
    {
        // Se asume que CD_TasaCambio contiene los métodos para interactuar con la BD.
        private CD_TasaCambio oCD_TasaCambio = new CD_TasaCambio();

        /// <summary>
        /// Obtiene un diccionario de las últimas tasas de cambio registradas en la BD para la inicialización de formularios.
        /// </summary>
        /// <returns>Diccionario con la MonedaAbreviacion como clave y el objeto TasaCambio como valor.</returns>
        public Dictionary<string, TasaCambio> ObtenerTasasDisponibles()
        {
            return oCD_TasaCambio.ObtenerUltimasTasas();
        }

        /// <summary>
        /// Guarda un registro individual de operación de tasa de cambio asociada a un usuario.
        /// </summary>
        public bool GuardarRegistroIndividual(Usuario oUsuario, TasaCambio tasa, decimal montoOperacion)
        {
            // Simple validación de negocio antes de llamar a la Capa de Datos
            if (oUsuario == null || oUsuario.IdUsuario <= 0)
            {
                throw new ArgumentException("El usuario de la sesión es inválido.");
            }

            if (montoOperacion <= 0)
            {
                throw new ArgumentException("El monto de la operación debe ser un valor positivo.");
            }

            return oCD_TasaCambio.RegistrarRegistroUsuario(oUsuario, tasa, montoOperacion);
        }

        // ==============================================================================
        // MÉTODOS DE HISTORIAL Y TASA GENERAL
        // ==============================================================================

        /// <summary>
        /// Obtiene el historial de todas las operaciones de tasas registradas 
        /// previamente por un usuario.
        /// </summary>
        public List<object[]> ObtenerHistorialOperacionesPorUsuario(int idUsuario)
        {
            if (idUsuario <= 0)
            {
                return new List<object[]>();
            }

            return oCD_TasaCambio.ObtenerHistorialUsuario(idUsuario);
        }

        /// <summary>
        /// Establece una tasa de cambio específica (Moneda y Monto) como 
        /// la 'Tasa General' o predeterminada del sistema para este usuario.
        /// </summary>
        public bool EstablecerTasaGeneralUsuario(int idUsuario, TasaCambio tasaOriginal, decimal montoOperacion)
        {
            if (idUsuario <= 0)
            {
                throw new ArgumentException("El ID de usuario es inválido para establecer la Tasa General.");
            }

            if (montoOperacion <= 0)
            {
                throw new ArgumentException("El monto de la Tasa General debe ser un valor positivo.");
            }

            return oCD_TasaCambio.GuardarTasaGeneralUsuario(idUsuario, tasaOriginal.MonedaAbreviacion, montoOperacion);
        }

        /// <summary>
        /// [AGREGADO] Carga la tasa de cambio preferida o "General" guardada por el usuario.
        /// </summary>
        /// <param name="idUsuario">ID del usuario.</param>
        /// <returns>Objeto TasaGeneralUsuario o null si no se encontró ninguna preferencia.</returns>
        public TasaGeneralUsuario ObtenerTasaGeneral(int idUsuario)
        {
            if (idUsuario <= 0)
            {
                return null;
            }
            return oCD_TasaCambio.ObtenerTasaGeneralUsuario(idUsuario);
        }
    }
}
