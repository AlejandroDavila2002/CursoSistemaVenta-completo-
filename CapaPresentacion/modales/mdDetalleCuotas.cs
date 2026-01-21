using CapaEntidad;
using CapaNegocio;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace CapaPresentacion.modales
{
    public partial class mdlDetalleCuotas : Form
    {
        private int _idCuenta;
        private string _fechaVencimientoVenta;

        // Constructor modificado: Recibe ID Cuenta y la Fecha Límite de la Venta
        public mdlDetalleCuotas(int idCuenta, string fechaVencimiento)
        {
            InitializeComponent();
            _idCuenta = idCuenta;
            _fechaVencimientoVenta = fechaVencimiento;
        }

        private void mdlDetalleCuotas_Load(object sender, EventArgs e)
        {
            ConfigurarGrid();
            CargarHistorial();
        }

        private void ConfigurarGrid()
        {
            label2.Text = "Historial de Pagos Registrados";
        }

        private void CargarHistorial()
        {
            // Traemos los abonos reales desde la BD
            List<Abono> lista = new CN_CuentaPorCobrar().ListarAbonos(_idCuenta);
            dgvCuotas.DataSource = lista;

            // Ocultar ID
            if (dgvCuotas.Columns.Contains("IdAbono")) dgvCuotas.Columns["IdAbono"].Visible = false;

            // Encabezados
            if (dgvCuotas.Columns.Contains("FechaRegistro")) dgvCuotas.Columns["FechaRegistro"].HeaderText = "Fecha de Pago";
            if (dgvCuotas.Columns.Contains("Monto")) dgvCuotas.Columns["Monto"].HeaderText = "Monto Abonado";
            if (dgvCuotas.Columns.Contains("Nota")) dgvCuotas.Columns["Nota"].HeaderText = "Nota / Ref";

            dgvCuotas.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        }

        private void dgvCuotas_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            // PINTAR ROJO/VERDE SEGÚN FECHA
            if (dgvCuotas.Columns[e.ColumnIndex].Name == "FechaRegistro" && e.Value != null)
            {
                // 1. Obtener fecha del pago (Abono)
                DateTime fechaPago;
                if (!DateTime.TryParse(e.Value.ToString(), out fechaPago)) return;

                // 2. Obtener fecha de vencimiento de la venta
                DateTime fechaLimite;
                if (DateTime.TryParse(_fechaVencimientoVenta, out fechaLimite))
                {
                    // LÓGICA: Si pagó DESPUÉS de la fecha límite -> ROJO
                    if (fechaPago.Date > fechaLimite.Date)
                    {
                        e.CellStyle.BackColor = Color.LightCoral;
                        e.CellStyle.ForeColor = Color.Black;
                    }
                    else
                    {
                        // Pagó a tiempo -> VERDE
                        e.CellStyle.BackColor = Color.LightGreen;
                        e.CellStyle.ForeColor = Color.Black;
                    }
                }
            }
        }
    }
}