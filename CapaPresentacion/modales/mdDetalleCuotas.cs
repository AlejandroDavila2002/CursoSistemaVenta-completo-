using CapaEntidad;
using CapaNegocio; // Referencia correcta
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace CapaPresentacion.modales
{
    public partial class mdlDetalleCuotas : Form
    {
        private int _idVenta;

        // Constructor
        public mdlDetalleCuotas(int idVenta, string fechaVencimiento)
        {
            InitializeComponent();
            _idVenta = idVenta; // Asignamos
        }

        private void mdlDetalleCuotas_Load(object sender, EventArgs e)
        {
            ConfigurarGrid();
            CargarHistorial();
        }

        private void ConfigurarGrid()
        {
            label2.Text = "Detalle de Cuotas del Crédito";
        }

        private void CargarHistorial()
        {
            // CORRECCIÓN 1: Llamamos a la Capa de NEGOCIO, no a Datos
            List<Cuota> lista = new CN_Cuota().ListarPorVenta(_idVenta);

            dgvCuotas.DataSource = lista;

            // Ocultar columnas técnicas
            string[] columnasOcultas = { "IdCuota", "IdVenta", "IdCuentaPorCobrar" };
            foreach (string col in columnasOcultas)
            {
                if (dgvCuotas.Columns.Contains(col)) dgvCuotas.Columns[col].Visible = false;
            }

            // Renombrar encabezados para el usuario
            if (dgvCuotas.Columns.Contains("NumeroCuota"))
            {
                dgvCuotas.Columns["NumeroCuota"].HeaderText = "Nro";
                dgvCuotas.Columns["NumeroCuota"].Width = 40;
            }
            if (dgvCuotas.Columns.Contains("FechaProgramada")) dgvCuotas.Columns["FechaProgramada"].HeaderText = "Fecha Vencimiento";
            if (dgvCuotas.Columns.Contains("MontoCuota")) dgvCuotas.Columns["MontoCuota"].HeaderText = "Monto";
            if (dgvCuotas.Columns.Contains("Estado")) dgvCuotas.Columns["Estado"].HeaderText = "Estado";
            if (dgvCuotas.Columns.Contains("FechaPago")) dgvCuotas.Columns["FechaPago"].HeaderText = "Pagado El";

            dgvCuotas.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        }

        // --- FUSIÓN DE LÓGICA DE COLORES ---
        private void dgvCuotas_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            // Aplicamos el formato a toda la fila para que se vea claro
            if (dgvCuotas.Columns[e.ColumnIndex].Name == "Estado" && e.Value != null)
            {
                string estado = e.Value.ToString();
                DataGridViewRow row = dgvCuotas.Rows[e.RowIndex];

                // 1. Obtenemos las fechas clave de la fila actual
                DateTime fechaVencimiento; // La fecha limite de ESTA cuota
                DateTime.TryParse(row.Cells["FechaProgramada"].Value.ToString(), out fechaVencimiento);

                // CASO A: CUOTA YA PAGADA
                if (estado == "Pagada")
                {
                    DateTime fechaPago;
                    DateTime.TryParse(row.Cells["FechaPago"].Value.ToString(), out fechaPago);

                    // Lógica del Usuario: ¿Pagó después de la fecha?
                    if (fechaPago.Date > fechaVencimiento.Date)
                    {
                        // Pagó, pero TARDE (Rojo claro / Alerta)
                        row.DefaultCellStyle.BackColor = Color.LightCoral;
                        row.DefaultCellStyle.ForeColor = Color.Black;
                    }
                    else
                    {
                        // Pagó A TIEMPO (Verde Éxito)
                        row.DefaultCellStyle.BackColor = Color.LightGreen;
                        row.DefaultCellStyle.ForeColor = Color.Black;
                    }
                }
                // CASO B: CUOTA PENDIENTE
                else
                {
                    // Mi Lógica: ¿Ya se venció y no ha pagado?
                    if (DateTime.Now.Date > fechaVencimiento.Date)
                    {
                        // MOROSO (Salmón Intenso)
                        row.DefaultCellStyle.BackColor = Color.Salmon;
                        row.DefaultCellStyle.ForeColor = Color.White;
                        row.DefaultCellStyle.Font = new Font(dgvCuotas.Font, FontStyle.Bold);
                    }
                    else
                    {
                        // PENDIENTE FUTURA (Blanco normal)
                        row.DefaultCellStyle.BackColor = Color.White;
                        row.DefaultCellStyle.ForeColor = Color.Black;
                    }
                }
            }
        }
    }
}