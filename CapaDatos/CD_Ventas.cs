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


        public bool RegistrarVenta(Venta obj, DataTable DetalleVenta, CuentaPorCobrar oCuenta, out string Mensaje)
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

                    // --- Biomoneda ---
                    cmd.Parameters.AddWithValue("MontoPago", obj.MontoPago);
                    cmd.Parameters.AddWithValue("MontoCambio", obj.MontoCambio);
                    cmd.Parameters.AddWithValue("MontoTotal", obj.MontoTotal);
                    cmd.Parameters.AddWithValue("MontoBs", obj.MontoBs);
                    cmd.Parameters.AddWithValue("TasaCambio", obj.TasaCambio);
                    cmd.Parameters.AddWithValue("TipoMoneda", obj.TipoMoneda);

                    SqlParameter t_detalle = cmd.Parameters.AddWithValue("DetalleVenta", DetalleVenta);
                    t_detalle.SqlDbType = SqlDbType.Structured;
                    t_detalle.TypeName = "dbo.EDetalle_Venta";

                    // --- NUEVOS PARAMETROS PARA PLAN DE PAGO ---
                    if (oCuenta != null)
                    {
                        // Si hay crédito, mandamos los datos
                        // Convertimos la fecha string a DateTime para SQL
                        cmd.Parameters.AddWithValue("FechaVencimiento", Convert.ToDateTime(oCuenta.FechaVencimiento));
                        cmd.Parameters.AddWithValue("DescripcionPlan", oCuenta.DescripcionPlan);
                        cmd.Parameters.AddWithValue("PorcentajeMora", oCuenta.PorcentajeMora);
                    }
                    else
                    {
                        // Si es contado, mandamos NULL
                        cmd.Parameters.AddWithValue("FechaVencimiento", DBNull.Value);
                        cmd.Parameters.AddWithValue("DescripcionPlan", DBNull.Value);
                        cmd.Parameters.AddWithValue("PorcentajeMora", 0);
                    }

                    cmd.Parameters.Add("Resultado", SqlDbType.Bit).Direction = ParameterDirection.Output;
                    cmd.Parameters.Add("Mensaje", SqlDbType.VarChar, 500).Direction = ParameterDirection.Output;

                    cmd.CommandType = CommandType.StoredProcedure;
                    oconexion.Open();
                    cmd.ExecuteNonQuery();

                    Resultado = Convert.ToBoolean(cmd.Parameters["Resultado"].Value);
                    Mensaje = cmd.Parameters["Mensaje"].Value.ToString();
                }
            }
            catch (Exception ex)
            {
                Resultado = false;
                Mensaje = ex.Message;
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
                    // Asegúrate de tener esta coma aquí despues de TasaCambio
                    query.AppendLine("v.MontoPago, v.MontoCambio, v.MontoTotal, v.MontoBs, v.TasaCambio, v.TipoMoneda,");
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
                        if (reader.Read())
                        {
                            obj = new Venta()
                            {
                                IdVenta = Convert.ToInt32(reader["IdVenta"]),
                                oUsuario = new Usuario() { NombreCompleto = reader["NombreCompleto"].ToString() },
                                DocumentoCliente = reader["DocumentoCliente"].ToString(),
                                NombreCliente = reader["NombreCliente"].ToString(),
                                TipoDocumento = reader["TipoDocumento"].ToString(),
                                NumeroDocumento = reader["NumeroDocumento"].ToString(),
                                MontoPago = Convert.ToDecimal(reader["MontoPago"]),
                                MontoCambio = Convert.ToDecimal(reader["MontoCambio"]),
                                MontoTotal = Convert.ToDecimal(reader["MontoTotal"]),
                                MontoBs = Convert.ToDecimal(reader["MontoBs"]),
                                TasaCambio = Convert.ToDecimal(reader["TasaCambio"]),
                                TipoMoneda = reader["TipoMoneda"].ToString(),
                                FechaRegistro = reader["FechaRegistro"].ToString()
                            };
                        }
                    }

                    // --- ESTO ES LO QUE FALTA SI EL GRID SALE VACÍO O EN 0 ---
                    if (obj.IdVenta != 0)
                    {
                        obj.oDetalleVenta = ObtenerDetalleVenta(obj.IdVenta);
                    }
                }
                catch (Exception ex)
                {
                    obj = new Venta();
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
                    query.AppendLine("select p.IdProducto, p.Nombre, p.PrecioCompra, dv.PrecioVenta, dv.Cantidad, dv.SubTotal, dv.SubTotalBs");
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
                                SubTotal = Convert.ToDecimal(reader["SubTotal"]),
                                SubTotalBs = Convert.ToDecimal(reader["SubTotalBs"])
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

        public List<Venta> ListarVentasResumen()
        {
            List<Venta> lista = new List<Venta>();
            using (SqlConnection oconexion = new SqlConnection(Conexion.cadena))
            {
                try
                {
                    StringBuilder query = new StringBuilder();
                    // 1. Agregamos TipoMoneda al Select
                    query.AppendLine("select NumeroDocumento, NombreCliente, MontoTotal, MontoBs, TasaCambio, TipoMoneda, FechaRegistro from VENTA order by IdVenta desc");

                    SqlCommand cmd = new SqlCommand(query.ToString(), oconexion);
                    cmd.CommandType = CommandType.Text;
                    oconexion.Open();

                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            lista.Add(new Venta()
                            {
                                NumeroDocumento = dr["NumeroDocumento"].ToString(),
                                NombreCliente = dr["NombreCliente"].ToString(),
                                MontoTotal = Convert.ToDecimal(dr["MontoTotal"]),
                                MontoBs = dr["MontoBs"] != DBNull.Value ? Convert.ToDecimal(dr["MontoBs"]) : 0,
                                // 2. Mapeamos la nueva bandera
                                TipoMoneda = dr["TipoMoneda"].ToString(),
                                FechaRegistro = dr["FechaRegistro"] != DBNull.Value ?
                                    Convert.ToDateTime(dr["FechaRegistro"]).ToString("dd/MM/yyyy") : ""
                            });
                        }
                    }
                }
                catch { lista = new List<Venta>(); }
            }
            return lista;
        }
    }

}
