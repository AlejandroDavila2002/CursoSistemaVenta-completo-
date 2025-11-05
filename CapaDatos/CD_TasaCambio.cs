using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CapaEntidad;


namespace CapaDatos
{
    public class CD_TasaCambio
    {
        // Usa la cadena de conexión de la clase estática de CapaDatos
        private string CadenaConexion = Conexion.cadena;

        /// <summary>
        /// Obtiene la última tasa registrada en la base de datos para cada moneda.
        /// (Usado por BcvScraper para verificar si necesita actualizar).
        /// </summary>
        /// <returns>Diccionario con la última TasaCambio por abreviación de moneda.</returns>
        public Dictionary<string, TasaCambio> ObtenerUltimasTasas()
        {
            var listaTasas = new Dictionary<string, TasaCambio>();

            // Query para obtener la última tasa registrada para cada moneda (USD, EUR, etc.)
            string query = @"
                SELECT T.*
                FROM TASA_CAMBIO T
                INNER JOIN (
                    SELECT MonedaAbreviacion, MAX(FechaRegistro) AS MaxFecha
                    FROM TASA_CAMBIO
                    GROUP BY MonedaAbreviacion
                ) AS UltimaTasa ON T.MonedaAbreviacion = UltimaTasa.MonedaAbreviacion 
                                AND T.FechaRegistro = UltimaTasa.MaxFecha";

            try
            {
                using (SqlConnection conexion = new SqlConnection(CadenaConexion))
                {
                    conexion.Open();
                    SqlCommand cmd = new SqlCommand(query, conexion);
                    cmd.CommandType = CommandType.Text;

                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            var tasa = new TasaCambio()
                            {
                                IdTasaCambio = Convert.ToInt32(dr["IdTasaCambio"]),
                                MonedaAbreviacion = dr["MonedaAbreviacion"].ToString(),
                                NombreCompleto = dr["NombreCompleto"].ToString(),
                                Valor = Convert.ToDecimal(dr["Valor"]),
                                FechaValor = dr["FechaValor"].ToString(),
                                FechaRegistro = Convert.ToDateTime(dr["FechaRegistro"])
                            };
                            listaTasas[tasa.MonedaAbreviacion] = tasa;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // En una aplicación real, se usaría un sistema de logs.
                Console.WriteLine($"Error al obtener últimas tasas de DB: {ex.Message}");
            }
            return listaTasas;
        }

        /// <summary>
        /// Guarda un nuevo registro de tasa de cambio del BCV en la tabla TASA_CAMBIO.
        /// (Usado por BcvScraper).
        /// </summary>
        public void RegistrarTasa(TasaCambio tasa)
        {
            string query = @"INSERT INTO TASA_CAMBIO (MonedaAbreviacion, NombreCompleto, Valor, FechaValor, FechaRegistro) 
                             VALUES (@MonedaAbreviacion, @NombreCompleto, @Valor, @FechaValor, @FechaRegistro)";

            try
            {
                using (SqlConnection conexion = new SqlConnection(CadenaConexion))
                {
                    SqlCommand cmd = new SqlCommand(query, conexion);
                    cmd.CommandType = CommandType.Text;

                    cmd.Parameters.AddWithValue("@MonedaAbreviacion", tasa.MonedaAbreviacion);
                    cmd.Parameters.AddWithValue("@NombreCompleto", tasa.NombreCompleto);
                    cmd.Parameters.AddWithValue("@Valor", tasa.Valor);
                    cmd.Parameters.AddWithValue("@FechaValor", tasa.FechaValor);
                    cmd.Parameters.AddWithValue("@FechaRegistro", tasa.FechaRegistro);

                    conexion.Open();
                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al registrar tasa en DB: {ex.Message}");
            }
        }

        // ==============================================================================
        // MÉTODO DE REGISTRO DE OPERACIÓN DE USUARIO
        // ==============================================================================

        /// <summary>
        /// Registra una operación de usuario en la tabla REGISTRO_TASA_USUARIO,
        /// utilizando el objeto Usuario para vincular el registro.
        /// </summary>
        /// <param name="oUsuario">El objeto Usuario logueado (necesario para el IdUsuario).</param>
        /// <param name="tasa">Objeto TasaCambio que contiene los detalles de la tasa del BCV.</param>
        /// <param name="montoOperacion">El monto específico de la operación introducido por el usuario.</param>
        /// <returns>Verdadero si el SP devuelve éxito, Falso en caso contrario.</returns>
        public bool RegistrarRegistroUsuario(Usuario oUsuario, TasaCambio tasa, decimal montoOperacion)
        {
            bool registroExitoso = false;
            // Usamos el Stored Procedure definido en SQL Server
            string spNombre = "SP_RegistrarTasaUsuario";

            try
            {
                using (SqlConnection oConexion = new SqlConnection(CadenaConexion))
                {
                    SqlCommand cmd = new SqlCommand(spNombre, oConexion);

                    // 1. Parámetro del usuario (Tomado del objeto CapaEntidad.Usuario)
                    cmd.Parameters.AddWithValue("@IdUsuario", oUsuario.IdUsuario);

                    // 2. Parámetros de la Tasa de Cambio
                    cmd.Parameters.AddWithValue("@MonedaAbreviacion", tasa.MonedaAbreviacion);
                    cmd.Parameters.AddWithValue("@NombreCompleto", tasa.NombreCompleto);
                    cmd.Parameters.AddWithValue("@ValorTasa", tasa.Valor);
                    cmd.Parameters.AddWithValue("@FechaValorBCV", tasa.FechaValor);

                    // 3. Parámetro de la Operación
                    cmd.Parameters.AddWithValue("@MontoOperacion", montoOperacion);

                    // 4. Parámetro de SALIDA del Stored Procedure
                    SqlParameter paramExito = new SqlParameter("@Resultado", SqlDbType.Bit);
                    paramExito.Direction = ParameterDirection.Output;
                    cmd.Parameters.Add(paramExito);

                    cmd.CommandType = CommandType.StoredProcedure; // Indicamos que es un SP
                    oConexion.Open();
                    cmd.ExecuteNonQuery();

                    // Obtenemos el valor de retorno (1 si fue exitoso, 0 si no)
                    registroExitoso = Convert.ToBoolean(cmd.Parameters["@Resultado"].Value);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al registrar operación de usuario: {ex.Message}");
                registroExitoso = false;
            }

            return registroExitoso;
        }

        // ==============================================================================
        // NUEVOS MÉTODOS REQUERIDOS POR CN_RegistroUsuario
        // ==============================================================================

        /// <summary>
        /// Obtiene el historial de las operaciones de tasas registradas por un usuario.
        /// Este método debe consultar la tabla REGISTRO_TASA_USUARIO.
        /// </summary>
        /// <param name="idUsuario">ID del usuario logueado.</param>
        /// <returns>Una lista de arrays de objetos con los datos del historial.</returns>
        public List<object[]> ObtenerHistorialUsuario(int idUsuario)
        {
            var historial = new List<object[]>();
            string query = @"
                SELECT TOP 50
                    NombreCompleto, -- Nombre de la Moneda (ej: 'Euro')
                    FechaValorBCV,  -- Fecha del BCV de esa tasa
                    MontoOperacion, -- Tasa/Monto que el usuario registró
                    FechaRegistro   -- Fecha de registro de la operación
                FROM REGISTRO_TASA_USUARIO
                WHERE IdUsuario = @IdUsuario
                ORDER BY FechaRegistro DESC"; // Mostrar los más recientes primero

            try
            {
                using (SqlConnection conexion = new SqlConnection(CadenaConexion))
                {
                    conexion.Open();
                    SqlCommand cmd = new SqlCommand(query, conexion);
                    cmd.Parameters.AddWithValue("@IdUsuario", idUsuario);
                    cmd.CommandType = CommandType.Text;

                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            // Mapeamos los campos a un array de objetos para que la Capa de Negocio lo consuma
                            historial.Add(new object[]
                            {
                                dr["NombreCompleto"].ToString(), // Columna 1: Moneda
                                dr["FechaValorBCV"].ToString(),  // Columna 2: Fecha BCV
                                Convert.ToDecimal(dr["MontoOperacion"]), // Columna 3: Monto Operación
                                Convert.ToDateTime(dr["FechaRegistro"]).ToString("dd/MM/yyyy HH:mm") // Columna 4: Fecha de Registro
                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al obtener historial de usuario: {ex.Message}");
            }
            return historial;
        }

        /// <summary>
        /// Guarda o actualiza la tasa preferida (Tasa General) del usuario.
        /// Este método debe usar una tabla para guardar la preferencia (ej: PREFERENCIAS_USUARIO).
        /// </summary>
        /// <param name="idUsuario">ID del usuario logueado.</param>
        /// <param name="monedaAbreviacion">La abreviación de la moneda seleccionada (ej: USD).</param>
        /// <param name="montoTasaGeneral">El valor de la tasa que el usuario desea como general.</param>
        /// <returns>Verdadero si el proceso fue exitoso.</returns>
        public bool GuardarTasaGeneralUsuario(int idUsuario, string monedaAbreviacion, decimal montoTasaGeneral)
        {
            bool exito = false;
            // Usaremos un Stored Procedure que insertará o actualizará (UPSERT) la preferencia.
            // Asumo que tienes una tabla PREFERENCIAS_USUARIO con IdUsuario, MonedaTasaGeneral, ValorTasaGeneral
            string spNombre = "SP_GuardarTasaGeneral";

            try
            {
                using (SqlConnection oConexion = new SqlConnection(CadenaConexion))
                {
                    SqlCommand cmd = new SqlCommand(spNombre, oConexion);

                    cmd.Parameters.AddWithValue("@IdUsuario", idUsuario);
                    cmd.Parameters.AddWithValue("@MonedaAbreviacion", monedaAbreviacion);
                    cmd.Parameters.AddWithValue("@ValorTasaGeneral", montoTasaGeneral);

                    // Parámetro de SALIDA
                    SqlParameter paramExito = new SqlParameter("@Resultado", SqlDbType.Bit);
                    paramExito.Direction = ParameterDirection.Output;
                    cmd.Parameters.Add(paramExito);

                    cmd.CommandType = CommandType.StoredProcedure;
                    oConexion.Open();
                    cmd.ExecuteNonQuery();

                    exito = Convert.ToBoolean(cmd.Parameters["@Resultado"].Value);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al guardar Tasa General: {ex.Message}");
                exito = false;
            }

            return exito;
        }

        // ==============================================================================
        // MÉTODO ADICIONAL REQUERIDO: OBTENER TASA GENERAL DEL USUARIO
        // ==============================================================================

        /// <summary>
        /// Obtiene la Tasa General (preferida) registrada por el usuario.
        /// </summary>
        /// <param name="idUsuario">ID del usuario logueado.</param>
        /// <returns>Un objeto TasaGeneralUsuario (que deberás crear como CapaEntidad) o null si no existe.</returns>
        public TasaGeneralUsuario ObtenerTasaGeneralUsuario(int idUsuario)
        {
            TasaGeneralUsuario tasaGeneral = null;
            string query = @"
                SELECT TOP 1 
                    MonedaTasaGeneral, 
                    ValorTasaGeneral 
                FROM PREFERENCIAS_USUARIO 
                WHERE IdUsuario = @IdUsuario";

            try
            {
                using (SqlConnection conexion = new SqlConnection(CadenaConexion))
                {
                    conexion.Open();
                    SqlCommand cmd = new SqlCommand(query, conexion);
                    cmd.Parameters.AddWithValue("@IdUsuario", idUsuario);
                    cmd.CommandType = CommandType.Text;

                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        if (dr.Read())
                        {
                            // NOTA: Asume que existe una entidad 'TasaGeneralUsuario' con estas propiedades
                            tasaGeneral = new TasaGeneralUsuario()
                            {
                                MonedaAbreviacion = dr["MonedaTasaGeneral"].ToString(),
                                Valor = Convert.ToDecimal(dr["ValorTasaGeneral"])
                            };
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al obtener Tasa General de usuario: {ex.Message}");
                // Retorna null en caso de error
            }
            return tasaGeneral;
        }

    }
}
