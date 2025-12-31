using CapaEntidad;
using CapaNegocio;
using iTextSharp.text.pdf;
using iTextSharp.tool.xml;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
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
                    decimal precioBs = dv.PrecioVenta * oVenta.TasaCambio;
                    dgvData.Rows.Add(new object[]
                    {
                        dv.oProducto.IdProducto,
                        dv.oProducto.NombreProducto,
                        dv.PrecioVenta.ToString("0.00", CultureInfo.InvariantCulture),
                        precioBs.ToString("0.00", CultureInfo.InvariantCulture),
                        dv.Cantidad,
                        dv.SubTotal.ToString("0.00", CultureInfo.InvariantCulture),
                        dv.SubTotalBs.ToString("0.00", CultureInfo.InvariantCulture)
                    });
                }

                // 3. Totales en Labels
                txtMontoTotal.Text = oVenta.MontoTotal.ToString("0.00", CultureInfo.InvariantCulture);
                txtMontoTotalBs.Text = oVenta.MontoBs.ToString("N2", CultureInfo.InvariantCulture);

                // --- LÓGICA PARA MOSTRAR SÍMBOLO DE MONEDA EN PANTALLA ---
                string simbolo = "$";
                decimal netoPagado = oVenta.MontoPago - oVenta.MontoCambio;

                // Si lo pagado (neto) coincide con el total en Bolívares, asumimos que fue en Bs.
                if (Math.Abs(netoPagado - oVenta.MontoBs) < 0.1m)
                {
                    simbolo = "Bs.";
                }

                // Asignamos el texto con el símbolo incluido
                txtMontoCambio.Text = string.Format("{0} {1}", simbolo, oVenta.MontoCambio.ToString("0.00", CultureInfo.InvariantCulture));
                txtMontoPago.Text = string.Format("{0} {1}", simbolo, oVenta.MontoPago.ToString("0.00", CultureInfo.InvariantCulture));
                
                // Mostrar la tasa histórica de la venta
                txtTasaVenta.Text = oVenta.TasaCambio.ToString("0.00", CultureInfo.InvariantCulture);
            }
            else
            {
                MessageBox.Show("No se encontró la venta con el número de documento proporcionado.", "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                txtNumeroDocumento.BackColor = Color.MistyRose;
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
            txtMontoTotalBs.Text = "";
            txtTasaVenta.Text = "";

            txtNumeroDocumento.Select();

        }

        private void btnExportarPDF_Click(object sender, EventArgs e)
        {
            // 1. Validación básica
            if (txtTipoDocumento.Text == "")
            {
                MessageBox.Show("No se encontraron resultados para exportar", "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            // 2. Cargar plantilla desde Resources
            string Texto_html = Properties.Resources.PlantillaVenta.ToString();

            // Obtener datos del negocio (Logo, RUC, etc.)
            Detalles_Negocio oDatos = new CN_DetallesNegocio().ObtenerDetallesNegocio();

            // 3. Reemplazo de datos del NEGOCIO
            Texto_html = Texto_html.Replace("@nombrenegocio", oDatos.Nombre.ToUpper());
            Texto_html = Texto_html.Replace("@docnegocio", oDatos.RUC);
            Texto_html = Texto_html.Replace("@direcnegocio", oDatos.Direccion);

            // 4. Reemplazo de datos de la VENTA y CLIENTE
            Texto_html = Texto_html.Replace("@tipodocumento", txtTipoDocumento.Text.ToUpper());
            Texto_html = Texto_html.Replace("@numerodocumento", txtNumeroDocumento.Text);
            Texto_html = Texto_html.Replace("@doccliente", txtdocCliente.Text);
            Texto_html = Texto_html.Replace("@nombrecliente", txtNombreCliente.Text);
            Texto_html = Texto_html.Replace("@fecharegistro", txtFecha.Text);
            Texto_html = Texto_html.Replace("@usuarioregistro", txtUsuario.Text);

            // 5. Generación de las FILAS de la tabla
            string filas = string.Empty;
            foreach (DataGridViewRow row in dgvData.Rows)
            {
                filas += "<tr>";
                filas += "<td>" + row.Cells["Producto"].Value?.ToString() + "</td>";
                filas += "<td>" + row.Cells["PrecioVenta"].Value?.ToString() + "</td>";
                filas += "<td>" + row.Cells["PrecioBs"].Value?.ToString() + "</td>";
                filas += "<td>" + row.Cells["Cantidad"].Value?.ToString() + "</td>";
                filas += "<td>" + row.Cells["Subtotal"].Value?.ToString() + "</td>";
                filas += "<td>" + row.Cells["SubTotalBs"].Value?.ToString() + "</td>";
                filas += "</tr>";
            }
            Texto_html = Texto_html.Replace("@filas", filas);

            // 6. Reemplazo de TOTALES y TASAS
            // A) Reemplazamos primero @montototalbs
            Texto_html = Texto_html.Replace("@montototalbs", txtMontoTotalBs.Text.Replace("bs", "").Replace("BS", "").Trim());
            Texto_html = Texto_html.Replace("@montototal", txtMontoTotal.Text);
            Texto_html = Texto_html.Replace("@tasacambio", txtTasaVenta.Text);

            // B) Reemplazamos Pago y Cambio DIRECTAMENTE (Ya incluyen el símbolo desde el formulario)
            Texto_html = Texto_html.Replace("@pagocon", txtMontoPago.Text);
            Texto_html = Texto_html.Replace("@cambio", txtMontoCambio.Text);

            // 7. Configurar guardado del archivo
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.FileName = string.Format("Venta_{0}.pdf", txtNumeroDocumento.Text);
            saveFileDialog.Filter = "Pdf Files|*.pdf";

            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                using (FileStream stream = new FileStream(saveFileDialog.FileName, FileMode.Create))
                {
                    iTextSharp.text.Document pdfDoc = new iTextSharp.text.Document(iTextSharp.text.PageSize.A4, 25, 25, 25, 25);
                    PdfWriter writer = PdfWriter.GetInstance(pdfDoc, stream);
                    pdfDoc.Open();

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

                    using (StringReader sr = new StringReader(Texto_html))
                    {
                        XMLWorkerHelper.GetInstance().ParseXHtml(writer, pdfDoc, sr);
                    }

                    pdfDoc.Close();
                    stream.Close();
                    MessageBox.Show("Ticket PDF generado exitosamente", "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }


    }
}
