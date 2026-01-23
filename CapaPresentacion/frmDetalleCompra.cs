using CapaEntidad;
using CapaNegocio;
using CapaPresentacion.modales;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.tool.xml;
using System;
using System.IO;
using System.Windows.Forms;



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
            // 1. Si el campo de búsqueda está vacío, abrimos el modal
            if (txtBusqueda.Text.Trim() == "")
            {
                using (var modal = new mdCodigoCompra())
                {
                    var result = modal.ShowDialog();

                    if (result == DialogResult.OK)
                    {
                        // Asignamos el documento seleccionado al txtBusqueda
                        txtBusqueda.Text = modal.DocumentoSeleccionado;

                        // Ejecutamos la búsqueda automáticamente
                        CargarDetalleCompra();
                    }
                    else
                    {
                        // Si el usuario cancela o cierra el modal sin seleccionar, salimos
                        return;
                    }
                }
            }
            else
            {
                // 2. Si ya hay texto, buscamos directamente
                CargarDetalleCompra();
            }
        }

        private void CargarDetalleCompra()
        {
            Compra oCompra = new CN_Compra().ObtenerCompra(txtBusqueda.Text);

            if (oCompra != null && oCompra.IdCompra != 0)
            {
                nrmDocumento.Text = oCompra.NumeroDocumento;

                txtFecha.Text = oCompra.FechaRegistro;
                txtTipoDocumento.Text = oCompra.TipoDocumento;
                txtUsuario.Text = oCompra.oUsuario.NombreCompleto;
                txtdocProveedor.Text = oCompra.oProveedor.Documento;
                txtRazonSocial.Text = oCompra.oProveedor.RazonSocial;

                bool esBolivares = oCompra.EsCompraEnBs;

                if (esBolivares)
                {
                    txtTasa.Visible = true;
                    LbTasa.Visible = true;
                    txtTasa.Text = oCompra.TasaCambio.ToString("N2");
                }
                else
                {
                    txtTasa.Visible = false;
                    LbTasa.Visible = false;
                }

                dgvData.Columns["PrecioCompraBs"].Visible = esBolivares;
                dgvData.Columns["PrecioVentaBs"].Visible = esBolivares;
                dgvData.Columns["SubTotalBs"].Visible = esBolivares;

                label12.Text = esBolivares ? "Total a Pagar Bs:" : "Total a Pagar $:";

                dgvData.Rows.Clear();
                foreach (Detalle_Compra dc in oCompra.oDetalleCommpra)
                {
                    decimal tasa = oCompra.TasaCambio;
                    decimal precioCompraBs = dc.PrecioCompra * tasa;
                    decimal precioVentaBs = dc.PrecioVenta * tasa;
                    decimal subTotalBs = dc.MontoTotal * tasa;

                    dgvData.Rows.Add(new object[] {
                dc.oProducto.IdProducto,
                dc.oProducto.NombreProducto,
                dc.PrecioCompra.ToString("N2"),
                precioCompraBs.ToString("N2"),
                dc.PrecioVenta.ToString("N2"),
                precioVentaBs.ToString("N2"),
                dc.Cantidad,
                dc.MontoTotal.ToString("N2"),
                subTotalBs.ToString("N2")
            });
                }

                if (oCompra.EsCompraEnBs)
                {
                    // Nota: Aquí asumo que quieres calcular el total en Bs basado en lo almacenado o calculado
                    // Si tienes un campo MontoTotalBs en la BD, úsalo. Si no, calcúlalo:
                    decimal totalEnBs = oCompra.MontoTotal;
                    txtTotalaPagar.Text = totalEnBs.ToString("N2");
                }
                else
                {
                    txtTotalaPagar.Text = oCompra.MontoTotal.ToString("N2");
                }
            }
            else
            {
                MessageBox.Show("No se encontró una compra con ese número de documento", "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtBusqueda.Select();
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
            // 1. Validación inicial
            if (txtTipoDocumento.Text == "")
            {
                MessageBox.Show("No se encontraron resultados para exportar", "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            // --- RECUPERAR DATOS DE LA COMPRA PARA TASA Y MONEDA ---
            // Necesitamos traer el objeto Compra para saber la Tasa exacta y si fue en Bs, 
            // ya que esos datos no están en los TextBox simples del formulario.
            Compra oCompra = new CN_Compra().ObtenerCompra(txtBusqueda.Text);

            if (oCompra == null)
            {
                MessageBox.Show("Error al recuperar los datos de la compra", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Definimos el símbolo y la tasa a mostrar
            string simboloMoneda = oCompra.EsCompraEnBs ? "Bs. " : "$ ";
            string tasaTexto = oCompra.TasaCambio.ToString("N2");


            // --- GENERAR ENCABEZADOS DE TABLA DINÁMICOS ---
            string titulos = "<tr style='background-color:#CACACA'>";
            foreach (DataGridViewColumn columna in dgvData.Columns)
            {
                if (columna.Visible && columna.Name != "btnEliminar" && columna.Name != "IdProducto")
                {
                    titulos += "<th>" + columna.HeaderText + "</th>";
                }
            }
            titulos += "</tr>";


            // 2. Cargar plantilla
            string Texto_html = Properties.Resources.PlantillaCompra.ToString();
            Detalles_Negocio oDatos = new CN_DetallesNegocio().ObtenerDetallesNegocio();

            // 3. Reemplazos de datos generales
            Texto_html = Texto_html.Replace("@nombrenegocio", oDatos.Nombre.ToUpper());
            Texto_html = Texto_html.Replace("@docnegocio", oDatos.RUC);
            Texto_html = Texto_html.Replace("@direcnegocio", oDatos.Direccion);
            Texto_html = Texto_html.Replace("@tipodocumento", txtTipoDocumento.Text.ToUpper());
            Texto_html = Texto_html.Replace("@numerodocumento", nrmDocumento.Text);
            Texto_html = Texto_html.Replace("@docproveedor", txtdocProveedor.Text);
            Texto_html = Texto_html.Replace("@nombreproveedor", txtRazonSocial.Text);
            Texto_html = Texto_html.Replace("@fecharegistro", txtFecha.Text);
            Texto_html = Texto_html.Replace("@usuarioregistro", txtUsuario.Text);

            // --- REEMPLAZO DE LA TASA (NUEVO) ---
            Texto_html = Texto_html.Replace("@tasacambio", tasaTexto);

            // Reemplazo de encabezados
            Texto_html = Texto_html.Replace("@titulos", titulos);


            // 4. Generar Filas
            string filas = string.Empty;
            foreach (DataGridViewRow row in dgvData.Rows)
            {
                filas += "<tr>";
                foreach (DataGridViewColumn columna in dgvData.Columns)
                {
                    if (columna.Visible && columna.Name != "btnEliminar" && columna.Name != "IdProducto")
                    {
                        string valor = row.Cells[columna.Name].Value?.ToString();
                        filas += "<td>" + valor + "</td>";
                    }
                }
                filas += "</tr>";
            }

            Texto_html = Texto_html.Replace("@filas", filas);

            // --- REEMPLAZO DEL TOTAL CON SÍMBOLO DE MONEDA ---
            // Usamos el texto que ya está en el TextBox (que ya calculamos antes) y le pegamos el símbolo
            Texto_html = Texto_html.Replace("@montototal", simboloMoneda + txtTotalaPagar.Text);


            // 5. Guardar y Generar PDF
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.FileName = string.Format("Detalle_compra_{0}.pdf", nrmDocumento.Text);
            saveFileDialog.Filter = "Pdf Files|*.pdf";

            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                using (FileStream stream = new FileStream(saveFileDialog.FileName, FileMode.Create))
                {
                    // Usamos .Rotate() para girar la hoja A4 a horizontal
                    iTextSharp.text.Document pdfDoc = new iTextSharp.text.Document(iTextSharp.text.PageSize.A4.Rotate(), 25, 25, 25, 25);
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
                    MessageBox.Show("Documento Generado", "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }

        private void frmDetalleCompra_Load(object sender, EventArgs e)
        {

        }

      
    } 
}
