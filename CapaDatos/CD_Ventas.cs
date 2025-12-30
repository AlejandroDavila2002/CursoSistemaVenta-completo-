using CapaEntidad;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CapaDatos
{
    public class CD_Ventas
    {
        public int ObtenerCorrelativo()
        {
            int idCorrelativo = 0;

            using (SqlConnection oconexion = new SqlConnection(Conexion.cadena))
            {

                try
                {
                    StringBuilder query = new StringBuilder();
                    query.AppendLine("select count (*) + 1 from VENTA");

                    SqlCommand cmd = new SqlCommand(query.ToString(), oconexion);
                    cmd.CommandType = CommandType.Text;

                    oconexion.Open();

                    idCorrelativo = Convert.ToInt32(cmd.ExecuteScalar());

                }
                catch
                {
                    idCorrelativo = 0;
                }
            }
            return idCorrelativo;
        }


        public bool RestarStock(int idProducto, int cantidad)
        {
            bool Resultado = false;

            using (SqlConnection oconexion = new SqlConnection(Conexion.cadena))
            {
                try
                {
                    StringBuilder query = new StringBuilder();
                    query.AppendLine("update PRODUCTO set Stock = Stock - @Cantidad where IdProducto = @IdProducto");
                    SqlCommand cmd = new SqlCommand(query.ToString(), oconexion);
                    cmd.Parameters.AddWithValue("@Cantidad", cantidad);
                    cmd.Parameters.AddWithValue("@IdProducto", idProducto);
                    cmd.CommandType = CommandType.Text;
                    oconexion.Open();

                    Resultado = cmd.ExecuteNonQuery() > 0 ? true : false;


                }
                catch (Exception ex)
                {
                    Resultado = false;

                }


            }

            return Resultado;

        }

        public bool sumarStock(int IdProducto, int Cantidad)
        {
            bool Resultado = false;

            using (SqlConnection oconexion = new SqlConnection(Conexion.cadena))
            {
                try
                {
                    StringBuilder query = new StringBuilder();
                    query.AppendLine("update PRODUCTO set Stock = Stock + @Cantidad where IdProducto = @IdProducto");
                    SqlCommand cmd = new SqlCommand(query.ToString(), oconexion);
                    cmd.Parameters.AddWithValue("@Cantidad", Cantidad);
                    cmd.Parameters.AddWithValue("@IdProducto", IdProducto);
                    cmd.CommandType = CommandType.Text;
                    oconexion.Open();
                    Resultado = cmd.ExecuteNonQuery() > 0 ? true : false;
                }
                catch (Exception ex)
                {
                    Resultado = false;
                }

            }
            return Resultado;

        }


        public bool RegistrarVenta(Venta obj, DataTable DetalleVenta, out string Mensaje)
        {
            bool Resultado = false;
            Mensaje = string.Empty;

            try
            {
                using (SqlConnection oconexion = new SqlConnection(Conexion.cadena))
                {
                    SqlCommand cmd = new SqlCommand("SP_RegistrarVenta", oconexion);
                    cmd.Parameters.AddWithValue("IdUsuario", obj.oUsuario.IdUsuario);
                    cmd.Parameters.AddWithValue("TipoDocumento", obj.TipoDocumento);
                    cmd.Parameters.AddWithValue("NumeroDocumento", obj.NumeroDocumento);
                    cmd.Parameters.AddWithValue("DocumentoCliente", obj.DocumentoCliente);
                    cmd.Parameters.AddWithValue("NombreCliente", obj.NombreCliente);
                    cmd.Parameters.AddWithValue("MontoPago", obj.MontoPago);
                    cmd.Parameters.AddWithValue("MontoCambio", obj.MontoCambio);
                    cmd.Parameters.AddWithValue("MontoTotal", obj.MontoTotal);
                    cmd.Parameters.AddWithValue("DetalleVenta", DetalleVenta);
                    cmd.Parameters.AddWithValue("@MontoBs", obj.MontoBs);
                    cmd.Parameters.AddWithValue("@TasaCambio", obj.TasaCambio);
                    cmd.Parameters.Add("Resultado", SqlDbType.Int).Direction = ParameterDirection.Output;
                    cmd.Parameters.Add("Mensaje", SqlDbType.VarChar, 500).Direction = ParameterDirection.Output;
                    cmd.CommandType = CommandType.StoredProcedure;
                    oconexion.Open();
                    cmd.ExecuteNonQuery();
                    Resultado = Convert.ToBoolean(cmd.Parameters["Resultado"].Value);
                    Mensaje = cmd.Parameters["Mensaje"].Value.ToString();
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show("Error en SQL: " + ex.Message);
                Resultado = false;
            }

            return Resultado;
        }

        public Venta ObtenerVenta(string NumeroDocumento)
        {
            Venta obj = new Venta();
            using (SqlConnection oconexion = new SqlConnection(Conexion.cadena))
            {
                try
                {
                    StringBuilder query = new StringBuilder();
                    query.AppendLine("select v.IdVenta, u.NombreCompleto,"); 
                    query.AppendLine("v.DocumentoCliente, v.NombreCliente,");
                    query.AppendLine("v.TipoDocumento, v.NumeroDocumento,");
                    query.AppendLine("v.MontoPago, v.MontoCambio, v.MontoTotal,");
                    query.AppendLine("convert(char(10),v.FechaRegistro,103)[FechaRegistro]");
                    query.AppendLine("from VENTA v");
                    query.AppendLine("inner join USUARIO u on u.IdUsuario = v.IdUsuario");
                    query.AppendLine("where v.NumeroDocumento = @NumeroDocumento");
                    SqlCommand cmd = new SqlCommand(query.ToString(), oconexion);
                    cmd.Parameters.AddWithValue("@NumeroDocumento", NumeroDocumento);
                    cmd.CommandType = CommandType.Text;
                    oconexion.Open();

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                       while (reader.Read())
                        {
                            obj.IdVenta = Convert.ToInt32(reader["IdVenta"]);
                            obj.oUsuario = new Usuario() {NombreCompleto = reader["NombreCompleto"].ToString() };
                            obj.DocumentoCliente = reader["DocumentoCliente"].ToString();
                            obj.NombreCliente = reader["NombreCliente"].ToString();
                            obj.TipoDocumento = reader["TipoDocumento"].ToString();
                            obj.NumeroDocumento = reader["NumeroDocumento"].ToString();
                            obj.MontoPago = Convert.ToDecimal(reader["MontoPago"]);
                            obj.MontoCambio = Convert.ToDecimal(reader["MontoCambio"]);
                            obj.MontoTotal = Convert.ToDecimal(reader["MontoTotal"]);
                            obj.FechaRegistro = reader["FechaRegistro"].ToString();
                        }
                    }
                }
                catch(Exception ex)
                {
                    obj = new Venta(); // si llega aqui es porque hubo un error y retornamos un objeto vacio.
                    MessageBox.Show(ex.Message);
                }
            }
            return obj;
        }

        public List<Detalle_Venta> ObtenerDetalleVenta(int IdVenta) 
        {
            List<Detalle_Venta> oLista = new List<Detalle_Venta>();
            using (SqlConnection oconexion = new SqlConnection(Conexion.cadena))
            {
                try
                {
                    StringBuilder query = new StringBuilder();
                    query.AppendLine("select p.IdProducto, p.Nombre, p.PrecioCompra, dv.PrecioVenta, dv.Cantidad, dv.SubTotal");
                    query.AppendLine("from DETALLE_VENTA dv inner join PRODUCTO p on p.IdProducto");
                    query.AppendLine("= dv.IdProducto where dv.IdVenta = @IdVenta");

                    SqlCommand cmd = new SqlCommand(query.ToString(), oconexion);
                    cmd.Parameters.AddWithValue("@IdVenta", IdVenta);
                    cmd.CommandType = CommandType.Text;
                    oconexion.Open();

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            oLista.Add(new Detalle_Venta()
                            {
                                oProducto = new Producto() { 
                                IdProducto = Convert.ToInt32(reader["IdProducto"]),
                                NombreProducto = reader["Nombre"].ToString(),
                                PrecioCompra = Convert.ToDecimal(reader["PrecioCompra"])},
                                PrecioVenta = Convert.ToDecimal(reader["PrecioVenta"]),
                                Cantidad = Convert.ToInt32(reader["Cantidad"]),
                                SubTotal = Convert.ToDecimal(reader["SubTotal"])
                            });
                        }
                    }
                }
                catch(Exception ex)
                {
                    oLista = new List<Detalle_Venta>();
                    MessageBox.Show(ex.Message);
                }
            }
            return oLista;
        }
    }

}
