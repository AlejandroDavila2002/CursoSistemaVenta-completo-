using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CapaEntidad;
using CapaNegocio;
using DocumentFormat.OpenXml.Drawing;

namespace CapaPresentacion
{
    public partial class frmDetallesNegocio : Form
    {
        public frmDetallesNegocio()
        {
            InitializeComponent();
        }

        public Image ByteToImage(byte[] imagenByte)
        {
            MemoryStream ms = new MemoryStream(imagenByte);

            ms.Write(imagenByte, 0, imagenByte.Length);
            Image image = new Bitmap(ms);

            return image;
        }

        private void frmDetallesNegocio_Load(object sender, EventArgs e)
        {
            bool obtenido = false;
            byte[] byteimagen = new CN_DetallesNegocio().obtenerLogo(out obtenido);

            if (obtenido)
            {
                picLogo.Image = ByteToImage(byteimagen);
            }


            Detalles_Negocio Datos = new CN_DetallesNegocio().ObtenerDetallesNegocio();

            txtNombreNegocio.Text = Datos.Nombre;
            txtRUC.Text = Datos.RUC;
            txtDireccion.Text = Datos.Direccion;




        }

        private void btnSubir_Click(object sender, EventArgs e)
        {
            string mensaje = string.Empty;
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.FileName = "Files|* .jpg; *.jpeg; *.png; *.gif; *.bmp";
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                byte[] byteimagen = File.ReadAllBytes(openFileDialog.FileName);
                bool respuesta = new CN_DetallesNegocio().ActualizarLogo(byteimagen, out mensaje);

                if(respuesta)
                {
                    picLogo.Image = ByteToImage(byteimagen);
                    MessageBox.Show("Logo actualizado correctamente.", "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show(mensaje, "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }


        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            string mensaje = string.Empty;

            Detalles_Negocio objDetallesNegocio = new Detalles_Negocio()
            {
                Nombre = txtNombreNegocio.Text,
                RUC = txtRUC.Text,
                Direccion = txtDireccion.Text
            };

            bool respuesta = new CN_DetallesNegocio().GuardarDatos(objDetallesNegocio, out mensaje);
            if(respuesta)
            {
                MessageBox.Show("Datos guardados correctamente.", "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show(mensaje, "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
    }
}
