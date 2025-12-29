using CapaEntidad;
using CapaNegocio;
using CapaPresentacion.utilidades;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ClosedXML.Excel;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.tool.xml;
using System.IO;

namespace CapaPresentacion
{
    public partial class frmReporteInventario : Form
    {
        public frmReporteInventario()
        {
            InitializeComponent();
        }

        private void frmReporteInventario_Load(object sender, EventArgs e)
        {
            // 1. Cargar combo de Categorías (usando tu CN_Categoria existente)
            List<Categoria> listaCategoria = new CN_Categoria().listar();
            cboBusquedaCategorias.Items.Add(new OpcionCombo() { Valor = 0, Texto = "Todas las Categorías" });
            foreach (Categoria item in listaCategoria)
            {
                cboBusquedaCategorias.Items.Add(new OpcionCombo() { Valor = item.IdCategoria, Texto = item.Descripcion });
            }
            cboBusquedaCategorias.DisplayMember = "Texto";
            cboBusquedaCategorias.ValueMember = "Valor";
            cboBusquedaCategorias.SelectedIndex = 0;

            // 2. Cargar combo de Filtro por Stock
            cboBusquedaPorStock.Items.Add("Todos los Stocks");
            cboBusquedaPorStock.Items.Add("Stock Bajo (< 4)");
            cboBusquedaPorStock.Items.Add("Agotados (0)");
            cboBusquedaPorStock.SelectedIndex = 0;

            // 3. Cargar los datos por defecto
            CargarDatosReporte();
        }

        private void CargarDatosReporte()
        {
            // Obtenemos la lista desde la Capa de Negocio
            List<ReporteInventario> lista = new CN_Reporte().ObtenerInventario();

            // Limpiamos los 4 DataGridViews antes de llenar
            dgvDataProducto.Rows.Clear();
            dgvDataCosto.Rows.Clear();
            dvgDataVenta.Rows.Clear();
            dgvDataAccion.Rows.Clear();

            // Variables para los contadores de los GroupBox
            decimal sumCostoTotal = 0;
            decimal sumVentaTotal = 0;
            int contadorBajo = 0;
            int contadorAgotado = 0;

            foreach (ReporteInventario item in lista)
            {
                // 1. Llenar dgvDataProducto
                dgvDataProducto.Rows.Add(new object[] { "", item.Codigo, item.Producto, item.Stock });

                // 2. Llenar dgvDataCosto
                dgvDataCosto.Rows.Add(new object[] { item.CostoUnitario.ToString("N2"), item.TotalCosto.ToString("N2") });

                // 3. Llenar dvgDataVenta
                dvgDataVenta.Rows.Add(new object[] { item.PrecioVenta.ToString("N2"), item.TotalVenta.ToString("N2") });

                // 4. Llenar dgvDataAccion
                dgvDataAccion.Rows.Add(new object[] { item.Estado, "VER DETALLE" });

                // --- CÁLCULOS PARA LOS GROUPBOX ---
                sumCostoTotal += item.TotalCosto;
                sumVentaTotal += item.TotalVenta;

                if (item.Stock == 0) contadorAgotado++;
                else if (item.Stock < 4) contadorBajo++;
            }

            // --- ACTUALIZAR LABELS EN VIVO ---
            totalproductos.Text = lista.Count.ToString(); // groupBox1
            valorInventario_Costo.Text = sumCostoTotal.ToString("N2"); // groupBox2
            valorInventario_Venta.Text = sumVentaTotal.ToString("N2"); // groupBox3
            ProductosBajo.Text = contadorBajo.ToString(); // groupBox4
            ProductosAgotados.Text = contadorAgotado.ToString(); // groupBox5
        }

        private void dgvDataProducto_Scroll(object sender, ScrollEventArgs e)
        {
            int index = dgvDataProducto.FirstDisplayedScrollingRowIndex;
            if (index >= 0)
            {
                dgvDataCosto.FirstDisplayedScrollingRowIndex = index;
                dvgDataVenta.FirstDisplayedScrollingRowIndex = index;
                dgvDataAccion.FirstDisplayedScrollingRowIndex = index;
            }
        }

        private void btnLimpiarCombo_AutoSizeChanged(object sender, EventArgs e)
        {
            txtBusqueda.Text = "";
            cboBusquedaCategorias.SelectedIndex = 0;
            cboBusquedaPorStock.SelectedIndex = 0;

            foreach (DataGridViewRow row in dgvDataProducto.Rows)
            {
                row.Visible = true;
                dgvDataCosto.Rows[row.Index].Visible = true;
                dvgDataVenta.Rows[row.Index].Visible = true;
                dgvDataAccion.Rows[row.Index].Visible = true;
            }

            CalcularResumenFiltrado();
        }

        private void btnBuscar_Click(object sender, EventArgs e)
        {
            string busquedaTexto = txtBusqueda.Text.Trim().ToUpper();
            string categoriaSeleccionada = ((OpcionCombo)cboBusquedaCategorias.SelectedItem).Texto.ToUpper();
            string filtroStock = cboBusquedaPorStock.SelectedItem.ToString();

            // Recorremos las filas de la tabla principal (dgvDataProducto)
            foreach (DataGridViewRow row in dgvDataProducto.Rows)
            {
                // 1. Validar Filtro de Texto (en la columna PRODUCTO)
                bool cumpleTexto = row.Cells["PRODUCTO"].Value.ToString().ToUpper().Contains(busquedaTexto);

                // 2. Validar Filtro de Categoría (necesitas que la entidad o el SP devuelva la categoría)
                // Nota: Si no tienes la columna invisible "Categoria" en el dgv, se asume que cumple para este ejemplo
                bool cumpleCategoria = (categoriaSeleccionada == "TODAS LAS CATEGORÍAS") ||
                                       (row.Cells["PRODUCTO"].Tag?.ToString().ToUpper() == categoriaSeleccionada);

                // 3. Validar Filtro de Stock
                int stock = Convert.ToInt32(row.Cells["STOCK"].Value);
                bool cumpleStock = false;

                if (filtroStock == "Todos los Stocks") cumpleStock = true;
                else if (filtroStock == "Stock Bajo (< 4)") cumpleStock = (stock > 0 && stock < 4);
                else if (filtroStock == "Agotados (0)") cumpleStock = (stock == 0);

                // Aplicar visibilidad simultánea a las 4 tablas
                bool filaVisible = cumpleTexto && cumpleStock; // Agrega cumpleCategoria si mapeas la celda

                row.Visible = filaVisible;
                dgvDataCosto.Rows[row.Index].Visible = filaVisible;
                dvgDataVenta.Rows[row.Index].Visible = filaVisible;
                dgvDataAccion.Rows[row.Index].Visible = filaVisible;
            }

            // Después de filtrar, recalculamos los indicadores de los GroupBoxes
            CalcularResumenFiltrado();
        }

        private void CalcularResumenFiltrado()
        {
            decimal totalCosto = 0;
            decimal totalVenta = 0;
            int productosBajo = 0;
            int productosAgotados = 0;
            int totalItems = 0;

            foreach (DataGridViewRow row in dgvDataProducto.Rows)
            {
                if (row.Visible)
                {
                    totalItems++;
                    int stock = Convert.ToInt32(row.Cells["STOCK"].Value);

                    // Obtener valores de las otras tablas por el mismo índice
                    decimal costoFila = Convert.ToDecimal(dgvDataCosto.Rows[row.Index].Cells["TOTALCOSTO"].Value);
                    decimal ventaFila = Convert.ToDecimal(dvgDataVenta.Rows[row.Index].Cells["TOTALVENTA"].Value);

                    totalCosto += costoFila;
                    totalVenta += ventaFila;

                    if (stock == 0) productosAgotados++;
                    else if (stock < 4) productosBajo++;
                }
            }

            // Actualizar la interfaz
            totalproductos.Text = totalItems.ToString();
            valorInventario_Costo.Text = totalCosto.ToString("N2");
            valorInventario_Venta.Text = totalVenta.ToString("N2");
            ProductosBajo.Text = productosBajo.ToString();
            ProductosAgotados.Text = productosAgotados.ToString();
        }


        private void btnExportarExcel_Click(object sender, EventArgs e)
        {
            if (dgvDataProducto.Rows.Count < 1)
            {
                MessageBox.Show("No hay datos para exportar", "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            DataTable dt = new DataTable();
            dt.Columns.Add("Codigo", typeof(string));
            dt.Columns.Add("Producto", typeof(string));
            dt.Columns.Add("Stock", typeof(int));
            dt.Columns.Add("Costo Unit", typeof(string));
            dt.Columns.Add("Total Costo", typeof(decimal)); // Usamos decimal para que Excel sume correctamente
            dt.Columns.Add("Precio Venta", typeof(string));
            dt.Columns.Add("Total Venta", typeof(decimal));

            decimal sumCosto = 0;
            decimal sumVenta = 0;

            foreach (DataGridViewRow row in dgvDataProducto.Rows)
            {
                if (row.Visible)
                {
                    int i = row.Index;
                    decimal tCosto = Convert.ToDecimal(dgvDataCosto.Rows[i].Cells["TOTALCOSTO"].Value);
                    decimal tVenta = Convert.ToDecimal(dvgDataVenta.Rows[i].Cells["TOTALVENTA"].Value);

                    dt.Rows.Add(
                        row.Cells["CODIGO"].Value.ToString(),
                        row.Cells["PRODUCTO"].Value.ToString(),
                        Convert.ToInt32(row.Cells["STOCK"].Value),
                        dgvDataCosto.Rows[i].Cells["COSTOUNT"].Value.ToString(),
                        tCosto,
                        dvgDataVenta.Rows[i].Cells["PRECIOVENTA"].Value.ToString(),
                        tVenta
                    );

                    sumCosto += tCosto;
                    sumVenta += tVenta;
                }
            }

            // Agregamos fila de totales (igual que en los GroupBoxes del sistema)
            dt.Rows.Add("", "TOTALES", null, "", sumCosto, "", sumVenta);

            SaveFileDialog savefile = new SaveFileDialog();
            savefile.FileName = string.Format("Reporte_Inventario_{0}.xlsx", DateTime.Now.ToString("ddMMyyyyHHmmss"));
            savefile.Filter = "Excel Files | *.xlsx";

            if (savefile.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    using (XLWorkbook wb = new XLWorkbook())
                    {
                        var hoja = wb.Worksheets.Add(dt, "Inventario");
                        hoja.Columns().AdjustToContents(); // Ajusta el ancho de las celdas automáticamente
                        wb.SaveAs(savefile.FileName);
                    }
                    MessageBox.Show("Excel generado con éxito", "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message, "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnDescargarPDF_Click(object sender, EventArgs e)
        {
            if (dgvDataProducto.Rows.Count == 0)
            {
                MessageBox.Show("No hay datos para exportar", "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            try
            {
                // 1. Obtener datos del negocio y Logo
                bool obtenido = false;
                CN_DetallesNegocio negocio = new CN_DetallesNegocio();
                byte[] byteImage = negocio.obtenerLogo(out obtenido);
                Detalles_Negocio datosNegocio = negocio.ObtenerDatos();

                // 2. Cargar la plantilla HTML desde Recursos
                string texto_html = string.Empty;
                var recurso = Properties.Resources.PlantillaInventario;

                if (recurso is byte[])
                    texto_html = Encoding.UTF8.GetString((byte[])recurso);
                else
                    texto_html = recurso.ToString();

                // 3. Preparar el logo en Base64
                string logoFullSource = "";
                if (obtenido && byteImage != null && byteImage.Length > 0)
                {
                    string base64String = Convert.ToBase64String(byteImage).Replace("\n", "").Replace("\r", "");
                    logoFullSource = "data:image/png;base64," + base64String;
                }

                // 4. Generar dinámicamente las filas con colores de fondo
                StringBuilder filas = new StringBuilder();
                foreach (DataGridViewRow row in dgvDataProducto.Rows)
                {
                    if (row.Visible)
                    {
                        int i = row.Index;

                        // Lógica de validación de Stock para asignar la clase CSS a la fila
                        int stock = Convert.ToInt32(row.Cells["STOCK"].Value);
                        // Definimos un umbral para stock bajo (puedes ajustarlo o traerlo de la DB)
                        int stockMinimo = 10;

                        string claseFila = "";
                        if (stock <= 0)
                        {
                            claseFila = "class='stock-agotado'"; // Fondo Rojo
                        }
                        else if (stock <= stockMinimo)
                        {
                            claseFila = "class='stock-bajo'";    // Fondo Amarillo
                        }

                        // Construcción de la fila HTML
                        filas.Append($"<tr {claseFila}>");
                        filas.Append($"<td class='text-center'>{row.Cells["CODIGO"].Value}</td>");
                        filas.Append($"<td>{row.Cells["PRODUCTO"].Value}</td>");
                        filas.Append($"<td class='text-center'>{stock}</td>");
                        filas.Append($"<td class='text-right'>{dgvDataCosto.Rows[i].Cells["COSTOUNT"].Value}</td>");
                        filas.Append($"<td class='text-right'>{dgvDataCosto.Rows[i].Cells["TOTALCOSTO"].Value}</td>");
                        filas.Append($"<td class='text-right'>{dvgDataVenta.Rows[i].Cells["PRECIOVENTA"].Value}</td>");
                        filas.Append($"<td class='text-right'>{dvgDataVenta.Rows[i].Cells["TOTALVENTA"].Value}</td>");
                        filas.Append("</tr>");
                    }
                }

                // 5. Reemplazos en la plantilla
                texto_html = texto_html.Replace("{{logo}}", logoFullSource)
                                       .Replace("{{nombrenegocio}}", datosNegocio.Nombre.ToUpper())
                                       .Replace("{{rifnegocio}}", datosNegocio.RUC)
                                       .Replace("{{direccionnegocio}}", datosNegocio.Direccion)
                                       .Replace("{{fecha}}", DateTime.Now.ToString("dd/MM/yyyy"))
                                       .Replace("{{filas}}", filas.ToString())
                                       .Replace("{{totalprods}}", totalproductos.Text)
                                       .Replace("{{vcosto}}", valorInventario_Costo.Text)
                                       .Replace("{{vventa}}", valorInventario_Venta.Text)
                                       .Replace("{{sbajo}}", ProductosBajo.Text)
                                       .Replace("{{sagotado}}", ProductosAgotados.Text);

                // 6. Configuración de Guardado y Generación del PDF
                SaveFileDialog savefile = new SaveFileDialog();
                savefile.FileName = $"ReporteInventario_{DateTime.Now:ddMMyyyy_HHmm}.pdf";
                savefile.Filter = "Pdf Files|*.pdf";

                if (savefile.ShowDialog() == DialogResult.OK)
                {
                    using (FileStream stream = new FileStream(savefile.FileName, FileMode.Create))
                    {
                        // A4 Horizontal para que quepa bien la tabla valorizada
                        Document pdfDoc = new Document(PageSize.A4.Rotate(), 25, 25, 25, 25);
                        PdfWriter writer = PdfWriter.GetInstance(pdfDoc, stream);
                        pdfDoc.Open();

                        using (StringReader sr = new StringReader(texto_html))
                        {
                            XMLWorkerHelper.GetInstance().ParseXHtml(writer, pdfDoc, sr);
                        }

                        pdfDoc.Close();
                        stream.Close();
                    }
                    MessageBox.Show("Reporte PDF generado correctamente.", "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al generar el PDF: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

    }

}
