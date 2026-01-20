using CapaEntidad;
using CapaNegocio;
using CapaPresentacion.utilidades;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace CapaPresentacion.modales
{
    public partial class mdCodigoCompra : Form
    {
        // Propiedad para devolver el documento seleccionado al formulario padre
        public string DocumentoSeleccionado { get; set; }

        private List<Compra> _listaCompras;

        public mdCodigoCompra()
        {
            InitializeComponent();
        }

        private void mdCodigoCompra_Load(object sender, EventArgs e)
        {
            // 1. Cargar la lista de compras desde la BD
            _listaCompras = new CN_Compra().Listar();

            // 2. Llenar el ComboBox con las columnas visibles del Grid
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

            // 3. Mostrar los datos en el Grid
            MostrarCompras();
        }

        private void MostrarCompras()
        {
            dgvData.Rows.Clear();

            // Lógica de filtro de moneda (igual que en ventas)
            // Si el checkbox está marcado, buscamos compras en BS (EsCompraEnBs == true)
            // Si no, buscamos en USD (EsCompraEnBs == false)
            bool buscarEnBs = cbVerEnBs.Checked;

            if (_listaCompras != null)
            {
                // Filtramos la lista según la moneda
                var listaFiltrada = _listaCompras.Where(c => c.EsCompraEnBs == buscarEnBs).ToList();

                foreach (Compra c in listaFiltrada)
                {
                    string montoFormateado;

                    if (buscarEnBs)
                    {
                        // Calculamos el monto en Bs usando la tasa histórica de esa compra
                        decimal montoEnBs = c.MontoTotal * c.TasaCambio; // O el campo que uses para total en Bs
                        montoFormateado = $"Bs. {montoEnBs.ToString("N2")}";
                    }
                    else
                    {
                        // Monto en Dólares
                        montoFormateado = $"$ {c.MontoTotal.ToString("N2")}";
                    }

                    // Añadimos la fila al DataGridView
                    int indice = dgvData.Rows.Add();

                    dgvData.Rows[indice].Cells["CodigoCompra"].Value = c.NumeroDocumento;
                    dgvData.Rows[indice].Cells["NombreProveedor"].Value = c.oProveedor.RazonSocial; // O NombreCompleto
                    dgvData.Rows[indice].Cells["MontoTotal"].Value = montoFormateado;
                    dgvData.Rows[indice].Cells["FechaRegistro"].Value = c.FechaRegistro;

                    // Estilo de color según moneda
                    if (buscarEnBs)
                    {
                        dgvData.Rows[indice].Cells["MontoTotal"].Style.ForeColor = Color.DarkGreen;
                        dgvData.Rows[indice].Cells["MontoTotal"].Style.SelectionForeColor = Color.LimeGreen;
                    }
                    else
                    {
                        dgvData.Rows[indice].Cells["MontoTotal"].Style.ForeColor = Color.Blue;
                    }
                }
            }
        }

        private void cbVerEnBs_CheckedChanged(object sender, EventArgs e)
        {
            MostrarCompras();
        }

        private void dgvData_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            int iRow = e.RowIndex;
            if (iRow >= 0)
            {
                // Obtenemos el número de documento de la celda correspondiente
                DocumentoSeleccionado = dgvData.Rows[iRow].Cells["CodigoCompra"].Value.ToString();
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