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
            int idProveedores =  Convert.ToInt32(((OpcionCombo)cboProveedor.SelectedItem).Valor.ToString());

            List<ReporteCompras> lista = new List<ReporteCompras>();

            lista = new CN_Reporte().compras(txtInicio.Value.ToString(), txtFin.Value.ToString(), idProveedores);

            dgvData.Rows.Clear();

            foreach(ReporteCompras rc in lista)
            {
                dgvData.Rows.Add(new object[] {
                    rc.FechaRegistro,
                    rc.TipoDocumento,
                    rc.NumeroDocumento,
                    rc.MontoTotal,
                    rc.UsuarioRegistro,
                    rc.DocumentoProveedor,
                    rc.RazonSocial,
                    rc.CodigoProducto,
                    rc.NombreProducto,
                    rc.Categoria,
                    rc.PrecioCompra,
                    rc.PrecioVenta,
                    rc.Cantidad,
                    rc.SubTotal
                });
            }
        }

        private void btnExportarExcel_Click(object sender, EventArgs e)
        {
            if (dgvData.Rows.Count < 1)
            {
                MessageBox.Show("No hay datos para exportar", "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            else
            {
                DataTable dt = new DataTable();

                // 1. Agregar las columnas al DataTable
                foreach (DataGridViewColumn columna in dgvData.Columns)
                {
                    if (columna.HeaderText != "" && columna.Visible)
                        dt.Columns.Add(columna.HeaderText, typeof(string));
                }

                // 2. Agregar las filas al DataTable
                foreach (DataGridViewRow row in dgvData.Rows)
                {
                    if (row.Visible)
                    {
                        dt.Rows.Add(new object[] {
                    
                  
                   
                            row.Cells["FechaRegistro"].Value?.ToString() ?? "",
                            row.Cells["TipoDocumento"].Value?.ToString() ?? "",
                            row.Cells["NumeroDocumento"].Value?.ToString() ?? "",
                            row.Cells["MontoTotal"].Value?.ToString() ?? "",
                            row.Cells["UsuarioRegistro"].Value?.ToString() ?? "",
                            row.Cells["DocumentoProveedor"].Value?.ToString() ?? "",
                            row.Cells["RazonSocial"].Value?.ToString() ?? "",
                            row.Cells["CodigoProducto"].Value?.ToString() ?? "",
                            row.Cells["NombreProducto"].Value?.ToString() ?? "",
                            row.Cells["Categoria"].Value?.ToString() ?? "",
                            row.Cells["PrecioCompra"].Value?.ToString() ?? "",
                            row.Cells["PrecioVenta"].Value?.ToString() ?? "",
                            row.Cells["Cantidad"].Value?.ToString() ?? "",
                            row.Cells["SubTotal"].Value?.ToString() ?? ""
       
                        });
                    }
                }

                // 3. Guardar el archivo
                SaveFileDialog savefile = new SaveFileDialog();
                savefile.FileName = string.Format("ReporteCompras_{0}.xlsx", DateTime.Now.ToString("ddMMyyyyHHmmss"));
                savefile.Filter = "Excel Files | *.xlsx";

                if (savefile.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        XLWorkbook wb = new XLWorkbook();
                        var hoja = wb.Worksheets.Add(dt, "Informe");
                        hoja.ColumnsUsed().AdjustToContents();
                        wb.SaveAs(savefile.FileName);
                        MessageBox.Show("Reporte Generado", "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    catch
                    {
                        MessageBox.Show("Error al generar reporte", "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void btnExportarPDF_Click(object sender, EventArgs e)
        {
            if (dgvData.Rows.Count < 1)
            {
                MessageBox.Show("No hay datos para exportar", "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            else
            {
                
                string Texto_Html = "<html><head><style>table {border-collapse: collapse;} th {background-color: #D6EEEE;}</style></head><body><h2>Reporte de Compras</h2><table border='1' cellpadding='5'><thead><tr><th>Fecha</th><th>Documento</th><th>Proveedor</th><th>Producto</th><th>Cantidad</th><th>SubTotal</th></tr></thead><tbody>@filas</tbody></table></body></html>";

                // Si usas plantilla de recursos, descomenta esta línea y comenta la de arriba:
                // string Texto_Html = Properties.Resources.PlantillaReporte.ToString();

                string filas = string.Empty;

                // 2. Llenar las filas (CORREGIDO: PROTECCIÓN CONTRA NULOS)
                foreach (DataGridViewRow row in dgvData.Rows)
                {
                    if (row.Visible)
                    {
                        filas += "<tr>";
                        // Usamos ?.ToString() ?? "" para que si es null ponga un texto vacío
                        filas += "<td>" + (row.Cells["FechaRegistro"].Value?.ToString() ?? "") + "</td>";
                        filas += "<td>" + (row.Cells["NumeroDocumento"].Value?.ToString() ?? "") + "</td>";
                        filas += "<td>" + (row.Cells["RazonSocial"].Value?.ToString() ?? "") + "</td>";
                        filas += "<td>" + (row.Cells["NombreProducto"].Value?.ToString() ?? "") + "</td>";
                        filas += "<td>" + (row.Cells["Cantidad"].Value?.ToString() ?? "") + "</td>";
                        filas += "<td>" + (row.Cells["SubTotal"].Value?.ToString() ?? "") + "</td>";
                        filas += "</tr>";
                    }
                }

                // Reemplazar la marca @filas en el HTML
                Texto_Html = Texto_Html.Replace("@filas", filas);

                // 3. Guardar el archivo PDF
                SaveFileDialog savefile = new SaveFileDialog();
                savefile.FileName = string.Format("ReporteCompras_{0}.pdf", DateTime.Now.ToString("ddMMyyyyHHmmss"));
                savefile.Filter = "Pdf Files|*.pdf";

                if (savefile.ShowDialog() == DialogResult.OK)
                {
                    using (FileStream stream = new FileStream(savefile.FileName, FileMode.Create))
                    {
                        Document pdfDoc = new Document(PageSize.A4, 25, 25, 25, 25);
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
