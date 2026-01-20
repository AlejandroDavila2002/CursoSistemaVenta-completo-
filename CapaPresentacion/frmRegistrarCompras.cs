using CapaEntidad;
using CapaNegocio;
using CapaPresentacion.modales;
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
            decimal tasaActual = _usuarioActual.oTasaGeneral != null ? _usuarioActual.oTasaGeneral.Valor : 0;

            if (tasaActual <= 0)
            {
                chkCompraEnBs.Checked = false;
                chkCompraEnBs.Enabled = false;
                chkCompraEnBs.Text = "Compra en Bs (No hay tasa definida)";
            }
            else
            {
                chkCompraEnBs.Enabled = true;
                chkCompraEnBs.Text = $"Compra en Bolívares (Convertir) ";
            }

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
                    txtDocumento.Text = mdProveedores._Proveedor.Documento;
                    txtRazonSocialProveedor.Text = mdProveedores._Proveedor.RazonSocial;
                }
                else
                {
                    txtIdProveedor.Text = "0";
                    txtDocumento.Text = "";
                    txtRazonSocialProveedor.Text = "";

                    MessageBox.Show("No se seleccionó ningún proveedor", "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }

            }
        }

        private void btnBuscarProducto_Click(object sender, EventArgs e)
        {
            using (var modal = new mdProductos())
            {
                // 1. CREAR LA LISTA DE EXCLUIDOS
                List<int> idsEnGrilla = new List<int>();

                // 2. RECORRER EL DATAGRIDVIEW ACTUAL
                foreach (DataGridViewRow row in dgvData.Rows)
                {
                    // Aseguramos que el valor no sea nulo antes de convertir
                    if (row.Cells["IdProducto"].Value != null)
                    {
                        idsEnGrilla.Add(Convert.ToInt32(row.Cells["IdProducto"].Value));
                    }
                }

                // 3. PASAR LA LISTA AL MODAL
                modal._ListaNegra = idsEnGrilla;

                var result = modal.ShowDialog();

                if (result == DialogResult.OK)
                {
                    // ... (asignación de ID, Nombre, etc.) ...
                    txtIdProducto.Text = modal._Producto.IdProducto.ToString();
                    txtCodigoProducto.Text = modal._Producto.Codigo;
                    txtNombreProducto.Text = modal._Producto.NombreProducto;

                    // --- CORRECCIÓN AQUÍ ---
                    decimal precioCompraBase = modal._Producto.PrecioCompra; // Viene en USD
                    decimal precioVentaBase = modal._Producto.PrecioVenta;   // Viene en USD
                    decimal tasa = _usuarioActual.oTasaGeneral.Valor;

                    // Preguntamos: ¿Está el modo Bolívares activado AHORA MISMO?
                    if (chkCompraEnBs.Checked)
                    {
                        // Si está activado, mostramos YA convertido en Bs
                        txtCompraProducto.Text = (precioCompraBase * tasa).ToString("N2");
                        txtVentaProducto.Text = (precioVentaBase * tasa).ToString("N2");
                    }
                    else
                    {
                        // Si no, mostramos en USD normal
                        txtCompraProducto.Text = precioCompraBase.ToString("N2");
                        txtVentaProducto.Text = precioVentaBase.ToString("N2");
                    }
                    // -----------------------

                    txtCantidad.Select();
                }
            }
        }

        private void txtCodigoProducto_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.Enter)
            {
                Producto oProducto = new CN_Producto().listar().Where(p => p.Codigo == txtCodigoProducto.Text && p.Estado == true).FirstOrDefault();

                if (oProducto != null)
                {
                    txtCodigoProducto.BackColor = Color.Honeydew;
                    txtIdProducto.Text = oProducto.IdProducto.ToString();
                    txtNombreProducto.Text = oProducto.NombreProducto;

                    // --- CORRECCIÓN AQUÍ ---
                    decimal precioCompraBase = oProducto.PrecioCompra;
                    decimal precioVentaBase = oProducto.PrecioVenta;
                    decimal tasa = _usuarioActual.oTasaGeneral.Valor;

                    if (chkCompraEnBs.Checked)
                    {
                        // Convertimos a Bs INMEDIATAMENTE al cargar
                        txtCompraProducto.Text = (precioCompraBase * tasa).ToString("N2");
                        txtVentaProducto.Text = (precioVentaBase * tasa).ToString("N2");
                    }
                    else
                    {
                        txtCompraProducto.Text = precioCompraBase.ToString("N2");
                        txtVentaProducto.Text = precioVentaBase.ToString("N2");
                    }
                    // -----------------------

                    txtCantidad.Select();
                }
                else
                {
                    txtCodigoProducto.BackColor = Color.MistyRose;
                    txtIdProducto.Text = "0";
                    txtNombreProducto.Text = "";
                    // Si no encuentra, limpiamos todo
                    txtCompraProducto.Text = "";
                    txtVentaProducto.Text = "";
                }
            }
        }

        private void btnAgregar_Click(object sender, EventArgs e)
        {
            decimal precioCompra = 0;
            decimal precioVenta = 0;
            bool Producto_existe = false;

            // Validaciones iniciales
            if (txtIdProducto.Text == "0")
            {
                MessageBox.Show("Debe seleccionar un Producto", "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            // 1. Validamos que sean números válidos
            if (!decimal.TryParse(txtCompraProducto.Text, out precioCompra) || !decimal.TryParse(txtVentaProducto.Text, out precioVenta))
            {
                MessageBox.Show("Formato de moneda incorrecto", "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                txtCompraProducto.Select();
                return;
            }

            // 2. Validamos que sean positivos
            if (precioCompra <= 0 || precioVenta <= 0)
            {
                MessageBox.Show("Los precios de Compra y Venta deben ser mayores a 0.", "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            // 3. Advertencia de Rentabilidad (Opcional pero útil)
            if (precioVenta < precioCompra)
            {
                DialogResult pregunta = MessageBox.Show(
                    $"El precio de venta ({precioVenta}) es menor al costo ({precioCompra}).\n¿Está seguro que desea registrar pérdida?",
                    "Advertencia de Rentabilidad",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Warning);

                if (pregunta == DialogResult.No) return;
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
                // Calculamos el subtotal con el valor visual actual
                decimal subtotal = precioCompra * txtCantidad.Value;

                dgvData.Rows.Add(new object[] {
                    txtIdProducto.Text,
                    txtNombreProducto.Text,
                    precioCompra.ToString("N2"), // Precio tal cual se ve en pantalla
                    precioVenta.ToString("N2"),  // Venta tal cual se ve en pantalla
                    txtCantidad.Value.ToString(),
                    subtotal.ToString("N2"),     // Subtotal tal cual se ve en pantalla
                    "" // Botón eliminar
                    });

                CalcularTotal();
                limpiarProducto(); // Asegúrate que tu método se llame LimpiarProducto o limpiarProducto (mayúscula/minúscula)
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
            txtTotalaPagar.Text = total.ToString("N2");
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
                    txtCompraProducto.Text = valor.ToString("N2");
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
                txtVentaProducto.Text = "N2";
            }
            else
            {
                decimal valor;
                // Intentamos convertir a decimal
                if (decimal.TryParse(txtVentaProducto.Text, out valor))
                {
                    // Si funciona, le damos formato de 2 decimales
                    txtVentaProducto.Text = valor.ToString("N2");
                }
                else
                {
                    // Si escribió algo raro que se pasó el filtro (muy difícil), lo limpiamos
                    txtVentaProducto.Text = "N2";
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

            decimal tasa = _usuarioActual.oTasaGeneral.Valor;
            bool esBolivares = chkCompraEnBs.Checked;

            foreach (DataGridViewRow row in dgvData.Rows)
            {
                // 1. Obtenemos los valores visuales (lo que ve el usuario)
                decimal dPrecioCompra = Convert.ToDecimal(row.Cells["PrecioCompra"].Value);
                decimal dPrecioVenta = Convert.ToDecimal(row.Cells["PrecioVenta"].Value);
                int dCantidad = Convert.ToInt32(row.Cells["Cantidad"].Value);
                decimal dSubTotal = Convert.ToDecimal(row.Cells["SubTotal"].Value);

                // 2. LOGICA DE CONVERSIÓN PARA BASE DE DATOS
                if (esBolivares)
                {
                    // Si el usuario ingresó Bolívares (ej. 4560), dividimos entre la tasa (380)
                    // para guardar Dólares en la BD (ej. 12$)
                    dPrecioCompra = dPrecioCompra / tasa;
                    dPrecioVenta = dPrecioVenta / tasa;
                    dSubTotal = dSubTotal / tasa;
                }

                // 3. Agregamos a la tabla los valores YA CONVERTIDOS A USD
                DetalleCompra.Rows.Add(
                    new object[] {
                        Convert.ToInt32(row.Cells["IdProducto"].Value),
                        dPrecioCompra,
                        dPrecioVenta,
                        dCantidad,
                        dSubTotal
                    }
                );
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
                EsCompraEnBs = chkCompraEnBs.Checked,
                TasaCambio = _usuarioActual.oTasaGeneral.Valor

            };

            string Mensaje = string.Empty;
            bool respuesta = new CN_Compra().RegistrarComprar(oCompra, DetalleCompra, out Mensaje);

            if (respuesta)
            {
                var result = MessageBox.Show("Numero de compra generada " + numeroDocumento + " \n\nDesea copiar al portapapeles??", "Mensaje", MessageBoxButtons.YesNo, MessageBoxIcon.Information);

                if (result == DialogResult.Yes)
                {
                    Clipboard.SetText(numeroDocumento);
                }

                txtIdProveedor.Text = "0";
                txtDocumento.Text = "";
                txtNombreProducto.Text = "";
                dgvData.Rows.Clear();
                CalcularTotal();
            }
            else
            {


            }
        }

        private void chkCompraEnBs_CheckedChanged(object sender, EventArgs e)
        {
            // 1. Obtener tasa y validar
            decimal tasa = _usuarioActual.oTasaGeneral.Valor;

            if (tasa == 0)
            {
                MessageBox.Show("No hay una tasa de cambio configurada", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                chkCompraEnBs.Checked = false;
                return;
            }

            // 2. Capturar valores actuales de los Inputs (Para convertirlos también)
            decimal precioCompraInput = 0;
            decimal precioVentaInput = 0;

            // Usamos TryParse para que no falle si el cuadro está vacío
            decimal.TryParse(txtCompraProducto.Text, out precioCompraInput);
            decimal.TryParse(txtVentaProducto.Text, out precioVentaInput);


            // 3. Lógica de Conversión Visual
            if (chkCompraEnBs.Checked)
            {
                // --- MODO BOLÍVARES ---

                // A. Etiquetas y Estilos
                lblCompra.Text = "Precio Compra (Bs):";
                lblVenta.Text = "Precio Venta (Bs):";
                txtCompraProducto.BackColor = Color.LightYellow;
                txtVentaProducto.BackColor = Color.LightYellow;

                // B. Cambiar el Label del Total (Requerimiento nuevo)
                label12.Text = "Total a Pagar Bs:";

                // C. Convertir los Inputs: De USD a Bs (Multiplicar)
                // Solo convertimos si tienen valores mayores a 0 para no llenar de ceros innecesarios
                if (precioCompraInput > 0)
                    txtCompraProducto.Text = (precioCompraInput * tasa).ToString("N2");

                if (precioVentaInput > 0)
                    txtVentaProducto.Text = (precioVentaInput * tasa).ToString("N2");

                // D. Convertir la Grilla
                RecalcularGrilla(true, tasa);
            }
            else
            {
                // --- MODO DÓLARES ---

                // A. Etiquetas y Estilos
                lblCompra.Text = "Precio Compra ($):";
                lblVenta.Text = "Precio Venta ($):";
                txtCompraProducto.BackColor = Color.White;
                txtVentaProducto.BackColor = Color.White;

                // B. Cambiar el Label del Total (Requerimiento nuevo)
                label12.Text = "Total a Pagar $:";

                // C. Convertir los Inputs: De Bs a USD (Dividir)
                if (precioCompraInput > 0)
                    txtCompraProducto.Text = (precioCompraInput / tasa).ToString("N2");

                if (precioVentaInput > 0)
                    txtVentaProducto.Text = (precioVentaInput / tasa).ToString("N2");

                // D. Convertir la Grilla
                RecalcularGrilla(false, tasa);
            }

            // 4. Recalcular el total final numérico
            CalcularTotal();
        }

        // MÉTODO AUXILIAR PARA RECALCULAR FILAS
        private void RecalcularGrilla(bool convertirABolivares, decimal tasa)
        {
            foreach (DataGridViewRow row in dgvData.Rows)
            {
                // Obtenemos los valores actuales de la celda
                decimal pCompra = Convert.ToDecimal(row.Cells["PrecioCompra"].Value);
                decimal pVenta = Convert.ToDecimal(row.Cells["PrecioVenta"].Value);
                decimal subTotal = Convert.ToDecimal(row.Cells["Subtotal"].Value);

                if (convertirABolivares)
                {
                    // USD -> BS (Multiplicamos)
                    row.Cells["PrecioCompra"].Value = (pCompra * tasa).ToString("N2");
                    row.Cells["PrecioVenta"].Value = (pVenta * tasa).ToString("N2");
                    row.Cells["SubTotal"].Value = (subTotal * tasa).ToString("N2");
                }
                else
                {
                    // BS -> USD (Dividimos)
                    row.Cells["PrecioCompra"].Value = (pCompra / tasa).ToString("N2");
                    row.Cells["PrecioVenta"].Value = (pVenta / tasa).ToString("N2");
                    row.Cells["SubTotal"].Value = (subTotal / tasa).ToString("N2");
                }
            }
        }


        private void dgvData_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            // Verificamos que se haya editado la columna de PrecioCompra o Cantidad
            if (dgvData.Columns[e.ColumnIndex].Name == "PrecioCompra" || dgvData.Columns[e.ColumnIndex].Name == "Cantidad")
            {
                DataGridViewRow row = dgvData.Rows[e.RowIndex];

                decimal precioCompra = 0;
                int cantidad = 0;

                // Validamos y obtenemos valores
                decimal.TryParse(row.Cells["PrecioCompra"].Value.ToString(), out precioCompra);
                int.TryParse(row.Cells["Cantidad"].Value.ToString(), out cantidad);

                // Calculamos nuevo SubTotal (funciona igual para Bs o USD)
                decimal subTotal = precioCompra * cantidad;
                row.Cells["SubTotal"].Value = subTotal.ToString("N2");

                // Actualizamos el Total Final
                CalcularTotal();
            }
        }


    }


}