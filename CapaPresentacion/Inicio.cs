using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
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

            

            try
            {
                CN_RegistroUsuario cnRegistro = new CN_RegistroUsuario();
                TasaGeneralUsuario tasaRecuperada = cnRegistro.ObtenerTasaGeneral(UsuarioActual.IdUsuario);

                if (tasaRecuperada != null)
                {
                    UsuarioActual.oTasaGeneral = tasaRecuperada;
                    ActualizarLabelTasa(); // Llamamos al método que pinta el texto
                }
                else
                {
                    lbltasa.Text = "Tasa: No definida";
                }
            }
            catch
            {
                lbltasa.Text = "Tasa: Error al cargar";
            }
        }

        // Método para refrescar el texto del label desde cualquier parte del sistema
        public void ActualizarLabelTasa()
        {
            if (UsuarioActual.oTasaGeneral != null)
            {
                lbltasa.Text = $"Tasa: {UsuarioActual.oTasaGeneral.MonedaAbreviacion} = {UsuarioActual.oTasaGeneral.Valor:N2} VES";
                //lbltasa.Text = $"Tasa: {UsuarioActual.oTasaGeneral.DisplayTasa}";
                //lbltasa.ForeColor = Color.Green; // Opcional: para resaltar que está activa
            }
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


        // Eventos para abrir los formularios de reportes
        private void reportesDeVentasToolStripMenuItem_Click(object sender, EventArgs e)
        {

            frmReporteVentas formulario = new frmReporteVentas();

            int anchoNecesario = formulario.Width;
            int altoNecesario = formulario.Height + contenedor.Location.Y;

            this.ClientSize = new Size(anchoNecesario + 20, altoNecesario + 20);

            AbrirFormulario(menureportes, formulario);

        }

        private void reportesDeComprasToolStripMenuItem_Click(object sender, EventArgs e)
        {

            frmReporteCompras formulario = new frmReporteCompras();

            int anchoNecesario = formulario.Width;
            int altoNecesario = formulario.Height + contenedor.Location.Y;

            this.ClientSize = new Size(anchoNecesario + 20, altoNecesario + 20);

            AbrirFormulario(menureportes, formulario);
        }

        private void reportesDeInventarioToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmReporteInventario formulario = new frmReporteInventario();

            int anchoNecesario = formulario.Width;
            int altoNecesario = formulario.Height + contenedor.Location.Y;

            this.ClientSize = new Size(anchoNecesario + 20, altoNecesario + 20);

            AbrirFormulario(menureportes, formulario);
        }

        // ---------------------------------------------------------------------------------------

        private void menusistemaCambiario_Click(object sender, EventArgs e)
        {

            using (frmSistemaCambiario frm = new frmSistemaCambiario(UsuarioActual))
            {
                frm.ShowDialog();
            }

            // Una vez cerrada la ventana, refrescamos el label con el nuevo valor en memoria
            ActualizarLabelTasa();
        }

        private void detalleNegocioToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmDetallesNegocio formulario = new frmDetallesNegocio();

            int anchoNecesario = formulario.Width;
            int altoNecesario = formulario.Height + contenedor.Location.Y;
            this.ClientSize = new Size(anchoNecesario + 20, altoNecesario + 20);

            AbrirFormulario(menumantenedor, formulario);
        }

        private void menuflujo_caja_Click(object sender, EventArgs e)
        {
            frmFlujoCaja formulario = new frmFlujoCaja(UsuarioActual);

            int anchoNecesario = formulario.Width;
            int altoNecesario = formulario.Height + contenedor.Location.Y;
            this.ClientSize = new Size(anchoNecesario + 20, altoNecesario + 20);

            AbrirFormulario((IconMenuItem)sender, formulario);
        }
    }
}
