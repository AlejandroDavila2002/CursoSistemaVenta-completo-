using CapaEntidad;
using CapaNegocio;
using CapaPresentacion.utilidades;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Text;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ClosedXML.Excel;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.IO;
using iTextFont = iTextSharp.text.Font;






namespace CapaPresentacion
{
    public partial class frmProducto : Form
    {
        public frmProducto()
        {
            InitializeComponent();
        }

        private void frmProducto_Load(object sender, EventArgs e)
        {
            cboEstado.Items.Add(new OpcionCombo() { Valor = 1, Texto = "Activo" });
            cboEstado.Items.Add(new OpcionCombo() { Valor = 0, Texto = "No Activo" });
            cboEstado.DisplayMember = "Texto";
            cboEstado.ValueMember = "Valor";
            cboEstado.SelectedIndex = 0;


            List<Categoria> listaCategoria = new CN_Categoria().listar();
            foreach (Categoria item in listaCategoria)
            {
                cboCategoria.Items.Add(new OpcionCombo() { Valor = item.IdCategoria, Texto = item.Descripcion });
            }
            cboCategoria.DisplayMember = "Texto";
            cboCategoria.ValueMember = "Valor";
            cboCategoria.SelectedIndex = 0;


            // Configurar el combo de busqueda
            foreach (DataGridViewColumn columna in dgvData.Columns)
            {
                if (columna.Visible == true && columna.Name != "btnSeleccionar")
                {
                    cboBusqueda.Items.Add(new OpcionCombo() { Valor = columna.Name, Texto = columna.HeaderText });
                }
            }
            cboBusqueda.DisplayMember = "Texto";
            cboBusqueda.ValueMember = "Valor";
            cboBusqueda.SelectedIndex = 0;



            List<Producto> listaProductos = new CN_Producto().listar();
            foreach (Producto item in listaProductos)
            {
                dgvData.Rows.Add(new object[] {
                    "",
                    item.IdProducto,
                    item.Codigo,
                    item.NombreProducto,
                    item.Descripcion,
                    item.oCategoria.IdCategoria,
                    item.oCategoria.Descripcion,
                    item.Stock,
                    item.PrecioCompra,
                    item.PrecioVenta,
                    item.Estado == true ? 1 : 0,
                    item.Estado == true ? "Activo" : "No Activo"
               });
            }


        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            string Mensaje = string.Empty;

            Producto objProducto = new Producto()
            {
             IdProducto = Convert.ToInt32(txtId.Text),
             Codigo = txtCodigo.Text,
             NombreProducto = txtNombreProducto.Text,
             Descripcion = txtDescripcion.Text,         
             oCategoria = new Categoria() { IdCategoria = Convert.ToInt32(((OpcionCombo)cboCategoria.SelectedItem).Valor) },
             Estado = Convert.ToInt32(((OpcionCombo)cboEstado.SelectedItem).Valor) == 1 ? true : false
            };

            if (objProducto.IdProducto == 0)
            {
                int IdProductoGenerado = new CN_Producto().Registrar(objProducto, out Mensaje);

                if (IdProductoGenerado != 0)
                {
                    dgvData.Rows.Add(new object[] {
                    "",
                    IdProductoGenerado,
                    txtCodigo.Text,
                    txtNombreProducto.Text,
                    txtDescripcion.Text,
                    ((OpcionCombo)cboCategoria.SelectedItem).Valor.ToString(),
                    ((OpcionCombo)cboCategoria.SelectedItem).Texto.ToString(),
                    "0",
                    "0.00",
                    "0.00",
                    ((OpcionCombo)cboEstado.SelectedItem).Valor.ToString(),
                    ((OpcionCombo)cboEstado.SelectedItem).Texto.ToString(),
                    });

                    Limpiar();
                }
                else
                {
                    MessageBox.Show(Mensaje);
                }
            }
            else
            {
                bool resultado = new CN_Producto().Editar(objProducto, out Mensaje);
                if (resultado)
                {
                    DataGridViewRow row = dgvData.Rows[Convert.ToInt32(txtIndice.Text)];
                    row.Cells["Id"].Value = txtId.Text;
                    row.Cells["Codigo"].Value = txtCodigo.Text;
                    row.Cells["NombreProducto"].Value = txtNombreProducto.Text;
                    row.Cells["Descripcion"].Value = txtDescripcion.Text;
                    row.Cells["IdCategoria"].Value = ((OpcionCombo)cboCategoria.SelectedItem).Valor.ToString();
                    row.Cells["Categoria"].Value = ((OpcionCombo)cboCategoria.SelectedItem).Texto.ToString();
                    row.Cells["EstadoValor"].Value = ((OpcionCombo)cboEstado.SelectedItem).Valor.ToString();
                    row.Cells["Estado"].Value = ((OpcionCombo)cboEstado.SelectedItem).Texto.ToString();
                    Limpiar();

                }
                else
                {
                    MessageBox.Show(Mensaje);
                }
            }

        }

        private void Limpiar()
        {
            txtIndice.Text = "-1";
            txtId.Text = "";
            txtCodigo.Text = "";
            txtNombreProducto.Text = "";
            txtDescripcion.Text = "";
            cboCategoria.SelectedIndex = 0;
            cboEstado.SelectedIndex = 0;
        }

        private void dgvData_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            if (e.RowIndex < 0)
                return;

            if (e.ColumnIndex == 0)
            {

                e.Paint(e.CellBounds, DataGridViewPaintParts.All);
                var w = Properties.Resources.icons8_comprobado_20.Width;
                var h = Properties.Resources.icons8_comprobado_20.Height;
                var x = e.CellBounds.Left + (e.CellBounds.Width - w) / 2;
                var y = e.CellBounds.Top + (e.CellBounds.Height - h) / 2;
                e.Graphics.DrawImage(Properties.Resources.icons8_comprobado_20, new System.Drawing.Rectangle(x, y, w, h));
                e.Handled = true;

            }
        }

        private void dgvData_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dgvData.Columns[e.ColumnIndex].Name == "btnSeleccionar")
            {
                int Indice = e.RowIndex;
                if (Indice >= 0)
                {
                    txtIndice.Text = Indice.ToString();
                    txtId.Text = dgvData.Rows[Indice].Cells["Id"].Value.ToString();
                    txtCodigo.Text = dgvData.Rows[Indice].Cells["Codigo"].Value.ToString();
                    txtNombreProducto.Text = dgvData.Rows[Indice].Cells["NombreProducto"].Value.ToString();
                    txtDescripcion.Text = dgvData.Rows[Indice].Cells["Descripcion"].Value.ToString();

                    foreach (OpcionCombo oc in cboCategoria.Items)
                    {
                        if (Convert.ToInt32(oc.Valor) == Convert.ToInt32(dgvData.Rows[Indice].Cells["IdCategoria"].Value))
                        {
                            cboCategoria.SelectedItem = oc;
                            break;
                        }
                    }

                    foreach (OpcionCombo oc in cboEstado.Items)
                    {
                        if (Convert.ToInt32(oc.Valor) == Convert.ToInt32(dgvData.Rows[Indice].Cells["EstadoValor"].Value))
                        {
                            cboEstado.SelectedItem = oc;
                            break;
                        }


                    }


                }


            }
        }

        private void BtnEliminar_Click(object sender, EventArgs e)
        {
            if (Convert.ToInt32(txtId.Text) != 0)
            {
                if (MessageBox.Show("¿Desea eliminar el Producto?", "Mensaje", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    string Mensaje = string.Empty;
                    Producto objProducto = new Producto()
                    {
                        IdProducto = Convert.ToInt32(txtId.Text)
                    };
                    bool resultado = new CN_Producto().Eliminar(objProducto, out Mensaje);
                    if (resultado)
                    {
                        dgvData.Rows.RemoveAt(Convert.ToInt32(txtIndice.Text));
                        MessageBox.Show(Mensaje, "Confirmación", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        Limpiar();
                    }
                    else
                    {
                        MessageBox.Show(Mensaje, "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    }
                }
            }
        }

        private void btnBuscar_Click(object sender, EventArgs e)
        {
            string columnaFiltro = ((OpcionCombo)cboBusqueda.SelectedItem).Valor.ToString();
            foreach (DataGridViewRow row in dgvData.Rows)
            {
                if (row.Cells[columnaFiltro].Value.ToString().Trim().ToUpper().Contains(txtBusqueda.Text.Trim().ToUpper()))
                    row.Visible = true;
                else
                    row.Visible = false;
            }

        }

        private void btnLimpiarCombo_Click(object sender, EventArgs e)
        {
            txtBusqueda.Text = "";
            cboBusqueda.SelectedIndex = 0;
            foreach (DataGridViewRow row in dgvData.Rows)
            {
                row.Visible = true;
            }
        }

        private void btnLimpiar_Click(object sender, EventArgs e)
        {
            Limpiar();
        }

        private void BtnExcel_Click(object sender, EventArgs e)
        {
            if(dgvData.Rows.Count < 1)
            {
                MessageBox.Show("No hay datos para exportar", "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            else
            {
                DataTable dt = new DataTable();
                
                foreach(DataGridViewColumn column in dgvData.Columns)
                {
                    if(column.HeaderText != "" && column.Visible)
                    {
                        dt.Columns.Add(column.HeaderText, typeof(string));
                    }

                }

                foreach(DataGridViewRow row in dgvData.Rows)
                {
                    if(row.Visible)
                    {
                        dt.Rows.Add(new object[]
                        {
                            row.Cells[2].Value,
                            row.Cells[3].Value,
                            row.Cells[4].Value,
                            row.Cells[6].Value,
                            row.Cells[7].Value,
                            row.Cells[8].Value,
                            row.Cells[9].Value,
                            row.Cells[11].Value,
                        });
                    }
                }

                SaveFileDialog saveFile = new SaveFileDialog();
                saveFile.FileName = string.Format("ReporteProducto.xlsx", DateTime.Now.ToString("ddMMyyyyHHmmss"));
                saveFile.Filter = "Excel Files | *.xlsx";
                

                if(saveFile.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        XLWorkbook wb = new XLWorkbook();
                        var hoja = wb.Worksheets.Add(dt, "Informe");
                        wb.SaveAs(saveFile.FileName);
                        MessageBox.Show("Reporte Generado", "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    catch
                    {
                        MessageBox.Show("Error al generar el reporte", "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    }

                }

            }
        }

        private void BtnExportarPDF_Click(object sender, EventArgs e)
        {
            if (dgvData.Rows.Count < 1)
            {
                MessageBox.Show("No hay datos para exportar", "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            SaveFileDialog saveFile = new SaveFileDialog();
            saveFile.FileName = $"ReporteProducto_{DateTime.Now:ddMMyyyyHHmmss}.pdf";
            saveFile.Filter = "PDF Files | *.pdf";

            if (saveFile.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    using (FileStream stream = new FileStream(saveFile.FileName, FileMode.Create))
                    {
                        Document pdfDoc = new Document(PageSize.A4.Rotate(), 10f, 10f, 20f, 20f);
                        PdfWriter.GetInstance(pdfDoc, stream);
                        pdfDoc.Open();

                        PdfPTable pdfTable = new PdfPTable(8); // Número de columnas visibles
                        pdfTable.WidthPercentage = 100;

                        // Agregar encabezados
                        string[] headers = { "Código", "Nombre", "Descripción", "Categoría", "Stock", "Precio Compra", "Precio Venta", "Estado" };
                        foreach (string header in headers)
                        {
                            PdfPCell cell = new PdfPCell(new Phrase(header, FontFactory.GetFont("Arial", 10, iTextFont.BOLD)));
                            cell.BackgroundColor = BaseColor.LIGHT_GRAY;
                            pdfTable.AddCell(cell);
                        }

                        // Agregar filas
                        foreach (DataGridViewRow row in dgvData.Rows)
                        {
                            if (row.Visible)
                            {
                                pdfTable.AddCell(row.Cells[2].Value?.ToString());
                                pdfTable.AddCell(row.Cells[3].Value?.ToString());
                                pdfTable.AddCell(row.Cells[4].Value?.ToString());
                                pdfTable.AddCell(row.Cells[6].Value?.ToString());
                                pdfTable.AddCell(row.Cells[7].Value?.ToString());
                                pdfTable.AddCell(row.Cells[8].Value?.ToString());
                                pdfTable.AddCell(row.Cells[9].Value?.ToString());
                                pdfTable.AddCell(row.Cells[11].Value?.ToString());
                            }
                        }

                        pdfDoc.Add(pdfTable);
                        pdfDoc.Close();
                        stream.Close();
                    }

                    MessageBox.Show("PDF generado correctamente", "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error al generar el PDF: " + ex.Message, "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }


        }
    }
    
}
