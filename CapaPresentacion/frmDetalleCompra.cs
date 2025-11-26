using CapaEntidad;
using CapaNegocio;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CapaPresentacion
{
    public partial class frmDetalleCompra : Form
    {
        public frmDetalleCompra()
        {
            InitializeComponent();
        }

        private void btnBuscar_Click(object sender, EventArgs e)
        { 
            Compra oCompra = new CN_Compra().ObtenerCompra(txtBusqueda.Text);

            if(oCompra.IdCompra != 0)
            {
                nrmDocumento.Text = oCompra.NumeroDocumento;

                txtFecha.Text = oCompra.FechaRegistro;
                txtTipoDocumento.Text = oCompra.TipoDocumento;
                txtUsuario.Text = oCompra.oUsuario.NombreCompleto;
                txtdocProveedor.Text = oCompra.oProveedor.Documento;
                txtRazonSocial.Text = oCompra.oProveedor.RazonSocial;

                dgvData.Rows.Clear();
                foreach(Detalle_Compra dc in oCompra.oDetalleCommpra)
                {
                    dgvData.Rows.Add(new object[] {dc.oProducto.NombreProducto, dc.PrecioCompra, dc.PrecioVenta, dc.Cantidad, dc.MontoTotal});


                }

                txtTotalaPagar.Text = oCompra.MontoTotal.ToString("0.00");





            }
        }
    } 
}
