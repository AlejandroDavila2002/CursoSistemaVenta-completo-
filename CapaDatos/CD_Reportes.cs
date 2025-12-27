using CapaEntidad;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CapaDatos
{
    public class CD_Reportes
    {
        public List<ReporteCompras> compras(string FechaInicio, string fechaFin, int IdProveedor)
        {
            List<ReporteCompras> lista = new List<ReporteCompras>();

            using (SqlConnection oconexion = new SqlConnection(Conexion.cadena))
            {
                try
                {
                  

                    SqlCommand cmd = new SqlCommand("SP_ReportesCompra", oconexion);
                    cmd.Parameters.AddWithValue("@FechaInicio", FechaInicio);
                    cmd.Parameters.AddWithValue("@fechaFin", fechaFin);
                    cmd.Parameters.AddWithValue("@IdProveedor", IdProveedor);
                    cmd.CommandType = CommandType.StoredProcedure;

                    oconexion.Open();

                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        
                        while (dr.Read())
                        {
                            lista.Add(new ReporteCompras()
                            {
                                FechaRegistro = dr["FechaRegistro"].ToString(),
                                TipoDocumento = dr["TipoDocumento"].ToString(),
                                NumeroDocumento = dr["NumeroDocumento"].ToString(),
                                MontoTotal = dr["MontoTotal"].ToString(),
                                UsuarioRegistro = dr["UsuarioRegistro"].ToString(),
                                DocumentoProveedor = dr["DocumentoProveedor"].ToString(),
                                RazonSocial = dr["RazonSocial"].ToString(),
                                CodigoProducto = dr["CodigoProducto"].ToString(),
                                NombreProducto = dr["NombreProducto"].ToString(),
                                Categoria = dr["Categoria"].ToString(),
                                PrecioCompra = dr["PrecioCompra"].ToString(),
                                PrecioVenta = dr["PrecioVenta"].ToString(),
                                Cantidad = dr["Cantidad"].ToString(),
                                SubTotal = dr["SubTotal"].ToString(),
                            });
                        }
                    }
                }
                catch (Exception ex)
                {
                   
                    lista = new List<ReporteCompras>();
                }
            }

            return lista;
        }

        public List<ReporteVenta> Venta(string FechaInicio, string fechaFin)
        {
            List<ReporteVenta> lista = new List<ReporteVenta>();

            using (SqlConnection oconexion = new SqlConnection(Conexion.cadena))
            {
                try
                {
                    SqlCommand cmd = new SqlCommand("SP_ReportesVenta", oconexion);
                    cmd.Parameters.AddWithValue("@FechaInicio", FechaInicio);
                    cmd.Parameters.AddWithValue("@fechaFin", fechaFin);
                    cmd.CommandType = CommandType.StoredProcedure;

                    oconexion.Open();

                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            lista.Add(new ReporteVenta()
                            {
                                FechaRegistro = dr["FechaRegistro"].ToString(),
                                TipoDocumento = dr["TipoDocumento"].ToString(),
                                NumeroDocumento = dr["NumeroDocumento"].ToString(),
                                MontoTotal = dr["MontoTotal"].ToString(),
                                UsuarioRegistro = dr["UsuarioRegistro"].ToString(),
                                DocumentoCliente = dr["DocumentoCliente"].ToString(),
                                NombreCliente = dr["NombreCliente"].ToString(),
                                CodigoProducto = dr["CodigoProducto"].ToString(),
                                NombreProducto = dr["NombreProducto"].ToString(),
                                Categoria = dr["Categoria"].ToString(),
                                PrecioVenta = dr["PrecioVenta"].ToString(),
                                Cantidad = dr["Cantidad"].ToString(),
                                SubTotal = dr["SubTotal"].ToString(),
                            });
                        }
                    }
                }
                catch (Exception ex)
                {
                    System.Windows.Forms.MessageBox.Show("Error en SQL: " + ex.Message);
                    lista = new List<ReporteVenta>();
                }
            }

            return lista;
        }

     
        public List<ReporteInventario> ObtenerInventario()
        {
            List<ReporteInventario> lista = new List<ReporteInventario>();
            using (SqlConnection oconexion = new SqlConnection(Conexion.cadena))
            { // Usa tu cadena de conexión
                try
                {
                    SqlCommand cmd = new SqlCommand("SP_REPORTE_INVENTARIO", oconexion);
                    cmd.CommandType = CommandType.StoredProcedure;
                    oconexion.Open();
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            lista.Add(new ReporteInventario()
                            {
                                Codigo = dr["Codigo"].ToString(),
                                Producto = dr["Producto"].ToString(),
                                Categoria = dr["Categoria"].ToString(),
                                Stock = Convert.ToInt32(dr["Stock"]),
                                CostoUnitario = Convert.ToDecimal(dr["CostoUnitario"]),
                                TotalCosto = Convert.ToDecimal(dr["TotalCosto"]),
                                PrecioVenta = Convert.ToDecimal(dr["PrecioVenta"]),
                                TotalVenta = Convert.ToDecimal(dr["TotalVenta"]),
                                Estado = Convert.ToBoolean(dr["Estado"]) ? "Activo" : "No Activo"
                            });
                        }
                    }
                }
                catch (Exception ex)
                {
                    lista = new List<ReporteInventario>();
                }
            }
            return lista;
        }



    }

}
