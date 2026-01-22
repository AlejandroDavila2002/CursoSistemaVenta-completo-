using CapaEntidad;
using CapaNegocio; // Referencia correcta
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
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

            // 1. Obtener Datos
            List<Cuota> lista = new CN_Cuota().ListarPorVenta(_idVenta);

            // Obtener lo pagado realmente (la "bolsa de dinero" del cliente)
            var cuenta = new CN_CuentaPorCobrar().Listar().Where(c => c.oVenta.IdVenta == _idVenta).FirstOrDefault();
            decimal totalPagadoReal = cuenta != null ? cuenta.MontoPagado : 0;

            // 2. Llenar Grid
            dgvCuotas.DataSource = lista;

            // 3. AGREGAR COLUMNA "ABONADO" (Si no existe)
            if (!dgvCuotas.Columns.Contains("Abonado"))
            {
                DataGridViewTextBoxColumn col = new DataGridViewTextBoxColumn();
                col.Name = "Abonado";
                col.HeaderText = "Abonado";
                col.Width = 80;
                // Insertamos la columna antes de "Estado" para que se vea ordenado
                // (Asumiendo que Estado está por el final, ajusta el índice si quieres)
                dgvCuotas.Columns.Insert(dgvCuotas.Columns.Count - 2, col);
            }

            // 4. Ocultar columnas IDs
            string[] columnasOcultas = { "IdCuota", "IdVenta", "IdCuentaPorCobrar" };
            foreach (string col in columnasOcultas)
            {
                if (dgvCuotas.Columns.Contains(col)) dgvCuotas.Columns[col].Visible = false;
            }

            // 5. Renombrar
            if (dgvCuotas.Columns.Contains("NumeroCuota")) dgvCuotas.Columns["NumeroCuota"].HeaderText = "Nro";
            if (dgvCuotas.Columns.Contains("FechaProgramada")) dgvCuotas.Columns["FechaProgramada"].HeaderText = "Fecha Venc.";
            if (dgvCuotas.Columns.Contains("MontoCuota")) dgvCuotas.Columns["MontoCuota"].HeaderText = "Monto";
               dgvCuotas.Columns["MontoCuota"].DefaultCellStyle.Format = "N2";
            if (dgvCuotas.Columns.Contains("FechaPago")) dgvCuotas.Columns["FechaPago"].HeaderText = "Fecha Pago";

            dgvCuotas.Columns["NumeroCuota"].AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
            dgvCuotas.Columns["NumeroCuota"].Width = 40;

            dgvCuotas.Columns["Estado"].AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
            dgvCuotas.Columns["Estado"].Width = 200;





            // 6. LÓGICA DE CÁLCULO (Estados y Montos)
            decimal acumuladoProcesado = 0; // Rastrea cuánto cuesta cubrir las cuotas anteriores

            foreach (DataGridViewRow row in dgvCuotas.Rows)
            {
                decimal montoCuota = Convert.ToDecimal(row.Cells["MontoCuota"].Value);

                // --- CÁLCULO DEL ABONO PARA ESTA CUOTA ---
                // Dinero disponible para esta cuota = TotalPagado - Lo que ya gastamos en las anteriores
                decimal dineroDisponible = totalPagadoReal - acumuladoProcesado;
                decimal abonoEnEstaCuota = 0;

                if (dineroDisponible >= montoCuota)
                {
                    abonoEnEstaCuota = montoCuota; // Cubre todo
                }
                else if (dineroDisponible > 0)
                {
                    abonoEnEstaCuota = dineroDisponible; // Cubre solo lo que sobra
                }
                else
                {
                    abonoEnEstaCuota = 0; // No alcanza nada
                }

                // Escribimos el valor en la nueva columna
                row.Cells["Abonado"].Value = abonoEnEstaCuota.ToString("N2");
                // -----------------------------------------

                // Actualizamos el acumulado para la siguiente vuelta
                acumuladoProcesado += montoCuota;

                // --- ASIGNACIÓN DE ESTADO (Texto) ---
                if (abonoEnEstaCuota >= montoCuota)
                {
                    row.Cells["Estado"].Value = "Pagada";
                }
                else if (abonoEnEstaCuota > 0)
                {
                    decimal resta = montoCuota - abonoEnEstaCuota;
                    row.Cells["Estado"].Value = $"Parcial (Resta {resta:N2})";
                }
                else
                {
                    row.Cells["Estado"].Value = "Pendiente";
                }
            }
        }

        // --- FUSIÓN DE LÓGICA DE COLORES ---
        private void dgvCuotas_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            // Aseguramos que sea la columna Estado y tenga valor
            if (this.dgvCuotas.Columns[e.ColumnIndex].Name == "Estado" && e.Value != null)
            {
                string estadoTexto = e.Value.ToString();

                // CASO 1: PAGADA (Verde)
                if (estadoTexto == "Pagada")
                {
                    e.CellStyle.BackColor = Color.LightGreen;
                    e.CellStyle.ForeColor = Color.Black;
                }
                // CASO 2: PARCIAL (Amarillo)
                else if (estadoTexto.Contains("Parcial"))
                {
                    e.CellStyle.BackColor = Color.LemonChiffon; // Amarillo suave
                    e.CellStyle.ForeColor = Color.Black;
                    e.CellStyle.Font = new Font(e.CellStyle.Font, FontStyle.Italic);
                }
                // CASO 3: PENDIENTE (Blanco o Rojo)
                else
                {
                    // Lógica de fechas para ver si está vencida
                    var celdaFecha = dgvCuotas.Rows[e.RowIndex].Cells["FechaProgramada"].Value;

                    if (celdaFecha != null)
                    {
                        DateTime fechaVenc;
                        // Convertimos la fecha del string del grid a DateTime
                        if (DateTime.TryParse(celdaFecha.ToString(), out fechaVenc))
                        {
                            if (DateTime.Now.Date > fechaVenc.Date)
                            {
                                // VENCIDA -> Rojo
                                e.CellStyle.BackColor = Color.Salmon;
                                e.CellStyle.ForeColor = Color.White;
                            }
                            else
                            {
                                // VIGENTE -> Blanco
                                e.CellStyle.BackColor = Color.White;
                                e.CellStyle.ForeColor = Color.Black;
                            }
                        }
                    }
                }
            }
        }
    }
}