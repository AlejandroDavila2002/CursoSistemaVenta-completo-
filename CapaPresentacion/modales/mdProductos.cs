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

        public List<int> _ListaNegra { get; set; } = new List<int>();

        public mdProductos()
        {
            InitializeComponent();
        }

        // por defecto, solamente deberian ser visibles el codigo, el nombre del producto y categoria
        // pero se dejan los demas por si en el futuro se requieren. 20/11/2025
        // Adicionalmente, el boton de busqueda no estara visible, pero se dejara por si en el futuro se requiere.

        private void mdProductos_Load(object sender, EventArgs e)
        {

            if (dgvData.Columns.Contains("PrecioVenta"))
            {
                dgvData.Columns["PrecioVenta"].Visible = true;
                // Opcional: Darle formato de moneda visualmente
                dgvData.Columns["PrecioVenta"].DefaultCellStyle.Format = "N2";
            }

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
                if (_ListaNegra.Contains(item.IdProducto))
                {
                    continue; // Pasa al siguiente producto sin agregarlo a la grilla
                }

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
            AplicarFiltro();
        }
        private void AplicarFiltro()
        {
            string filtroTexto = txtBusqueda.Text.Trim().ToUpper();
            string columnaFiltro = ((OpcionCombo)cboBusqueda.SelectedItem).Valor.ToString();

            // 1. Estado Deseado (Activo / Inactivo)
            string estadoDeseado = chkMostrarInactivos.Checked ? "No Activo" : "Activo";

            foreach (DataGridViewRow row in dgvData.Rows)
            {
                // A. Filtro de Texto
                bool coincideTexto = row.Cells[columnaFiltro].Value.ToString().Trim().ToUpper().Contains(filtroTexto);

                // B. Filtro de Estado
                string estadoFila = row.Cells["Estado"].Value.ToString();
                bool coincideEstado = (estadoFila == estadoDeseado);

                // C. Filtro de Stock (NUEVO)
                // Obtenemos el valor del stock de la celda
                int stock = Convert.ToInt32(row.Cells["Stock"].Value);
                bool coincideStock = false;

                if (chkAgotados.Checked)
                {
                    // Si el check está MARCADO: Mostrar SOLO los que tienen stock 0
                    coincideStock = (stock <= 0);
                }
                else
                {
                    // Si el check está DESMARCADO: Mostrar SOLO los que tienen stock disponible
                    coincideStock = (stock > 0);
                }

                // 3. Resultado Final: Debe cumplir LAS TRES condiciones
                if (coincideTexto && coincideEstado && coincideStock)
                {
                    row.Visible = true;
                }
                else
                {
                    row.Visible = false;
                }
            }
        }


        private void btnBuscar_Click(object sender, EventArgs e)
        {
            AplicarFiltro();

        }

        private void chkMostrarInactivos_CheckedChanged(object sender, EventArgs e)
        {
            AplicarFiltro();
        }

        private void chkAgotados_CheckedChanged(object sender, EventArgs e)
        {
            AplicarFiltro();
        }


        private void btnLimpiarCombo_Click(object sender, EventArgs e)
        {
            txtBusqueda.Text = "";
            cboBusqueda.SelectedIndex = 0;
            chkMostrarInactivos.Checked = false;
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

                object valorCelda = dgvData.Rows[iRow].Cells["Estado"].Value;
                bool estaActivo = false;

                // Intenta interpretar el valor
                if (valorCelda is bool)
                {
                    estaActivo = (bool)valorCelda;
                }
                else if (valorCelda != null)
                {
                    // Si es un "1" o "True", lo tomamos como válido
                    string sValor = valorCelda.ToString().ToLower();
                    estaActivo = (sValor == "1" || sValor == "true" || sValor == "activo");
                }

                // Validación final
                if (!estaActivo)
                {
                    MessageBox.Show("No puedes seleccionar un producto INACTIVO.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

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
               
            }
        }

        
    }

}
