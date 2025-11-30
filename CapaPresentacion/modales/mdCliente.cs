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
    public partial class mdCliente : Form
    {
        public Cliente _Cliente {  get; set; } // Aquí almacenamos la variable una ves se cierre el mdClientes.

        public mdCliente()
        {
            InitializeComponent();
        }

        private void mdCliente_Load(object sender, EventArgs e)
        {
            // Configurar el combo de busqueda
            foreach (DataGridViewColumn columna in dgvData.Columns)
            {
                if (columna.Visible == true && columna.Name != "btnSeleccionar")
                {
                    cboBusqueda.Items.Add(new OpcionCombo() { Valor = columna.Name, Texto = columna.HeaderText });
                }
            }
            cboBusqueda.DisplayMember = "Texto";
            cboBusqueda.ValueMember = "Valor";
            cboBusqueda.SelectedIndex = 0;


            // solo las columnas de Documento y nombreCompleto estan visibles, las demas las dejaremos por si acaso.
            List<Cliente> listaClientes = new CN_Cliente().listar();
            foreach (Cliente item in listaClientes)
            {
                if (item.Estado)
                {
                    dgvData.Rows.Add(new object[] {
                    "",
                    item.IdCliente,
                    item.Documento,
                    item.NombreCompleto,
                    item.Correo,
                    item.Telefono,

                    item.Estado == true ? 1 : 0,
                    item.Estado == true ? "Activo" : "No Activo",
                });
                }
                else
                {
                    dgvData.Rows.Add();
                    MessageBox.Show("No Existen Clientes.", "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
               
            }

        }

        private void btnBuscar_Click(object sender, EventArgs e)
        {

            string columnaFiltro = ((OpcionCombo)cboBusqueda.SelectedItem).Valor.ToString();
            foreach (DataGridViewRow row in dgvData.Rows)
            {
                if (row.Cells[columnaFiltro].Value.ToString().Trim().ToUpper().Contains(txtBusqueda.Text.Trim().ToUpper()))
                    row.Visible = true;
                else
                    row.Visible = false;
            }

        }

        private void btnLimpiarCombo_Click(object sender, EventArgs e)
        {
            txtBusqueda.Text = "";
            cboBusqueda.SelectedIndex = 0;
            foreach (DataGridViewRow row in dgvData.Rows)
            {
                row.Visible = true;
            }
        }

        private void dgvData_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            int row = e.RowIndex;
            int column = e.ColumnIndex;
            if (row >= 0 && column > 0)
            {
                _Cliente = new Cliente()
                {
                    Documento = dgvData.Rows[row].Cells["Documento"].Value.ToString(),
                    NombreCompleto = dgvData.Rows[row].Cells["NombreCompleto"].Value.ToString(),
                    Correo = dgvData.Rows[row].Cells["Correo"].Value.ToString(),
                    Telefono = dgvData.Rows[row].Cells["Telefono"].Value.ToString(),
                   

                };
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
        }
    }
}

