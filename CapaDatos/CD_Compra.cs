using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Linq.Expressions;
using System.Net.Http.Headers;
using System.Data;
using CapaEntidad;

namespace CapaDatos
{
    public class CD_Compra
    {
        public int ObtenerCorrelativo()
        {
            int idCorrelativo = 0;

            using (SqlConnection oconexion = new SqlConnection(Conexion.cadena))
            {

                try
                {
                    StringBuilder query = new StringBuilder();
                    query.AppendLine("select count (*) + 1 from COMPRA");

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

        public bool RegistrarCompra(Compra obj, DataTable DetalleCompra, out string Mensaje)
        {
            bool Resultado = false;
            Mensaje = string.Empty;
            using (SqlConnection oconexion = new SqlConnection(Conexion.cadena))
            {

                try
                {


                    SqlCommand cmd = new SqlCommand("SP_RegistrarCompra", oconexion);
                    cmd.Parameters.AddWithValue("IdUsuario", obj.oUsuario.IdUsuario);
                    cmd.Parameters.AddWithValue("IdProveedor", obj.oProveedor.IdProveedor);
                    cmd.Parameters.AddWithValue("TipoDocumento", obj.TipoDocumento);
                    cmd.Parameters.AddWithValue("NumeroDocumento", obj.NumeroDocumento);
                    cmd.Parameters.AddWithValue("MontoTotal", obj.MontoTotal);
                    cmd.Parameters.AddWithValue("EsCompraEnBs", obj.EsCompraEnBs);
                    cmd.Parameters.AddWithValue("TasaCambio", obj.TasaCambio);

                    cmd.Parameters.AddWithValue("DetalleCompra", DetalleCompra);
                    cmd.Parameters.Add("Resultado", SqlDbType.Int).Direction = ParameterDirection.Output;
                    cmd.Parameters.Add("Mensaje", SqlDbType.VarChar, 500).Direction = ParameterDirection.Output;

                    cmd.CommandType = CommandType.StoredProcedure;
                    oconexion.Open();
                    cmd.ExecuteNonQuery();

                    Resultado = Convert.ToBoolean(cmd.Parameters["Resultado"].Value);
                    Mensaje = cmd.Parameters["Mensaje"].Value.ToString();

                }
                catch (Exception ex)
                {
                    Resultado = false;
                    Mensaje = ex.Message;
                }
            }

            return Resultado;
        }


        public Compra ObtenerCompra(string NumeroDocumento)
        {
            Compra obj = null;
            using (SqlConnection oconexion = new SqlConnection(Conexion.cadena))
            {
                try
                {
                    StringBuilder query = new StringBuilder();

                    
                    query.AppendLine("select c.IdCompra, u.NombreCompleto, pr.Documento, pr.RazonSocial, c.TipoDocumento,");
                    query.AppendLine("c.NumeroDocumento, c.MontoTotal, c.EsCompraEnBs, c.TasaCambio, CONVERT(char(10),c.FechaRegistro,103)[FehcaRegistro]");
                    query.AppendLine("from COMPRA c");
                    query.AppendLine("inner join USUARIO u on u.IdUsuario = c.IdUsuario");
                    query.AppendLine("inner join PROVEEDOR pr on pr.IdProveedor = c.IdProveedor");
                    query.AppendLine("where c.NumeroDocumento = @NumeroDocumento");

                    SqlCommand cmd = new SqlCommand(query.ToString(), oconexion);
                    cmd.Parameters.AddWithValue("@NumeroDocumento", NumeroDocumento);
                    cmd.CommandType = CommandType.Text;
                    oconexion.Open();
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        if (dr.Read())
                        {
                            obj = new Compra()
                            {
                                IdCompra = Convert.ToInt32(dr["IdCompra"]),
                                oUsuario = new Usuario() { NombreCompleto = dr["NombreCompleto"].ToString() },
                                oProveedor = new Proveedor() { Documento = dr["Documento"].ToString(), RazonSocial = dr["RazonSocial"].ToString() },
                                TipoDocumento = dr["TipoDocumento"].ToString(),
                                NumeroDocumento = dr["NumeroDocumento"].ToString(),
                                MontoTotal = Convert.ToDecimal(dr["MontoTotal"]),
                                FechaRegistro = dr["FehcaRegistro"].ToString(),
                 
                                EsCompraEnBs = Convert.ToBoolean(dr["EsCompraEnBs"]),
                                TasaCambio = Convert.ToDecimal(dr["TasaCambio"])
                            };
                        }
                    }
                }
                catch (Exception ex) // Agregué 'ex' para que puedas ver errores si depuras
                {
                    obj = null;
                    
                }
            }
            return obj;
        }

        public List<Detalle_Compra> ObtenerDetalleCompra(int IdCompra)
        {
            List<Detalle_Compra> oLista = new List<Detalle_Compra>();
            using (SqlConnection oconexion = new SqlConnection(Conexion.cadena))
            {
                try
                {
                    StringBuilder query = new StringBuilder();
                    query.AppendLine("select p.Nombre, dc.PrecioCompra, dc.PrecioVenta,");
                    query.AppendLine("dc.Cantidad, dc.MontoTotal from DETALLE_COMPRA dc");
                    query.AppendLine("inner join PRODUCTO p on p.IdProducto = dc.IdProducto ");
                    query.AppendLine("where dc.IdCompra = @IdCompra");
                    SqlCommand cmd = new SqlCommand(query.ToString(), oconexion);
                    cmd.Parameters.AddWithValue("@IdCompra", IdCompra);
                    cmd.CommandType = CommandType.Text;
                    oconexion.Open();
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            oLista.Add(new Detalle_Compra()
                            {
                                oProducto = new Producto() { NombreProducto = dr["Nombre"].ToString() },
                                PrecioCompra = Convert.ToDecimal(dr["PrecioCompra"]),
                                PrecioVenta = Convert.ToDecimal(dr["PrecioVenta"]),
                                Cantidad = Convert.ToInt32(dr["Cantidad"]),
                                MontoTotal = Convert.ToDecimal(dr["MontoTotal"])

                            });
                        }
                    }
                }
                catch
                {
                    oLista = new List<Detalle_Compra>();
                }
            }
            return oLista;



        }




        public List<Compra> ListarCompras()
        {
            List<Compra> lista = new List<Compra>();
            using (SqlConnection oconexion = new SqlConnection(Conexion.cadena))
            {
                try
                {
                    StringBuilder query = new StringBuilder();

                    // SQL ROBUSTO:
                    query.AppendLine("select c.IdCompra, c.NumeroDocumento, p.RazonSocial, c.MontoTotal, c.EsCompraEnBs, c.TasaCambio, convert(char(10),c.FechaRegistro,103)[FechaRegistro]");
                    query.AppendLine("from COMPRA c");
                    query.AppendLine("inner join PROVEEDOR p on p.IdProveedor = c.IdProveedor");

                    // 1. FILTRO SQL: Ignoramos las filas donde la configuración de moneda sea NULA
                    // Esto cumple con "no tomarlas en cuenta"
                    query.AppendLine("WHERE c.EsCompraEnBs IS NOT NULL");

                    query.AppendLine("order by c.IdCompra desc");

                    SqlCommand cmd = new SqlCommand(query.ToString(), oconexion);
                    cmd.CommandType = CommandType.Text;
                    oconexion.Open();

                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            lista.Add(new Compra()
                            {
                                IdCompra = Convert.ToInt32(dr["IdCompra"]),
                                NumeroDocumento = dr["NumeroDocumento"].ToString(),

                                // 2. VALIDACIÓN C#: Usamos lógica condicional para evitar crashes si llega un NULL
                                MontoTotal = dr["MontoTotal"] != DBNull.Value ? Convert.ToDecimal(dr["MontoTotal"]) : 0,
                                EsCompraEnBs = dr["EsCompraEnBs"] != DBNull.Value && Convert.ToBoolean(dr["EsCompraEnBs"]), // Si es null, asume falso
                                TasaCambio = dr["TasaCambio"] != DBNull.Value ? Convert.ToDecimal(dr["TasaCambio"]) : 0,

                                FechaRegistro = dr["FechaRegistro"].ToString(),
                                oProveedor = new Proveedor()
                                {
                                    RazonSocial = dr["RazonSocial"].ToString()
                                }
                            });
                        }
                    }
                }
                catch (Exception ex)
                {
                    // En caso de error, devolvemos la lista vacía para no romper el programa
                    lista = new List<Compra>();
                }
            }
            return lista;
        }
    }

}


