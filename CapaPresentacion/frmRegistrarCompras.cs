using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CapaEntidad;
using CapaPresentacion.utilidades;

namespace CapaPresentacion
{
    public partial class frmRegistrarCompras : Form
    {
       private Usuario _usuarioActual;
        public frmRegistrarCompras(Usuario usuarioActual)
        {
            InitializeComponent();
            _usuarioActual = usuarioActual;
        }

        private void frmRegistrarCompras_Load(object sender, EventArgs e)
        { 

            cboTipoDocumento.Items.Add(new OpcionCombo() { Valor = "Boleta", Texto = "Boleta" });
            cboTipoDocumento.Items.Add(new OpcionCombo() { Valor = "Factura", Texto = "Factura" });
            cboTipoDocumento.DisplayMember = "Texto";
            cboTipoDocumento.ValueMember = "Valor";
            cboTipoDocumento.SelectedIndex = 0;

            txtFecha.Text = DateTime.Now.ToString("dd/MM/yyyy");

            txtIdProveedor.Text = "0";
            txtIdProducto.Text = "0";


        }

        private void btnBuscarProveedor_Click(object sender, EventArgs e)
        {
            using (modales.mdProveedores mdProveedores = new modales.mdProveedores())
            {
                mdProveedores.ShowDialog();

                if (mdProveedores._Proveedor != null)
                {
                    txtIdProveedor.Text = mdProveedores._Proveedor.IdProveedor.ToString();
                    txtDocumentoProveedor.Text = mdProveedores._Proveedor.Documento;
                    txtRazonSocialProveedor.Text = mdProveedores._Proveedor.RazonSocial;
                }
                else
                {
                    txtIdProveedor.Text = "0";
                    txtDocumentoProveedor.Text = "";
                    txtRazonSocialProveedor.Text = "";

                    MessageBox.Show("No se seleccionó ningún proveedor", "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }

            }
        }
    }
}
