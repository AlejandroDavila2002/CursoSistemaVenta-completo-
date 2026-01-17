using CapaEntidad;
using CapaNegocio;
using CapaPresentacion.utilidades;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using CapaPresentacion.modales;

namespace CapaPresentacion
{
    public partial class frmFlujoCaja : Form
    {
        private Usuario usuarioActual;

        public frmFlujoCaja(Usuario oUsuario)
        {
            InitializeComponent();
            usuarioActual = oUsuario;
        }

        private void frmFlujoCaja_Load(object sender, EventArgs e)
        {
            dgvDataGastosOperativos.CellPainting += new DataGridViewCellPaintingEventHandler(dgvDataGastosOperativos_CellPainting);

            // Inicializar fechas al día de hoy
            txtInicio.Value = DateTime.Now;
            txtFin.Value = DateTime.Now;

            CargarCombos();
           // ConfigurarGrid();

            // Cargar datos por defecto (del día de hoy)
            CargarGastosOperativos();
            CalcularFinanzas();



        }

        private void CargarCombos()
        {
            // 1. CARGAR FORMAS DE PAGO (Desde Base de Datos)
            cboFormaPago.Items.Clear();
            List<FormaPago> listaPagos = new CN_FlujoCaja().ListarFormasPago();

            foreach (FormaPago item in listaPagos)
            {
                cboFormaPago.Items.Add(new OpcionCombo() { Valor = item.Descripcion, Texto = item.Descripcion });
            }
            cboFormaPago.DisplayMember = "Texto";
            cboFormaPago.ValueMember = "Valor";

            if (cboFormaPago.Items.Count > 0) cboFormaPago.SelectedIndex = 0;


            // 2. CARGAR CATEGORÍAS (Desde Base de Datos)
            cboCategoria.Items.Clear();
            List<CategoriaGasto> listaCategoria = new CN_FlujoCaja().ListarCategorias();

            foreach (CategoriaGasto item in listaCategoria)
            {
                // OJO: Aquí el Valor es el ID (int), no el texto
                cboCategoria.Items.Add(new OpcionCombo() { Valor = item.IdCategoriaGasto, Texto = item.Descripcion });
            }
            cboCategoria.DisplayMember = "Texto";
            cboCategoria.ValueMember = "Valor";

            if (cboCategoria.Items.Count > 0) cboCategoria.SelectedIndex = 0;


            // 3. FILTROS DE BÚSQUEDA (Grilla)
            cboGastosOperativos.Items.Clear();
            foreach (DataGridViewColumn col in dgvDataGastosOperativos.Columns)
            {
                if (col.Visible && col.Name != "btnEliminar")
                {
                    cboGastosOperativos.Items.Add(new OpcionCombo() { Valor = col.Name, Texto = col.HeaderText });
                }
            }
            cboGastosOperativos.DisplayMember = "Texto";
            cboGastosOperativos.ValueMember = "Valor";

            if (cboGastosOperativos.Items.Count > 0) cboGastosOperativos.SelectedIndex = 0;
        }

        private void CargarGastosOperativos()
        {
            string fechaInicio = txtInicio.Value.ToString("yyyy-MM-dd");
            string fechaFin = txtFin.Value.ToString("yyyy-MM-dd");

            List<Gasto> lista = new CN_FlujoCaja().Listar(fechaInicio, fechaFin);

            dgvDataGastosOperativos.Rows.Clear();
            decimal total = 0;

            foreach (Gasto g in lista)
            {
                // CORRECCIÓN: El orden debe ser IDÉNTICO al de las columnas en el Diseño
                dgvDataGastosOperativos.Rows.Add(
                    g.IdGasto,                     // Columna 0
                    g.FechaRegistro,               // Columna 1 (Fecha)
                    g.oCategoriaGasto.Descripcion, // Columna 2 (Categoria)
                    g.Descripcion,                 // Columna 3 (Descripción)
                    g.FormaPago,                   // Columna 4 (FormaPago)
                    g.Referencia,                  // Columna 5 (Referencia)
                    g.Monto,                       // Columna 6 (Monto)
                    ""                             // Columna 7 (Botón Eliminar)
                );
                total += g.Monto;
            }

            lblTotaldgvDataGastosOperativos.Text =  "- "+ total.ToString("N2");
            lblTotaldgvDataGastosOperativos.ForeColor = Color.Red;

        }

        // --- BOTÓN GUARDAR ---
        private void btnGuardarGastosOperativos_Click(object sender, EventArgs e)
        {
            string mensaje = string.Empty;

            Gasto obj = new Gasto()
            {
                oUsuario = new Usuario() { IdUsuario = usuarioActual.IdUsuario },
                oCategoriaGasto = new CategoriaGasto() { IdCategoriaGasto = Convert.ToInt32(((OpcionCombo)cboCategoria.SelectedItem).Valor) },
                Descripcion = txtDescripcion.Text,
                Monto = string.IsNullOrEmpty(txtMonto.Text) ? 0 : Convert.ToDecimal(txtMonto.Text),
                Referencia = txtReferencia.Text,
                FormaPago = ((OpcionCombo)cboFormaPago.SelectedItem).Valor.ToString(),
            };

            bool resultado = new CN_FlujoCaja().Registrar(obj, out mensaje);

            if (resultado)
            {
                MessageBox.Show("Gasto registrado correctamente", "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LimpiarCampos();
                CargarGastosOperativos();
                CalcularFinanzas();
            }
            else
            {
                MessageBox.Show(mensaje, "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        // --- BOTÓN ELIMINAR (En la celda) ---
        private void dgvDataGastosOperativos_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dgvDataGastosOperativos.Columns[e.ColumnIndex].Name == "btnEliminar" && e.RowIndex >= 0)
            {
                // Validación de Administrador (Rol = 1)
                if (usuarioActual.oRol.IdRol != 1)
                {
                    MessageBox.Show("Solo los administradores pueden eliminar gastos.", "Seguridad", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (MessageBox.Show("¿Desea eliminar este gasto permanentemente?", "Confirmar", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    int idGasto = Convert.ToInt32(dgvDataGastosOperativos.Rows[e.RowIndex].Cells["IdGasto"].Value); // Asegúrate que la columna se llame así en el diseño
                    string mensaje = string.Empty;

                    bool respuesta = new CN_FlujoCaja().Eliminar(idGasto, out mensaje);

                    if (respuesta)
                    {
                        dgvDataGastosOperativos.Rows.RemoveAt(e.RowIndex);
                        // Recalcular total visualmente sin ir a BD si prefieres optimizar, o recargar todo:
                        CargarGastosOperativos();
                        CalcularFinanzas();
                    }
                    else
                    {
                        MessageBox.Show(mensaje, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        // --- PINTAR EL ICONO ---
        private void dgvDataGastosOperativos_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            // Evitamos cabeceras o índices inválidos
            if (e.RowIndex < 0 || e.ColumnIndex < 0 || e.ColumnIndex >= dgvDataGastosOperativos.Columns.Count)
                return;

            // Comprobamos por nombre de columna (asegúrate que existe una columna llamada "btnEliminar" en el diseñador)
            if (dgvDataGastosOperativos.Columns[e.ColumnIndex].Name == "btnEliminar")
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


        // --- FILTROS Y LIMPIEZA ---
        private void btnBuscarGastosOperativos_Click(object sender, EventArgs e)
        {
            string columnaFiltro = ((OpcionCombo)cboGastosOperativos.SelectedItem).Valor.ToString();
            string texto = txtGastosOperativos.Text.Trim().ToUpper();

            foreach (DataGridViewRow row in dgvDataGastosOperativos.Rows)
            {
                if (row.Cells[columnaFiltro].Value.ToString().Trim().ToUpper().Contains(texto))
                    row.Visible = true;
                else
                    row.Visible = false;
            }

        }

        private void btnLimpiarGastosOperativos_Click(object sender, EventArgs e)
        {
            txtGastosOperativos.Text = "";
            cboGastosOperativos.SelectedIndex = 0;
            foreach (DataGridViewRow row in dgvDataGastosOperativos.Rows) row.Visible = true;
        }

        private void LimpiarCampos()
        {
            txtDescripcion.Text = "";
            txtReferencia.Text = "";
            txtMonto.Text = "";
            cboCategoria.SelectedIndex = 0;
            cboFormaPago.SelectedIndex = 0;
            dtpFechaGasto.Value = DateTime.Now;
        }

        // Evento del botón "Buscar" general (txtInicio, txtFin)
        private void btnBuscar_Click(object sender, EventArgs e)
        {
            CargarGastosOperativos();

            CalcularFinanzas();
        }


        private void CalcularFinanzas()
        {
            string fechaInicio = txtInicio.Value.ToString("yyyy-MM-dd");
            string fechaFin = txtFin.Value.ToString("yyyy-MM-dd");

            // Obtenemos el resumen y nos aseguramos de no tener null
            var resumen = new CN_FlujoCaja().ObtenerResumen(fechaInicio, fechaFin) ?? new Dictionary<string, decimal>();

            // Valores por defecto
            decimal ingresos = 0m;
            decimal egresosMercancia = 0m;
            decimal gastosOperativos = 0m;

            // Intentamos leer las claves de forma segura
            resumen.TryGetValue("TotalIngresos", out ingresos);
            resumen.TryGetValue("TotalEgresosMercancia", out egresosMercancia);
            resumen.TryGetValue("TotalGastosOperativos", out gastosOperativos);

            lblCantidadIngresos.Text = ingresos.ToString("N2");
            lblCantidadEgresos.Text = egresosMercancia.ToString("N2");

            decimal totalSalidasReales = egresosMercancia + gastosOperativos;
            decimal utilidadNeta = ingresos - totalSalidasReales;

            lblUtilidadNeta.Text = utilidadNeta.ToString("N2");

            if (utilidadNeta >= 0)
            {
                lblUtilidadNeta.ForeColor = Color.White;
                lblUtilidadNeta.Text = "+ " + lblUtilidadNeta.Text;
            }
            else
            {
                lblUtilidadNeta.ForeColor = Color.Red;
            }

            CargarGrafico(ingresos, totalSalidasReales);
        }

        private void CargarGrafico(decimal ingresos, decimal gastosTotales)
        {
            // Limpiamos configuración previa
            VentasVSGastos.Series.Clear();
            VentasVSGastos.Titles.Clear();
            VentasVSGastos.Titles.Add("Balance Financiero");

            // Creamos la serie
            Series serie = new Series("Finanzas");
            serie.ChartType = SeriesChartType.Doughnut; // Tipo Dona (o Pie)
            serie.IsValueShownAsLabel = true; // Mostrar valor en el gráfico
            serie.LabelFormat = "N2";

            // Agregamos los puntos
            // Punto 1: Ingresos (Verde)
            DataPoint pIngresos = new DataPoint(0, (double)ingresos);
            pIngresos.LegendText = "Ingresos";
            pIngresos.Color = Color.SeaGreen;
            serie.Points.Add(pIngresos);

            // Punto 2: Gastos Totales (Rojo)
            DataPoint pGastos = new DataPoint(0, (double)gastosTotales);
            pGastos.LegendText = "Egresos Totales";
            pGastos.Color = Color.IndianRed;
            serie.Points.Add(pGastos);

            // Agregamos la serie al chart
            VentasVSGastos.Series.Add(serie);

            // Forzamos actualización visual
            VentasVSGastos.Update();
        }



        // --- BOTÓN AGREGAR CATEGORÍA (+) ---
        private void btnAgregarCategoria_Click(object sender, EventArgs e)
        {
            // Abrimos el modal como una pequeña ventana emergente
            using (var modal = new mdCategoriaGasto())
            {
                var result = modal.ShowDialog();

                // Si el usuario guardó o eliminó algo (nos devolvió OK), recargamos la lista
                if (result == DialogResult.OK)
                {
                    CargarCombos();
                }
            }
        }

        // --- BOTÓN AGREGAR FORMA DE PAGO (+) ---
        private void btnAgregarFormaPago_Click(object sender, EventArgs e)
        {
            using (var modal = new mdFormaPago())
            {
                var result = modal.ShowDialog();

                if (result == DialogResult.OK)
                {
                    CargarCombos();
                }
            }
        }





    }
}