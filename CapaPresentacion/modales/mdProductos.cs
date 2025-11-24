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
    public partial class mdProductos : Form
    {
        public Producto _Producto { get; set; }
        public mdProductos()
        {
            InitializeComponent();
        }

        // por defecto, solamente deberian ser visibles el codigo, el nombre del producto y categoria
        // pero se dejan los demas por si en el futuro se requieren. 20/11/2025
        // Adicionalmente, el boton de busqueda no estara visible, pero se dejara por si en el futuro se requiere.
        private void mdProductos_Load(object sender, EventArgs e)
        {
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



            List<Producto> listaProductos = new CN_Producto().listar();
            foreach (Producto item in listaProductos)
            {
                dgvData.Rows.Add(new object[] {
                    "",
                    item.IdProducto,
                    item.Codigo,
                    item.NombreProducto,
                    item.Descripcion,
                    item.oCategoria.IdCategoria,
                    item.oCategoria.Descripcion,
                    item.Stock,
                    item.PrecioCompra,
                    item.PrecioVenta,
                    item.Estado == true ? 1 : 0,
                    item.Estado == true ? "Activo" : "No Activo"
               });
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

        private void dgvData_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            int iRow = e.RowIndex;
            int iCol = e.ColumnIndex;

            if (iRow >= 0 && iCol > 0)
            {
                _Producto = new Producto()
                {
                    IdProducto = Convert.ToInt32(dgvData.Rows[iRow].Cells["Id"].Value.ToString()),
                    Codigo = dgvData.Rows[iRow].Cells["Codigo"].Value.ToString(),
                    NombreProducto = dgvData.Rows[iRow].Cells["NombreProducto"].Value.ToString(),
                    Descripcion = dgvData.Rows[iRow].Cells["Descripcion"].Value.ToString(),
                    oCategoria = new Categoria()
                    {
                        IdCategoria = Convert.ToInt32(dgvData.Rows[iRow].Cells["IdCategoria"].Value.ToString()),
                        Descripcion = dgvData.Rows[iRow].Cells["Categoria"].Value.ToString(),
                    },
                    Stock = Convert.ToInt32(dgvData.Rows[iRow].Cells["Stock"].Value.ToString()),
                    PrecioCompra = Convert.ToDecimal(dgvData.Rows[iRow].Cells["PrecioCompra"].Value.ToString()),
                    PrecioVenta = Convert.ToDecimal(dgvData.Rows[iRow].Cells["PrecioVenta"].Value.ToString()),
                };
                this.Close();
                this.DialogResult = DialogResult.OK;
                if (DialogResult == DialogResult.OK)
                {
                    MessageBox.Show("Producto seleccionado correctamente", "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }
    }

}
