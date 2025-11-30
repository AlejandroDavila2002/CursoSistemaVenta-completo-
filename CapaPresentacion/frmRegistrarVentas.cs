using CapaEntidad;
using CapaNegocio;
using CapaPresentacion.modales;
using CapaPresentacion.utilidades;
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
                if(result == DialogResult.OK)
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

            if(int.Parse(txtIdProducto.Text) == 0)
            {
                MessageBox.Show("Debe seleccionar un mensaje", "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            if(!decimal.TryParse(txtPrecio.Text, out precio))
            {
                MessageBox.Show("Precio o formato de venta incorrecto", "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            if(Convert.ToInt32(txtStock.Text) < Convert.ToInt32(txtCantidad.Text))
            {
                MessageBox.Show("La cantidad no puede ser mayor al Stock", "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            foreach(DataGridViewRow row in dgvData.Rows)
            {
                if (row.Cells["IdProducto"].Value.ToString() == txtIdProducto.Text)
                {
                    productoExiste = true;
                    break;
                }
            }


            if (!productoExiste)
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
                    dgvData.Rows.RemoveAt(indice);
                    CalcularTotal();

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

    }
}
