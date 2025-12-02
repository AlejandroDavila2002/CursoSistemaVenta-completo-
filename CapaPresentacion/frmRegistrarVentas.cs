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
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CapaPresentacion
{
    public partial class frmRegistrarVentas : Form
    {
        private Usuario _UsuarioActual;
        public frmRegistrarVentas(Usuario usuarioActual)
        {
            InitializeComponent();
            _UsuarioActual = usuarioActual;
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
            decimal precio = 0;
            bool productoExiste = false;

            if (int.Parse(txtIdProducto.Text) == 0)
            {
                MessageBox.Show("Debe seleccionar un producto", "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            if (!decimal.TryParse(txtPrecio.Text, out precio))
            {
                MessageBox.Show("Precio o formato de venta incorrecto", "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            if (Convert.ToInt32(txtStock.Text) < Convert.ToInt32(txtCantidad.Text))
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
                    dgvData.Rows.Add(new object[]
                    {
                    txtIdProducto.Text,
                    txtNombreProducto.Text,
                    precio.ToString("0.00"),
                    txtCantidad.Value.ToString(),
                    (txtCantidad.Value * precio).ToString("0.00")
                    });

                    CalcularTotal();
                    LImpiarProducto();
                }


            }
            else 
            {
                MessageBox.Show("El producto ya se encuentra agregado. Si necesita más cantidad, elimínelo y vuelva a agregarlo con la cantidad total.", "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }


        }

        private void CalcularTotal()
        {
            decimal total = 0;

            if (dgvData.Rows.Count > 0)
            {
                foreach (DataGridViewRow row in dgvData.Rows)
                {
                    total += Convert.ToDecimal(row.Cells["Subtotal"].Value.ToString());

                }
            }
            txtTotalapagar.Text = total.ToString("0.00");
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

            if (e.ColumnIndex == 5)
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
                        Convert.ToInt32(dgvData.Rows[indice].Cells["IdProducto"].Value.ToString()),
                        Convert.ToInt32(dgvData.Rows[indice].Cells["Cantidad"].Value.ToString()));

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
            // A veces el clic anula la selección del Enter, esto lo fuerza de nuevo
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
            // A veces el clic anula la selección del Enter, esto lo fuerza de nuevo
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
            if (txtDocumento.Text.Trim() == "")
            {
                MessageBox.Show("Debe ingresar el documento del cliente", "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            if (txtNombreCliente.Text.Trim() == "")
            {
                MessageBox.Show("Debe ingresar el nombre del cliente", "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            if (dgvData.Rows.Count < 1)
            {
                MessageBox.Show("Debe ingresar productos a la venta", "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }


            // se conservarán estos 3 bloques de validacion para mayor seguridad, por ahora. 20/11/2025
            if (string.IsNullOrWhiteSpace(txtPagocon.Text))
            {
                MessageBox.Show("Debe ingresar con cuánto paga el cliente", "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return; // ESTO DETIENE EL GUARDADO
            }

            // 2. Validar que el pago cubra el total
            decimal pagoCon;
            decimal total = Convert.ToDecimal(txtTotalapagar.Text);

            if (!decimal.TryParse(txtPagocon.Text, out pagoCon))
            {
                MessageBox.Show("Formato de moneda incorrecto", "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            // 3. Validar que el pago no sea menor al total
            if (pagoCon < total)
            {
                MessageBox.Show("El monto de pago no puede ser menor al total a pagar", "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return; // ESTO DETIENE EL GUARDADO
            }



            DataTable detalleVenta = new DataTable();
            detalleVenta.Columns.Add("IdProducto", typeof(int));
            detalleVenta.Columns.Add("Precio", typeof(decimal));
            detalleVenta.Columns.Add("Cantidad", typeof(int));
            detalleVenta.Columns.Add("Subtotal", typeof(decimal));

            foreach (DataGridViewRow row in dgvData.Rows)
            {
                detalleVenta.Rows.Add(
                    Convert.ToInt32(row.Cells["IdProducto"].Value.ToString()),
                    Convert.ToDecimal(row.Cells["Precio"].Value.ToString()),
                    Convert.ToInt32(row.Cells["Cantidad"].Value.ToString()),
                    Convert.ToDecimal(row.Cells["Subtotal"].Value.ToString())
                );
            }


            int idCorrelativo = new CN_Venta().ObtenerCorrelativo();
            string NumeroDocumento = string.Format("{0:00000}", idCorrelativo);
            CalcularCambio();

            Venta oVenta = new Venta()
            {
                oUsuario = new Usuario() { IdUsuario = _UsuarioActual.IdUsuario },
                TipoDocumento = ((OpcionCombo)cboTipoDocumento.SelectedItem).Valor.ToString(),
                NumeroDocumento = NumeroDocumento,
                DocumentoCliente = txtDocumento.Text.Trim(),
                NombreCliente = txtNombreCliente.Text.Trim(),
                MontoPago = Convert.ToDecimal(txtPagocon.Text),
                MontoCambio = Convert.ToDecimal(txtCambio.Text),
                MontoTotal = Convert.ToDecimal(txtTotalapagar.Text)
            };

            string mensaje = string.Empty;
            bool resultado = new CN_Venta().RegistrarVenta(oVenta, detalleVenta, out mensaje);

            if (resultado)
            {
                // 1. Mostrar el mensaje y capturar la respuesta (Si/No)
                var result = MessageBox.Show(
                    "Venta registrada exitosamente: \n" + NumeroDocumento + "\n\n¿Desea copiar al portapapeles?",
                    "Mensaje",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Information); // Usé 'Information' para éxito, queda mejor que 'Exclamation'

                // 2. Lógica para copiar al portapapeles si el usuario dice "Sí"
                if (result == DialogResult.Yes)
                {
                    Clipboard.SetText(NumeroDocumento);
                }

                // 3. Reiniciar el formulario para una nueva venta
                txtDocumento.Text = "";
                txtNombreCliente.Text = "";
                dgvData.Rows.Clear();
                CalcularTotal();
                txtPagocon.Text = "";
                txtCambio.Text = "";

                txtCodigoProducto.Text = "";
                txtNombreProducto.Text = "";
                txtPrecio.Text = "0.00";
                txtStock.Text = "0";
                txtCantidad.Value = 1;



            }
            else
            {
                // Lógica si algo falló
                MessageBox.Show(mensaje, "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }


        }

    }
}
