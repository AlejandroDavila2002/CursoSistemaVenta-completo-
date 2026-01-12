using CapaEntidad;
using CapaNegocio;
using CapaPresentacion.utilidades;
using ClosedXML.Excel;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.tool.xml;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace CapaPresentacion
{
    public partial class frmReporteCompras : Form
    {
        public frmReporteCompras()
        {
            InitializeComponent();
        }

        private void frmReporteCompras_Load(object sender, EventArgs e)
        {
            // cbo de proveedores en la barra principal
            List<Proveedor> Lista = new CN_Proveedor().listar();
            cboProveedor.Items.Add(new OpcionCombo() { Valor = 0, Texto = "TODOS" });
            foreach(Proveedor item in Lista)
            {
                cboProveedor.Items.Add(new OpcionCombo() { Valor = item.IdProveedor, Texto = item.RazonSocial });
            }
            cboProveedor.DisplayMember = "Texto";
            cboProveedor.ValueMember = "Valor";
            cboProveedor.SelectedIndex = 0;

            //cbo para el dgvData
            foreach(DataGridViewColumn colum in dgvData.Columns)
            {
                cboBusqueda.Items.Add(new OpcionCombo() { Valor = colum.Name, Texto = colum.HeaderText });
            }
            cboBusqueda.DisplayMember = "Texto";
            cboBusqueda.ValueMember = "Valor";
            cboBusqueda.SelectedIndex = 0;

        }

        private void btnBuscar_Click(object sender, EventArgs e)
        {
            // 1. Obtener ID del proveedor seleccionado
            int idProveedores = Convert.ToInt32(((OpcionCombo)cboProveedor.SelectedItem).Valor.ToString());

            // 2. Obtener la lista de datos
            List<ReporteCompras> lista = new List<ReporteCompras>();
            lista = new CN_Reporte().compras(txtInicio.Value.ToString(), txtFin.Value.ToString(), idProveedores);

            // 3. Limpiar la tabla
            dgvData.Rows.Clear();

            // 4. Llenar la tabla USANDO NOMBRES DE COLUMNAS (Para evitar inconsistencias)
            foreach (ReporteCompras rc in lista)
            {
                // Agregamos una fila vacía y obtenemos su índice
                int rowId = dgvData.Rows.Add();
                DataGridViewRow row = dgvData.Rows[rowId];

                // --- COLUMNAS DE TEXTO (Se quedan igual) ---
                row.Cells["FechaRegistro"].Value = rc.FechaRegistro;
                row.Cells["TipoDocumento"].Value = rc.TipoDocumento;
                row.Cells["NumeroDocumento"].Value = rc.NumeroDocumento;
                row.Cells["UsuarioRegistro"].Value = rc.UsuarioRegistro;
                row.Cells["DocumentoProveedor"].Value = rc.DocumentoProveedor;
                row.Cells["RazonSocial"].Value = rc.RazonSocial;
                row.Cells["CodigoProducto"].Value = rc.CodigoProducto;
                row.Cells["NombreProducto"].Value = rc.NombreProducto;
                row.Cells["Categoria"].Value = rc.Categoria;
                row.Cells["Cantidad"].Value = rc.Cantidad; // La cantidad suele dejarse sin decimales o como viene

                // --- COLUMNAS MONETARIAS (Aplicamos formato 0.00) ---
                // Usamos Convert.ToDecimal() para asegurar que sea número y luego formateamos

                // Totales del Documento
                row.Cells["MontoTotal"].Value = Convert.ToDecimal(rc.MontoTotal).ToString("N2");
                row.Cells["MontoTotalBs"].Value = Convert.ToDecimal(rc.MontoTotalBs).ToString("N2");

                // Precios de Compra
                row.Cells["PrecioCompra"].Value = Convert.ToDecimal(rc.PrecioCompra).ToString("N2");
                row.Cells["PrecioCompraBs"].Value = Convert.ToDecimal(rc.PrecioCompraBs).ToString("N2");

                // Precios de Venta
                row.Cells["PrecioVenta"].Value = Convert.ToDecimal(rc.PrecioVenta).ToString("N2");
                row.Cells["PrecioVentaBs"].Value = Convert.ToDecimal(rc.PrecioVentaBs).ToString("N2");

                // Subtotales
                row.Cells["SubTotal"].Value = Convert.ToDecimal(rc.SubTotal).ToString("N2");
                row.Cells["SubTotalBs"].Value = Convert.ToDecimal(rc.SubTotalBs).ToString("N2");

                // Tasa de Cambio
                row.Cells["TasaCambio"].Value = Convert.ToDecimal(rc.TasaCambio).ToString("N2");

                // --- COLUMNA OPCIONAL (Si/No) ---
                if (dgvData.Columns.Contains("EsCompraEnBs"))
                {
                    row.Cells["EsCompraEnBs"].Value = rc.EsCompraEnBs;
                }
            }

            dgvData.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            dgvData.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.DisplayedCells;
        
        }

        private void btnExportarExcel_Click(object sender, EventArgs e)
        {
            if (dgvData.Rows.Count < 1)
            {
                MessageBox.Show("No hay datos para exportar", "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            try
            {
                DataTable dt = new DataTable();

                // 1. GENERAR COLUMNAS AUTOMÁTICAMENTE
                // Solo agrega al Excel las columnas que el usuario puede ver
                foreach (DataGridViewColumn columna in dgvData.Columns)
                {
                    if (columna.Visible && !string.IsNullOrEmpty(columna.HeaderText))
                    {
                        dt.Columns.Add(columna.HeaderText, typeof(string));
                    }
                }

                // 2. LLENAR FILAS AUTOMÁTICAMENTE
                foreach (DataGridViewRow row in dgvData.Rows)
                {
                    if (row.Visible)
                    {
                        dt.Rows.Add(new object[dt.Columns.Count]); // Crea fila vacía

                        int i = 0;
                        foreach (DataGridViewColumn columna in dgvData.Columns)
                        {
                            if (columna.Visible && !string.IsNullOrEmpty(columna.HeaderText))
                            {
                                // Llena celda por celda dinámicamente
                                dt.Rows[dt.Rows.Count - 1][i] = row.Cells[columna.Name].Value?.ToString() ?? "";
                                i++;
                            }
                        }
                    }
                }

                // 3. GUARDAR
                SaveFileDialog savefile = new SaveFileDialog();
                savefile.FileName = string.Format("ReporteCompras_{0}.xlsx", DateTime.Now.ToString("ddMMyyyyHHmmss"));
                savefile.Filter = "Excel Files | *.xlsx";

                if (savefile.ShowDialog() == DialogResult.OK)
                {
                    XLWorkbook wb = new XLWorkbook();
                    var hoja = wb.Worksheets.Add(dt, "Informe");
                    hoja.ColumnsUsed().AdjustToContents();
                    wb.SaveAs(savefile.FileName);
                    MessageBox.Show("Reporte Generado", "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al generar reporte: " + ex.Message, "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnExportarPDF_Click(object sender, EventArgs e)
        {
            if (dgvData.Rows.Count < 1)
            {
                MessageBox.Show("No hay datos para exportar", "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            // --- 1. CONSTRUCCIÓN DINÁMICA DEL HTML ---

            // A. Encabezados de la tabla (@titulos)
            string titulos = "<tr>";
            foreach (DataGridViewColumn columna in dgvData.Columns)
            {
                if (columna.Visible && columna.HeaderText != "")
                {
                    titulos += "<th style='background-color:#D6EEEE;'>" + columna.HeaderText + "</th>";
                }
            }
            titulos += "</tr>";

            // B. Filas de datos (@filas)
            string filas = string.Empty;
            foreach (DataGridViewRow row in dgvData.Rows)
            {
                if (row.Visible)
                {
                    filas += "<tr>";
                    foreach (DataGridViewColumn columna in dgvData.Columns)
                    {
                        if (columna.Visible && columna.HeaderText != "")
                        {
                            string valor = row.Cells[columna.Name].Value?.ToString() ?? "";
                            filas += "<td>" + valor + "</td>";
                        }
                    }
                    filas += "</tr>";
                }
            }

            // C. Plantilla HTML Base
            // IMPORTANTE: 'font-size: 7px' reduce la letra para que quepan todas las columnas nuevas
            string Texto_Html = "<html><head><style>";
            Texto_Html += "table {border-collapse: collapse; width: 100%; font-family: Arial; font-size: 7px;}";
            Texto_Html += "th, td {border: 1px solid black; padding: 4px; text-align: center;}";
            Texto_Html += "</style></head><body>";
            Texto_Html += "<h2 style='text-align:center; font-family: Arial;'>Reporte de Compras</h2>";
            Texto_Html += "<table>";
            Texto_Html += "<thead>" + titulos + "</thead>";
            Texto_Html += "<tbody>" + filas + "</tbody>";
            Texto_Html += "</table></body></html>";


            // --- 2. GENERAR Y GUARDAR PDF ---
            SaveFileDialog savefile = new SaveFileDialog();
            savefile.FileName = string.Format("ReporteCompras_{0}.pdf", DateTime.Now.ToString("ddMMyyyyHHmmss"));
            savefile.Filter = "Pdf Files|*.pdf";

            if (savefile.ShowDialog() == DialogResult.OK)
            {
                using (FileStream stream = new FileStream(savefile.FileName, FileMode.Create))
                {
                    // IMPORTANTE: .Rotate() para poner la hoja en Horizontal
                    Document pdfDoc = new Document(PageSize.A4.Rotate(), 10, 10, 10, 10);

                    PdfWriter writer = PdfWriter.GetInstance(pdfDoc, stream);
                    pdfDoc.Open();

                    using (StringReader sr = new StringReader(Texto_Html))
                    {
                        XMLWorkerHelper.GetInstance().ParseXHtml(writer, pdfDoc, sr);
                    }

                    pdfDoc.Close();
                    stream.Close();
                    MessageBox.Show("Documento Generado", "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }

        private void iconButton1_Click(object sender, EventArgs e)
        {
            // 1. Obtenemos la columna por la que vamos a filtrar
            string columnaFiltro = ((OpcionCombo)cboBusqueda.SelectedItem).Valor.ToString();

            if (dgvData.Rows.Count > 0) // Solo buscamos si hay filas
            {
                foreach (DataGridViewRow row in dgvData.Rows)
                {
                    // --- AQUÍ ESTABA EL ERROR ---
                    // Usamos ?. para evitar el crash si la celda es nula
                    // Usamos ?? "" para que si es nula, la trate como texto vacío
                    string valorCelda = row.Cells[columnaFiltro].Value?.ToString().Trim().ToUpper() ?? "";

                    string busqueda = txtBusqueda.Text.Trim().ToUpper();

                    // 3. Comparamos
                    if (valorCelda.Contains(busqueda))
                    {
                        row.Visible = true;
                    }
                    else
                    {
                        row.Visible = false;
                    }
                }
            }
        }

        private void btnLimpiarCombo_Click(object sender, EventArgs e)
        {
            txtBusqueda.Text = "";

            // Evita error si el combo está vacío
            if (cboBusqueda.Items.Count > 0)
            {
                cboBusqueda.SelectedIndex = 0;
            }

            // Volver a hacer visibles TODAS las filas
            foreach (DataGridViewRow row in dgvData.Rows)
            {
                row.Visible = true;
            }
        }
    }
}
