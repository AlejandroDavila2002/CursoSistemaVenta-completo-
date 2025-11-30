using CapaEntidad;
using CapaNegocio;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.IO;
using System.Windows.Forms;
using iTextSharp.tool.xml;



namespace CapaPresentacion
{
    public partial class frmDetalleCompra : Form
    {
        public frmDetalleCompra()
        {
            InitializeComponent();
        }

        private void btnBuscar_Click(object sender, EventArgs e)
        {
            if (txtBusqueda.Text.Trim() == "")
            {
                MessageBox.Show("Debe colocar el codigo del Documento", "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return; // El 'return' detiene el código aquí, no sigue bajando.
            }


            Compra oCompra = new CN_Compra().ObtenerCompra(txtBusqueda.Text);

            if(oCompra != null && oCompra.IdCompra != 0)
            {
                nrmDocumento.Text = oCompra.NumeroDocumento;

                txtFecha.Text = oCompra.FechaRegistro;
                txtTipoDocumento.Text = oCompra.TipoDocumento;
                txtUsuario.Text = oCompra.oUsuario.NombreCompleto;
                txtdocProveedor.Text = oCompra.oProveedor.Documento;
                txtRazonSocial.Text = oCompra.oProveedor.RazonSocial;
                

                dgvData.Rows.Clear();
                foreach(Detalle_Compra dc in oCompra.oDetalleCommpra)
                {
                    dgvData.Rows.Add(new object[] {dc.oProducto.IdProducto ,dc.oProducto.NombreProducto, dc.PrecioCompra, dc.PrecioVenta, dc.Cantidad, dc.MontoTotal});


                }

                txtTotalaPagar.Text = oCompra.MontoTotal.ToString("0.00");
            }
            else
            {
                MessageBox.Show("No se encontró una compra con ese número de documento", "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void btnLimpiar_Click(object sender, EventArgs e)
        {
            txtFecha.Text = "";
            txtTipoDocumento.Text = "";
            txtUsuario.Text = "";
            txtdocProveedor.Text = "";
            txtRazonSocial.Text = "";
            nrmDocumento.Text = "";
            txtBusqueda.Text = "";
            txtTotalaPagar.Text = "0";

            dgvData.Rows.Clear();

        }

        private void btnExportarPDF_Click(object sender, EventArgs e)
        {
            // 1. Validación básica
            if (txtTipoDocumento.Text == "")
            {
                MessageBox.Show("No se encontraron resultados", "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            // 2. Cargar plantilla y datos del negocio
            string Texto_html = Properties.Resources.PlantillaCompra.ToString();

            // Asegúrate de usar la clase correcta aquí (CN_Negocio o CN_DetallesNegocio según tu proyecto)
            Detalles_Negocio oDatos = new CN_DetallesNegocio().ObtenerDetallesNegocio();

            // 3. Reemplazo de datos del negocio y documento
            Texto_html = Texto_html.Replace("@nombrenegocio", oDatos.Nombre.ToUpper());
            Texto_html = Texto_html.Replace("@docnegocio", oDatos.RUC);
            Texto_html = Texto_html.Replace("@direcnegocio", oDatos.Direccion);

            Texto_html = Texto_html.Replace("@tipodocumento", txtTipoDocumento.Text.ToUpper());
            Texto_html = Texto_html.Replace("@numerodocumento", nrmDocumento.Text);

            Texto_html = Texto_html.Replace("@docproveedor", txtdocProveedor.Text);
            Texto_html = Texto_html.Replace("@nombreproveedor", txtRazonSocial.Text);
            Texto_html = Texto_html.Replace("@fecharegistro", txtFecha.Text);
            Texto_html = Texto_html.Replace("@usuarioregistro", txtUsuario.Text);

            // 4. Generación de las filas (CORREGIDO HTML Y COLUMNAS)
            string fila = string.Empty;
            foreach (DataGridViewRow row in dgvData.Rows)
            {
                fila += "<tr>";
                // Importante: El orden debe coincidir con los <th> de tu HTML
                // Usamos ?.ToString() para evitar errores si la celda es null
                fila += "<td>" + row.Cells["Producto"].Value?.ToString() + "</td>";
                fila += "<td>" + row.Cells["PrecioCompra"].Value?.ToString() + "</td>";
                fila += "<td>" + row.Cells["PrecioVenta"].Value?.ToString() + "</td>";
                fila += "<td>" + row.Cells["Cantidad"].Value?.ToString() + "</td>";
                fila += "<td>" + row.Cells["Subtotal"].Value?.ToString() + "</td>";
                fila += "</tr>";
            }

            Texto_html = Texto_html.Replace("@filas", fila);
            Texto_html = Texto_html.Replace("@montototal", txtTotalaPagar.Text);

            // 5. Configurar guardado del archivo
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            // CORREGIDO: Se agregó {0} para que funcione el string.Format
            saveFileDialog.FileName = string.Format("Detalle_compra_{0}.pdf", nrmDocumento.Text);
            saveFileDialog.Filter = "Pdf Files|*.pdf";

            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                using (FileStream stream = new FileStream(saveFileDialog.FileName, FileMode.Create))
                {
                    // 6. Creación del PDF (CORREGIDO CONFLICTO DE NAMESPACES)
                    // Especificamos iTextSharp.text.Document explícitamente
                    iTextSharp.text.Document pdfDoc = new iTextSharp.text.Document(iTextSharp.text.PageSize.A4, 25, 25, 25, 25);

                    PdfWriter writer = PdfWriter.GetInstance(pdfDoc, stream);
                    pdfDoc.Open();

                    // 7. Insertar Logo
                    bool obtenido = true;
                    // Verifica si tu método se llama 'obtenerLogo' o 'ObtenerLogo' (mayúscula/minúscula)
                    byte[] byteImage = new CN_DetallesNegocio().obtenerLogo(out obtenido);

                    if (obtenido)
                    {
                        iTextSharp.text.Image img = iTextSharp.text.Image.GetInstance(byteImage);
                        img.ScaleToFit(60, 60);
                        img.Alignment = iTextSharp.text.Image.UNDERLYING;
                        img.SetAbsolutePosition(pdfDoc.Left, pdfDoc.GetTop(51));
                        pdfDoc.Add(img);
                    }

                    // 8. Insertar HTML parseado
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
