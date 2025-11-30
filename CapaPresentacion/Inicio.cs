using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CapaEntidad;
using CapaNegocio;
using FontAwesome.Sharp;

namespace CapaPresentacion
{
    public partial class Inicio : Form
    {

        private static Usuario UsuarioActual;
        private static IconMenuItem MenuActivo = null;
        private static Form FormularioActivo = null;

        public Inicio(Usuario objusuario = null)
        {

            if (objusuario == null)
            {
                UsuarioActual = new Usuario() { NombreCompleto = "PRUEBA ADMIN" , IdUsuario = 22};



            }
            else
            {
                UsuarioActual = objusuario;
            }

                InitializeComponent();

        }
        private void Inicio_Load(object sender, EventArgs e)
        {
            List<Permiso> listaPermiso = new CN_Permiso().listar(UsuarioActual.IdUsuario);

            foreach(IconMenuItem item in menu.Items)
            {
                bool encontrado = listaPermiso.Any(m => m.NombreMenu == item.Name);

                if(encontrado == false)
                {
                    item.Visible = false;

                }

            }



                lblUsuario.Text = UsuarioActual.NombreCompleto;
        }


        private void AbrirFormulario(IconMenuItem menu, Form formulario)
        {
            if (MenuActivo != null){
                MenuActivo.BackColor = Color.White;

            }
            menu.BackColor = Color.Silver;
            MenuActivo = menu;

            if(FormularioActivo != null)
            {
                FormularioActivo.Close();
            }

            FormularioActivo = formulario;
            formulario.TopLevel = false;
            formulario.FormBorderStyle = FormBorderStyle.None;
            formulario.Dock = DockStyle.Fill;
            formulario.BackColor = Color.SteelBlue;
            contenedor.Controls.Add(formulario);
            formulario.Show();


        }


        // Evento para abrir el formulario de gestión de usuarios
        private void menuusuarios_Click(object sender, EventArgs e)
        {
            frmUsuarios formulario = new frmUsuarios();


            int anchoNecesario = formulario.Width;
            int altoNecesario = formulario.Height + contenedor.Location.Y;

            this.ClientSize = new Size(anchoNecesario + 20, altoNecesario + 20);


            AbrirFormulario((IconMenuItem)sender, formulario);
        }


        // Evento para abrir el formulario de categorías dentro del menú mantenedor
        private void subMenuCategoria_Click(object sender, EventArgs e)
        {
            frmCategoria formulario = new frmCategoria();

            int anchoNecesario = formulario.Width;
            int altoNecesario = formulario.Height + contenedor.Location.Y;

            this.ClientSize = new Size(anchoNecesario + 20, altoNecesario + 20);


            AbrirFormulario(menumantenedor, formulario);
        }


        // Evento para abrir el formulario de productos dentro del menú mantenedor
        private void subMenuProducto_Click(object sender, EventArgs e)
        {
            frmProducto formulario = new frmProducto();

            int anchoNecesario = formulario.Width;
            int altoNecesario = formulario.Height + contenedor.Location.Y;

            this.ClientSize = new Size(anchoNecesario + 20, altoNecesario + 20);

            AbrirFormulario(menumantenedor, formulario);
        }


        // Evento para abrir el formulario de registro de ventas
        private void submenuregistrarVenta_Click_1(object sender, EventArgs e)
        {
            frmRegistrarVentas formulario = new frmRegistrarVentas(UsuarioActual);

            int anchoNecesario = formulario.Width;
            int altoNecesario = formulario.Height + contenedor.Location.Y;

            this.ClientSize = new Size(anchoNecesario + 20, altoNecesario + 20);


            AbrirFormulario(menuventas, formulario);
        }


        // Evento para abrir el formulario de detalle de ventas
        private void submenuverdetalleVenta_Click(object sender, EventArgs e)
        {
            frmDetalleVentas formulario = new frmDetalleVentas();

            int anchoNecesario = formulario.Width;
            int altoNecesario = formulario.Height + contenedor.Location.Y;

            this.ClientSize = new Size(anchoNecesario + 20, altoNecesario + 20);

            AbrirFormulario(menuventas, formulario);
        }


        // Evento para abrir el formulario de registro de compras
        private void submenuregistrarCompra_Click(object sender, EventArgs e)
        {
            // 1. Instanciamos el formulario para poder leer sus propiedades (Tamaño)
            frmRegistrarCompras formulario = new frmRegistrarCompras(UsuarioActual);

            // 2. Calculamos el nuevo tamaño para la ventana Inicio
            // Ancho: El ancho del formulario hijo
            // Alto: La altura del formulario hijo + la posición Y donde empieza el contenedor (que son tus menús)
            int anchoNecesario = formulario.Width;
            int altoNecesario = formulario.Height + contenedor.Location.Y;

            // 3. Aplicamos el tamaño al área cliente (el área útil de la ventana sin contar bordes de Windows)
            // Le sumamos un pequeño margen (ej. 20px) para que no quede muy pegado a los bordes
            this.ClientSize = new Size(anchoNecesario + 20, altoNecesario + 20);

            // Opcional: Centrar la ventana en la pantalla después de cambiar su tamaño
            this.CenterToScreen();

            // 4. Finalmente abrimos el formulario con tu método original
            AbrirFormulario(menucompras, formulario);

        }


        // Evento para abrir el formulario de detalle de compras
        private void submenuverdetalleCompra_Click(object sender, EventArgs e)
        {
            frmDetalleCompra formulario = new frmDetalleCompra();

            int anchoNecesario = formulario.Width;
            int altoNecesario = formulario.Height + contenedor.Location.Y;
            this.ClientSize = new Size(anchoNecesario + 20, altoNecesario + 20);



            AbrirFormulario(menucompras, formulario);
        }


        // Evento para abrir el formulario de clientes
        private void menucliente_Click_1(object sender, EventArgs e)
        {
            frmClientes formulario = new frmClientes();

            int anchoNecesario = formulario.Width;
            int altoNecesario = formulario.Height + contenedor.Location.Y;
            this.ClientSize = new Size(anchoNecesario + 20, altoNecesario + 20);

            AbrirFormulario(menucliente, formulario);
        }

        // Evento para abrir el formulario de proveedores
        private void menuproveedores_Click_1(object sender, EventArgs e)
        {
            frmProveedores formulario = new frmProveedores();
            
            int anchoNecesario = formulario.Width;
            int altoNecesario = formulario.Height + contenedor.Location.Y;
            this.ClientSize = new Size(anchoNecesario + 20, altoNecesario + 20);

            AbrirFormulario(menuproveedores, formulario);
        }


        // Evento para abrir el formulario de reportes
        private void menureportes_Click(object sender, EventArgs e)
        {
            frmReportes formulario = new frmReportes();

            int anchoNecesario = formulario.Width;
            int altoNecesario = formulario.Height + contenedor.Location.Y;

            this.ClientSize = new Size(anchoNecesario + 20, altoNecesario + 20);
            AbrirFormulario(menureportes, new frmReportes());
        }

        private void menusistemaCambiario_Click(object sender, EventArgs e)
        {
         
            frmSistemaCambiario frmSistemaCambiario = new frmSistemaCambiario(UsuarioActual);

            frmSistemaCambiario.ShowDialog();
            frmSistemaCambiario.Dispose();
        }

        private void detalleNegocioToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmDetallesNegocio formulario = new frmDetallesNegocio();

            int anchoNecesario = formulario.Width;
            int altoNecesario = formulario.Height + contenedor.Location.Y;
            this.ClientSize = new Size(anchoNecesario + 20, altoNecesario + 20);

            AbrirFormulario(menumantenedor, formulario);
        }

        
    }
}
