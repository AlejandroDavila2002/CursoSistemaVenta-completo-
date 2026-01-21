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
    }
}