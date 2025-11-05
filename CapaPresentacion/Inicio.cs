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

        public Inicio(Usuario objusuario)
        {

            UsuarioActual = objusuario;
                    

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
            AbrirFormulario((IconMenuItem)sender, new frmUsuarios());
        }


        // Evento para abrir el formulario de categorías dentro del menú mantenedor
        private void subMenuCategoria_Click(object sender, EventArgs e)
        {
            AbrirFormulario(menumantenedor, new frmCategoria());
        }


        // Evento para abrir el formulario de productos dentro del menú mantenedor
        private void subMenuProducto_Click(object sender, EventArgs e)
        {
            AbrirFormulario(menumantenedor, new frmProducto());
        }


        // Evento para abrir el formulario de registro de ventas
        private void submenuregistrarVenta_Click_1(object sender, EventArgs e)
        {
            AbrirFormulario(menuventas, new frmRegistrarVentas());
        }


        // Evento para abrir el formulario de detalle de ventas
        private void submenuverdetalleVenta_Click(object sender, EventArgs e)
        {
            AbrirFormulario(menuventas, new frmDetalleVentas());
        }


        // Evento para abrir el formulario de registro de compras
        private void submenuregistrarCompra_Click(object sender, EventArgs e)
        {
            AbrirFormulario(menucompras, new frmRegistrarCompras());
        }


        // Evento para abrir el formulario de detalle de compras
        private void submenuverdetalleCompra_Click(object sender, EventArgs e)
        {
            AbrirFormulario(menucompras, new frmDetalleCompra());
        }


        // Evento para abrir el formulario de clientes
        private void menucliente_Click_1(object sender, EventArgs e)
        {
            AbrirFormulario(menucliente, new frmClientes());
        }

        // Evento para abrir el formulario de proveedores
        private void menuproveedores_Click_1(object sender, EventArgs e)
        {
            AbrirFormulario(menuproveedores, new frmProveedores());
        }


        // Evento para abrir el formulario de reportes
        private void menureportes_Click(object sender, EventArgs e)
        {
            AbrirFormulario(menureportes, new frmReportes());
        }

        private void menusistemaCambiario_Click(object sender, EventArgs e)
        {
            // CAMBIO CLAVE: 
            // Ahora le pasamos 'UsuarioActual' (que tiene tu ID 6) al constructor
            frmSistemaCambiario frmSistemaCambiario = new frmSistemaCambiario(UsuarioActual);

            frmSistemaCambiario.ShowDialog();
            frmSistemaCambiario.Dispose();
        }
    }
}
