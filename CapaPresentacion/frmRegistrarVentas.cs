using CapaEntidad;
using CapaNegocio;
using CapaPresentacion.modales;
using CapaPresentacion.utilidades;
using DocumentFormat.OpenXml.Drawing;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CapaPresentacion
{
    public partial class frmRegistrarVentas : Form
    {
        private Usuario _UsuarioActual;
        private decimal _totalUSD = 0; // Totales acumulados en memoria para rapidez
        private decimal _totalVES = 0;
        public frmRegistrarVentas(Usuario usuarioActual)
        {
            InitializeComponent();
            _UsuarioActual = usuarioActual;
        }

        public void RecalcularTodoPorCambioTasa()
        {
            // 1. Obtenemos la nueva tasa que ya fue actualizada en el objeto global
            decimal nuevaTasa = _UsuarioActual.oTasaGeneral != null ? _UsuarioActual.oTasaGeneral.Valor : 0;

            foreach (DataGridViewRow row in dgvData.Rows)
            {
                // 2. Extraemos los valores base en Dólares (que son nuestra fuente de verdad)
                decimal precioUSD = Convert.ToDecimal(row.Cells["Precio"].Value, CultureInfo.InvariantCulture);
                int cantidad = Convert.ToInt32(row.Cells["Cantidad"].Value);

                // 3. Calculamos los nuevos valores con la tasa actualizada
                decimal nuevoPrecioBs = precioUSD * nuevaTasa;
                decimal nuevoSubTotalBs = (precioUSD * cantidad) * nuevaTasa;

                // 4. Actualizamos las celdas del Grid dinámicamente
                row.Cells["PrecioBs"].Value = nuevoPrecioBs.ToString("0.00", CultureInfo.InvariantCulture);
                row.Cells["SubTotalBs"].Value = nuevoSubTotalBs.ToString("0.00", CultureInfo.InvariantCulture);
            }

            // 5. Finalmente, recalculamos los totales generales para actualizar el txtTotalapagar
            CalcularTotal();
        }

        private void DevolverStockCompleto()
        {
            if (dgvData.Rows.Count > 0)
            {
                CN_Venta objVenta = new CN_Venta();
                foreach (DataGridViewRow row in dgvData.Rows)
                {
                    int idProd = Convert.ToInt32(row.Cells["IdProducto"].Value);
                    int cant = Convert.ToInt32(row.Cells["Cantidad"].Value);

                    // Sumamos de nuevo lo que habíamos restado preventivamente
                    objVenta.sumarStock(idProd, cant);
                }
                dgvData.Rows.Clear();
                CalcularTotal();
            }
        }

        private void frmRegistrarVentas_FormClosing(object sender, FormClosingEventArgs e)
        {
            // Si cierras y hay productos, devolvemos el stock a la DB
            DevolverStockCompleto();
        }
        private void frmRegistrarVentas_Load(object sender, EventArgs e)
        {
            cboTipoDocumento.Items.Add(new OpcionCombo() { Valor = "Boleta", Texto = "Boleta" });
            cboTipoDocumento.Items.Add(new OpcionCombo() { Valor = "Factura", Texto = "Factura" });
            cboTipoDocumento.DisplayMember = "Texto";
            cboTipoDocumento.ValueMember = "Valor";
            cboTipoDocumento.SelectedIndex = 0;

            txtFecha.Text = DateTime.Now.ToString("dd/MM/yyyy");
            txtIdProducto.Text = "0";

            txtTotalapagar.Text = "0";
            txtPagocon.Text = "";
            txtCambio.Text = "";
        }

        private void btnBuscarCliente_Click(object sender, EventArgs e)
        {
            using (var modal = new mdCliente())
            {
                var result = modal.ShowDialog();
                if (result == DialogResult.OK)
                {
                    txtDocumento.Text = modal._Cliente.Documento.ToString();
                    txtNombreCliente.Text = modal._Cliente.NombreCompleto.ToString();
                    txtCodigoProducto.Select();
                }
                else
                {
                    txtDocumento.Text = "";
                    txtNombreCliente.Text = "";
                    txtDocumento.Select();
                    MessageBox.Show("Debe seleccionar un Cliente", "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);

                }
            }
        }

        private void btnBuscarProducto_Click(object sender, EventArgs e)
        {

            using (modales.mdProductos mdProductos = new modales.mdProductos())
            {
                mdProductos.ShowDialog();
                if (mdProductos._Producto != null)
                {
                    txtIdProducto.Text = mdProductos._Producto.IdProducto.ToString();
                    txtCodigoProducto.BackColor = Color.White;
                    txtCodigoProducto.Text = mdProductos._Producto.Codigo;
                    txtNombreProducto.Text = mdProductos._Producto.NombreProducto;
                    txtPrecio.Text = mdProductos._Producto.PrecioVenta.ToString("0.00");
                    txtStock.Text = mdProductos._Producto.Stock.ToString();
                    txtCantidad.Select();
                }
                else
                {
                    txtIdProducto.Text = "0";
                    txtCodigoProducto.Text = "";
                    txtCodigoProducto.BackColor = Color.White;
                    txtNombreProducto.Text = "";
                    txtPrecio.Text = "";
                    txtStock.Text = "";
                    txtCantidad.Value = 1;
                    MessageBox.Show("No se seleccionó ningún producto", "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }

        private void txtCodigoProducto_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                Producto oProducto = new CN_Producto().listar().Where(p => p.Codigo == txtCodigoProducto.Text && p.Estado == true).FirstOrDefault();

                if (oProducto != null)
                {
                    txtCodigoProducto.BackColor = Color.Honeydew;
                    txtIdProducto.Text = oProducto.IdProducto.ToString();
                    txtNombreProducto.Text = oProducto.NombreProducto.ToString();
                    txtPrecio.Select();
                }
                else
                {
                    txtCodigoProducto.BackColor = Color.MistyRose;
                    txtIdProducto.Text = "0";
                    txtNombreProducto.Text = "";


                }
            }
        }

        private void btnAgregar_Click(object sender, EventArgs e)
        {
            decimal precioUSD = 0;
            bool productoExiste = false;

            if (int.Parse(txtIdProducto.Text) == 0)
            {
                MessageBox.Show("Debe seleccionar un producto", "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            if (_UsuarioActual.oTasaGeneral == null || _UsuarioActual.oTasaGeneral.Valor <= 0)
            {
                MessageBox.Show("No hay una Tasa de Cambio configurada o su valor es 0.\n\nPor favor, vaya al módulo 'Sistema Cambiario' y establezca una Tasa General antes de vender.",
                                "Tasa No Definida", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return; // Detenemos la ejecución aquí
            }

            if (!decimal.TryParse(txtPrecio.Text, NumberStyles.Any, CultureInfo.InvariantCulture, out precioUSD))
            {
                MessageBox.Show("Precio o formato incorrecto", "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            if (Convert.ToInt32(txtStock.Text) < Convert.ToInt32(txtCantidad.Value))
            {
                MessageBox.Show("La cantidad no puede ser mayor al Stock", "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            foreach (DataGridViewRow row in dgvData.Rows)
            {
                if (row.Cells["IdProducto"].Value.ToString() == txtIdProducto.Text)
                {
                    productoExiste = true;
                    break;
                }
            }

            if (!productoExiste)
            {
                bool resultado = new CN_Venta().RestarStock(int.Parse(txtIdProducto.Text), Convert.ToInt32(txtCantidad.Value));

                if (resultado)
                {
                    // LÓGICA BIMONEDA POR FILA
                    decimal tasa = _UsuarioActual.oTasaGeneral != null ? _UsuarioActual.oTasaGeneral.Valor : 0;
                    decimal precioVES = precioUSD * tasa;
                    decimal subtotalUSD = txtCantidad.Value * precioUSD;
                    decimal subtotalVES = txtCantidad.Value * precioVES;

                    // El orden debe coincidir con tu DGV (según tu descripción de 6 columnas):
                    // 0: IdProducto, 1: Producto, 2: Precio (USD), 3: PrecioBs, 4: Cantidad, 5: Subtotal (USD), 6: SubTotalBs
                    dgvData.Rows.Add(new object[] {
                        txtIdProducto.Text,
                        txtNombreProducto.Text,
                        precioUSD.ToString("0.00", CultureInfo.InvariantCulture),
                        precioVES.ToString("0.00", CultureInfo.InvariantCulture), // Nueva Columna PrecioBs
                        txtCantidad.Value.ToString(),
                        subtotalUSD.ToString("0.00", CultureInfo.InvariantCulture),
                        subtotalVES.ToString("0.00", CultureInfo.InvariantCulture)
                    });

                    CalcularTotal();
                    LImpiarProducto();
                }
            }
            else
            {
                MessageBox.Show("El producto ya se encuentra agregado.", "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void CalcularTotal()
        {
            _totalUSD = 0;
            _totalVES = 0;

            try
            {
                foreach (DataGridViewRow row in dgvData.Rows)
                {
                    // Validamos que la celda no sea nula y tenga valor antes de convertir
                    if (row.Cells["Subtotal"].Value != null && row.Cells["SubTotalBs"].Value != null)
                    {
                        _totalUSD += Convert.ToDecimal(row.Cells["Subtotal"].Value, CultureInfo.InvariantCulture);
                        _totalVES += Convert.ToDecimal(row.Cells["SubTotalBs"].Value, CultureInfo.InvariantCulture);
                    }
                }
            }
            catch (Exception ex)
            {
                // Esto te dirá exactamente qué columna está fallando si persiste el error
                Console.WriteLine("Error en CalcularTotal: " + ex.Message);
            }

            RefrescarMontoPantalla();
        }

        // --- MÉTODO REFRESCAR PANTALLA (CONTROLA EL CHECKBOX) ---
        private void RefrescarMontoPantalla()
        {
            // Verificamos que el control exista y no estemos en proceso de carga
            if (txtTotalapagar == null) return;

            decimal tasa = (_UsuarioActual.oTasaGeneral != null) ? _UsuarioActual.oTasaGeneral.Valor : 1;

            if (CambioMoneda.Checked)
            {
                // Muestra Bolívares
                txtTotalapagar.Text = _totalVES.ToString("N2", CultureInfo.InvariantCulture);
                label13.Text = "Paga con (Bs):"; // Actualizamos la etiqueta
            }
            else
            {
                // Muestra Dólares
                txtTotalapagar.Text = _totalUSD.ToString("0.00", CultureInfo.InvariantCulture);
                label13.Text = "Paga con ($):"; // Actualizamos la etiqueta
            }

            // Solo calcular cambio si hay un monto en 'Paga con'
            if (!string.IsNullOrWhiteSpace(txtPagocon.Text))
            {
                CalcularCambio();
            }
        }

        private void CambioMoneda_CheckedChanged(object sender, EventArgs e)
        {
            // Evitamos errores si el total aún no se ha calculado al iniciar el form
            RefrescarMontoPantalla();

            // Limpieza de seguridad
            txtPagocon.Text = "";
            txtCambio.Text = "0.00";
        }

        private void LImpiarProducto()
        {
            txtIdProducto.Text = "0";
            txtCodigoProducto.Text = "";
            txtCodigoProducto.BackColor = Color.White;
            txtNombreProducto.Text = "";
            txtPrecio.Text = "0.00";
            txtStock.Text = "0.00";
            txtCantidad.Value = 1;
            txtCodigoProducto.Select();
        }

        private void dgvData_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            if (e.RowIndex < 0)
                return;

            if (e.ColumnIndex == 7)
            {

                e.Paint(e.CellBounds, DataGridViewPaintParts.All);
                var w = Properties.Resources.icons8_borrar_25.Width;
                var h = Properties.Resources.icons8_borrar_25.Height;
                var x = e.CellBounds.Left + (e.CellBounds.Width - w) / 2;
                var y = e.CellBounds.Top + (e.CellBounds.Height - h) / 2;
                e.Graphics.DrawImage(Properties.Resources.icons8_borrar_25, new System.Drawing.Rectangle(x, y, w, h));
                e.Handled = true;

            }
        }

        private void dgvData_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dgvData.Columns[e.ColumnIndex].Name == "btnEliminar")
            {
                int indice = e.RowIndex;
                if (indice >= 0)
                {
                    bool resultado = new CN_Venta().sumarStock(
                        Convert.ToInt32(dgvData.Rows[indice].Cells["IdProducto"].Value),
                        Convert.ToInt32(dgvData.Rows[indice].Cells["Cantidad"].Value));

                    if (resultado)
                    {
                        dgvData.Rows.RemoveAt(indice);
                        CalcularTotal();
                    }
                }
            }
        }



        private void txtPrecio_KeyPress(object sender, KeyPressEventArgs e)
        {
            // 1. Si es un dígito (numero), lo dejamos pasar
            if (Char.IsDigit(e.KeyChar))
            {
                e.Handled = false;
                return;
            }

            // 2. Si es una tecla de control (como Borrar/Backspace), la dejamos pasar
            if (Char.IsControl(e.KeyChar))
            {
                e.Handled = false;
                return;
            }

            // 3. Verificamos si es un punto decimal
            if (e.KeyChar.ToString() == ".")
            {
                // A. Si la caja de texto YA tiene un punto, bloqueamos este nuevo punto
                //    (para evitar "15.5.5")
                if (txtPrecio.Text.Contains("."))
                {
                    e.Handled = true;
                    return;
                }

                // B. Si el punto es el PRIMER caracter, lo bloqueamos (opcional, gusto personal)
                //    (para evitar que escriban ".50")
                if (txtPrecio.Text.Trim().Length == 0)
                {
                    e.Handled = true;
                    return;
                }

                // Si pasó las validaciones anteriores, dejamos pasar el punto
                e.Handled = false;
                return;
            }

            // 4. Si no fue nada de lo anterior (letras, simbolos), LO BLOQUEAMOS
            e.Handled = true;
        }

        private void txtPrecio_Leave(object sender, EventArgs e)
        {
            // Si el texto está vacío, pon 0.00
            if (string.IsNullOrEmpty(txtPrecio.Text))
            {
                txtPrecio.Text = "0.00";
            }
            else
            {
                decimal valor;
                // Intentamos convertir a decimal
                if (decimal.TryParse(txtPrecio.Text, out valor))
                {
                    // Si funciona, le damos formato de 2 decimales
                    txtPrecio.Text = valor.ToString("0.00");
                }
                else
                {
                    // Si escribió algo raro que se pasó el filtro (muy difícil), lo limpiamos
                    txtPrecio.Text = "0.00";
                }
            }
        }

        private void txtPrecio_Enter(object sender, EventArgs e)
        {
            // Selecciona todo el texto al recibir el foco
            txtPrecio.SelectAll();
        }

        private void txtPrecio_MouseClick(object sender, MouseEventArgs e)
        {
            // A veces el clic anula la selección del Enter, esto lo forza de nuevo
            txtPrecio.SelectAll();
        }



        private void txtTotalapagar_KeyPress(object sender, KeyPressEventArgs e)
        {
            // 1. Si es un dígito (numero), lo dejamos pasar
            if (Char.IsDigit(e.KeyChar))
            {
                e.Handled = false;
                return;
            }

            // 2. Si es una tecla de control (como Borrar/Backspace), la dejamos pasar
            if (Char.IsControl(e.KeyChar))
            {
                e.Handled = false;
                return;
            }

            // 3. Verificamos si es un punto decimal
            if (e.KeyChar.ToString() == ".")
            {
                // A. Si la caja de texto YA tiene un punto, bloqueamos este nuevo punto
                //    (para evitar "15.5.5")
                if (txtTotalapagar.Text.Contains("."))
                {
                    e.Handled = true;
                    return;
                }

                // B. Si el punto es el PRIMER caracter, lo bloqueamos (opcional, gusto personal)
                //    (para evitar que escriban ".50")
                if (txtTotalapagar.Text.Trim().Length == 0)
                {
                    e.Handled = true;
                    return;
                }

                // Si pasó las validaciones anteriores, dejamos pasar el punto
                e.Handled = false;
                return;
            }

            // 4. Si no fue nada de lo anterior (letras, simbolos), LO BLOQUEAMOS
            e.Handled = true;
        }

        private void txtTotalapagar_Leave(object sender, EventArgs e)
        {
            // Si el texto está vacío, pon 0.00
            if (string.IsNullOrEmpty(txtTotalapagar.Text))
            {
                txtTotalapagar.Text = "0.00";
            }
            else
            {
                decimal valor;
                // Intentamos convertir a decimal
                if (decimal.TryParse(txtTotalapagar.Text, out valor))
                {
                    // Si funciona, le damos formato de 2 decimales
                    txtTotalapagar.Text = valor.ToString("0.00");
                }
                else
                {
                    // Si escribió algo raro que se pasó el filtro (muy difícil), lo limpiamos
                    txtTotalapagar.Text = "0.00";
                }
            }
        }

        private void txtTotalapagar_Enter(object sender, EventArgs e)
        {
            // Selecciona todo el texto al recibir el foco
            txtTotalapagar.SelectAll();
        }

        private void txtTotalapagar_MouseClick(object sender, MouseEventArgs e)
        {
            // A veces el clic anula la selección del Enter, esto lo forza de nuevo
            txtTotalapagar.SelectAll();
        }




        private void CalcularCambio()
        {
            if (txtTotalapagar.Text.Trim() == "")
            {
                MessageBox.Show("No existen productos en la venta", "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            decimal pagocon = 0;
            decimal total = Convert.ToDecimal(txtTotalapagar.Text);

            if (txtPagocon.Text.Trim() == "")
            {
                txtPagocon.Text = "0";
            }

            if (decimal.TryParse(txtPagocon.Text, out pagocon))
            {
                if (pagocon < total)
                {
                    MessageBox.Show("El monto con el que se paga no puede ser menor al total a pagar", "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    txtCambio.Text = "0.00";
                }
                else
                {
                    decimal cambio = pagocon - total;
                    txtCambio.Text = cambio.ToString("0.00");
                }
            }
            else
            {
                MessageBox.Show("Monto con el que se paga incorrecto", "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }


        }

        private void txtPagocon_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                CalcularCambio();
            }
        }

        private void btnRegistrar_Click(object sender, EventArgs e)
        {
            // Validaciones iniciales
            if (string.IsNullOrWhiteSpace(txtDocumento.Text)) { MessageBox.Show("Seleccione cliente", "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Warning); return; }
            if (dgvData.Rows.Count < 1) { MessageBox.Show("Agregue productos", "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Warning); return; }
            if (string.IsNullOrWhiteSpace(txtPagocon.Text)) { MessageBox.Show("Ingrese el monto de pago", "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Warning); return; }

            decimal pagoCon = Convert.ToDecimal(txtPagocon.Text, CultureInfo.InvariantCulture);
            decimal totalActual = Convert.ToDecimal(txtTotalapagar.Text, CultureInfo.InvariantCulture);

            if (pagoCon < totalActual)
            {
                MessageBox.Show("El pago no cubre el total", "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return;
            }

            // --- CORRECCIÓN: Calcular el cambio aquí mismo ---
            // Esto evita el error si txtCambio.Text está vacío
            decimal montoCambioCalculado = pagoCon - totalActual;

            // PREPARAR DATATABLE PARA SQL
            DataTable detalleVenta = new DataTable();
            detalleVenta.Columns.Add("IdProducto", typeof(int));
            detalleVenta.Columns.Add("PrecioVenta", typeof(decimal));
            detalleVenta.Columns.Add("Cantidad", typeof(int));
            detalleVenta.Columns.Add("SubTotal", typeof(decimal));
            detalleVenta.Columns.Add("SubTotalBs", typeof(decimal));

            foreach (DataGridViewRow row in dgvData.Rows)
            {
                detalleVenta.Rows.Add(
                    Convert.ToInt32(row.Cells["IdProducto"].Value),
                    Convert.ToDecimal(row.Cells["Precio"].Value, CultureInfo.InvariantCulture),
                    Convert.ToInt32(row.Cells["Cantidad"].Value),
                    Convert.ToDecimal(row.Cells["Subtotal"].Value, CultureInfo.InvariantCulture),
                    Convert.ToDecimal(row.Cells["SubTotalBs"].Value, CultureInfo.InvariantCulture)
                );
            }

            decimal tasaActual = _UsuarioActual.oTasaGeneral != null ? _UsuarioActual.oTasaGeneral.Valor : 0;
            int idCorrelativo = new CN_Venta().ObtenerCorrelativo();
            string NumeroDocumento = string.Format("{0:00000}", idCorrelativo);

            Venta oVenta = new Venta()
            {
                oUsuario = new Usuario() { IdUsuario = _UsuarioActual.IdUsuario },
                TipoDocumento = ((OpcionCombo)cboTipoDocumento.SelectedItem).Texto,
                NumeroDocumento = NumeroDocumento,
                DocumentoCliente = txtDocumento.Text.Trim(),
                NombreCliente = txtNombreCliente.Text.Trim(),
                MontoPago = pagoCon,
                // USAMOS LA VARIABLE CALCULADA, NO EL TEXTBOX
                MontoCambio = montoCambioCalculado,
                MontoTotal = _totalUSD,
                MontoBs = _totalVES,
                TasaCambio = tasaActual,
                TipoMoneda = CambioMoneda.Checked ? "VES" : "USD"
            };

            string mensaje = string.Empty;
            bool resultado = new CN_Venta().RegistrarVenta(oVenta, detalleVenta, out mensaje);

            if (resultado)
            {
                MessageBox.Show("Venta registrada: " + NumeroDocumento, "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LimpiarFormularioCompleto();
            }
            else
            {
                MessageBox.Show(mensaje, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LimpiarFormularioCompleto()
        {
            txtDocumento.Text = "";
            txtNombreCliente.Text = "";
            dgvData.Rows.Clear();
            _totalUSD = 0;
            _totalVES = 0;
            txtPagocon.Text = "";
            txtCambio.Text = "0.00";
            RefrescarMontoPantalla();
            LImpiarProducto();
        }






    }
}
