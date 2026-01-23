using CapaEntidad;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;


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
        // Dentro de CapaDatos/CD_FlujoCaja.cs

        public Dictionary<string, decimal> ObtenerResumenFinanciero(string fechaInicio, string fechaFin)
        {
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
                    StringBuilder query = new StringBuilder();

                    // 1. INGRESOS POR VENTAS (MontoPago con conversión según TipoMoneda)
                    query.AppendLine("DECLARE @DineroVentas DECIMAL(18,2) = (");
                    query.AppendLine("    SELECT ISNULL(SUM(");
                    query.AppendLine("       CASE ");
                    // Si la venta fue en Bolívares (VES), convertimos a Dólares
                    query.AppendLine("           WHEN TipoMoneda = 'VES' THEN (MontoPago / ISNULL(NULLIF(TasaCambio, 0), 1))");
                    // Si fue en USD, tomamos el pago directo
                    query.AppendLine("           ELSE MontoPago ");
                    query.AppendLine("       END");
                    query.AppendLine("    ), 0) FROM VENTA");
                    query.AppendLine("    WHERE CONVERT(DATE, FechaRegistro) BETWEEN @FInicio AND @FFin");
                    query.AppendLine(");");

                    // 2. INGRESOS POR ABONOS (Se mantiene igual, asumiendo que el abono se registra ya convertido o en moneda base)
                    // Si tus abonos también tienen moneda, habría que aplicar lógica similar. Por ahora lo dejamos estándar.
                    query.AppendLine("DECLARE @DineroAbonos DECIMAL(18,2) = (");
                    query.AppendLine("    SELECT ISNULL(SUM(Monto), 0) FROM ABONO");
                    query.AppendLine("    WHERE CONVERT(DATE, FechaRegistro) BETWEEN @FInicio AND @FFin");
                    query.AppendLine(");");

                    // 3. EGRESOS POR COMPRAS (Mercancía) - Lógica inteligente (Mantenida del paso anterior)
                    query.AppendLine("DECLARE @EgresosCompras DECIMAL(18,2) = (");
                    query.AppendLine("    SELECT ISNULL(SUM(");
                    query.AppendLine("       CASE ");
                    // Si EsCompraEnBs es 1 (True), convertimos a Dólares
                    query.AppendLine("           WHEN EsCompraEnBs = 1 THEN (MontoTotal / ISNULL(NULLIF(TasaCambio, 0), 1))");
                    // Si es 0 (USD), tomamos el total directo
                    query.AppendLine("           ELSE MontoTotal ");
                    query.AppendLine("       END");
                    query.AppendLine("    ), 0) FROM COMPRA");
                    query.AppendLine("    WHERE CONVERT(DATE, FechaRegistro) BETWEEN @FInicio AND @FFin");
                    query.AppendLine(");");

                    // 4. GASTOS OPERATIVOS
                    query.AppendLine("DECLARE @GastosOperativos DECIMAL(18,2) = (");
                    query.AppendLine("    SELECT ISNULL(SUM(Monto), 0) FROM GASTO");
                    query.AppendLine("    WHERE CONVERT(DATE, FechaRegistro) BETWEEN @FInicio AND @FFin");
                    query.AppendLine(");");

                    // RESULTADO FINAL
                    query.AppendLine("SELECT ");
                    query.AppendLine("(@DineroVentas + @DineroAbonos) AS TotalIngresosReal,");
                    query.AppendLine("@EgresosCompras AS TotalCompras,");
                    query.AppendLine("@GastosOperativos AS TotalGastos");

                    SqlCommand cmd = new SqlCommand(query.ToString(), oconexion);
                    cmd.Parameters.AddWithValue("@FInicio", fechaInicio);
                    cmd.Parameters.AddWithValue("@FFin", fechaFin);
                    cmd.CommandType = CommandType.Text;

                    oconexion.Open();
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        if (dr.Read())
                        {
                            resumen["TotalIngresos"] = dr["TotalIngresosReal"] == DBNull.Value ? 0m : Convert.ToDecimal(dr["TotalIngresosReal"]);
                            resumen["TotalEgresosMercancia"] = dr["TotalCompras"] == DBNull.Value ? 0m : Convert.ToDecimal(dr["TotalCompras"]);
                            resumen["TotalGastosOperativos"] = dr["TotalGastos"] == DBNull.Value ? 0m : Convert.ToDecimal(dr["TotalGastos"]);
                        }
                    }
                }
                catch (Exception ex)
                {
                    // Manejo de errores
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


        // 1. MÉTODO PARA OBTENER LAS VENTAS QUE SE VAN A CERRAR (Con cálculo USD incluido)
        public DataTable ObtenerVentasParaCierre(string fechaInicio, string fechaFin)
        {
            DataTable dt = new DataTable();
            // Definimos las columnas que espera nuestro Type SQL [EDetalleCierreVenta]
            dt.Columns.Add("IdVenta", typeof(int));
            dt.Columns.Add("MontoOriginal", typeof(decimal));
            dt.Columns.Add("MonedaOriginal", typeof(string));
            dt.Columns.Add("TasaUtilizada", typeof(decimal));
            dt.Columns.Add("MontoCalculadoUSD", typeof(decimal));

            using (SqlConnection oconexion = new SqlConnection(Conexion.cadena))
            {
                try
                {
                    StringBuilder query = new StringBuilder();
                    // Esta consulta replica tu lógica de conversión para obtener fila por fila
                    query.AppendLine("SELECT IdVenta, MontoPago, TipoMoneda, TasaCambio,");
                    query.AppendLine("CASE");
                    query.AppendLine("   WHEN TipoMoneda = 'VES' THEN (MontoPago / ISNULL(NULLIF(TasaCambio,0),1))");
                    query.AppendLine("   ELSE MontoPago");
                    query.AppendLine("END as MontoUSD");
                    query.AppendLine("FROM VENTA");
                    query.AppendLine("WHERE CONVERT(DATE, FechaRegistro) BETWEEN @FInicio AND @FFin");

                    SqlCommand cmd = new SqlCommand(query.ToString(), oconexion);
                    cmd.Parameters.AddWithValue("@FInicio", fechaInicio);
                    cmd.Parameters.AddWithValue("@FFin", fechaFin);
                    cmd.CommandType = CommandType.Text;

                    oconexion.Open();
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            // Llenamos el DataTable con los resultados
                            dt.Rows.Add(
                                Convert.ToInt32(dr["IdVenta"]),
                                Convert.ToDecimal(dr["MontoPago"]),
                                dr["TipoMoneda"].ToString(),
                                Convert.ToDecimal(dr["TasaCambio"]),
                                Convert.ToDecimal(dr["MontoUSD"])
                            );
                        }
                    }
                }
                catch (Exception ex)
                {
                    dt = new DataTable(); // Retornar vacío en error
                }
            }
            return dt;
        }

        // 2. MÉTODO PARA REGISTRAR EL CIERRE (Recibe el Objeto y el DataTable de ventas)
        public bool RegistrarCierre(CierreCaja obj, DataTable dtDetalle, out string Mensaje)
        {
            bool respuesta = false;
            Mensaje = string.Empty;

            using (SqlConnection oconexion = new SqlConnection(Conexion.cadena))
            {
                try
                {
                    SqlCommand cmd = new SqlCommand("SP_RegistrarCierreCompleto", oconexion);
                    cmd.Parameters.AddWithValue("IdUsuario", obj.oUsuario.IdUsuario);
                    cmd.Parameters.AddWithValue("MontoTeorico", obj.MontoTeorico);
                    cmd.Parameters.AddWithValue("MontoReal", obj.MontoReal);
                    cmd.Parameters.AddWithValue("Observacion", obj.Observacion);

                    // AQUÍ PASAMOS LA LISTA DE VENTAS AL SQL
                    cmd.Parameters.AddWithValue("DetalleVentas", dtDetalle);

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