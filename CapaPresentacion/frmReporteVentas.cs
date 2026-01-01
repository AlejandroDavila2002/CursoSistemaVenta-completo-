using CapaEntidad;
using CapaNegocio;
using CapaPresentacion.utilidades;
using ClosedXML.Excel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CapaPresentacion
{
    public partial class frmReporteVentas : Form
    {
        public frmReporteVentas()
        {
            InitializeComponent();
        }

        private void frmReporteVentas_Load(object sender, EventArgs e)
        {
            btnBuscar.Select();

            // cbo para el dgvData
            foreach (DataGridViewColumn colum in dgvData.Columns)
            {
                cboBusqueda.Items.Add(new OpcionCombo() { Valor = colum.Name, Texto = colum.HeaderText });
            }
            cboBusqueda.DisplayMember = "Texto";
            cboBusqueda.ValueMember = "Valor";
            cboBusqueda.SelectedIndex = 0;
        }

        private void btnBuscar_Click(object sender, EventArgs e)
        {
            List<ReporteVenta> lista = new List<ReporteVenta>();

            lista = new CN_Reporte().Ventas(txtInicio.Value.ToString("dd/MM/yyyy"), txtFin.Value.ToString("dd/MM/yyyy"));

            dgvData.Rows.Clear();

            foreach (ReporteVenta row in lista)
            {
                dgvData.Rows.Add(new object[] {
                row.FechaRegistro,
                row.TipoDocumento,
                row.NumeroDocumento,
                row.MontoTotal,     // Total USD
                row.MontoBs,        // Total BS (NUEVO)
                row.TasaCambio,     // Tasa aplicada (NUEVO)
                row.TipoMoneda,     // Moneda de pago
                row.UsuarioRegistro,
                row.DocumentoCliente,
                row.NombreCliente,
                row.CodigoProducto,
                row.NombreProducto,
                row.Categoria,
                row.PrecioVenta,
                row.Cantidad,
                row.SubTotal
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

                // 1. DEFINIR LAS COLUMNAS EXACTAS (Total: 16 columnas)
                dt.Columns.Add("Fecha Registro", typeof(string));
                dt.Columns.Add("Tipo Documento", typeof(string));
                dt.Columns.Add("Numero Documento", typeof(string));
                dt.Columns.Add("Monto Total USD", typeof(string));
                dt.Columns.Add("Monto Bs", typeof(string));
                dt.Columns.Add("Tasa", typeof(string));
                dt.Columns.Add("Tipo Moneda", typeof(string));
                dt.Columns.Add("Usuario Registro", typeof(string));
                dt.Columns.Add("Documento Cliente", typeof(string));
                dt.Columns.Add("Nombre Cliente", typeof(string));
                dt.Columns.Add("Codigo Producto", typeof(string));
                dt.Columns.Add("Nombre Producto", typeof(string));
                dt.Columns.Add("Categoria", typeof(string));
                dt.Columns.Add("Precio Venta", typeof(string));
                dt.Columns.Add("Cantidad", typeof(string));
                dt.Columns.Add("SubTotal", typeof(string));

                // 2. AGREGAR LAS FILAS (Asegúrate de pasar exactamente 16 valores)
                foreach (DataGridViewRow row in dgvData.Rows)
                {
                    if (row.Visible)
                    {
                        dt.Rows.Add(new object[] {
                    row.Cells["FechaRegistro"].Value?.ToString() ?? "",
                    row.Cells["TipoDocumento"].Value?.ToString() ?? "",
                    row.Cells["NumeroDocumento"].Value?.ToString() ?? "",
                    row.Cells["MontoTotal"].Value?.ToString() ?? "", // Este es "Monto Total USD"
                    row.Cells["MontoBs"].Value?.ToString() ?? "",
                    row.Cells["TasaCambio"].Value?.ToString() ?? "",
                    row.Cells["TipoMoneda"].Value?.ToString() ?? "",
                    row.Cells["UsuarioRegistro"].Value?.ToString() ?? "",
                    row.Cells["DocumentoCliente"].Value?.ToString() ?? "",
                    row.Cells["NombreCliente"].Value?.ToString() ?? "",
                    row.Cells["CodigoProducto"].Value?.ToString() ?? "",
                    row.Cells["NombreProducto"].Value?.ToString() ?? "",
                    row.Cells["Categoria"].Value?.ToString() ?? "",
                    row.Cells["PrecioVenta"].Value?.ToString() ?? "",
                    row.Cells["Cantidad"].Value?.ToString() ?? "",
                    row.Cells["SubTotal"].Value?.ToString() ?? ""
                });
                    }
                }

                // 3. GUARDAR EL ARCHIVO
                SaveFileDialog savefile = new SaveFileDialog();
                savefile.FileName = string.Format("ReporteVentas_{0}.xlsx", DateTime.Now.ToString("ddMMyyyyHHmmss"));
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
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error al generar reporte: " + ex.Message, "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void btnBuscarReporte_Click(object sender, EventArgs e)
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
