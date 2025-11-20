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
    public partial class mdProveedores : Form
    {

        public Proveedor _Proveedor { get; set; }

        public mdProveedores()
        {
            InitializeComponent();
        }

        private void mdProveedores_Load(object sender, EventArgs e)
        {

            // Configurar el combo de busqueda
            // El boton busqueda no estara visible, pero se dejara por si en el futuro se requiere.
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

            // en este caso, solo seran visibles numero de documento y razon social. Pero dejaremos los demas por si en el futuro se requieren.
            List<Proveedor> listaProveedors = new CN_Proveedor().listar();
            foreach (Proveedor item in listaProveedors)
            {
                dgvData.Rows.Add(new object[] {
                    "",
                    item.IdProveedor,
                    item.Documento,
                    item.RazonSocial,
                    item.Correo,
                    item.Telefono,

                    item.Estado == true ? 1 : 0,
                    item.Estado == true ? "Activo" : "No Activo",
                });
            }


        }

        private void dgvData_CellContentDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            int iRow = e.RowIndex;
            int iCol = e.ColumnIndex;

            if (iRow >= 0 && iCol > 0)
            {
                _Proveedor = new Proveedor()
                {
                    IdProveedor = Convert.ToInt32(dgvData.Rows[iRow].Cells["IdProveedor"].Value),
                    Documento = dgvData.Rows[iRow].Cells["Documento"].Value.ToString(),
                    RazonSocial = dgvData.Rows[iRow].Cells["RazonSocial"].Value.ToString(),
                    Correo = dgvData.Rows[iRow].Cells["Correo"].Value.ToString(),
                    Telefono = dgvData.Rows[iRow].Cells["Telefono"].Value.ToString(),
                    Estado = Convert.ToInt32(dgvData.Rows[iRow].Cells["EstadoValor"].Value) == 1 ? true : false,
                };

                this.DialogResult = DialogResult.OK;
                this.Close();

                
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
    }
}
