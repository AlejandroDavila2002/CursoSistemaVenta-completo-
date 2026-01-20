using System;
using System.Drawing; // Necesario para los colores
using System.Windows.Forms;

public partial class mdlDetalleCuotas : Form
{
    private int _idVenta;

    // Constructor que recibe el ID de la venta/crédito
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
        // 1. Aquí llamas a tu Capa de Negocio para obtener la lista de cuotas
        // Ejemplo: List<Cuota> lista = new CN_Cuota().Listar(_idVenta);
        // dgvCuotas.DataSource = lista;

        // ESTRUCTURA ESPERADA DE LAS COLUMNAS (Asegúrate de traer estos datos):
        // - NumeroCuota (int)
        // - FechaProgramada (DateTime) -> Fecha Vencimiento
        // - FechaPago (DateTime?) -> Puede ser null si no ha pagado
        // - Monto (decimal)
        // - Estado (string)
    }

    // ESTA ES LA PARTE CLAVE: El pintado condicional
    private void dgvCuotas_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
    {
        // Aseguramos que no sea la cabecera y que haya filas
        if (this.dgvCuotas.Columns[e.ColumnIndex].Name != "Estado" || e.Value == null)
        {
            return;
        }
        // Obtenemos la fila actual
        DataGridViewRow row = dgvCuotas.Rows[e.RowIndex];

        // Recuperamos las fechas (Ajusta los nombres de celdas a tu DGV)
        var fechaVencimiento = Convert.ToDateTime(row.Cells["FechaProgramada"].Value);

        // Verificamos si hay fecha de pago (si es null o DBNull, no está pagado)
        var celdaPago = row.Cells["FechaPago"].Value;
        bool pagado = celdaPago != null && celdaPago != DBNull.Value;

        if (pagado)
        {
            DateTime fechaRealPago = Convert.ToDateTime(celdaPago);

            // Lógica de Colores
            if (fechaRealPago > fechaVencimiento)
            {
                // PAGO TARDE (ROJO)
                e.CellStyle.BackColor = Color.LightCoral;
                e.CellStyle.ForeColor = Color.Black;
                row.Cells["Estado"].Value = "Atrasado"; // Opcional: Cambiar texto
            }
            else
            {
                // PAGO A TIEMPO O ADELANTADO (VERDE)
                e.CellStyle.BackColor = Color.LightGreen;
                e.CellStyle.ForeColor = Color.Black;

                // Si quieres distinguir "Adelantado" vs "El mismo día", puedes agregar otro if:
                if (fechaRealPago < fechaVencimiento.Date)
                {
                    row.Cells["Estado"].Value = "Adelantado";
                    // Podrías usar un verde más oscuro o diferente si quisieras
                }
                else
                {
                    row.Cells["Estado"].Value = "A Tiempo";
                }
            }
        }
        else
        {
            // NO PAGADO AÚN
            // Si la fecha actual ya pasó el vencimiento y no ha pagado:
            if (DateTime.Now > fechaVencimiento)
            {
                e.CellStyle.BackColor = Color.OrangeRed; // Alerta de deuda
                e.CellStyle.ForeColor = Color.White;
            }
            else
            {
                e.CellStyle.BackColor = Color.White; // Pendiente normal
            }
        }
    }
}