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
        // Propiedades para retornar datos al formulario padre
        public decimal MontoRealIngresado { get; private set; }
        public string ObservacionIngresada { get; private set; }

        // Variables internas
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
            // 1. Mostrar Monto Teórico
            txtMontoTeorico.Text = _montoTeorico.ToString("N2");
            txtMontoReal.Text = "0.00";
            txtDiferencia.Text = (0 - _montoTeorico).ToString("N2");

            // 2. Configurar y Llenar el DataGridView
            ConfigurarColumnasGrid();
            dgvData.DataSource = _dtVentas;

            // Formato de colores para diferenciar monedas en el grid
            foreach (DataGridViewRow row in dgvData.Rows)
            {
                string moneda = row.Cells["TipoMoneda"].Value.ToString();
                if (moneda == "VES")
                {
                    row.Cells["MontoPago"].Style.ForeColor = Color.Green; // Bs en Verde
                }
            }

            txtMontoReal.Select(); // Enfocar para escribir rápido
        }

        private void ConfigurarColumnasGrid()
        {
            // Opcional: Si no creaste las columnas en el diseñador, 
            // asegúrate de que el AutoGenerateColumns esté en true o defínelas aquí.
            dgvData.AutoGenerateColumns = false;

            // Ejemplo de columnas manuales (Asegúrate de tenerlas en el diseñador o agregarlas aquí)
            if (dgvData.Columns.Count == 0)
            {
                dgvData.Columns.Add(new DataGridViewTextBoxColumn() { DataPropertyName = "FechaRegistro", HeaderText = "Hora" });
                dgvData.Columns.Add(new DataGridViewTextBoxColumn() { DataPropertyName = "NumeroDocumento", HeaderText = "N° Venta" });
                dgvData.Columns.Add(new DataGridViewTextBoxColumn() { DataPropertyName = "NombreCliente", HeaderText = "Cliente", Width = 150 });
                dgvData.Columns.Add(new DataGridViewTextBoxColumn() { DataPropertyName = "TipoMoneda", HeaderText = "Moneda", Width = 50 });
                dgvData.Columns.Add(new DataGridViewTextBoxColumn() { DataPropertyName = "MontoPago", HeaderText = "Monto Orig." });
                dgvData.Columns.Add(new DataGridViewTextBoxColumn() { DataPropertyName = "TasaCambio", HeaderText = "Tasa" });
                dgvData.Columns.Add(new DataGridViewTextBoxColumn() { DataPropertyName = "MontoUSD", HeaderText = "Total USD" });
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

            // Feedback Visual: Rojo si falta dinero, Verde/Negro si sobra o cuadra
            if (diferencia < 0)
            {
                txtDiferencia.BackColor = Color.MistyRose;
                txtDiferencia.ForeColor = Color.Red;
            }
            else
            {
                txtDiferencia.BackColor = Color.Honeydew;
                txtDiferencia.ForeColor = Color.Green;
            }
        }

        private void btnAceptar_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtMontoReal.Text))
            {
                MessageBox.Show("Por favor ingrese el monto real contado en caja.", "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            decimal montoReal = 0;
            if (!decimal.TryParse(txtMontoReal.Text, out montoReal))
            {
                MessageBox.Show("Formato de moneda incorrecto.", "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            // Validar si hay diferencia y NO hay observación
            decimal diferencia = montoReal - _montoTeorico;
            if (diferencia != 0 && string.IsNullOrWhiteSpace(txtObservacion.Text))
            {
                MessageBox.Show("Existe una diferencia en la caja. Por favor ingrese una observación justificando el sobrante o faltante.", "Observación Requerida", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Guardar datos para que el padre los recoja
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

        // Validación para solo escribir números y un punto decimal
        private void txtMontoReal_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && (e.KeyChar != '.'))
            {
                e.Handled = true;
            }
            // Solo permitir un punto decimal
            if ((e.KeyChar == '.') && ((sender as TextBox).Text.IndexOf('.') > -1))
            {
                e.Handled = true;
            }
        }
    }
}