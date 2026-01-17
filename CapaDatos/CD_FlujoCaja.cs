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
    }
}