using CapaEntidad;
using CapaNegocio;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

public partial class mdlDetalleCuotas : Form
{
    private int _idVenta;

    public mdlDetalleCuotas(int idVenta)
    {
        InitializeComponent();
        _idVenta = idVenta;
    }

    private void mdlDetalleCuotas_Load(object sender, EventArgs e)
    {
        CargarCuotas();
    }

    private void CargarCuotas()
    {
        List<Cuota> lista = new CN_Cuota().Listar(_idVenta);
        dgvCuotas.DataSource = lista;

        // Ocultar columnas internas
        if (dgvCuotas.Columns.Contains("IdCuota")) dgvCuotas.Columns["IdCuota"].Visible = false;
        if (dgvCuotas.Columns.Contains("IdVenta")) dgvCuotas.Columns["IdVenta"].Visible = false;

        // Ajustar títulos
        if (dgvCuotas.Columns.Contains("FechaProgramada")) dgvCuotas.Columns["FechaProgramada"].HeaderText = "F. Vencimiento";
        if (dgvCuotas.Columns.Contains("FechaPago")) dgvCuotas.Columns["FechaPago"].HeaderText = "F. Pagado";
    }

    private void dgvCuotas_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
    {
        if (this.dgvCuotas.Columns[e.ColumnIndex].Name == "Estado")
        {
            var row = dgvCuotas.Rows[e.RowIndex];

            // Seguridad: Validar que existan los valores
            if (row.Cells["FechaProgramada"].Value == null) return;
            string strVencimiento = row.Cells["FechaProgramada"].Value.ToString();
            string strPago = row.Cells["FechaPago"].Value != null ? row.Cells["FechaPago"].Value.ToString() : "";

            DateTime fechaVencimiento;
            if (!DateTime.TryParse(strVencimiento, out fechaVencimiento)) return;

            // CASO 1: YA PAGÓ (FechaPago tiene valor)
            if (!string.IsNullOrEmpty(strPago))
            {
                DateTime fechaRealPago = Convert.ToDateTime(strPago);

                if (fechaRealPago.Date > fechaVencimiento.Date)
                {
                    // PAGÓ TARDE -> Rojo Suave
                    e.CellStyle.BackColor = Color.LightCoral;
                    e.CellStyle.ForeColor = Color.Black;
                    e.Value = "Atrasado";
                }
                else
                {
                    // PAGÓ A TIEMPO -> Verde
                    e.CellStyle.BackColor = Color.LightGreen;
                    e.CellStyle.ForeColor = Color.Black;
                    e.Value = "Pagado";
                }
            }
            // CASO 2: NO HA PAGADO (FechaPago vacía)
            else
            {
                if (DateTime.Now.Date > fechaVencimiento.Date)
                {
                    // VENCIDO Y SIN PAGAR -> Rojo Fuerte / Alerta
                    e.CellStyle.BackColor = Color.Red;
                    e.CellStyle.ForeColor = Color.White;
                    e.Value = "Vencido";
                }
                else
                {
                    // AÚN ESTÁ EN TIEMPO -> Blanco o Amarillo claro
                    e.CellStyle.BackColor = Color.White;
                    e.Value = "Pendiente";
                }
            }
        }
    }
}