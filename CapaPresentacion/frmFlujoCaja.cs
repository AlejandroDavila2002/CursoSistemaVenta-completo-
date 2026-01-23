using CapaEntidad;
using CapaNegocio;
using CapaPresentacion.modales;
using CapaPresentacion.utilidades;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

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
            dgvDataDeudores.CellDoubleClick += new DataGridViewCellEventHandler(dgvDataDeudores_CellDoubleClick);

            // Inicializar fechas al día de hoy
            txtInicio.Value = DateTime.Now;
            txtFin.Value = DateTime.Now;

            CargarCombos();
            InicializarComboDeudores();
            // ConfigurarGrid();

            // Cargar datos por defecto (del día de hoy)
            CargarGastosOperativos();
            CalcularFinanzas();

            CargarDeudores();
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

            // 1. Obtenemos los datos totales desde la BD
            var resumen = new CN_FlujoCaja().ObtenerResumen(fechaInicio, fechaFin);

            decimal ingresos = resumen["TotalIngresos"];          // Total Ventas
            decimal egresosMercancia = resumen["TotalEgresosMercancia"]; // Total Compras
            decimal gastosOperativos = resumen["TotalGastosOperativos"]; // Total Gastos (Luz, Nomina...)

            // 2. Calculamos Resultados Monetarios
            decimal totalSalidasReales = egresosMercancia + gastosOperativos;
            decimal utilidadNeta = ingresos - totalSalidasReales;

            // 3. Mostramos Montos (Formato Moneda)
            lblCantidadIngresos.Text = ingresos.ToString("N2");

            // Aquí mostramos SOLO lo gastado en mercancía (Compras), según tu requerimiento anterior
            lblCantidadEgresos.Text = egresosMercancia.ToString("N2");

            lblUtilidadNeta.Text = utilidadNeta.ToString("N2");

            // --- 4. CÁLCULO DE MÁRGENES (PORCENTAJES) ---
            // Usamos validación (ingresos > 0) para evitar error de "división entre cero"

            // A. Margen Bruto (En Ingresos): Cuánto ganamos sobre el producto vendido
            // Nota: Si es negativo, significa que compramos más mercancía de la que vendimos (stock)
            decimal margenIngresos = ingresos > 0 ? ((ingresos - egresosMercancia) / ingresos) * 100 : 0;
            lblMargenIngresos.Text = string.Format("{0:N2}%", margenIngresos);

            // B. Margen Egresos (En Egresos): Qué % de la venta se va en reponer mercancía
            decimal margenEgresos = ingresos > 0 ? (egresosMercancia / ingresos) * 100 : 0;
            lblMargenEgresos.Text = string.Format("{0:N2}%", margenEgresos);

            // C. Margen Utilidad (Neto): El % real de ganancia final
            decimal margenUtilidad = ingresos > 0 ? (utilidadNeta / ingresos) * 100 : 0;
            lblMargenUtilidad.Text = string.Format("{0:N2}%", margenUtilidad);

            // --- 5. COLORES INTELIGENTES ---

            // Si la utilidad es positiva, verde. Negativa, rojo.
            if (utilidadNeta >= 0)
            {
                lblUtilidadNeta.ForeColor = Color.White;
                lblMargenUtilidad.ForeColor = Color.White;
                lblUtilidadNeta.Text = "+ " + lblUtilidadNeta.Text;
            }
            else
            {
                lblUtilidadNeta.ForeColor = Color.IndianRed;
                lblMargenUtilidad.ForeColor = Color.IndianRed;
            }

            // 6. Actualizar Gráfico
            // Enviamos Ingresos vs Salidas Totales (Compras + Gastos) para ver la realidad
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



        // --- SECCIÓN DEUDORES PENDIENTES (CRÉDITOS) ---

        // Dentro de frmFlujoCaja.cs

        private void CargarDeudores()
        {
            // 1. Configuración de Columnas (Programática para seguridad)
            dgvDataDeudores.Columns.Clear();

            // Columna OCULTA (La llave para abrir el modal)
            var colId = dgvDataDeudores.Columns.Add("IdVenta", "IdVenta");
            dgvDataDeudores.Columns[colId].Visible = false;

            // Columnas VISIBLES
            dgvDataDeudores.Columns.Add("Cliente", "Cliente");
            dgvDataDeudores.Columns.Add("MontoCliente", "Monto Deuda");
            dgvDataDeudores.Columns.Add("FechaDeuda", "Fecha Deuda");

            // Ajustes estéticos
            dgvDataDeudores.AllowUserToAddRows = false;
            dgvDataDeudores.ReadOnly = true;
            dgvDataDeudores.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvDataDeudores.SelectionMode = DataGridViewSelectionMode.FullRowSelect; // Selección de fila completa

            // 2. Obtener datos y llenar
            List<CuentaPorCobrar> lista = new CN_CuentaPorCobrar().Listar();
            decimal totalDeudaPendiente = 0;

            foreach (CuentaPorCobrar c in lista)
            {
                if (c.SaldoPendiente > 0)
                {
                    dgvDataDeudores.Rows.Add(
                        c.oVenta.IdVenta,                 // Valor Oculto (IdVenta)
                        c.oCliente.NombreCompleto,        // Cliente
                        c.SaldoPendiente.ToString("N2"),  // Monto
                        c.FechaRegistro                   // Fecha
                    );

                    totalDeudaPendiente += c.SaldoPendiente;
                }
            }

            lblTotalDeudoresPendientes.Text = totalDeudaPendiente.ToString("N2");
        }
        private void InicializarComboDeudores()
        {
            // Configuración del combo de búsqueda para deudores
            cboBusquedaDeudores.Items.Clear();

            // Agregamos las opciones basadas en las columnas visibles que definimos
            // Asegúrate que el "Name" de la columna en el diseñador coincida con "Cliente", "Monto", "Fecha" o ajusta aquí
            cboBusquedaDeudores.Items.Add(new OpcionCombo() { Valor = "Cliente", Texto = "Cliente" });
            cboBusquedaDeudores.Items.Add(new OpcionCombo() { Valor = "FechaDeuda", Texto = "Fecha" });
            cboBusquedaDeudores.Items.Add(new OpcionCombo() { Valor = "MontoCliente", Texto = "Monto" });

            cboBusquedaDeudores.DisplayMember = "Texto";
            cboBusquedaDeudores.ValueMember = "Valor";

            if (cboBusquedaDeudores.Items.Count > 0) cboBusquedaDeudores.SelectedIndex = 0;
        }

        // --- EVENTOS DE BOTONES (BÚSQUEDA Y LIMPIEZA) ---

        private void btnBuscarDeudores_Click(object sender, EventArgs e)
        {
            string columnaFiltro = ((OpcionCombo)cboBusquedaDeudores.SelectedItem).Valor.ToString();
            string textoBusqueda = txtBusquedaDeudores.Text.Trim().ToUpper();

            if (dgvDataDeudores.Rows.Count > 0)
            {
                foreach (DataGridViewRow row in dgvDataDeudores.Rows)
                {
                    // Obtenemos el valor de la celda según la columna seleccionada
                    // NOTA: Asegúrate que las columnas en el diseñador se llamen "Cliente" y "FechaDeuda"
                    if (row.Cells[columnaFiltro].Value.ToString().Trim().ToUpper().Contains(textoBusqueda))
                    {
                        row.Visible = true;
                    }
                    else
                    {
                        row.Visible = false;
                    }
                }
            }
        }

        private void btnLimpiarDeudores_Click(object sender, EventArgs e)
        {
            txtBusquedaDeudores.Text = "";
            foreach (DataGridViewRow row in dgvDataDeudores.Rows)
            {
                row.Visible = true;
            }
        }

        private void dgvDataDeudores_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            // Validamos que el clic sea en una fila válida
            if (e.RowIndex >= 0)
            {
                try
                {
                    // 1. Recuperamos el ID de la venta de la columna oculta "IdVenta"
                    int idVentaSeleccionada = Convert.ToInt32(dgvDataDeudores.Rows[e.RowIndex].Cells["IdVenta"].Value);

                    // 2. Recuperamos la fecha (solo por si el modal la pide visualmente)
                    string fechaDeuda = dgvDataDeudores.Rows[e.RowIndex].Cells["FechaDeuda"].Value.ToString();

                    // 3. Abrimos el modal reutilizando el que ya existe
                    using (var modal = new CapaPresentacion.modales.mdlDetalleCuotas(idVentaSeleccionada, fechaDeuda))
                    {
                        modal.ShowDialog();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error al abrir el detalle: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnCerrarCaja_Click(object sender, EventArgs e)
        {
            // 1. Validar si ya se cerró HOY (Lógica de Turnos)
            string fechaCierre = DateTime.Now.ToString("yyyy-MM-dd");
            CN_FlujoCaja negocio = new CN_FlujoCaja();
            string mensajeTurno = string.Empty;
            bool existeCierreHoy = negocio.ValidarCierreExistente(fechaCierre, out mensajeTurno);

            // 2. Obtener Datos y ALERTA DE PENDIENTES ANTIGUOS
            decimal montoTeoricoNeto = 0;
            bool hayPendientesAntiguos = false;

            // Llamamos al nuevo método
            DataTable dtVentasCierre = negocio.ObtenerDetalleCierre(fechaCierre, out montoTeoricoNeto, out hayPendientesAntiguos);

            // --- CONSTRUCCIÓN DEL MENSAJE INTELIGENTE ---
            string mensajeFinal = "¿Desea realizar el Cierre de Caja?";
            MessageBoxIcon icono = MessageBoxIcon.Question;

            if (hayPendientesAntiguos)
            {
                mensajeFinal = "⚠️ ¡ADVERTENCIA IMPORTANTE!\n\n" +
                               "Se han detectado ventas o movimientos de DÍAS ANTERIORES que no fueron cerrados.\n" +
                               "Estos montos se incluirán automáticamente en este cierre para regularizar la caja.\n\n" +
                               "¿Desea proceder con el Cierre Acumulado?";
                icono = MessageBoxIcon.Warning;
            }
            else if (existeCierreHoy)
            {
                mensajeFinal = "Ya existen cierres registrados el día de hoy.\n\n" +
                               "¿Desea cerrar el TURNO ACTUAL y contabilizar solo los movimientos nuevos?";
                icono = MessageBoxIcon.Information;
            }

            // Mostrar el mensaje construido
            if (MessageBox.Show(mensajeFinal, "Confirmar Cierre", MessageBoxButtons.YesNo, icono) == DialogResult.No)
            {
                return;
            }

            // Validar si hay datos (Si no hay nada de nada)
            if (dtVentasCierre.Rows.Count == 0 && montoTeoricoNeto == 0)
            {
                if (MessageBox.Show("No hay movimientos nuevos para cerrar. ¿Cerrar en cero?", "Sin Movimientos", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.No)
                    return;
            }

            // 3. ABRIR EL MODAL (Igual que antes)
            using (var modal = new CapaPresentacion.modales.mdCierreCaja(montoTeoricoNeto, dtVentasCierre))
            {
                var result = modal.ShowDialog();
                if (result == DialogResult.OK)
                {
                    try
                    {
                        CierreCaja oCierre = new CierreCaja()
                        {
                            oUsuario = new Usuario() { IdUsuario = 1 }, // Variable Global
                            MontoTeorico = montoTeoricoNeto,
                            MontoReal = modal.MontoRealIngresado,
                            Observacion = modal.ObservacionIngresada
                        };

                        string mensaje = string.Empty;
                        bool respuesta = negocio.RegistrarCierre(oCierre, dtVentasCierre, out mensaje);

                        if (respuesta)
                            MessageBox.Show($"Cierre Exitoso.\nDiferencia: $ {oCierre.MontoReal - oCierre.MontoTeorico}", "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        else
                            MessageBox.Show(mensaje, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error: " + ex.Message);
                    }
                }
            }
        }
    }
}