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
            MessageBox.Show($"Usuario actual: {_usuarioActual.NombreCompleto}", "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);

            cboTipoDocumento.Items.Add(new OpcionCombo() { Valor = "Boleta", Texto = "Boleta" });
            cboTipoDocumento.Items.Add(new OpcionCombo() { Valor = "Factura", Texto = "Factura" });
            cboTipoDocumento.DisplayMember = "Texto";
            cboTipoDocumento.ValueMember = "Valor";
            cboTipoDocumento.SelectedIndex = 0;

        }
    }
}
