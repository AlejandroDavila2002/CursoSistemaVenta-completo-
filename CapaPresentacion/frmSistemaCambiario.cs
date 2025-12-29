using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using CapaEntidad; // Necesario para el objeto Usuario y TasaCambio
using CapaNegocio; // Necesario para CN_RegistroUsuario
using System.Globalization;
using System.Threading.Tasks;
using System.Drawing;


// (nota, debes enlazar los datos de las tasas segun el IdUsuario a una variable general para que sea utilizada por el sistema )
//(nota cuando guardas un dato de alguna tasa, te sale un error emergente, debes corregirlo) 
namespace CapaPresentacion
{
    public partial class frmSistemaCambiario : Form
    {

        private Usuario UsuarioActual;

        // Diccionario para mantener el registro completo de las tasas BCV actuales (Abreviación -> TasaCambio)
        private Dictionary<string, TasaCambio> _tasasActuales = new Dictionary<string, TasaCambio>();



        public frmSistemaCambiario(Usuario usuarioLogueado)
        {
            InitializeComponent();
            this.UsuarioActual = usuarioLogueado;
            InicializarDataGridView();
            // Vinculamos el evento para la selección del botón de Tasa General
            dgvtasasdecambio.CellContentClick += dgvtasasdecambio_CellContentClick;
            // SetupTimer(); // Si tienes un Timer para actualización automática
        }

        // CAMBIO 1: Convertido a "async void"
        private async void frmSistemaCambiario_Load(object sender, EventArgs e)
        {
            // Carga los datos al iniciar el formulario
            // CAMBIO 2: Añadido "await"
            await CargarDatosTasaCambio();

            // 2. FUNCIÓN CLAVE: Carga el historial de operaciones del usuario logueado en la misma tabla
            CargarHistorialUsuario();
            // _actualizacionTimer.Start(); 
        }

        /// <summary>
        /// Carga el historial de operaciones de tasas de cambio registradas previamente por el usuario actual.
        /// Estos datos se cargan en el dgvtasasdecambio.
        /// </summary>
        private void CargarHistorialUsuario()
        {
            if (UsuarioActual == null || UsuarioActual.IdUsuario <= 0) return;

            try
            {
                CN_RegistroUsuario cnRegistro = new CN_RegistroUsuario();

                // Llama a la capa de negocio para obtener el historial.
                // Se espera que 'historial' sea una lista que se pueda mapear a las columnas Moneda, FechaBCV y Monto.
                // Ejemplo de lo que debe retornar la CapaNegocio: List<object[]> donde object[] = { "Euro (EUR)", "2024-11-05", 45000.50m }
                var historial = cnRegistro.ObtenerHistorialOperacionesPorUsuario(UsuarioActual.IdUsuario);

                // **PASO CLAVE 1: Limpiar la tabla antes de cargar el historial**
                // Esto asegura que solo se muestre el estado actual de la BD o las nuevas entradas después de un 'Agregar'.
                dgvtasasdecambio.Rows.Clear();

                foreach (var registro in historial)
                {
                    // Asumiendo que 'registro' es una tupla, DataRow o una clase que se mapea a los valores:
                    // [0] = Moneda (ej: "Euro (EUR)")
                    // [1] = FechaBCV (ej: "2024-11-05")
                    // [2] = Monto Operación (decimal)

                    int index = dgvtasasdecambio.Rows.Add();
                    dgvtasasdecambio.Rows[index].Cells["Moneda"].Value = registro[0];
                    dgvtasasdecambio.Rows[index].Cells["FechaBCV"].Value = registro[1];
                    // Formatea el monto para visualización
                    dgvtasasdecambio.Rows[index].Cells["Monto"].Value = ((decimal)registro[2]).ToString("N2", CultureInfo.InvariantCulture);

                    // Opcional: Aplicar un estilo diferente para diferenciar el historial (ya guardado) de las nuevas filas (aún no guardadas)
                    dgvtasasdecambio.Rows[index].DefaultCellStyle.BackColor = Color.LightGray;
                }

            }
            catch (Exception ex)
            {
                // Un error aquí no es crítico para el uso de la aplicación, pero debe ser notificado.
                MessageBox.Show($"Error al cargar el historial del usuario: {ex.Message}. La tabla se mostrará vacía.", "Error de Carga de Historial", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        /// <summary>
        /// Carga las tasas de cambio desde la Capa de Negocio y llena el ComboBox y las variables internas.
        /// </summary>
        // CAMBIO 3: Convertido a "async Task"
        private async Task CargarDatosTasaCambio()
        {
            try
            {
                CN_RegistroUsuario cnRegistro = new CN_RegistroUsuario();

                // 1. Obtener las últimas tasas
                // CAMBIO 4: Se llama al nuevo método asíncrono que invoca al Scraper
                _tasasActuales = await cnRegistro.ObtenerTasasActualizadasAsync();

                // 2. Limpiar y llenar el ComboBox
                cboTasaDeCambio.Items.Clear();

                if (_tasasActuales.Count > 0)
                {
                    // Llenar el ComboBox con el formato "NombreCompleto (Abreviacion)"
                    foreach (var tasa in _tasasActuales.Values.OrderBy(t => t.MonedaAbreviacion))
                    {
                        cboTasaDeCambio.Items.Add($"{tasa.NombreCompleto} ({tasa.MonedaAbreviacion})");
                    }
                    cboTasaDeCambio.SelectedIndex = 4; // Seleccionar el primer elemento para disparar el evento Changed
                    cboTasaDeCambio.Enabled = true;
                    txtMonto.Enabled = true;
                    // Uso btnBuscar ya que es la variable declarada en la línea 105, aunque el evento se llama btnAgregar_Click
                    btnBuscar.Enabled = true;
                }
                else
                {
                    // Manejo si no hay datos
                    cboTasaDeCambio.Items.Add("No hay tasas disponibles");
                    cboTasaDeCambio.SelectedIndex = 4;
                    cboTasaDeCambio.Enabled = false;
                    txtMonto.Enabled = false;
                    btnBuscar.Enabled = false;
                    MessageBox.Show("No se encontraron tasas de cambio registradas. Verifique la base de datos.", "Alerta de Datos", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar datos iniciales: {ex.Message}", "Error de Conexión o Carga", MessageBoxButtons.OK, MessageBoxIcon.Error);
                cboTasaDeCambio.Items.Add("Error de Carga");
                cboTasaDeCambio.SelectedIndex = 0;
            }
        }

        /// <summary>
        /// Configura las columnas del DataGridView con los nombres de columna correctos y el nuevo botón.
        /// </summary>
        private void InicializarDataGridView()
        {
            dgvtasasdecambio.Columns.Clear();
            dgvtasasdecambio.ReadOnly = false;
            dgvtasasdecambio.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvtasasdecambio.MultiSelect = false;
            dgvtasasdecambio.AllowUserToAddRows = false; // Se cambia a false para evitar filas vacías innecesarias

            // Columna 1: Moneda (Nombre y Abreviación), solo lectura
            dgvtasasdecambio.Columns.Add(new DataGridViewTextBoxColumn()
            {
                Name = "Moneda",
                HeaderText = "Moneda",
                ReadOnly = true,
                FillWeight = 40
            });

            // Columna 2: Fecha BCV (Fecha en que se registró la tasa BCV), solo lectura
            dgvtasasdecambio.Columns.Add(new DataGridViewTextBoxColumn()
            {
                Name = "FechaBCV",
                HeaderText = "Fecha BCV",
                ReadOnly = true,
                FillWeight = 30
            });

            // Columna 3: Monto Operación (Monto que el usuario desea registrar), editable
            dgvtasasdecambio.Columns.Add(new DataGridViewTextBoxColumn()
            {
                Name = "Monto",
                HeaderText = "Monto Operación",
                ReadOnly = false,
                FillWeight = 30
            });

            // Columna 4: Botón de Selección de Tasa General (NUEVO REQUERIMIENTO)
            dgvtasasdecambio.Columns.Add(new DataGridViewButtonColumn()
            {
                Name = "btnSeleccionarTasa",
                HeaderText = "Tasa General",
                Text = "Usar", // Texto que se muestra en el botón de cada fila
                UseColumnTextForButtonValue = true,
                FillWeight = 20
            });

            dgvtasasdecambio.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        }

        // ==============================================================================
        // MÉTODOS DE EVENTOS (INTERACCIÓN DEL USUARIO)
        // ==============================================================================

        /// <summary>
        /// Evento que se dispara al cambiar la selección en el ComboBox.
        /// Actualiza los campos de 'Fecha' y 'Monto' con los datos BCV de la tasa seleccionada.
        /// </summary>
        private void cboTasaDeCambio_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboTasaDeCambio.SelectedItem == null || !_tasasActuales.Any())
            {
                txtFecha.Clear();
                txtMonto.Clear();
                return;
            }

            // Extraer la abreviación de la cadena seleccionada (ej: "Euro (EUR)" -> "EUR")
            string selectedText = cboTasaDeCambio.SelectedItem.ToString();
            // Esto asume que el formato siempre es 'Nombre (ABR)'
            string abreviacion = selectedText.Split('(').Last().TrimEnd(')');

            if (_tasasActuales.TryGetValue(abreviacion, out TasaCambio tasa))
            {
                // Muestra la fecha y el valor BCV asociado a la moneda
                txtFecha.Text = tasa.FechaValor;
                // Muestra el valor de la tasa BCV como sugerencia en el campo Monto
                txtMonto.Text = tasa.Valor.ToString("N4", CultureInfo.InvariantCulture);
            }
            else
            {
                txtFecha.Clear();
                txtMonto.Clear();
            }
        }

        /// <summary>
        /// Lógica para agregar la tasa seleccionada con el monto de operación a la tabla (DataGridView).
        /// Este es el evento del botón verde (btnAgregar).
        /// </summary>
        private void btnAgregar_Click(object sender, EventArgs e)
        {
            // 1. Validaciones básicas
            if (cboTasaDeCambio.SelectedItem == null || cboTasaDeCambio.Text.Contains("No hay tasas"))
            {
                MessageBox.Show("Debe seleccionar una tasa de cambio válida.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!decimal.TryParse(txtMonto.Text, NumberStyles.Any, CultureInfo.InvariantCulture, out decimal monto))
            {
                MessageBox.Show("El valor del monto debe ser un número válido.", "Error de Formato", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (monto <= 0)
            {
                MessageBox.Show("El monto de la operación debe ser mayor a cero.", "Error de Formato", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Extraer la abreviación (necesaria para la clave del diccionario y la verificación de duplicados)
            string selectedText = cboTasaDeCambio.SelectedItem.ToString();
            string abreviacion = selectedText.Split('(').Last().TrimEnd(')');

            // 2. Verificar si la moneda ya fue agregada a la tabla.
            // La tabla puede contener registros de HISTORIAL (guardados) y registros NUEVOS (aún no guardados).
            bool monedaYaAgregadaEnSesion = dgvtasasdecambio.Rows.Cast<DataGridViewRow>()
                // Ignoramos la última fila si AllowUserToAddRows fuera true, aunque la pusimos en false.
                .Where(row => !row.IsNewRow)
                .Any(row => row.Cells["Moneda"].Value != null && row.Cells["Moneda"].Value.ToString().Contains($"({abreviacion})"));

            if (monedaYaAgregadaEnSesion)
            {
                // Si la moneda está ya en la tabla (ya sea como historial o como nueva entrada), sugerimos editar.
                MessageBox.Show($"La moneda '{abreviacion}' ya está registrada en la tabla. Si desea modificar el monto, edite la fila directamente y presione 'Aceptar'.", "Moneda Duplicada en Sesión", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }


            // 3. Obtener la TasaCambio original para obtener la Fecha BCV exacta
            if (!_tasasActuales.TryGetValue(abreviacion, out TasaCambio tasaBCV))
            {
                MessageBox.Show($"Error interno: No se pudo obtener la tasa BCV original para '{abreviacion}'.", "Error Interno", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // 4. Agregar la fila al DataGridView (se añade como una nueva entrada en la tabla combinada)
            int index = dgvtasasdecambio.Rows.Add();
            dgvtasasdecambio.Rows[index].Cells["Moneda"].Value = selectedText;
            dgvtasasdecambio.Rows[index].Cells["FechaBCV"].Value = tasaBCV.FechaValor;
            // Guardamos el monto ingresado por el usuario con 2 decimales para visualización
            dgvtasasdecambio.Rows[index].Cells["Monto"].Value = monto.ToString("N2", CultureInfo.InvariantCulture);

            // Opcional: Resaltar la fila con un color diferente para indicar que es una nueva entrada (aún no guardada)
            dgvtasasdecambio.Rows[index].DefaultCellStyle.BackColor = Color.LightYellow;

            // Resaltar la fila agregada para feedback
            dgvtasasdecambio.Rows[index].Selected = true;
            dgvtasasdecambio.FirstDisplayedScrollingRowIndex = index;

            // Limpiar el campo de monto para facilitar la siguiente entrada
            txtMonto.Clear();
        }



        /// Evento click del botón de Aceptar: Itera sobre el DataGridView y guarda los registros 
        /// de las operaciones de tasa realizadas por el usuario en la base de datos.
        /// </summary>
        private void btnAceptar_Click(object sender, EventArgs e)
        {
            if (dgvtasasdecambio.Rows.Count == 0 || (dgvtasasdecambio.Rows.Count == 1 && dgvtasasdecambio.Rows[0].IsNewRow))
            {
                MessageBox.Show("No hay registros en la tabla para confirmar y guardar.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (UsuarioActual == null || UsuarioActual.IdUsuario <= 0)
            {
                MessageBox.Show("Error de Sesión: No se pudo obtener el usuario logueado. Cierre e inicie sesión de nuevo.", "Error Crítico", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            CN_RegistroUsuario cnRegistro = new CN_RegistroUsuario();
            int registrosGuardados = 0;
            int idUsuario = UsuarioActual.IdUsuario;

            foreach (DataGridViewRow row in dgvtasasdecambio.Rows)
            {
                if (row.IsNewRow) continue;

                try
                {
                    string displayMoneda = row.Cells["Moneda"].Value.ToString();
                    string abreviacion = displayMoneda.Split('(').Last().TrimEnd(')');

                    if (!_tasasActuales.ContainsKey(abreviacion)) continue;
                    TasaCambio tasaOriginal = _tasasActuales[abreviacion];

                    // CORRECCIÓN DE DECIMALES: Reemplazamos coma por punto para asegurar el parseo correcto
                    string valorTexto = row.Cells["Monto"].Value.ToString().Replace(",", ".");

                    if (decimal.TryParse(valorTexto, NumberStyles.Any, CultureInfo.InvariantCulture, out decimal montoOperacion))
                    {
                        bool resultado = cnRegistro.GuardarRegistroIndividual(UsuarioActual, tasaOriginal, montoOperacion);

                        if (resultado)
                        {
                            registrosGuardados++;
                            row.DefaultCellStyle.BackColor = Color.LightGray;

                            // SINCRONIZACIÓN: Si esta moneda es la Tasa General actual, actualizamos el valor en memoria
                            if (UsuarioActual.oTasaGeneral != null && UsuarioActual.oTasaGeneral.MonedaAbreviacion == abreviacion)
                            {
                                UsuarioActual.oTasaGeneral.Valor = montoOperacion;
                            }
                        }
                    }
                    else
                    {
                        MessageBox.Show($"Error de formato en el monto para {displayMoneda}.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error al procesar la fila: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }

            if (registrosGuardados > 0)
            {
                MessageBox.Show($"¡Proceso completado! {registrosGuardados} registros procesados.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

            CargarHistorialUsuario();
        }

        /// <summary>
        /// Maneja el evento de click en el botón "Usar" para establecer una Tasa General para el usuario.
        /// </summary>

        private void dgvtasasdecambio_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            // 1. Verificación del click en la columna de botón "Usar"
            if (e.RowIndex < 0 || e.ColumnIndex != dgvtasasdecambio.Columns["btnSeleccionarTasa"].Index) return;
            if (dgvtasasdecambio.Rows[e.RowIndex].IsNewRow) return;

            DataGridViewRow row = dgvtasasdecambio.Rows[e.RowIndex];
            string displayMoneda = row.Cells["Moneda"].Value?.ToString();

            // Limpieza de cadena para evitar errores de formato decimal
            string montoText = row.Cells["Monto"].Value?.ToString().Replace(",", ".");

            if (string.IsNullOrEmpty(displayMoneda) || string.IsNullOrEmpty(montoText)) return;

            string abreviacion = displayMoneda.Split('(').Last().TrimEnd(')');

            if (!_tasasActuales.TryGetValue(abreviacion, out TasaCambio tasaOriginal))
            {
                MessageBox.Show("No se pudo recuperar la información base.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (!decimal.TryParse(montoText, NumberStyles.Any, CultureInfo.InvariantCulture, out decimal montoOperacion))
            {
                MessageBox.Show("Monto no válido.", "Error de Formato", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // 4. Confirmación del usuario
            DialogResult result = MessageBox.Show(
                $"¿Desea establecer {displayMoneda} ({montoOperacion:N2}) como Tasa General?",
                "Confirmar",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                try
                {
                    CN_RegistroUsuario cnRegistro = new CN_RegistroUsuario();

                    // USO DE UsuarioActual: Coincide con la variable definida en Inicio.cs
                    bool guardadoExitoso = cnRegistro.EstablecerTasaGeneralUsuario(UsuarioActual.IdUsuario, tasaOriginal, montoOperacion);

                    if (guardadoExitoso)
                    {
                        // ACTUALIZACIÓN DE SESIÓN EN MEMORIA
                        UsuarioActual.oTasaGeneral = new TasaGeneralUsuario()
                        {
                            MonedaAbreviacion = abreviacion,
                            Valor = montoOperacion
                        };

                        MessageBox.Show("Tasa General establecida exitosamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);

                        // Feedback visual en la tabla
                        foreach (DataGridViewRow r in dgvtasasdecambio.Rows)
                        {
                            r.DefaultCellStyle.BackColor = Color.LightGray;
                        }
                        row.DefaultCellStyle.BackColor = Color.LightGreen;

                        // REFRESCAR EL LABEL EN LA VENTANA PRINCIPAL
                        // Buscamos el formulario de Inicio entre los abiertos
                        Inicio principal = Application.OpenForms.OfType<Inicio>().FirstOrDefault();
                        if (principal != null)
                        {
                            principal.ActualizarLabelTasa();
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }


        // ==============================================================================
        // MÉTODOS DE MANEJO DE VENTANA (Opcionales/Placeholder)
        // ==============================================================================

        private void frmSistemaCambiario_FormClosing(object sender, FormClosingEventArgs e)
        {
            // Lógica para detener el Timer si existe
        }

        // private void SetupTimer() { /* Implementación del Timer */ }
        // private void ActualizacionTimer_Tick(object sender, EventArgs e) { /* Lógica de actualización */ }
    }
}