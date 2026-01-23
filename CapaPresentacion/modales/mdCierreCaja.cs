using CapaEntidad;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace CapaPresentacion.modales
{
    public partial class mdCierreCaja : Form
    {
        // Propiedades de retorno
        public decimal MontoRealIngresado { get; private set; }
        public string ObservacionIngresada { get; private set; }

        private decimal _montoTeorico;
        private DataTable _dtVentas;

        public mdCierreCaja(decimal montoTeorico, DataTable dtVentas)
        {
            InitializeComponent();
            _montoTeorico = montoTeorico;
            _dtVentas = dtVentas;
        }

        private void mdCierreCaja_Load(object sender, EventArgs e)
        {
            // 1. Cargar Datos Numéricos
            txtMontoTeorico.Text = _montoTeorico.ToString("N2");
            txtMontoReal.Text = "0.00";

            // La diferencia inicia en negativo porque el real es 0. 
            // (Es decir: "Faltan" 500 porque no has contado nada aún).
            CalcularDiferencia();

            // 2. Configurar Grid (CRÍTICO: Definir columnas antes de cargar datos)
            ConfigurarColumnasGrid();

            // 3. Cargar Datos al Grid
            dgvData.DataSource = _dtVentas;

            // 4. Aplicar Colores (Ahora sí funcionará porque las columnas tienen Nombre)
            PintarColores();

            txtMontoReal.Select();
        }

        private void ConfigurarColumnasGrid()
        {
            dgvData.AutoGenerateColumns = false;
            dgvData.Columns.Clear();

            // 1. Columna TIPO (Vital para diferenciar colores). 
            // La ponemos Visible = false si no quieres que ocupe espacio visual, 
            // pero debe estar ahí para que la lógica de colores funcione.
            dgvData.Columns.Add(new DataGridViewTextBoxColumn()
            {
                Name = "Tipo",
                DataPropertyName = "Tipo",
                Visible = false
            });

            // 2. Hora
            dgvData.Columns.Add(new DataGridViewTextBoxColumn()
            {
                Name = "Hora",
                DataPropertyName = "Hora",
                HeaderText = "Hora",
                Width = 60
            });

            // 3. Documento / Descripción (Aquí saldrá el Nro Factura o "ABONO CREDITO")
            dgvData.Columns.Add(new DataGridViewTextBoxColumn()
            {
                Name = "NumeroDocumento",
                DataPropertyName = "NumeroDocumento",
                HeaderText = "Descripción",
                Width = 120 // Un poco más ancho
            });

            // 4. Cliente
            dgvData.Columns.Add(new DataGridViewTextBoxColumn()
            {
                Name = "NombreCliente",
                DataPropertyName = "NombreCliente",
                HeaderText = "Cliente",
                Width = 140
            });

            // 5. Moneda
            dgvData.Columns.Add(new DataGridViewTextBoxColumn()
            {
                Name = "TipoMoneda",
                DataPropertyName = "TipoMoneda",
                HeaderText = "Div.",
                Width = 40
            });

            // 6. Monto Original
            dgvData.Columns.Add(new DataGridViewTextBoxColumn()
            {
                Name = "MontoOriginal",
                DataPropertyName = "MontoOriginal",
                HeaderText = "Monto Orig.",
                Width = 90
            });

            // 7. Tasa
            dgvData.Columns.Add(new DataGridViewTextBoxColumn()
            {
                Name = "TasaUtilizada",
                DataPropertyName = "TasaUtilizada",
                HeaderText = "Tasa",
                Width = 50
            });

            // 8. Total USD
            dgvData.Columns.Add(new DataGridViewTextBoxColumn()
            {
                Name = "MontoCalculadoUSD",
                DataPropertyName = "MontoCalculadoUSD",
                HeaderText = "Total USD",
                Width = 90
            });
        }

        private void PintarColores()
        {
            foreach (DataGridViewRow row in dgvData.Rows)
            {
                // Validamos que la fila tenga datos para evitar errores
                if (row.Cells["Tipo"].Value != null)
                {
                    string tipo = row.Cells["Tipo"].Value.ToString();

                    // Verificamos moneda con seguridad (por si viene nula)
                    string moneda = "";
                    if (row.Cells["TipoMoneda"].Value != null)
                        moneda = row.Cells["TipoMoneda"].Value.ToString();

                    // LÓGICA DE COLORES
                    if (tipo == "ABONO")
                    {
                        // CASO 1: ES UN ABONO (Fondo Naranja Claro)
                        // Usamos BackColor para resaltar toda la fila
                        row.DefaultCellStyle.BackColor = Color.FromArgb(255, 240, 220);
                        row.DefaultCellStyle.SelectionBackColor = Color.FromArgb(255, 200, 150);

                        // Ponemos la letra en un color fuerte para contraste
                        row.DefaultCellStyle.ForeColor = Color.Chocolate;

                        // Negrita para que destaque
                        row.Cells["NumeroDocumento"].Style.Font = new Font(dgvData.Font, FontStyle.Bold);
                    }
                    else
                    {
                        // CASO 2: ES UNA VENTA (Fondo Blanco normal)

                        if (moneda == "VES")
                        {
                            // Venta en Bs -> Texto VERDE
                            row.Cells["MontoOriginal"].Style.ForeColor = Color.Green;
                            row.Cells["TipoMoneda"].Style.ForeColor = Color.Green;
                        }
                        else
                        {
                            // Venta en USD -> Texto AZUL
                            row.Cells["MontoOriginal"].Style.ForeColor = Color.Blue;
                            row.Cells["TipoMoneda"].Style.ForeColor = Color.Blue;
                        }
                    }
                }
            }
        }

        private void txtMontoReal_TextChanged(object sender, EventArgs e)
        {
            CalcularDiferencia();
        }

        private void CalcularDiferencia()
        {
            decimal montoReal = 0;
            decimal.TryParse(txtMontoReal.Text, out montoReal);

            decimal diferencia = montoReal - _montoTeorico;
            txtDiferencia.Text = diferencia.ToString("N2");

            // Feedback Visual:
            if (diferencia < 0) // Faltante
            {
                txtDiferencia.BackColor = Color.MistyRose; // Fondo Rojo Claro
                txtDiferencia.ForeColor = Color.Red;
            }
            else if (diferencia > 0) // Sobrante
            {
                txtDiferencia.BackColor = Color.Honeydew; // Fondo Verde Claro
                txtDiferencia.ForeColor = Color.Green;
            }
            else // Cuadre Exacto
            {
                txtDiferencia.BackColor = Color.White;
                txtDiferencia.ForeColor = Color.Black;
            }
        }

        private void txtMontoReal_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && (e.KeyChar != '.'))
            {
                e.Handled = true;
            }
            if ((e.KeyChar == '.') && ((sender as TextBox).Text.IndexOf('.') > -1))
            {
                e.Handled = true;
            }
        }

        private void btnAceptar_Click(object sender, EventArgs e)
        {
            // Validaciones
            decimal montoReal = 0;
            if (!decimal.TryParse(txtMontoReal.Text, out montoReal))
            {
                MessageBox.Show("Formato de moneda incorrecto.", "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            // Exigir Observación si no cuadra
            decimal diferencia = montoReal - _montoTeorico;
            if (diferencia != 0 && string.IsNullOrWhiteSpace(txtObservacion.Text))
            {
                MessageBox.Show("Existe una diferencia en caja.\nPor favor ingrese una observación explicando el motivo.", "Observación Requerida", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            MontoRealIngresado = montoReal;
            ObservacionIngresada = txtObservacion.Text;

            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
}