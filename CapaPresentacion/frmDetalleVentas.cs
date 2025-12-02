using CapaEntidad;
using CapaNegocio;
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
    public partial class frmDetalleVentas : Form
    {
        public frmDetalleVentas()
        {
            InitializeComponent();
        }
        private void frmDetalleVentas_Load(object sender, EventArgs e)
        {
            txtNumeroDocumento.Select();
        }

        private void btnBuscar_Click(object sender, EventArgs e)
        {
            Venta oVenta = new CN_Venta().ObtenerVenta(txtNumeroDocumento.Text.Trim());

            if (oVenta.IdVenta != 0)
            {
                txtFecha.Text = oVenta.FechaRegistro;
                txtTipoDocumento.Text = oVenta.TipoDocumento;
                txtUsuario.Text = oVenta.oUsuario.NombreCompleto;

                txtdocCliente.Text = oVenta.DocumentoCliente;
                txtNombreCliente.Text = oVenta.NombreCliente;

                dgvData.Rows.Clear();
                foreach (Detalle_Venta dv in oVenta.oDetalleVenta)
                {
                    dgvData.Rows.Add(new object[]
                    {
                        dv.oProducto.IdProducto, // por ahora no lo mostramos en el detalle de venta.
                        dv.oProducto.NombreProducto,
                        dv.oProducto.PrecioCompra.ToString("0.00"), // por ahora no lo mostramos en el detalle de venta.
                        dv.PrecioVenta.ToString("0.00"),
                        dv.Cantidad,
                        dv.SubTotal.ToString("0.00")
                    });
                }

                txtMontoTotal.Text = oVenta.MontoTotal.ToString("0.00");
                txtMontoCambio.Text = oVenta.MontoCambio.ToString("0.00");
                txtMontoPago.Text = oVenta.MontoPago.ToString("0.00");

            }
            else
            {
                MessageBox.Show("No se encontró la venta con el número de documento proporcionado.", "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }


        }

        private void btnLimpiar_Click(object sender, EventArgs e)
        {
            // Limpiar todos los campos del formulario
            txtNumeroDocumento.Text = "";
            txtFecha.Text = "";
            txtTipoDocumento.Text = "";
            txtUsuario.Text = "";
            txtdocCliente.Text = "";
            txtNombreCliente.Text = "";
            dgvData.Rows.Clear();
            txtMontoTotal.Text = "";
            txtMontoCambio.Text = "";
            txtMontoPago.Text = "";

            txtNumeroDocumento.Select();

        }

        private void btnExportarPDF_Click(object sender, EventArgs e)
        {
            // 1. Validación básica
            if (txtTipoDocumento.Text == "")
            {
                MessageBox.Show("No se encontraron resultados", "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            // 2. Cargar plantilla
            string Texto_html = Properties.Resources.PlantillaVenta.ToString();

            // Obtener datos del negocio
            Detalles_Negocio oDatos = new CN_DetallesNegocio().ObtenerDetallesNegocio();

            // 3. Reemplazo de datos del NEGOCIO y DOCUMENTO
            Texto_html = Texto_html.Replace("@nombrenegocio", oDatos.Nombre.ToUpper());
            Texto_html = Texto_html.Replace("@docnegocio", oDatos.RUC);
            Texto_html = Texto_html.Replace("@direcnegocio", oDatos.Direccion);

            Texto_html = Texto_html.Replace("@tipodocumento", txtTipoDocumento.Text.ToUpper());
            Texto_html = Texto_html.Replace("@numerodocumento", txtNumeroDocumento.Text);

      
            Texto_html = Texto_html.Replace("@doccliente", txtdocCliente.Text);
            Texto_html = Texto_html.Replace("@nombrecliente", txtNombreCliente.Text);
            Texto_html = Texto_html.Replace("@fecharegistro", txtFecha.Text);
            Texto_html = Texto_html.Replace("@usuarioregistro", txtUsuario.Text);

            // 5. Generación de las filas
            string fila = string.Empty;
            foreach (DataGridViewRow row in dgvData.Rows)
            {
                fila += "<tr>";
            
                // "Producto", "Precio", "Cantidad", "SubTotal"
                fila += "<td>" + row.Cells["Producto"].Value?.ToString() + "</td>";
                fila += "<td>" + row.Cells["PrecioVenta"].Value?.ToString() + "</td>";
                fila += "<td>" + row.Cells["Cantidad"].Value?.ToString() + "</td>";
                fila += "<td>" + row.Cells["SubTotal"].Value?.ToString() + "</td>";
                fila += "</tr>";
            }

            Texto_html = Texto_html.Replace("@filas", fila);

            // 6. Totales y Pagos (Con tus nombres de variables corregidos)
            Texto_html = Texto_html.Replace("@montototal", txtMontoTotal.Text);
            Texto_html = Texto_html.Replace("@pagocon", txtMontoPago.Text); // En el HTML es @pagocon, en tu form es txtMontoPago
            Texto_html = Texto_html.Replace("@cambio", txtMontoCambio.Text); // En el HTML es @cambio, en tu form es txtMontoCambio

            // 7. Configurar guardado del archivo
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.FileName = string.Format("Detalle_venta_{0}.pdf", txtNumeroDocumento.Text);
            saveFileDialog.Filter = "Pdf Files|*.pdf";

            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                using (FileStream stream = new FileStream(saveFileDialog.FileName, FileMode.Create))
                {
                    // Creación del PDF
                    iTextSharp.text.Document pdfDoc = new iTextSharp.text.Document(iTextSharp.text.PageSize.A4, 25, 25, 25, 25);

                    PdfWriter writer = PdfWriter.GetInstance(pdfDoc, stream);
                    pdfDoc.Open();

                    // Insertar Logo
                    bool obtenido = true;
                    byte[] byteImage = new CN_DetallesNegocio().obtenerLogo(out obtenido);

                    if (obtenido)
                    {
                        iTextSharp.text.Image img = iTextSharp.text.Image.GetInstance(byteImage);
                        img.ScaleToFit(60, 60);
                        img.Alignment = iTextSharp.text.Image.UNDERLYING;
                        img.SetAbsolutePosition(pdfDoc.Left, pdfDoc.GetTop(51));
                        pdfDoc.Add(img);
                    }

                    // Insertar HTML parseado
                    using (StringReader sr = new StringReader(Texto_html))
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
}
