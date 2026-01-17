using CapaEntidad;
using CapaNegocio;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace CapaPresentacion.modales
{
    public partial class mdFormaPago : Form
    {
        public mdFormaPago()
        {
            InitializeComponent();
        }

        private void mdFormaPago_Load(object sender, EventArgs e)
        {
            CargarDatos();
        }

        private void CargarDatos()
        {
            List<FormaPago> lista = new CN_FlujoCaja().ListarFormasPago();
            dgvData.Rows.Clear();

            foreach (FormaPago item in lista)
            {
                // Ajusta el orden según tus columnas en el diseño
                dgvData.Rows.Add(new object[] {
                    item.IdFormaPago,
                    item.Descripcion,
                    ""
                });
            }
        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            string mensaje = string.Empty;
            FormaPago obj = new FormaPago() { Descripcion = txtDescripcion.Text };

            bool resultado = new CN_FlujoCaja().RegistrarFormaPago(obj, out mensaje);

            if (resultado)
            {
                MessageBox.Show("Forma de Pago registrada correctamente", "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtDescripcion.Text = "";
                CargarDatos();
                this.DialogResult = DialogResult.OK;
            }
            else
            {
                MessageBox.Show(mensaje, "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private void dgvData_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dgvData.Columns[e.ColumnIndex].Name == "BtnEliminar" && e.RowIndex >= 0)
            {
                if (MessageBox.Show("¿Desea eliminar esta forma de pago?", "Mensaje", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    int id = Convert.ToInt32(dgvData.Rows[e.RowIndex].Cells["Id"].Value); // Verifica el nombre de tu columna ID
                    string mensaje = string.Empty;

                    bool respuesta = new CN_FlujoCaja().EliminarFormaPago(id, out mensaje);

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