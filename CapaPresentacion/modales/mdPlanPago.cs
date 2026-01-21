using CapaEntidad;
using CapaPresentacion.utilidades;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace CapaPresentacion.modales
{
    public partial class mdPlanPago : Form
    {
        private decimal _montoDeuda;

        // Propiedades para devolver los datos al formulario de ventas
        public List<Cuota> _ListaCuotas { get; set; } = new List<Cuota>();
        public CuentaPorCobrar _DatosPlan { get; set; }
        public bool _Confirmado { get; set; } = false;

        private decimal _tasaCambio;
        private bool _esBolivares;

        public mdPlanPago(decimal montoDeuda, decimal tasaCambio, bool esBolivares)
        {
            InitializeComponent();
            _montoDeuda = montoDeuda;
            _tasaCambio = tasaCambio;
            _esBolivares = esBolivares;
        }


        private void mdPlanPago_Load(object sender, EventArgs e)
        {
            // --- LÓGICA DE CONVERSIÓN DE MONEDA ---
            // Si la venta se hizo en Bs, convertimos la deuda a Dólares inmediatamente.
            if (_esBolivares && _tasaCambio > 0)
            {
                _montoDeuda = _montoDeuda / _tasaCambio;
            }

            // 1. Mostrar deuda inicial (Ahora siempre se verá en $)
            lblMontoDeuda.Text = _montoDeuda.ToString("N2");

            // 2. Configurar el ComboBox de Frecuencia
            cboFrecuencia.Items.Clear();
            cboFrecuencia.Items.Add(new OpcionCombo() { Valor = 7, Texto = "Semanal (7 días)" });
            cboFrecuencia.Items.Add(new OpcionCombo() { Valor = 15, Texto = "Quincenal (15 días)" });
            cboFrecuencia.Items.Add(new OpcionCombo() { Valor = 30, Texto = "Mensual (30 días)" });

            cboFrecuencia.DisplayMember = "Texto";
            cboFrecuencia.ValueMember = "Valor";
            cboFrecuencia.SelectedIndex = 1; // Por defecto Quincenal

            // 3. Configurar el DataGridView
            dgvData.Columns.Clear();
            dgvData.Columns.Add("NroCuota", "N°");
            dgvData.Columns.Add("Monto", "Monto Cuota");
            dgvData.Columns.Add("FechaPago", "Fecha de Pago");

            // Estilos del Grid
            dgvData.AllowUserToAddRows = false;
            dgvData.ReadOnly = true;
            dgvData.RowHeadersVisible = false;
            dgvData.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            // 4. Valores iniciales
            numericUpDown1.Value = 1;
            txtMora.Value = 0;

            CalcularPlan();
        }

        private void CalcularPlan()
        {
            // Validaciones para evitar errores si el usuario borra todo
            if (cboFrecuencia.SelectedItem == null) return;
            if (numericUpDown1.Value < 1) numericUpDown1.Value = 1;

            dgvData.Rows.Clear();

            int diasFrecuencia = (int)((OpcionCombo)cboFrecuencia.SelectedItem).Valor;
            int totalCuotas = (int)numericUpDown1.Value;

            // División simple del monto
            decimal montoCuota = _montoDeuda / totalCuotas;

            DateTime fechaBase = DateTime.Now;

            // BUCLE PARA GENERAR LAS FILAS DE PAGOS
            for (int i = 1; i <= totalCuotas; i++)
            {
                DateTime fechaPago;

                if (diasFrecuencia == 30)
                    fechaPago = fechaBase.AddMonths(i); // Suma mes exacto
                else
                    fechaPago = fechaBase.AddDays(diasFrecuencia * i); // Suma días exactos

                // Agregamos la fila calculada al Grid
                dgvData.Rows.Add(new object[] {
                    i,
                    montoCuota.ToString("N2"),
                    fechaPago.ToString("dd/MM/yyyy")
                });
            }
        }

        // --- EVENTOS QUE DISPARAN EL CALCULO AUTOMÁTICO ---

        private void cboFrecuencia_SelectedIndexChanged(object sender, EventArgs e)
        {
            CalcularPlan();
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            CalcularPlan();
        }

        // --- BOTONES ---

        private void btnRegistrar_Click(object sender, EventArgs e)
        {
            // Validar que haya filas generadas
            if (dgvData.Rows.Count == 0) return;

            // 1. Obtener la fecha de la ÚLTIMA cuota (Vencimiento final)
            int ultimaFila = dgvData.Rows.Count - 1;
            string fechaFinTexto = dgvData.Rows[ultimaFila].Cells["FechaPago"].Value.ToString();

            // 2. Obtener texto de frecuencia para la descripción
            string freqTexto = ((OpcionCombo)cboFrecuencia.SelectedItem).Texto.Split('(')[0].Trim();
            // Esto toma "Semanal" de "Semanal (7 días)"

            // 3. Guardar los datos en la propiedad pública
            _DatosPlan = new CuentaPorCobrar()
            {
                // Convertimos la fecha al formato que SQL Server entiende (Año-Mes-Dia)
                FechaVencimiento = DateTime.ParseExact(fechaFinTexto, "dd/MM/yyyy", null).ToString("yyyy-MM-dd"),

                // Descripción ej: "4 Cuotas - Quincenal"
                DescripcionPlan = $"{numericUpDown1.Value} Cuotas - {freqTexto}",

                PorcentajeMora = txtMora.Value
            };

            _ListaCuotas = new List<Cuota>();
            foreach (DataGridViewRow row in dgvData.Rows)
            {
                _ListaCuotas.Add(new Cuota()
                {
                    NumeroCuota = Convert.ToInt32(row.Cells["NroCuota"].Value),
                    MontoCuota = Convert.ToDecimal(row.Cells["Monto"].Value),
                    FechaProgramada = row.Cells["FechaPago"].Value.ToString()
                });
            }

            _Confirmado = true;
            this.Close();
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            _Confirmado = false;
            this.Close();
        }
    }
}