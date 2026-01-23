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

            CalcularDiferencia();

            // 2. Configurar Grid (Columnas)
            ConfigurarColumnasGrid();

            // --- NUEVO: Aumentar Fuente y Tamaño ---
            PersonalizarFuente();
            // ---------------------------------------

            // 3. Cargar Datos al Grid (Ahora las filas se crearán con la nueva altura)
            dgvData.DataSource = _dtVentas;

            // 4. Aplicar Colores (Se mantiene igual)
            PintarColores();

            txtMontoReal.Select();
        }


        private void ConfigurarColumnasGrid()
        {
            dgvData.AutoGenerateColumns = false;
            dgvData.Columns.Clear();

            // 1. Hora
            dgvData.Columns.Add(new DataGridViewTextBoxColumn()
            {
                Name = "Hora",
                DataPropertyName = "Hora",
                HeaderText = "Hora",
                Width = 60
            });

            // 2. Descripción (Tipo)
            dgvData.Columns.Add(new DataGridViewTextBoxColumn()
            {
                Name = "Tipo",
                DataPropertyName = "Tipo",
                HeaderText = "Descripción",
                Width = 80
            });

            // 3. Código / Referencia
            dgvData.Columns.Add(new DataGridViewTextBoxColumn()
            {
                Name = "NumeroDocumento",
                DataPropertyName = "NumeroDocumento",
                HeaderText = "Código / Ref",
                Width = 100
            });

            // 4. Cliente
            dgvData.Columns.Add(new DataGridViewTextBoxColumn()
            {
                Name = "NombreCliente",
                DataPropertyName = "NombreCliente",
                HeaderText = "Cliente",
                Width = 150
            });

            // 5. Divisa
            dgvData.Columns.Add(new DataGridViewTextBoxColumn()
            {
                Name = "TipoMoneda",
                DataPropertyName = "TipoMoneda",
                HeaderText = "Div.",
                Width = 40
            });

            // 6. Monto Original (FORMATO N2)
            dgvData.Columns.Add(new DataGridViewTextBoxColumn()
            {
                Name = "MontoOriginal",
                DataPropertyName = "MontoOriginal",
                HeaderText = "Monto Orig.",
                Width = 90,
                DefaultCellStyle = new DataGridViewCellStyle { Format = "N2", Alignment = DataGridViewContentAlignment.MiddleRight } // Alineado a la derecha se ve mejor
            });

            // 7. Tasa (También le ponemos formato para uniformidad)
            dgvData.Columns.Add(new DataGridViewTextBoxColumn()
            {
                Name = "TasaUtilizada",
                DataPropertyName = "TasaUtilizada",
                HeaderText = "Tasa",
                Width = 50,
                DefaultCellStyle = new DataGridViewCellStyle { Format = "N2", Alignment = DataGridViewContentAlignment.MiddleRight }
            });

            // 8. Total USD (FORMATO N2)
            dgvData.Columns.Add(new DataGridViewTextBoxColumn()
            {
                Name = "MontoCalculadoUSD",
                DataPropertyName = "MontoCalculadoUSD",
                HeaderText = "Total USD",
                Width = 90,
                DefaultCellStyle = new DataGridViewCellStyle { Format = "N2", Alignment = DataGridViewContentAlignment.MiddleRight }
            });
        }

        private void PintarColores()
        {
            foreach (DataGridViewRow row in dgvData.Rows)
            {
                // Validamos que la fila tenga datos
                if (row.Cells["Tipo"].Value != null)
                {
                    string tipo = row.Cells["Tipo"].Value.ToString();

                    // Obtenemos la moneda con seguridad
                    string moneda = "";
                    if (row.Cells["TipoMoneda"].Value != null)
                        moneda = row.Cells["TipoMoneda"].Value.ToString();

                    if (tipo == "ABONO")
                    {
                        // ESTILO ABONOS: Limpio, solo texto en Naranja
                        // Pintamos las columnas numéricas y la moneda
                        row.Cells["MontoOriginal"].Style.ForeColor = Color.DarkOrange;
                        row.Cells["TipoMoneda"].Style.ForeColor = Color.DarkOrange;

                        // Pintamos y resaltamos la descripción "ABONO"
                        row.Cells["Tipo"].Style.ForeColor = Color.DarkOrange;
                        row.Cells["Tipo"].Style.Font = new Font(dgvData.Font, FontStyle.Bold);

                        // Resto de la fila en color normal (Negro)
                        row.Cells["NumeroDocumento"].Style.ForeColor = Color.Black;
                        row.Cells["NombreCliente"].Style.ForeColor = Color.Black;
                    }
                    else
                    {
                        // ESTILO VENTAS: Verde (VES) o Azul (USD)
                        if (moneda == "VES")
                        {
                            row.Cells["MontoOriginal"].Style.ForeColor = Color.Green;
                            row.Cells["TipoMoneda"].Style.ForeColor = Color.Green;
                        }
                        else
                        {
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


        private void PersonalizarFuente()
        {
            // 1. Fuente para las celdas normales (Tamaño 10)
            dgvData.DefaultCellStyle.Font = new Font("Microsoft Sans Serif", 10, FontStyle.Regular);

            // 2. Fuente para los encabezados (Tamaño 10 y Negrita)
            dgvData.ColumnHeadersDefaultCellStyle.Font = new Font("Microsoft Sans Serif", 10, FontStyle.Bold);

            // 3. Aumentar la altura de las filas para que el texto "respire"
            dgvData.RowTemplate.Height = 32;

            // 4. Aumentar la altura de la cabecera
            dgvData.ColumnHeadersHeight = 35;
        }
    }
}