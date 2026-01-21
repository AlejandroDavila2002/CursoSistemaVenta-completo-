using CapaEntidad;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace CapaDatos
{
    public class CD_Cuota
    {
        // Método para listar las cuotas en el modal
        public List<Cuota> Listar(int idVenta)
        {
            List<Cuota> lista = new List<Cuota>();

            using (SqlConnection oconexion = new SqlConnection(Conexion.cadena))
            {
                try
                {
                    SqlCommand cmd = new SqlCommand("SP_ListarCuotasPorVenta", oconexion);
                    cmd.Parameters.AddWithValue("@IdVenta", idVenta);
                    cmd.CommandType = CommandType.StoredProcedure;
                    oconexion.Open();

                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            lista.Add(new Cuota()
                            {
                                IdCuota = Convert.ToInt32(dr["IdCuota"]),
                                NumeroCuota = Convert.ToInt32(dr["NumeroCuota"]),
                                FechaProgramada = dr["FechaProgramada"].ToString(),
                                MontoCuota = Convert.ToDecimal(dr["MontoCuota"]),
                                Estado = dr["Estado"].ToString(),
                                FechaPago = dr["FechaPago"].ToString()
                            });
                        }
                    }
                }
                catch (Exception ex)
                {
                    lista = new List<Cuota>();
                }
            }
            return lista;
        }

        // Método para registrar cuotas (Lo usaremos luego al guardar la venta)
        public bool Registrar(Cuota obj, out string Mensaje)
        {
            bool respuesta = false;
            Mensaje = string.Empty;
            using (SqlConnection oconexion = new SqlConnection(Conexion.cadena))
            {
                try
                {
                    SqlCommand cmd = new SqlCommand("SP_RegistrarCuota", oconexion);
                    cmd.Parameters.AddWithValue("@IdVenta", obj.IdVenta);
                    cmd.Parameters.AddWithValue("@NumeroCuota", obj.NumeroCuota);
                    cmd.Parameters.AddWithValue("@FechaProgramada", Convert.ToDateTime(obj.FechaProgramada));
                    cmd.Parameters.AddWithValue("@MontoCuota", obj.MontoCuota);
                    cmd.CommandType = CommandType.StoredProcedure;
                    oconexion.Open();
                    cmd.ExecuteNonQuery();
                    respuesta = true;
                }
                catch (Exception ex)
                {
                    respuesta = false;
                    Mensaje = ex.Message;
                }
            }
            return respuesta;
        }

        // Método para actualizar el estado de una cuota individualmente
        public bool ActualizarEstadoCuota(int idCuota, string estado, string fechaPago)
        {
            bool respuesta = false;
            using (SqlConnection oconexion = new SqlConnection(Conexion.cadena))
            {
                try
                {
                    // SQL Directo para no depender de SPs
                    string query = "UPDATE CUOTA SET Estado = @Estado, FechaPago = @FechaPago WHERE IdCuota = @IdCuota";

                    SqlCommand cmd = new SqlCommand(query, oconexion);
                    cmd.Parameters.AddWithValue("@Estado", estado);

                    // Manejo de fecha nula o vacía
                    if (string.IsNullOrEmpty(fechaPago))
                        cmd.Parameters.AddWithValue("@FechaPago", DBNull.Value);
                    else
                        cmd.Parameters.AddWithValue("@FechaPago", Convert.ToDateTime(fechaPago));

                    cmd.Parameters.AddWithValue("@IdCuota", idCuota);
                    cmd.CommandType = CommandType.Text;

                    oconexion.Open();
                    // Si afecta filas, devolvemos true
                    respuesta = cmd.ExecuteNonQuery() > 0;
                }
                catch (Exception ex)
                {
                    respuesta = false;
                }
            }
            return respuesta;
        }

        // EN CapaDatos -> CD_Cuota.cs

        public List<Cuota> ListarPorVenta(int idVenta) // CAMBIO: Recibimos idVenta
        {
            List<Cuota> lista = new List<Cuota>();

            using (SqlConnection oconexion = new SqlConnection(Conexion.cadena))
            {
                try
                {
                    // CORRECCIÓN CRÍTICA: Usamos WHERE IdVenta = @id
                    // (Asumiendo que tu tabla CUOTA tiene la columna IdVenta, que es lo estándar)
                    string query = "SELECT IdCuota, NumeroCuota, FechaProgramada, MontoCuota, Estado, FechaPago FROM CUOTA WHERE IdVenta = @id";

                    SqlCommand cmd = new SqlCommand(query, oconexion);
                    cmd.Parameters.AddWithValue("@id", idVenta);
                    cmd.CommandType = CommandType.Text;

                    oconexion.Open();

                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            lista.Add(new Cuota()
                            {
                                IdCuota = Convert.ToInt32(dr["IdCuota"]),
                                NumeroCuota = Convert.ToInt32(dr["NumeroCuota"]),
                                FechaProgramada = Convert.ToDateTime(dr["FechaProgramada"]).ToString("dd/MM/yyyy"),
                                MontoCuota = Convert.ToDecimal(dr["MontoCuota"]),
                                Estado = dr["Estado"].ToString(),
                                // Manejo de nulos seguro
                                FechaPago = dr["FechaPago"] != DBNull.Value ? Convert.ToDateTime(dr["FechaPago"]).ToString("dd/MM/yyyy") : "-"
                            });
                        }
                    }
                }
                catch (Exception ex)
                {
                    lista = new List<Cuota>();
                }
            }
            return lista;
        }

    }
}