using CapaEntidad;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;


namespace CapaDatos
{
    public class CD_FlujoCaja
    {
        // 1. REGISTRAR GASTO
        public bool RegistrarGasto(Gasto obj, out string Mensaje)
        {
            bool respuesta = false;
            Mensaje = string.Empty;

            using (SqlConnection oconexion = new SqlConnection(Conexion.cadena))
            {
                try
                {
                    SqlCommand cmd = new SqlCommand("SP_RegistrarGasto", oconexion);
                    cmd.Parameters.AddWithValue("IdUsuario", obj.oUsuario.IdUsuario);
                    // CAMBIO CLAVE: Usamos OCategoriaGasto y su IdCategoriaGasto
                    cmd.Parameters.AddWithValue("IdCategoriaGasto", obj.oCategoriaGasto.IdCategoriaGasto);
                    cmd.Parameters.AddWithValue("Descripcion", obj.Descripcion);
                    cmd.Parameters.AddWithValue("Monto", obj.Monto);
                    cmd.Parameters.AddWithValue("Referencia", obj.Referencia);
                    cmd.Parameters.AddWithValue("FormaPago", obj.FormaPago);
                    cmd.Parameters.Add("Resultado", SqlDbType.Int).Direction = ParameterDirection.Output;
                    cmd.Parameters.Add("Mensaje", SqlDbType.VarChar, 500).Direction = ParameterDirection.Output;
                    cmd.CommandType = CommandType.StoredProcedure;

                    oconexion.Open();
                    cmd.ExecuteNonQuery();

                    respuesta = Convert.ToBoolean(cmd.Parameters["Resultado"].Value);
                    Mensaje = cmd.Parameters["Mensaje"].Value.ToString();
                }
                catch (Exception ex)
                {
                    respuesta = false;
                    Mensaje = ex.Message;
                }
            }
            return respuesta;
        }

        // 2. LISTAR GASTOS
        public List<Gasto> ListarGastos(string fechaInicio, string fechaFin)
        {
            List<Gasto> lista = new List<Gasto>();
            using (SqlConnection oconexion = new SqlConnection(Conexion.cadena))
            {
                try
                {
                    SqlCommand cmd = new SqlCommand("SP_ReporteGastos", oconexion);
                    cmd.Parameters.AddWithValue("FechaInicio", fechaInicio);
                    cmd.Parameters.AddWithValue("FechaFin", fechaFin);
                    cmd.CommandType = CommandType.StoredProcedure;

                    oconexion.Open();
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            lista.Add(new Gasto()
                            {
                                IdGasto = Convert.ToInt32(dr["IdGasto"]),
                                oUsuario = new Usuario() { NombreCompleto = dr["Usuario"].ToString() },
                                // CAMBIO CLAVE: Instanciamos CategoriaGasto
                                oCategoriaGasto = new CategoriaGasto() { Descripcion = dr["Categoria"].ToString() },
                                Descripcion = dr["Descripcion"].ToString(),
                                FormaPago = dr["FormaPago"].ToString(),
                                Referencia = dr["Referencia"].ToString(),
                                Monto = Convert.ToDecimal(dr["Monto"]),
                                FechaRegistro = dr["Fecha"].ToString()
                            });
                        }
                    }
                }
                catch (Exception) { lista = new List<Gasto>(); }
            }
            return lista;
        }

        // 3. ELIMINAR GASTO
        public bool EliminarGasto(int id, out string Mensaje)
        {
            bool respuesta = false;
            Mensaje = string.Empty;
            using (SqlConnection oconexion = new SqlConnection(Conexion.cadena))
            {
                try
                {
                    SqlCommand cmd = new SqlCommand("SP_EliminarGasto", oconexion);
                    cmd.Parameters.AddWithValue("IdGasto", id);
                    cmd.Parameters.Add("Resultado", SqlDbType.Int).Direction = ParameterDirection.Output;
                    cmd.CommandType = CommandType.StoredProcedure;
                    oconexion.Open();
                    cmd.ExecuteNonQuery();
                    respuesta = Convert.ToBoolean(cmd.Parameters["Resultado"].Value);
                }
                catch (Exception ex) { Mensaje = ex.Message; respuesta = false; }
            }
            return respuesta;
        }


        // MÉTODO NUEVO PARA OBTENER RESUMEN FINANCIERO
        public Dictionary<string, decimal> ObtenerResumenFinanciero(string fechaInicio, string fechaFin)
        {
            // Inicializamos con valores por defecto para garantizar que siempre existan las claves
            Dictionary<string, decimal> resumen = new Dictionary<string, decimal>()
            {
                { "TotalIngresos", 0m },
                { "TotalEgresosMercancia", 0m },
                { "TotalGastosOperativos", 0m }
            };

            using (SqlConnection oconexion = new SqlConnection(Conexion.cadena))
            {
                try
                {
                    using (SqlCommand cmd = new SqlCommand("SP_ResumenFinanciero", oconexion))
                    {
                        cmd.Parameters.AddWithValue("FechaInicio", fechaInicio);
                        cmd.Parameters.AddWithValue("FechaFin", fechaFin);
                        cmd.CommandType = CommandType.StoredProcedure;

                        oconexion.Open();
                        using (SqlDataReader dr = cmd.ExecuteReader())
                        {
                            if (dr.Read())
                            {
                                // Comprobación de DBNull para evitar excepciones
                                resumen["TotalIngresos"] = dr["TotalIngresos"] == DBNull.Value ? 0m : Convert.ToDecimal(dr["TotalIngresos"]);
                                resumen["TotalEgresosMercancia"] = dr["TotalEgresosMercancia"] == DBNull.Value ? 0m : Convert.ToDecimal(dr["TotalEgresosMercancia"]);
                                resumen["TotalGastosOperativos"] = dr["TotalGastosOperativos"] == DBNull.Value ? 0m : Convert.ToDecimal(dr["TotalGastosOperativos"]);
                            }
                        }
                    }
                }
                catch (Exception)
                {
                    // En caso de error devolvemos los valores por defecto ya inicializados.
                    // Si quieres, aquí puedes registrar el error en un log.
                }
            }
            return resumen;
        }


        // --- SECCIÓN CATEGORIAS ---

        public List<CategoriaGasto> ListarCategorias()
        {
            List<CategoriaGasto> lista = new List<CategoriaGasto>();
            using (SqlConnection oconexion = new SqlConnection(Conexion.cadena))
            {
                try
                {
                    // CAMBIO: Ahora usamos el SP en lugar de texto directo
                    SqlCommand cmd = new SqlCommand("SP_ListarCategoriaGasto", oconexion);
                    cmd.CommandType = CommandType.StoredProcedure;

                    oconexion.Open();
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            lista.Add(new CategoriaGasto()
                            {
                                IdCategoriaGasto = Convert.ToInt32(dr["IdCategoriaGasto"]),
                                Descripcion = dr["Descripcion"].ToString()
                            });
                        }
                    }
                }
                catch (Exception) { lista = new List<CategoriaGasto>(); }
            }
            return lista;
        }

        public bool RegistrarCategoria(CategoriaGasto obj, out string Mensaje)
        {
            bool respuesta = false;
            Mensaje = string.Empty;
            using (SqlConnection oconexion = new SqlConnection(Conexion.cadena))
            {
                try
                {
                    SqlCommand cmd = new SqlCommand("SP_RegistrarCategoriaGasto", oconexion);
                    cmd.Parameters.AddWithValue("Descripcion", obj.Descripcion);
                    cmd.Parameters.Add("Resultado", SqlDbType.Int).Direction = ParameterDirection.Output;
                    cmd.Parameters.Add("Mensaje", SqlDbType.VarChar, 500).Direction = ParameterDirection.Output;
                    cmd.CommandType = CommandType.StoredProcedure;

                    oconexion.Open();
                    cmd.ExecuteNonQuery();

                    respuesta = Convert.ToBoolean(cmd.Parameters["Resultado"].Value);
                    Mensaje = cmd.Parameters["Mensaje"].Value.ToString();
                }
                catch (Exception ex)
                {
                    respuesta = false;
                    Mensaje = ex.Message;
                }
            }
            return respuesta;
        }

        public bool EliminarCategoria(int id, out string Mensaje)
        {
            bool respuesta = false;
            Mensaje = string.Empty;
            using (SqlConnection oconexion = new SqlConnection(Conexion.cadena))
            {
                try
                {
                    SqlCommand cmd = new SqlCommand("SP_EliminarCategoriaGasto", oconexion);
                    cmd.Parameters.AddWithValue("IdCategoriaGasto", id);
                    cmd.Parameters.Add("Resultado", SqlDbType.Int).Direction = ParameterDirection.Output;
                    cmd.Parameters.Add("Mensaje", SqlDbType.VarChar, 500).Direction = ParameterDirection.Output;
                    cmd.CommandType = CommandType.StoredProcedure;

                    oconexion.Open();
                    cmd.ExecuteNonQuery();

                    respuesta = Convert.ToBoolean(cmd.Parameters["Resultado"].Value);
                    Mensaje = cmd.Parameters["Mensaje"].Value.ToString();
                }
                catch (Exception ex) { respuesta = false; Mensaje = ex.Message; }
            }
            return respuesta;
        }

        // --- SECCIÓN FORMA DE PAGO ---

        public List<FormaPago> ListarFormasPago()
        {
            List<FormaPago> lista = new List<FormaPago>();
            using (SqlConnection oconexion = new SqlConnection(Conexion.cadena))
            {
                try
                {
                    SqlCommand cmd = new SqlCommand("SP_ListarFormaPago", oconexion);
                    cmd.CommandType = CommandType.StoredProcedure;

                    oconexion.Open();
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            lista.Add(new FormaPago()
                            {
                                IdFormaPago = Convert.ToInt32(dr["IdFormaPago"]),
                                Descripcion = dr["Descripcion"].ToString()
                            });
                        }
                    }
                }
                catch (Exception) { lista = new List<FormaPago>(); }
            }
            return lista;
        }

        public bool RegistrarFormaPago(FormaPago obj, out string Mensaje)
        {
            bool respuesta = false;
            Mensaje = string.Empty;
            using (SqlConnection oconexion = new SqlConnection(Conexion.cadena))
            {
                try
                {
                    SqlCommand cmd = new SqlCommand("SP_RegistrarFormaPago", oconexion);
                    cmd.Parameters.AddWithValue("Descripcion", obj.Descripcion);
                    cmd.Parameters.Add("Resultado", SqlDbType.Int).Direction = ParameterDirection.Output;
                    cmd.Parameters.Add("Mensaje", SqlDbType.VarChar, 500).Direction = ParameterDirection.Output;
                    cmd.CommandType = CommandType.StoredProcedure;

                    oconexion.Open();
                    cmd.ExecuteNonQuery();

                    respuesta = Convert.ToBoolean(cmd.Parameters["Resultado"].Value);
                    Mensaje = cmd.Parameters["Mensaje"].Value.ToString();
                }
                catch (Exception ex)
                {
                    respuesta = false;
                    Mensaje = ex.Message;
                }
            }
            return respuesta;
        }

        public bool EliminarFormaPago(int id, out string Mensaje)
        {
            bool respuesta = false;
            Mensaje = string.Empty;
            using (SqlConnection oconexion = new SqlConnection(Conexion.cadena))
            {
                try
                {
                    SqlCommand cmd = new SqlCommand("SP_EliminarFormaPago", oconexion);
                    cmd.Parameters.AddWithValue("IdFormaPago", id);
                    cmd.Parameters.Add("Resultado", SqlDbType.Int).Direction = ParameterDirection.Output;
                    cmd.Parameters.Add("Mensaje", SqlDbType.VarChar, 500).Direction = ParameterDirection.Output;
                    cmd.CommandType = CommandType.StoredProcedure;

                    oconexion.Open();
                    cmd.ExecuteNonQuery();

                    respuesta = Convert.ToBoolean(cmd.Parameters["Resultado"].Value);
                    Mensaje = cmd.Parameters["Mensaje"].Value.ToString();
                }
                catch (Exception ex) { respuesta = false; Mensaje = ex.Message; }
            }
            return respuesta;
        }
    }
}