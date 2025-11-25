using CapaEntidad;
using CapaNegocio;
using CapaPresentacion.utilidades;
using DocumentFormat.OpenXml.Drawing;
using DocumentFormat.OpenXml.Drawing.Diagrams;
using DocumentFormat.OpenXml.Vml;
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

namespace CapaPresentacion
{
    public partial class frmRegistrarCompras : Form
    {
        private Usuario _usuarioActual;
        public frmRegistrarCompras(Usuario usuarioActual)
        {
            InitializeComponent();
            _usuarioActual = usuarioActual;
        }

        private void frmRegistrarCompras_Load(object sender, EventArgs e)
        {

            cboTipoDocumento.Items.Add(new OpcionCombo() { Valor = "Boleta", Texto = "Boleta" });
            cboTipoDocumento.Items.Add(new OpcionCombo() { Valor = "Factura", Texto = "Factura" });
            cboTipoDocumento.DisplayMember = "Texto";
            cboTipoDocumento.ValueMember = "Valor";
            cboTipoDocumento.SelectedIndex = 0;

            txtFecha.Text = DateTime.Now.ToString("dd/MM/yyyy");

            txtIdProveedor.Text = "0";
            txtIdProducto.Text = "0";

        }

        private void btnBuscarProveedor_Click(object sender, EventArgs e)
        {
            using (modales.mdProveedores mdProveedores = new modales.mdProveedores())
            {
                mdProveedores.ShowDialog();

                if (mdProveedores._Proveedor != null)
                {
                    txtIdProveedor.Text = mdProveedores._Proveedor.IdProveedor.ToString();
                    txtDocumentoProveedor.Text = mdProveedores._Proveedor.Documento;
                    txtRazonSocialProveedor.Text = mdProveedores._Proveedor.RazonSocial;
                }
                else
                {
                    txtIdProveedor.Text = "0";
                    txtDocumentoProveedor.Text = "";
                    txtRazonSocialProveedor.Text = "";

                    MessageBox.Show("No se seleccionó ningún proveedor", "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
                    txtCompraProducto.Text = mdProductos._Producto.PrecioCompra.ToString("0.00");
                    txtVentaProducto.Text = mdProductos._Producto.PrecioVenta.ToString("0.00");



                }
                else
                {
                    txtIdProducto.Text = "0";
                    txtCodigoProducto.Text = "";
                    txtCodigoProducto.BackColor = Color.White;
                    txtNombreProducto.Text = "";
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
                    txtCompraProducto.Select();
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
            decimal precioCompra;
            decimal precioVenta;
            bool Producto_existe = false;

            if (txtIdProducto.Text == "0")
            {
                MessageBox.Show("Debe seleccionar un Producto", "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            if (!decimal.TryParse(txtCompraProducto.Text, out precioCompra))
            {
                MessageBox.Show("Formato de moneda incorrecto", "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                txtCompraProducto.Select();
                return;
            }

            if (!decimal.TryParse(txtVentaProducto.Text, out precioVenta))
            {
                MessageBox.Show("Formato de moneda incorrecto", "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                txtVentaProducto.Select();
                return;
            }

            foreach (DataGridViewRow fila in dgvData.Rows)
            {

                if (fila.Cells["IdProducto"].Value?.ToString() == txtIdProducto.Text)
                {
                    Producto_existe = true;
                    break;
                }
            }


            if (Producto_existe)
            {
                MessageBox.Show("Ya agregaste este Producto a la lista", "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            else
            {
                dgvData.Rows.Add(new object[] {

                    txtIdProducto.Text,
                    txtNombreProducto.Text,
                    precioCompra.ToString("0.00"),
                    precioVenta.ToString("0.00"),
                    txtCantidad.Value.ToString(),
                    (txtCantidad.Value * precioCompra).ToString("0.00") // Agregué formato al total también
                });


                CalcularTotal();
                limpiarProducto();
                txtCodigoProducto.Select();

            }


        }

        private void limpiarProducto()
        {
            txtIdProducto.Text = "0";
            txtCodigoProducto.Text = "";
            txtCodigoProducto.BackColor = Color.White;
            txtNombreProducto.Text = "";
            txtCompraProducto.Text = "0.00";
            txtVentaProducto.Text = "0.00";
            txtCantidad.Value = 1;
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
            txtTotalaPagar.Text = total.ToString("0.00");
        }

        private void dgvData_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            if (e.RowIndex < 0)
                return;

            if (e.ColumnIndex == 6)
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
                    dgvData.Rows.RemoveAt(indice);
                    CalcularTotal();

                }


            }
        }



        private void txtCompraProducto_KeyPress(object sender, KeyPressEventArgs e)
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
                if (txtCompraProducto.Text.Contains("."))
                {
                    e.Handled = true;
                    return;
                }

                // B. Si el punto es el PRIMER caracter, lo bloqueamos (opcional, gusto personal)
                //    (para evitar que escriban ".50")
                if (txtCompraProducto.Text.Trim().Length == 0)
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

        private void txtCompraProducto_Leave(object sender, EventArgs e)
        {
            // Si el texto está vacío, pon 0.00
            if (string.IsNullOrEmpty(txtCompraProducto.Text))
            {
                txtCompraProducto.Text = "0.00";
            }
            else
            {
                decimal valor;
                // Intentamos convertir a decimal
                if (decimal.TryParse(txtCompraProducto.Text, out valor))
                {
                    // Si funciona, le damos formato de 2 decimales
                    txtCompraProducto.Text = valor.ToString("0.00");
                }
                else
                {
                    // Si escribió algo raro que se pasó el filtro (muy difícil), lo limpiamos
                    txtCompraProducto.Text = "0.00";
                }
            }
        }

        private void txtCompraProducto_Enter(object sender, EventArgs e)
        {
            // Selecciona todo el texto al recibir el foco
            txtCompraProducto.SelectAll();
        }

        private void txtCompraProducto_MouseClick(object sender, MouseEventArgs e)
        {
            // A veces el clic anula la selección del Enter, esto lo fuerza de nuevo
            txtCompraProducto.SelectAll();
        }





        private void txtVentaProducto_KeyPress(object sender, KeyPressEventArgs e)
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
                if (txtVentaProducto.Text.Contains("."))
                {
                    e.Handled = true;
                    return;
                }

                // B. Si el punto es el PRIMER caracter, lo bloqueamos (opcional, gusto personal)
                //    (para evitar que escriban ".50")
                if (txtVentaProducto.Text.Trim().Length == 0)
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

        private void txtVentaProducto_Leave(object sender, EventArgs e)
        {
            // Si el texto está vacío, pon 0.00
            if (string.IsNullOrEmpty(txtVentaProducto.Text))
            {
                txtVentaProducto.Text = "0.00";
            }
            else
            {
                decimal valor;
                // Intentamos convertir a decimal
                if (decimal.TryParse(txtVentaProducto.Text, out valor))
                {
                    // Si funciona, le damos formato de 2 decimales
                    txtVentaProducto.Text = valor.ToString("0.00");
                }
                else
                {
                    // Si escribió algo raro que se pasó el filtro (muy difícil), lo limpiamos
                    txtVentaProducto.Text = "0.00";
                }
            }
        }

        private void txtVentaProducto_Enter(object sender, EventArgs e)
        {
            // Selecciona todo el texto al recibir el foco
            txtVentaProducto.SelectAll();
        }

        private void txtVentaProducto_MouseClick(object sender, MouseEventArgs e)
        {
            // A veces el clic anula la selección del Enter, esto lo fuerza de nuevo
            txtVentaProducto.SelectAll();
        }

        private void btnRegistrar_Click(object sender, EventArgs e)
        {
            if (Convert.ToUInt32(txtIdProveedor.Text) == 0)
            {
                MessageBox.Show("Debe seleccionar un proveedor", "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (dgvData.Rows.Count < 1)
            {
                MessageBox.Show("Debe ingresar productos en la compra", "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            DataTable DetalleCompra = new DataTable();
            DetalleCompra.Columns.Add("IdProducto", typeof(int));
            DetalleCompra.Columns.Add("PrecioCompra", typeof(decimal));
            DetalleCompra.Columns.Add("PrecioVenta", typeof(decimal));
            DetalleCompra.Columns.Add("Cantidad", typeof(int));
            DetalleCompra.Columns.Add("MontoTotal", typeof(decimal));

            foreach (DataGridViewRow row in dgvData.Rows)
            {
                DetalleCompra.Rows.Add(
                new object[]
                {
                    Convert.ToUInt32( row.Cells["IdProducto"].Value.ToString()),
                    row.Cells["PrecioCompra"].Value.ToString(),
                    row.Cells["PrecioVenta"].Value.ToString(),
                    row.Cells["Cantidad"].Value.ToString(),
                    row.Cells["Subtotal"].Value.ToString(),



                });

            }

            int idCorrelativo = new CN_Compra().ObtenerCorrelativo();
            string numeroDocumento = string.Format("{0:00000}", idCorrelativo);

            Compra oCompra = new Compra()
            {
                oUsuario = new Usuario() { IdUsuario = _usuarioActual.IdUsuario },
                oProveedor = new Proveedor() { IdProveedor = Convert.ToInt32(txtIdProveedor.Text) },
                TipoDocumento = ((OpcionCombo)cboTipoDocumento.SelectedItem).Texto,
                NumeroDocumento = numeroDocumento,
                MontoTotal = Convert.ToDecimal(txtTotalaPagar.Text),

            };

            string Mensaje = string.Empty;
            bool respuesta = new CN_Compra().RegistrarComprar(oCompra, DetalleCompra, out Mensaje);

            if (respuesta)
            {
                var result = MessageBox.Show("Numero de compra generada" + numeroDocumento + "\n\nDesea copiar al portapapeles??", "Mensaje", MessageBoxButtons.YesNo, MessageBoxIcon.Information);

                if (result == DialogResult.Yes)
                {
                    Clipboard.SetText(numeroDocumento);
                }

                txtIdProveedor.Text = "0";
                txtDocumentoProveedor.Text = "";
                txtNombreProducto.Text = "";
                dgvData.Rows.Clear();
                CalcularTotal();
            }
            else
            {

            }
        }

    }
}


