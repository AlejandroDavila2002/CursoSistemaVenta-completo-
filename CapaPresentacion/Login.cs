using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CapaNegocio;
using CapaEntidad;

namespace CapaPresentacion
{
    public partial class Login : Form
    {
        private int intentosFallidos = 0;



        public Login()
        {
            InitializeComponent();
        }

        private void btncancelar_Click(object sender, EventArgs e)
        {
            this.Close();

        }

        // Boton para ingresar.
        private void btningresar_Click(object sender, EventArgs e)
        {

            List<Usuario> test = new CN_Usuario().listar();
            int cantidad = test.Count;

            Usuario ousuario = new CN_Usuario().listar().Where(u => u.Documento == txtdocumento.Text && u.Clave == txtclave.Text).FirstOrDefault();

            
            if(txtdocumento.Text == "" && txtclave.Text == "")
            {

                MessageBox.Show("Los campos no pueden estar vacios", "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            

            if (ousuario != null) {
                
                Inicio form = new Inicio(ousuario);
                form.Show();

                this.Hide();

                form.FormClosing += frm_closing; 

            }
            else {
                intentosFallidos++;
                MessageBox.Show("No se encontro el usuario", "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);

                if (intentosFallidos >= 3)
                {
                    txtdocumento.Text = "";
                    txtclave.Text = "";
                    intentosFallidos = 0;
               
                }
               
            }
                
             


        }





        private void frm_closing(object sender, FormClosingEventArgs e)
        {
            txtdocumento.Text = "";
            txtclave.Text = "";
            this.Show();
        }

        
    }
}
