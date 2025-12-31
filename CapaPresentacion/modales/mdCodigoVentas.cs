using CapaEntidad;
using CapaNegocio;
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
            _listaVentas = new CN_Venta().ListarVentasResumen();
            MostrarVentas(); // Método para llenar el Grid
        }

        private void MostrarVentas()
        {
            dgvData.Rows.Clear();
            bool verEnBs = cbVerEnBs.Checked;

            if (_listaVentas != null) // Validación de seguridad
            {
                foreach (Venta v in _listaVentas)
                {
                    string montoAMostrar = verEnBs ?
                        v.MontoBs.ToString("N2") :
                        v.MontoTotal.ToString("N2");

                    // Añadimos una fila y capturamos el índice
                    int indice = dgvData.Rows.Add();

                    // Asignamos explícitamente a cada celda por el NOMBRE de la columna del Designer
                    dgvData.Rows[indice].Cells["CodigoVenta"].Value = v.NumeroDocumento;
                    dgvData.Rows[indice].Cells["NombreCliente"].Value = v.NombreCliente; // Usando el nombre que pusiste en el Designer
                    dgvData.Rows[indice].Cells["PrecioVenta"].Value = montoAMostrar;
                    dgvData.Rows[indice].Cells["FechaRegistro"].Value = v.FechaRegistro;
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





    }
}
