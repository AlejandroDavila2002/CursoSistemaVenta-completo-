using CapaEntidad;
using CapaNegocio;
using CapaPresentacion.utilidades;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CapaPresentacion.modales
{
    public partial class mdCodigoVentas : Form
    {
        public string NumeroFacturaSeleccionada { get; set; }
        private List<Venta> _listaVentas;
        public mdCodigoVentas()
        {
            InitializeComponent();
        }

        private void mdCodigoVentas_Load(object sender, EventArgs e)
        {
            // Cargamos la lista completa una sola vez
            _listaVentas = new CN_Venta().ListarVentasContado();

            foreach (DataGridViewColumn columna in dgvData.Columns)
            {
                // Solo agregamos las columnas que son visibles y no botones
                if (columna.Visible == true && columna.Name != "btnSeleccionar")
                {
                    cboBusqueda.Items.Add(new OpcionCombo() { Valor = columna.Name, Texto = columna.HeaderText });
                }
            }

            // Configurar qué mostrar en el ComboBox
            cboBusqueda.DisplayMember = "Texto";
            cboBusqueda.ValueMember = "Valor";
            cboBusqueda.SelectedIndex = 0;


            MostrarVentas(); // Método para llenar el Grid
        }

        private void MostrarVentas()
        {
            dgvData.Rows.Clear();

            // Determinamos qué bandera buscar según el estado del CheckBox
            // Si está marcado, filtramos por Bolívares (VES), si no, por Dólares (USD)
            string monedaABuscar = cbVerEnBs.Checked ? "VES" : "USD";

            if (_listaVentas != null)
            {
                // Filtramos la lista original para obtener solo las ventas de la moneda seleccionada
                var listaFiltrada = _listaVentas.Where(v => v.TipoMoneda == monedaABuscar).ToList();

                foreach (Venta v in listaFiltrada)
                {
                    string montoFormateado;

                    if (monedaABuscar == "VES")
                    {
                        // Formateamos con el símbolo de Bolívares
                        montoFormateado = $"Bs. {v.MontoBs.ToString("N2")}";
                    }
                    else
                    {
                        // Formateamos con el símbolo de Dólares
                        montoFormateado = $"$ {v.MontoTotal.ToString("N2")}";
                    }

                    // Añadimos la fila al DataGridView
                    int indice = dgvData.Rows.Add();

                    dgvData.Rows[indice].Cells["CodigoVenta"].Value = v.NumeroDocumento;
                    dgvData.Rows[indice].Cells["NombreCliente"].Value = v.NombreCliente;
                    dgvData.Rows[indice].Cells["PrecioVenta"].Value = montoFormateado;
                    dgvData.Rows[indice].Cells["FechaRegistro"].Value = v.FechaRegistro;

                    // Opcional: Aplicar un color distintivo a la celda si es Bolívares
                    if (monedaABuscar == "VES")
                    {
                        dgvData.Rows[indice].Cells["PrecioVenta"].Style.ForeColor = Color.DarkGreen;
                        dgvData.Rows[indice].Cells["PrecioVenta"].Style.SelectionForeColor = Color.LimeGreen;
                    }
                    else
                    {
                        dgvData.Rows[indice].Cells["PrecioVenta"].Style.ForeColor = Color.Blue;
                    }
                }
            }
        }

        // Evento del CheckBox para refrescar el Grid al cambiar
        private void cbVerEnBs_CheckedChanged(object sender, EventArgs e)
        {
            MostrarVentas();
        }


        private void dgvData_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            int iRow = e.RowIndex;
            if (iRow >= 0)
            {
                // Usamos "CodigoVenta" que es el nombre real en el Designer
                NumeroFacturaSeleccionada = dgvData.Rows[iRow].Cells["CodigoVenta"].Value.ToString();
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
        }



        private void btnBuscar_Click(object sender, EventArgs e)
        {
            string columnaFiltro = ((OpcionCombo)cboBusqueda.SelectedItem).Valor.ToString();

            if (dgvData.Rows.Count > 0)
            {
                foreach (DataGridViewRow row in dgvData.Rows)
                {
                    // Verificamos si el valor de la celda contiene lo escrito en txtBusqueda
                    if (row.Cells[columnaFiltro].Value.ToString().Trim().ToUpper().Contains(txtBusqueda.Text.Trim().ToUpper()))
                    {
                        row.Visible = true;
                    }
                    else
                    {
                        row.Visible = false;
                    }
                }
            }
        }

        private void btnLimpiarCombo_Click(object sender, EventArgs e)
        {
            txtBusqueda.Text = "";

            // Hacemos visibles todas las filas nuevamente
            foreach (DataGridViewRow row in dgvData.Rows)
            {
                row.Visible = true;
            }

            // Opcional: Reiniciar el combo al primero
            // cboBusqueda.SelectedIndex = 0; 
        }





    }
}
