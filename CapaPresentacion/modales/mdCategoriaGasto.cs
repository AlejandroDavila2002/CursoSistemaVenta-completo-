using CapaEntidad;
using CapaNegocio;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace CapaPresentacion.modales
{
    public partial class mdCategoriaGasto : Form
    {
        // Bandera para avisar al formulario padre si debe actualizarse
        private bool _seHicieronCambios = false;

        public mdCategoriaGasto()
        {
            InitializeComponent();
        }

        private void mdCategoriaGasto_Load(object sender, EventArgs e)
        {
            CargarDatos();
        }

        private void CargarDatos()
        {
            List<CategoriaGasto> lista = new CN_FlujoCaja().ListarCategorias();
            dgvData.Rows.Clear();

            foreach (CategoriaGasto item in lista)
            {
                // Aseguramos el orden: ID (0), Descripción (1), Botón (2)
                dgvData.Rows.Add(new object[] {
                    item.IdCategoriaGasto,
                    item.Descripcion,
                    ""
                });
            }
        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtDescripcion.Text))
            {
                MessageBox.Show("Escriba una descripción.", "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            string mensaje = string.Empty;
            CategoriaGasto obj = new CategoriaGasto() { Descripcion = txtDescripcion.Text };

            bool resultado = new CN_FlujoCaja().RegistrarCategoria(obj, out mensaje);

            if (resultado)
            {
                MessageBox.Show("Categoría registrada correctamente", "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtDescripcion.Text = "";
                CargarDatos();
                _seHicieronCambios = true; // Marcamos que hubo cambios
                // NO CERRAMOS EL FORMULARIO para permitir seguir agregando
            }
            else
            {
                MessageBox.Show(mensaje, "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

      
        private void dgvData_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dgvData.Columns[e.ColumnIndex].Name == "BtnEliminar" && e.RowIndex >= 0)
            {
                if (MessageBox.Show("¿Desea eliminar esta categoría?", "Mensaje", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    int id = Convert.ToInt32(dgvData.Rows[e.RowIndex].Cells["Id"].Value); // Verifica el nombre de tu columna ID
                    string mensaje = string.Empty;

                    bool respuesta = new CN_FlujoCaja().EliminarCategoria(id, out mensaje);

                    if (respuesta)
                    {
                        CargarDatos();
                        this.DialogResult = DialogResult.OK;
                    }
                    else
                    {
                        MessageBox.Show(mensaje, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }


























        // Evento que se dispara al cerrar el formulario (X o Cerrar)
        private void mdCategoriaGasto_FormClosing(object sender, FormClosingEventArgs e)
        {
            // Si hubo cambios, devolvemos OK para que frmFlujoCaja actualice los combos
            if (_seHicieronCambios)
            {
                this.DialogResult = DialogResult.OK;
            }
        }

        private void dgvData_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            // Evitamos cabeceras o índices inválidos
            if (e.RowIndex < 0 || e.ColumnIndex < 0 || e.ColumnIndex >= dgvData.Columns.Count)
                return;

            // Comprobamos por nombre de columna (asegúrate que existe una columna llamada "btnEliminar" en el diseñador)
            if (dgvData.Columns[e.ColumnIndex].Name == "BtnEliminar")
            {
                // Pintamos fondo y contenido por defecto
                e.Paint(e.CellBounds, DataGridViewPaintParts.All);

                // Dibujamos el icono centrado
                var img = Properties.Resources.icons8_borrar_para_siempre_25;
                if (img != null)
                {
                    var w = img.Width;
                    var h = img.Height;
                    var x = e.CellBounds.Left + (e.CellBounds.Width - w) / 2;
                    var y = e.CellBounds.Top + (e.CellBounds.Height - h) / 2;
                    e.Graphics.DrawImage(img, new Rectangle(x, y, w, h));
                }

                e.Handled = true;
            }
        }
    }
}