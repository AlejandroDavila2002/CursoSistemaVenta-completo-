using CapaEntidad;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaDatos
{
    public class CD_CuentaPorCobrar
    {
        public List<CuentaPorCobrar> Listar()
        {
            List<CuentaPorCobrar> lista = new List<CuentaPorCobrar>();

            using (SqlConnection oconexion = new SqlConnection(Conexion.cadena))
            {
                try
                {
                    SqlCommand cmd = new SqlCommand("SP_ListarCuentasPorCobrar", oconexion);
                    cmd.CommandType = CommandType.StoredProcedure;
                    oconexion.Open();

                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            lista.Add(new CuentaPorCobrar()
                            {
                                IdCuentaPorCobrar = Convert.ToInt32(dr["IdCuentaPorCobrar"]),
                                oVenta = new Venta() { IdVenta = Convert.ToInt32(dr["IdVenta"]), NumeroDocumento = dr["NumeroDocumento"].ToString() },
                                oCliente = new Cliente()
                                {
                                    IdCliente = Convert.ToInt32(dr["IdCliente"]),
                                    Documento = dr["Documento"].ToString(),
                                    NombreCompleto = dr["NombreCompleto"].ToString()
                                },
                                MontoTotal = Convert.ToDecimal(dr["MontoTotal"]),
                                MontoPagado = Convert.ToDecimal(dr["MontoPagado"]),
                                SaldoPendiente = Convert.ToDecimal(dr["SaldoPendiente"]),
                                FechaRegistro = dr["FechaRegistro"].ToString(),

                                // --- CORRECCIÓN ANTIBOMBA (NULOS) ---
                                // Si FechaVencimiento es NULL (ventas viejas), devolvemos vacio ""
                                FechaVencimiento = dr["FechaVencimiento"] != DBNull.Value
                                    ? Convert.ToDateTime(dr["FechaVencimiento"]).ToString("dd/MM/yyyy")
                                    : "",

                                // Si DescripcionPlan es NULL, ponemos un texto por defecto
                                DescripcionPlan = dr["DescripcionPlan"] != DBNull.Value
                                    ? dr["DescripcionPlan"].ToString()
                                    : "Crédito Simple",

                                // Si Mora es NULL, asumimos 0
                                PorcentajeMora = dr["PorcentajeMora"] != DBNull.Value
                                    ? Convert.ToDecimal(dr["PorcentajeMora"])
                                    : 0
                            });
                        }
                    }
                }
                catch (Exception ex)
                {
                    // Tip: Si sigue fallando, esto te dirá por qué en la ventana de Salida de Visual Studio
                    System.Diagnostics.Debug.WriteLine("Error en Listar Cuentas: " + ex.Message);
                    lista = new List<CuentaPorCobrar>();
                }
            }
            return lista;
        }
        public bool RegistrarAbono(int idCuenta, decimal monto, out string Mensaje)
        {
            bool respuesta = false;
            Mensaje = string.Empty;

            using (SqlConnection oconexion = new SqlConnection(Conexion.cadena))
            {
                try
                {
                    SqlCommand cmd = new SqlCommand("SP_RegistrarAbono", oconexion);
                    cmd.Parameters.AddWithValue("IdCuentaPorCobrar", idCuenta);
                    cmd.Parameters.AddWithValue("MontoAbono", monto);
                    cmd.Parameters.Add("Resultado", SqlDbType.Bit).Direction = ParameterDirection.Output;
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



        public bool Eliminar(int idCuenta, out string Mensaje)
        {
            bool respuesta = false;
            Mensaje = string.Empty;

            using (SqlConnection oconexion = new SqlConnection(Conexion.cadena))
            {
                try
                {
                    SqlCommand cmd = new SqlCommand("SP_EliminarCuentaPorCobrar", oconexion);
                    cmd.Parameters.AddWithValue("IdCuentaPorCobrar", idCuenta);
                    cmd.Parameters.Add("Resultado", SqlDbType.Bit).Direction = ParameterDirection.Output;
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
    }
}