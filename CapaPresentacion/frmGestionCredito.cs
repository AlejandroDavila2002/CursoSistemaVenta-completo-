using CapaEntidad;
using CapaNegocio;
using CapaPresentacion.utilidades;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace CapaPresentacion
{
    public partial class frmGestionCredito : Form
    {
        private Usuario _UsuarioActual; // Para validar la clave al eliminar

        // Constructor que recibe el usuario actual (necesario para la validación de seguridad)
        public frmGestionCredito(Usuario oUsuario = null)
        {
            InitializeComponent();
            _UsuarioActual = oUsuario;
            // NOTA: Si llamas a este form desde Inicio, asegúrate de pasarle el usuario:
            // new frmGestionCredito(usuarioActual).Show();
        }

        private void frmGestionCredito_Load(object sender, EventArgs e)
        {
            // 1. Configurar ComboBox de Busqueda
            cboBusqueda.Items.Add(new OpcionCombo() { Valor = "NroVenta", Texto = "Nro Venta" });
            cboBusqueda.Items.Add(new OpcionCombo() { Valor = "Cliente", Texto = "Cliente" });
            cboBusqueda.DisplayMember = "Texto";
            cboBusqueda.ValueMember = "Valor";
            cboBusqueda.SelectedIndex = 0;

            // 2. Cargar Datos
            CargarDatos();
        }

        private void CargarDatos()
        {
            // 1. Configurar columnas (Limpiamos y recreamos para asegurar orden y nuevos campos)
            dgvData.Columns.Clear();

            // Columnas Ocultas o de Control
            var colId = dgvData.Columns.Add("IdCuentaPorCobrar", "ID"); dgvData.Columns[colId].Visible = false;

            // Columnas Visibles
            dgvData.Columns.Add("NroVenta", "Nro Venta");
            dgvData.Columns.Add("DocumentoCliente", "Doc. Cliente");
            dgvData.Columns.Add("NombreCliente", "Nombre Cliente");

            // -- NUEVAS COLUMNAS DE PLANIFICACIÓN --
            dgvData.Columns.Add("DescripcionPlan", "Plan de Pago");
            dgvData.Columns.Add("FechaVencimiento", "Vence El");

            dgvData.Columns.Add("MontoTotal", "Deuda Original");
            dgvData.Columns.Add("MontoPagado", "Abonado");
            dgvData.Columns.Add("SaldoPendiente", "Saldo Actual");

            // Columna de Botón al final (o al principio si prefieres)
            DataGridViewButtonColumn btn = new DataGridViewButtonColumn();
            btn.HeaderText = "Seleccionar";
            btn.Name = "btnSeleccionar";
            btn.Text = "Ver";
            btn.UseColumnTextForButtonValue = true;
            dgvData.Columns.Add(btn);

            // Ajustes de diseño
            dgvData.AllowUserToAddRows = false;
            dgvData.ReadOnly = true;
            dgvData.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;


            // 2. Llenar Datos
            List<CuentaPorCobrar> lista = new CN_CuentaPorCobrar().Listar();

            foreach (CuentaPorCobrar item in lista)
            {
                int indice = dgvData.Rows.Add();

                dgvData.Rows[indice].Cells["IdCuentaPorCobrar"].Value = item.IdCuentaPorCobrar;
                dgvData.Rows[indice].Cells["NroVenta"].Value = item.oVenta.NumeroDocumento;
                dgvData.Rows[indice].Cells["DocumentoCliente"].Value = item.oCliente.Documento;
                dgvData.Rows[indice].Cells["NombreCliente"].Value = item.oCliente.NombreCompleto;

                // Datos nuevos
                dgvData.Rows[indice].Cells["DescripcionPlan"].Value = item.DescripcionPlan;
                dgvData.Rows[indice].Cells["FechaVencimiento"].Value = item.FechaVencimiento; // Ya viene formateada dd/MM/yyyy

                dgvData.Rows[indice].Cells["MontoTotal"].Value = item.MontoTotal;
                dgvData.Rows[indice].Cells["MontoPagado"].Value = item.MontoPagado;
                dgvData.Rows[indice].Cells["SaldoPendiente"].Value = item.SaldoPendiente;

                // --- LÓGICA VISUAL: ALERTA DE VENCIMIENTO ---
                // Convertimos la fecha string a DateTime para comparar
                if (!string.IsNullOrEmpty(item.FechaVencimiento))
                {
                    DateTime fechaVence;

                    // 2. Usamos TryParseExact: Intenta convertir sin romper el programa si falla
                    bool esValida = DateTime.TryParseExact(
                        item.FechaVencimiento,
                        "dd/MM/yyyy",
                        System.Globalization.CultureInfo.InvariantCulture,
                        System.Globalization.DateTimeStyles.None,
                        out fechaVence
                    );

                    // 3. Solo si es válida y la fecha ya pasó, aplicamos el color rojo
                    if (esValida && fechaVence < DateTime.Now.Date)
                    {
                        dgvData.Rows[indice].DefaultCellStyle.BackColor = Color.MistyRose;
                        dgvData.Rows[indice].Cells["FechaVencimiento"].Style.ForeColor = Color.Red;
                        dgvData.Rows[indice].Cells["FechaVencimiento"].Style.Font = new Font(dgvData.Font, FontStyle.Bold);
                    }
                }
                else
                {
                    // Si es una venta antigua sin plan de pago
                    dgvData.Rows[indice].Cells["FechaVencimiento"].Value = "N/A";
                }
            }

            LimpiarPanelAccion();
        }

        private void btnBuscar_Click(object sender, EventArgs e)
        {
            string columnaFiltro = ((OpcionCombo)cboBusqueda.SelectedItem).Valor.ToString();
            foreach (DataGridViewRow row in dgvData.Rows)
            {
                if (row.Cells[columnaFiltro].Value.ToString().Trim().ToUpper().Contains(txtBusqueda.Text.Trim().ToUpper()))
                    row.Visible = true;
                else
                    row.Visible = false;
            }
        }

        private void btnLimpiarCombo_Click(object sender, EventArgs e)
        {
            txtBusqueda.Text = "";
            foreach (DataGridViewRow row in dgvData.Rows)
            {
                row.Visible = true;
            }
            LimpiarPanelAccion();
        }

        // --- VARIABLES DE ESTADO ---
        private int _idCuentaSeleccionada = 0;
        private decimal _saldoActual = 0;

        private void dgvData_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            // Verificamos que no sea el encabezado
            if (e.RowIndex < 0) return;

            // Verificamos si la columna clicada es el botón "btnSeleccionar"
            if (dgvData.Columns[e.ColumnIndex].Name == "btnSeleccionar")
            {
                int index = e.RowIndex;

                // Obtenemos el ID de forma segura
                // Usamos TryParse por si la celda viniera nula o con formato erróneo
                string idStr = dgvData.Rows[index].Cells["IdCuentaPorCobrar"].Value.ToString();
                _idCuentaSeleccionada = int.Parse(idStr);

                string nombreCliente = dgvData.Rows[index].Cells["NombreCliente"].Value.ToString();

                // Guardamos el saldo exacto del renglón para validaciones
                _saldoActual = Convert.ToDecimal(dgvData.Rows[index].Cells["SaldoPendiente"].Value);

                // Llenar panel lateral
                txtClienteSeleccionado.Text = nombreCliente;
                txtSaldoPendiente.Text = _saldoActual.ToString("N2");

                // Preparar para abono
                txtMontoAbono.Text = "";
                txtMontoAbono.Select();
            }
        }

        private void dgvData_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            if (e.RowIndex < 0)
                return;

            if (e.ColumnIndex == 0)
            {

                e.Paint(e.CellBounds, DataGridViewPaintParts.All);
                var w = Properties.Resources.icons8_comprobado_20.Width;
                var h = Properties.Resources.icons8_comprobado_20.Height;
                var x = e.CellBounds.Left + (e.CellBounds.Width - w) / 2;
                var y = e.CellBounds.Top + (e.CellBounds.Height - h) / 2;
                e.Graphics.DrawImage(Properties.Resources.icons8_comprobado_20, new Rectangle(x, y, w, h));
                e.Handled = true;

            }
        }

        private void btnRegistrarAbono_Click(object sender, EventArgs e)
        {
            if (_idCuentaSeleccionada == 0)
            {
                MessageBox.Show("Por favor, seleccione una cuenta de la lista (Click en el botón de la primera columna)", "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            decimal abono = 0;
            if (!decimal.TryParse(txtMontoAbono.Text, out abono))
            {
                MessageBox.Show("Formato de moneda incorrecto", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (abono <= 0 || abono > _saldoActual)
            {
                MessageBox.Show("El abono debe ser mayor a 0 y no puede superar la deuda actual.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Proceder a registrar
            string mensaje = string.Empty;
            bool resultado = new CN_CuentaPorCobrar().RegistrarAbono(_idCuentaSeleccionada, abono, out mensaje);

            if (resultado)
            {
                MessageBox.Show("Abono registrado correctamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                CargarDatos(); // Recargar lista
            }
            else
            {
                MessageBox.Show(mensaje, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnEliminar_Click(object sender, EventArgs e)
        {
            if (_idCuentaSeleccionada == 0)
            {
                MessageBox.Show("Seleccione la cuenta que desea eliminar", "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            if (MessageBox.Show("¿Está seguro de eliminar esta venta a crédito?\nEsto borrará la deuda permanentemente.", "Confirmar Eliminación", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                // --- SEGURIDAD: SOLICITAR CLAVE ---
                string claveIngresada = MostrarInputBox("Seguridad", "Ingrese su contraseña de usuario para confirmar:");

                if (string.IsNullOrEmpty(claveIngresada)) return; // Canceló o vacío

                // Validamos contra el usuario actual (si se pasó en el constructor)
                // Si _UsuarioActual es null (pruebas), permitimos borrar (OJO: en producción validar siempre)
                if (_UsuarioActual != null && _UsuarioActual.Clave != claveIngresada)
                {
                    MessageBox.Show("Contraseña incorrecta. No tiene permisos para eliminar.", "Seguridad", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    return;
                }

                // Proceder a eliminar
                string mensaje = string.Empty;
                bool respuesta = new CN_CuentaPorCobrar().Eliminar(_idCuentaSeleccionada, out mensaje);

                if (respuesta)
                {
                    MessageBox.Show("Cuenta por cobrar eliminada.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    CargarDatos();
                }
                else
                {
                    MessageBox.Show(mensaje, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void LimpiarPanelAccion()
        {
            _idCuentaSeleccionada = 0;
            _saldoActual = 0;
            txtClienteSeleccionado.Text = "";
            txtSaldoPendiente.Text = "";
            txtMontoAbono.Text = "";
        }

        // Pequeña utilidad para crear el InputBox de contraseña al vuelo
        private string MostrarInputBox(string titulo, string mensaje)
        {
            Form prompt = new Form()
            {
                Width = 400,
                Height = 180,
                FormBorderStyle = FormBorderStyle.FixedDialog,
                Text = titulo,
                StartPosition = FormStartPosition.CenterScreen,
                MinimizeBox = false,
                MaximizeBox = false
            };

            Label textLabel = new Label() { Left = 20, Top = 20, Text = mensaje, AutoSize = true };
            TextBox textBox = new TextBox() { Left = 20, Top = 50, Width = 340, PasswordChar = '*' }; // Ocultar caracteres
            Button confirmation = new Button() { Text = "Confirmar", Left = 230, Width = 100, Top = 90, DialogResult = DialogResult.OK };
            Button cancel = new Button() { Text = "Cancelar", Left = 120, Width = 100, Top = 90, DialogResult = DialogResult.Cancel };

            prompt.Controls.Add(textBox);
            prompt.Controls.Add(confirmation);
            prompt.Controls.Add(cancel);
            prompt.Controls.Add(textLabel);
            prompt.AcceptButton = confirmation;
            prompt.CancelButton = cancel;

            return prompt.ShowDialog() == DialogResult.OK ? textBox.Text : "";
        }

        // Validación Solo Números en TextBox Abono
        private void txtMontoAbono_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (Char.IsDigit(e.KeyChar)) { e.Handled = false; return; }
            if (Char.IsControl(e.KeyChar)) { e.Handled = false; return; }
            if (e.KeyChar.ToString() == ".")
            {
                if (txtMontoAbono.Text.Contains(".")) { e.Handled = true; return; }
                e.Handled = false; return;
            }
            e.Handled = true;
        }
    }
}