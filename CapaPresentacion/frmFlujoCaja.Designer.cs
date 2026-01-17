namespace CapaPresentacion
{
    partial class frmFlujoCaja
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea1 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend1 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series1 = new System.Windows.Forms.DataVisualization.Charting.Series();
            this.label11 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.Estado = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cboFormaPago = new System.Windows.Forms.ComboBox();
            this.cboCategoria = new System.Windows.Forms.ComboBox();
            this.dtpFechaGasto = new System.Windows.Forms.DateTimePicker();
            this.label10 = new System.Windows.Forms.Label();
            this.txtReferencia = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.txtDescripcion = new System.Windows.Forms.TextBox();
            this.IdRol = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Clave = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Correo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.EstadoValor = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.label3 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.btnGuardarGastosOperativos = new FontAwesome.Sharp.IconButton();
            this.label8 = new System.Windows.Forms.Label();
            this.Rol = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.IdUsuario = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.btnSeleccionar = new System.Windows.Forms.DataGridViewButtonColumn();
            this.NombreCompleto = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Documento = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.label2 = new System.Windows.Forms.Label();
            this.gpbGastosOperativos = new System.Windows.Forms.GroupBox();
            this.btnLimpiarGastosOperativos = new FontAwesome.Sharp.IconButton();
            this.btnBuscarGastosOperativos = new FontAwesome.Sharp.IconButton();
            this.txtGastosOperativos = new System.Windows.Forms.TextBox();
            this.cboGastosOperativos = new System.Windows.Forms.ComboBox();
            this.label9 = new System.Windows.Forms.Label();
            this.lblTotaldgvDataGastosOperativos = new System.Windows.Forms.Label();
            this.dgvDataGastosOperativos = new System.Windows.Forms.DataGridView();
            this.IdGasto = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Categoria = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.FormaPago = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Descripción = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Fecha = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Referencia = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Monto = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.btnEliminar = new System.Windows.Forms.DataGridViewButtonColumn();
            this.label21 = new System.Windows.Forms.Label();
            this.gpbutilidad = new System.Windows.Forms.GroupBox();
            this.label22 = new System.Windows.Forms.Label();
            this.lblMargenUtilidad = new System.Windows.Forms.Label();
            this.lblUtilidadNeta = new System.Windows.Forms.Label();
            this.iconPictureBox1 = new FontAwesome.Sharp.IconPictureBox();
            this.gpbIngreso = new System.Windows.Forms.GroupBox();
            this.label24 = new System.Windows.Forms.Label();
            this.lblMargenIngresos = new System.Windows.Forms.Label();
            this.lblCantidadIngresos = new System.Windows.Forms.Label();
            this.iconPictureBox2 = new FontAwesome.Sharp.IconPictureBox();
            this.gpbegresos = new System.Windows.Forms.GroupBox();
            this.label27 = new System.Windows.Forms.Label();
            this.lblMargenEgresos = new System.Windows.Forms.Label();
            this.lblCantidadEgresos = new System.Windows.Forms.Label();
            this.iconPictureBox3 = new FontAwesome.Sharp.IconPictureBox();
            this.btnAgregarCategoria = new System.Windows.Forms.Button();
            this.btnAgregarFormaPago = new System.Windows.Forms.Button();
            this.btnBuscar = new FontAwesome.Sharp.IconButton();
            this.label13 = new System.Windows.Forms.Label();
            this.txtFin = new System.Windows.Forms.DateTimePicker();
            this.label14 = new System.Windows.Forms.Label();
            this.txtInicio = new System.Windows.Forms.DateTimePicker();
            this.VentasVSGastos = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.label12 = new System.Windows.Forms.Label();
            this.txtMonto = new System.Windows.Forms.TextBox();
            this.label15 = new System.Windows.Forms.Label();
            this.comboBox3 = new System.Windows.Forms.ComboBox();
            this.textBox3 = new System.Windows.Forms.TextBox();
            this.dgvDataDeudores = new System.Windows.Forms.DataGridView();
            this.Cliente = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.MontoCliente = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Empleado = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.FechaDeuda = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.iconButton6 = new FontAwesome.Sharp.IconButton();
            this.lblTotalDeudoresPendientes = new System.Windows.Forms.Label();
            this.iconButton5 = new FontAwesome.Sharp.IconButton();
            this.gpbDeudores = new System.Windows.Forms.GroupBox();
            this.label16 = new System.Windows.Forms.Label();
            this.gpbGastosOperativos.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvDataGastosOperativos)).BeginInit();
            this.gpbutilidad.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.iconPictureBox1)).BeginInit();
            this.gpbIngreso.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.iconPictureBox2)).BeginInit();
            this.gpbegresos.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.iconPictureBox3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.VentasVSGastos)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvDataDeudores)).BeginInit();
            this.gpbDeudores.SuspendLayout();
            this.SuspendLayout();
            // 
            // label11
            // 
            this.label11.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label11.BackColor = System.Drawing.Color.WhiteSmoke;
            this.label11.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label11.ForeColor = System.Drawing.Color.Gray;
            this.label11.Location = new System.Drawing.Point(2, 3);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(1904, 57);
            this.label11.TabIndex = 51;
            this.label11.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label1.BackColor = System.Drawing.SystemColors.Window;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(5, 58);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(1952, 781);
            this.label1.TabIndex = 65;
            this.label1.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // Estado
            // 
            this.Estado.HeaderText = "Estado";
            this.Estado.Name = "Estado";
            this.Estado.Width = 140;
            // 
            // cboFormaPago
            // 
            this.cboFormaPago.BackColor = System.Drawing.SystemColors.ActiveBorder;
            this.cboFormaPago.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboFormaPago.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cboFormaPago.FormattingEnabled = true;
            this.cboFormaPago.Location = new System.Drawing.Point(12, 228);
            this.cboFormaPago.Name = "cboFormaPago";
            this.cboFormaPago.Size = new System.Drawing.Size(288, 23);
            this.cboFormaPago.TabIndex = 91;
            // 
            // cboCategoria
            // 
            this.cboCategoria.BackColor = System.Drawing.SystemColors.ActiveBorder;
            this.cboCategoria.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboCategoria.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cboCategoria.FormattingEnabled = true;
            this.cboCategoria.Location = new System.Drawing.Point(12, 184);
            this.cboCategoria.Name = "cboCategoria";
            this.cboCategoria.Size = new System.Drawing.Size(288, 23);
            this.cboCategoria.TabIndex = 89;
            // 
            // dtpFechaGasto
            // 
            this.dtpFechaGasto.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dtpFechaGasto.Location = new System.Drawing.Point(12, 134);
            this.dtpFechaGasto.Name = "dtpFechaGasto";
            this.dtpFechaGasto.Size = new System.Drawing.Size(317, 21);
            this.dtpFechaGasto.TabIndex = 88;
            this.dtpFechaGasto.Value = new System.DateTime(2026, 1, 14, 12, 17, 46, 0);
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.BackColor = System.Drawing.Color.WhiteSmoke;
            this.label10.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label10.Location = new System.Drawing.Point(11, 70);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(160, 24);
            this.label10.TabIndex = 87;
            this.label10.Text = "Detalles del Gasto";
            // 
            // txtReferencia
            // 
            this.txtReferencia.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtReferencia.Location = new System.Drawing.Point(15, 373);
            this.txtReferencia.Name = "txtReferencia";
            this.txtReferencia.Size = new System.Drawing.Size(317, 21);
            this.txtReferencia.TabIndex = 85;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.BackColor = System.Drawing.Color.WhiteSmoke;
            this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.Location = new System.Drawing.Point(12, 354);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(67, 15);
            this.label7.TabIndex = 84;
            this.label7.Text = "Referencia";
            // 
            // txtDescripcion
            // 
            this.txtDescripcion.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtDescripcion.Location = new System.Drawing.Point(12, 281);
            this.txtDescripcion.Name = "txtDescripcion";
            this.txtDescripcion.Size = new System.Drawing.Size(317, 21);
            this.txtDescripcion.TabIndex = 83;
            // 
            // IdRol
            // 
            this.IdRol.HeaderText = "IdRol";
            this.IdRol.Name = "IdRol";
            this.IdRol.Visible = false;
            // 
            // Clave
            // 
            this.Clave.HeaderText = "Clave";
            this.Clave.Name = "Clave";
            this.Clave.Visible = false;
            // 
            // Correo
            // 
            this.Correo.HeaderText = "Correo";
            this.Correo.Name = "Correo";
            this.Correo.Width = 180;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.BackColor = System.Drawing.Color.WhiteSmoke;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(12, 258);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(72, 15);
            this.label5.TabIndex = 82;
            this.label5.Text = "Descripción";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.BackColor = System.Drawing.Color.WhiteSmoke;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(12, 166);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(60, 15);
            this.label4.TabIndex = 81;
            this.label4.Text = "Categoria";
            // 
            // EstadoValor
            // 
            this.EstadoValor.HeaderText = "EstadoValor";
            this.EstadoValor.Name = "EstadoValor";
            this.EstadoValor.Visible = false;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.BackColor = System.Drawing.Color.WhiteSmoke;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(12, 116);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(41, 15);
            this.label3.TabIndex = 80;
            this.label3.Text = "Fecha";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 36F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(461, 281);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(0, 55);
            this.label6.TabIndex = 78;
            // 
            // btnGuardarGastosOperativos
            // 
            this.btnGuardarGastosOperativos.BackColor = System.Drawing.Color.ForestGreen;
            this.btnGuardarGastosOperativos.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnGuardarGastosOperativos.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnGuardarGastosOperativos.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnGuardarGastosOperativos.ForeColor = System.Drawing.SystemColors.Control;
            this.btnGuardarGastosOperativos.IconChar = FontAwesome.Sharp.IconChar.Save;
            this.btnGuardarGastosOperativos.IconColor = System.Drawing.Color.White;
            this.btnGuardarGastosOperativos.IconFont = FontAwesome.Sharp.IconFont.Auto;
            this.btnGuardarGastosOperativos.IconSize = 24;
            this.btnGuardarGastosOperativos.Location = new System.Drawing.Point(11, 405);
            this.btnGuardarGastosOperativos.Margin = new System.Windows.Forms.Padding(2);
            this.btnGuardarGastosOperativos.Name = "btnGuardarGastosOperativos";
            this.btnGuardarGastosOperativos.Size = new System.Drawing.Size(321, 43);
            this.btnGuardarGastosOperativos.TabIndex = 86;
            this.btnGuardarGastosOperativos.Text = "Guardar";
            this.btnGuardarGastosOperativos.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnGuardarGastosOperativos.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnGuardarGastosOperativos.UseVisualStyleBackColor = false;
            this.btnGuardarGastosOperativos.Click += new System.EventHandler(this.btnGuardarGastosOperativos_Click);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.BackColor = System.Drawing.Color.WhiteSmoke;
            this.label8.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label8.Location = new System.Drawing.Point(12, 210);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(92, 15);
            this.label8.TabIndex = 90;
            this.label8.Text = "Forma de Pago";
            // 
            // Rol
            // 
            this.Rol.HeaderText = "Rol";
            this.Rol.Name = "Rol";
            this.Rol.Width = 140;
            // 
            // IdUsuario
            // 
            this.IdUsuario.HeaderText = "IdUsuario";
            this.IdUsuario.Name = "IdUsuario";
            this.IdUsuario.Visible = false;
            // 
            // btnSeleccionar
            // 
            this.btnSeleccionar.HeaderText = "";
            this.btnSeleccionar.Name = "btnSeleccionar";
            this.btnSeleccionar.Width = 40;
            // 
            // NombreCompleto
            // 
            this.NombreCompleto.HeaderText = "Nombre Completo";
            this.NombreCompleto.Name = "NombreCompleto";
            this.NombreCompleto.Width = 200;
            // 
            // Documento
            // 
            this.Documento.HeaderText = "Nro Documento";
            this.Documento.Name = "Documento";
            this.Documento.Width = 160;
            // 
            // label2
            // 
            this.label2.AllowDrop = true;
            this.label2.BackColor = System.Drawing.Color.WhiteSmoke;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(21, 24);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(294, 23);
            this.label2.TabIndex = 63;
            this.label2.Text = "Detalle del Estado de Resultados";
            // 
            // gpbGastosOperativos
            // 
            this.gpbGastosOperativos.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gpbGastosOperativos.BackColor = System.Drawing.Color.WhiteSmoke;
            this.gpbGastosOperativos.Controls.Add(this.btnLimpiarGastosOperativos);
            this.gpbGastosOperativos.Controls.Add(this.btnBuscarGastosOperativos);
            this.gpbGastosOperativos.Controls.Add(this.txtGastosOperativos);
            this.gpbGastosOperativos.Controls.Add(this.cboGastosOperativos);
            this.gpbGastosOperativos.Controls.Add(this.label9);
            this.gpbGastosOperativos.Controls.Add(this.lblTotaldgvDataGastosOperativos);
            this.gpbGastosOperativos.Controls.Add(this.dgvDataGastosOperativos);
            this.gpbGastosOperativos.Controls.Add(this.label21);
            this.gpbGastosOperativos.Controls.Add(this.label2);
            this.gpbGastosOperativos.Location = new System.Drawing.Point(340, 169);
            this.gpbGastosOperativos.Name = "gpbGastosOperativos";
            this.gpbGastosOperativos.Size = new System.Drawing.Size(1030, 669);
            this.gpbGastosOperativos.TabIndex = 64;
            this.gpbGastosOperativos.TabStop = false;
            // 
            // btnLimpiarGastosOperativos
            // 
            this.btnLimpiarGastosOperativos.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnLimpiarGastosOperativos.BackColor = System.Drawing.SystemColors.Control;
            this.btnLimpiarGastosOperativos.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnLimpiarGastosOperativos.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnLimpiarGastosOperativos.ForeColor = System.Drawing.Color.Gray;
            this.btnLimpiarGastosOperativos.IconChar = FontAwesome.Sharp.IconChar.Broom;
            this.btnLimpiarGastosOperativos.IconColor = System.Drawing.Color.Black;
            this.btnLimpiarGastosOperativos.IconFont = FontAwesome.Sharp.IconFont.Auto;
            this.btnLimpiarGastosOperativos.IconSize = 24;
            this.btnLimpiarGastosOperativos.Location = new System.Drawing.Point(976, 55);
            this.btnLimpiarGastosOperativos.Margin = new System.Windows.Forms.Padding(2);
            this.btnLimpiarGastosOperativos.Name = "btnLimpiarGastosOperativos";
            this.btnLimpiarGastosOperativos.Size = new System.Drawing.Size(48, 23);
            this.btnLimpiarGastosOperativos.TabIndex = 103;
            this.btnLimpiarGastosOperativos.UseVisualStyleBackColor = false;
            this.btnLimpiarGastosOperativos.Click += new System.EventHandler(this.btnLimpiarGastosOperativos_Click);
            // 
            // btnBuscarGastosOperativos
            // 
            this.btnBuscarGastosOperativos.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnBuscarGastosOperativos.BackColor = System.Drawing.SystemColors.Control;
            this.btnBuscarGastosOperativos.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnBuscarGastosOperativos.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnBuscarGastosOperativos.ForeColor = System.Drawing.Color.Gray;
            this.btnBuscarGastosOperativos.IconChar = FontAwesome.Sharp.IconChar.Search;
            this.btnBuscarGastosOperativos.IconColor = System.Drawing.Color.Black;
            this.btnBuscarGastosOperativos.IconFont = FontAwesome.Sharp.IconFont.Auto;
            this.btnBuscarGastosOperativos.IconSize = 24;
            this.btnBuscarGastosOperativos.Location = new System.Drawing.Point(924, 55);
            this.btnBuscarGastosOperativos.Margin = new System.Windows.Forms.Padding(2);
            this.btnBuscarGastosOperativos.Name = "btnBuscarGastosOperativos";
            this.btnBuscarGastosOperativos.Size = new System.Drawing.Size(48, 23);
            this.btnBuscarGastosOperativos.TabIndex = 102;
            this.btnBuscarGastosOperativos.UseVisualStyleBackColor = false;
            this.btnBuscarGastosOperativos.Click += new System.EventHandler(this.btnBuscarGastosOperativos_Click);
            // 
            // txtGastosOperativos
            // 
            this.txtGastosOperativos.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.txtGastosOperativos.Location = new System.Drawing.Point(740, 57);
            this.txtGastosOperativos.Name = "txtGastosOperativos";
            this.txtGastosOperativos.Size = new System.Drawing.Size(179, 20);
            this.txtGastosOperativos.TabIndex = 101;
            // 
            // cboGastosOperativos
            // 
            this.cboGastosOperativos.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.cboGastosOperativos.BackColor = System.Drawing.SystemColors.ActiveBorder;
            this.cboGastosOperativos.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboGastosOperativos.FormattingEnabled = true;
            this.cboGastosOperativos.Location = new System.Drawing.Point(525, 57);
            this.cboGastosOperativos.Name = "cboGastosOperativos";
            this.cboGastosOperativos.Size = new System.Drawing.Size(209, 21);
            this.cboGastosOperativos.TabIndex = 100;
            // 
            // label9
            // 
            this.label9.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label9.AutoSize = true;
            this.label9.BackColor = System.Drawing.Color.WhiteSmoke;
            this.label9.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label9.Location = new System.Drawing.Point(447, 65);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(72, 13);
            this.label9.TabIndex = 99;
            this.label9.Text = "Buscar por:";
            // 
            // lblTotaldgvDataGastosOperativos
            // 
            this.lblTotaldgvDataGastosOperativos.AllowDrop = true;
            this.lblTotaldgvDataGastosOperativos.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.lblTotaldgvDataGastosOperativos.BackColor = System.Drawing.Color.WhiteSmoke;
            this.lblTotaldgvDataGastosOperativos.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTotaldgvDataGastosOperativos.Location = new System.Drawing.Point(11, 633);
            this.lblTotaldgvDataGastosOperativos.Name = "lblTotaldgvDataGastosOperativos";
            this.lblTotaldgvDataGastosOperativos.Size = new System.Drawing.Size(997, 23);
            this.lblTotaldgvDataGastosOperativos.TabIndex = 75;
            this.lblTotaldgvDataGastosOperativos.Text = "Totales -";
            this.lblTotaldgvDataGastosOperativos.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // dgvDataGastosOperativos
            // 
            this.dgvDataGastosOperativos.AllowUserToAddRows = false;
            this.dgvDataGastosOperativos.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvDataGastosOperativos.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvDataGastosOperativos.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.IdGasto,
            this.Categoria,
            this.FormaPago,
            this.Descripción,
            this.Fecha,
            this.Referencia,
            this.Monto,
            this.btnEliminar});
            this.dgvDataGastosOperativos.Location = new System.Drawing.Point(22, 84);
            this.dgvDataGastosOperativos.Name = "dgvDataGastosOperativos";
            this.dgvDataGastosOperativos.ReadOnly = true;
            this.dgvDataGastosOperativos.Size = new System.Drawing.Size(1002, 541);
            this.dgvDataGastosOperativos.TabIndex = 74;
            this.dgvDataGastosOperativos.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvDataGastosOperativos_CellContentClick);
            this.dgvDataGastosOperativos.CellPainting += new System.Windows.Forms.DataGridViewCellPaintingEventHandler(this.dgvDataGastosOperativos_CellPainting);
            // 
            // IdGasto
            // 
            this.IdGasto.HeaderText = "IdGasto";
            this.IdGasto.Name = "IdGasto";
            this.IdGasto.ReadOnly = true;
            this.IdGasto.Visible = false;
            // 
            // Categoria
            // 
            this.Categoria.HeaderText = "Categoria";
            this.Categoria.Name = "Categoria";
            this.Categoria.ReadOnly = true;
            this.Categoria.Width = 180;
            // 
            // FormaPago
            // 
            this.FormaPago.HeaderText = "Forma de Pago";
            this.FormaPago.Name = "FormaPago";
            this.FormaPago.ReadOnly = true;
            this.FormaPago.Width = 180;
            // 
            // Descripción
            // 
            this.Descripción.HeaderText = "Descripción";
            this.Descripción.Name = "Descripción";
            this.Descripción.ReadOnly = true;
            this.Descripción.Width = 220;
            // 
            // Fecha
            // 
            this.Fecha.HeaderText = "Fecha";
            this.Fecha.Name = "Fecha";
            this.Fecha.ReadOnly = true;
            this.Fecha.Width = 70;
            // 
            // Referencia
            // 
            this.Referencia.HeaderText = "Referencia";
            this.Referencia.Name = "Referencia";
            this.Referencia.ReadOnly = true;
            this.Referencia.Width = 180;
            // 
            // Monto
            // 
            this.Monto.HeaderText = "Monto";
            this.Monto.Name = "Monto";
            this.Monto.ReadOnly = true;
            // 
            // btnEliminar
            // 
            this.btnEliminar.HeaderText = "";
            this.btnEliminar.Name = "btnEliminar";
            this.btnEliminar.ReadOnly = true;
            this.btnEliminar.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.btnEliminar.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.btnEliminar.Width = 40;
            // 
            // label21
            // 
            this.label21.AllowDrop = true;
            this.label21.BackColor = System.Drawing.Color.WhiteSmoke;
            this.label21.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label21.Location = new System.Drawing.Point(22, 58);
            this.label21.Name = "label21";
            this.label21.Size = new System.Drawing.Size(1003, 23);
            this.label21.TabIndex = 73;
            this.label21.Text = "3. GASTOS OPERATIVOS";
            // 
            // gpbutilidad
            // 
            this.gpbutilidad.BackColor = System.Drawing.Color.RoyalBlue;
            this.gpbutilidad.Controls.Add(this.label22);
            this.gpbutilidad.Controls.Add(this.lblMargenUtilidad);
            this.gpbutilidad.Controls.Add(this.lblUtilidadNeta);
            this.gpbutilidad.Controls.Add(this.iconPictureBox1);
            this.gpbutilidad.Location = new System.Drawing.Point(1079, 60);
            this.gpbutilidad.Name = "gpbutilidad";
            this.gpbutilidad.Size = new System.Drawing.Size(369, 106);
            this.gpbutilidad.TabIndex = 92;
            this.gpbutilidad.TabStop = false;
            // 
            // label22
            // 
            this.label22.AutoSize = true;
            this.label22.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label22.ForeColor = System.Drawing.Color.Cornsilk;
            this.label22.Location = new System.Drawing.Point(93, 10);
            this.label22.Name = "label22";
            this.label22.Size = new System.Drawing.Size(198, 16);
            this.label22.TabIndex = 3;
            this.label22.Text = "3. Utilidad Neta (Calculada)";
            // 
            // lblMargenUtilidad
            // 
            this.lblMargenUtilidad.AutoSize = true;
            this.lblMargenUtilidad.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblMargenUtilidad.ForeColor = System.Drawing.Color.Cornsilk;
            this.lblMargenUtilidad.Location = new System.Drawing.Point(5, 82);
            this.lblMargenUtilidad.Name = "lblMargenUtilidad";
            this.lblMargenUtilidad.Size = new System.Drawing.Size(120, 13);
            this.lblMargenUtilidad.TabIndex = 2;
            this.lblMargenUtilidad.Text = "% Margen Ganancia";
            // 
            // lblUtilidadNeta
            // 
            this.lblUtilidadNeta.AutoSize = true;
            this.lblUtilidadNeta.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblUtilidadNeta.ForeColor = System.Drawing.Color.Cornsilk;
            this.lblUtilidadNeta.Location = new System.Drawing.Point(18, 43);
            this.lblUtilidadNeta.Name = "lblUtilidadNeta";
            this.lblUtilidadNeta.Size = new System.Drawing.Size(81, 20);
            this.lblUtilidadNeta.TabIndex = 1;
            this.lblUtilidadNeta.Text = "Cantidad";
            // 
            // iconPictureBox1
            // 
            this.iconPictureBox1.BackColor = System.Drawing.Color.RoyalBlue;
            this.iconPictureBox1.ForeColor = System.Drawing.Color.SeaShell;
            this.iconPictureBox1.IconChar = FontAwesome.Sharp.IconChar.Archive;
            this.iconPictureBox1.IconColor = System.Drawing.Color.SeaShell;
            this.iconPictureBox1.IconFont = FontAwesome.Sharp.IconFont.Auto;
            this.iconPictureBox1.IconSize = 70;
            this.iconPictureBox1.Location = new System.Drawing.Point(293, 25);
            this.iconPictureBox1.Name = "iconPictureBox1";
            this.iconPictureBox1.Size = new System.Drawing.Size(71, 70);
            this.iconPictureBox1.TabIndex = 0;
            this.iconPictureBox1.TabStop = false;
            // 
            // gpbIngreso
            // 
            this.gpbIngreso.BackColor = System.Drawing.Color.ForestGreen;
            this.gpbIngreso.Controls.Add(this.label24);
            this.gpbIngreso.Controls.Add(this.lblMargenIngresos);
            this.gpbIngreso.Controls.Add(this.lblCantidadIngresos);
            this.gpbIngreso.Controls.Add(this.iconPictureBox2);
            this.gpbIngreso.Location = new System.Drawing.Point(339, 60);
            this.gpbIngreso.Name = "gpbIngreso";
            this.gpbIngreso.Size = new System.Drawing.Size(369, 106);
            this.gpbIngreso.TabIndex = 93;
            this.gpbIngreso.TabStop = false;
            // 
            // label24
            // 
            this.label24.AutoSize = true;
            this.label24.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label24.ForeColor = System.Drawing.Color.Cornsilk;
            this.label24.Location = new System.Drawing.Point(95, 8);
            this.label24.Name = "label24";
            this.label24.Size = new System.Drawing.Size(176, 16);
            this.label24.TabIndex = 3;
            this.label24.Text = "1. INGRESOS (VENTAS)";
            // 
            // lblMargenIngresos
            // 
            this.lblMargenIngresos.AutoSize = true;
            this.lblMargenIngresos.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblMargenIngresos.ForeColor = System.Drawing.Color.Cornsilk;
            this.lblMargenIngresos.Location = new System.Drawing.Point(5, 82);
            this.lblMargenIngresos.Name = "lblMargenIngresos";
            this.lblMargenIngresos.Size = new System.Drawing.Size(120, 13);
            this.lblMargenIngresos.TabIndex = 2;
            this.lblMargenIngresos.Text = "% Margen Ganancia";
            // 
            // lblCantidadIngresos
            // 
            this.lblCantidadIngresos.AutoSize = true;
            this.lblCantidadIngresos.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCantidadIngresos.ForeColor = System.Drawing.Color.Cornsilk;
            this.lblCantidadIngresos.Location = new System.Drawing.Point(18, 43);
            this.lblCantidadIngresos.Name = "lblCantidadIngresos";
            this.lblCantidadIngresos.Size = new System.Drawing.Size(81, 20);
            this.lblCantidadIngresos.TabIndex = 1;
            this.lblCantidadIngresos.Text = "Cantidad";
            // 
            // iconPictureBox2
            // 
            this.iconPictureBox2.BackColor = System.Drawing.Color.ForestGreen;
            this.iconPictureBox2.ForeColor = System.Drawing.Color.SeaShell;
            this.iconPictureBox2.IconChar = FontAwesome.Sharp.IconChar.Archive;
            this.iconPictureBox2.IconColor = System.Drawing.Color.SeaShell;
            this.iconPictureBox2.IconFont = FontAwesome.Sharp.IconFont.Auto;
            this.iconPictureBox2.IconSize = 70;
            this.iconPictureBox2.Location = new System.Drawing.Point(292, 25);
            this.iconPictureBox2.Name = "iconPictureBox2";
            this.iconPictureBox2.Size = new System.Drawing.Size(71, 70);
            this.iconPictureBox2.TabIndex = 0;
            this.iconPictureBox2.TabStop = false;
            // 
            // gpbegresos
            // 
            this.gpbegresos.BackColor = System.Drawing.Color.Firebrick;
            this.gpbegresos.Controls.Add(this.label27);
            this.gpbegresos.Controls.Add(this.lblMargenEgresos);
            this.gpbegresos.Controls.Add(this.lblCantidadEgresos);
            this.gpbegresos.Controls.Add(this.iconPictureBox3);
            this.gpbegresos.Location = new System.Drawing.Point(709, 60);
            this.gpbegresos.Name = "gpbegresos";
            this.gpbegresos.Size = new System.Drawing.Size(369, 106);
            this.gpbegresos.TabIndex = 94;
            this.gpbegresos.TabStop = false;
            // 
            // label27
            // 
            this.label27.AutoSize = true;
            this.label27.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label27.ForeColor = System.Drawing.Color.Cornsilk;
            this.label27.Location = new System.Drawing.Point(94, 8);
            this.label27.Name = "label27";
            this.label27.Size = new System.Drawing.Size(198, 16);
            this.label27.TabIndex = 3;
            this.label27.Text = "2. EGRESOS (MERCANCIA)";
            // 
            // lblMargenEgresos
            // 
            this.lblMargenEgresos.AutoSize = true;
            this.lblMargenEgresos.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblMargenEgresos.ForeColor = System.Drawing.Color.Cornsilk;
            this.lblMargenEgresos.Location = new System.Drawing.Point(5, 82);
            this.lblMargenEgresos.Name = "lblMargenEgresos";
            this.lblMargenEgresos.Size = new System.Drawing.Size(120, 13);
            this.lblMargenEgresos.TabIndex = 2;
            this.lblMargenEgresos.Text = "% Margen Ganancia";
            // 
            // lblCantidadEgresos
            // 
            this.lblCantidadEgresos.AutoSize = true;
            this.lblCantidadEgresos.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCantidadEgresos.ForeColor = System.Drawing.Color.Cornsilk;
            this.lblCantidadEgresos.Location = new System.Drawing.Point(18, 43);
            this.lblCantidadEgresos.Name = "lblCantidadEgresos";
            this.lblCantidadEgresos.Size = new System.Drawing.Size(81, 20);
            this.lblCantidadEgresos.TabIndex = 1;
            this.lblCantidadEgresos.Text = "Cantidad";
            // 
            // iconPictureBox3
            // 
            this.iconPictureBox3.BackColor = System.Drawing.Color.Firebrick;
            this.iconPictureBox3.ForeColor = System.Drawing.Color.SeaShell;
            this.iconPictureBox3.IconChar = FontAwesome.Sharp.IconChar.Archive;
            this.iconPictureBox3.IconColor = System.Drawing.Color.SeaShell;
            this.iconPictureBox3.IconFont = FontAwesome.Sharp.IconFont.Auto;
            this.iconPictureBox3.IconSize = 70;
            this.iconPictureBox3.Location = new System.Drawing.Point(293, 25);
            this.iconPictureBox3.Name = "iconPictureBox3";
            this.iconPictureBox3.Size = new System.Drawing.Size(71, 70);
            this.iconPictureBox3.TabIndex = 0;
            this.iconPictureBox3.TabStop = false;
            // 
            // btnAgregarCategoria
            // 
            this.btnAgregarCategoria.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnAgregarCategoria.Image = global::CapaPresentacion.Properties.Resources.icons8_lleno_más_2_matemáticas_25;
            this.btnAgregarCategoria.Location = new System.Drawing.Point(302, 183);
            this.btnAgregarCategoria.Name = "btnAgregarCategoria";
            this.btnAgregarCategoria.Size = new System.Drawing.Size(27, 23);
            this.btnAgregarCategoria.TabIndex = 95;
            this.btnAgregarCategoria.UseVisualStyleBackColor = true;
            this.btnAgregarCategoria.Click += new System.EventHandler(this.btnAgregarCategoria_Click);
            // 
            // btnAgregarFormaPago
            // 
            this.btnAgregarFormaPago.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnAgregarFormaPago.Image = global::CapaPresentacion.Properties.Resources.icons8_lleno_más_2_matemáticas_25;
            this.btnAgregarFormaPago.Location = new System.Drawing.Point(302, 228);
            this.btnAgregarFormaPago.Name = "btnAgregarFormaPago";
            this.btnAgregarFormaPago.Size = new System.Drawing.Size(27, 23);
            this.btnAgregarFormaPago.TabIndex = 98;
            this.btnAgregarFormaPago.UseVisualStyleBackColor = true;
            this.btnAgregarFormaPago.Click += new System.EventHandler(this.btnAgregarFormaPago_Click);
            // 
            // btnBuscar
            // 
            this.btnBuscar.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnBuscar.BackColor = System.Drawing.SystemColors.Control;
            this.btnBuscar.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnBuscar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnBuscar.ForeColor = System.Drawing.Color.DarkGray;
            this.btnBuscar.IconChar = FontAwesome.Sharp.IconChar.Search;
            this.btnBuscar.IconColor = System.Drawing.Color.Black;
            this.btnBuscar.IconFont = FontAwesome.Sharp.IconFont.Auto;
            this.btnBuscar.IconSize = 24;
            this.btnBuscar.Location = new System.Drawing.Point(1845, 20);
            this.btnBuscar.Margin = new System.Windows.Forms.Padding(2);
            this.btnBuscar.Name = "btnBuscar";
            this.btnBuscar.Size = new System.Drawing.Size(48, 23);
            this.btnBuscar.TabIndex = 105;
            this.btnBuscar.UseVisualStyleBackColor = false;
            this.btnBuscar.Click += new System.EventHandler(this.btnBuscar_Click);
            // 
            // label13
            // 
            this.label13.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label13.AutoSize = true;
            this.label13.BackColor = System.Drawing.Color.WhiteSmoke;
            this.label13.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label13.Location = new System.Drawing.Point(1670, 24);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(67, 15);
            this.label13.TabIndex = 102;
            this.label13.Text = "Fechas Fin";
            // 
            // txtFin
            // 
            this.txtFin.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.txtFin.CustomFormat = "dd/MM/yyyy";
            this.txtFin.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.txtFin.Location = new System.Drawing.Point(1744, 21);
            this.txtFin.Name = "txtFin";
            this.txtFin.Size = new System.Drawing.Size(95, 20);
            this.txtFin.TabIndex = 101;
            // 
            // label14
            // 
            this.label14.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label14.AutoSize = true;
            this.label14.BackColor = System.Drawing.Color.WhiteSmoke;
            this.label14.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label14.Location = new System.Drawing.Point(1493, 25);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(72, 15);
            this.label14.TabIndex = 100;
            this.label14.Text = "Fechs Inicio";
            // 
            // txtInicio
            // 
            this.txtInicio.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.txtInicio.CustomFormat = "dd/MM/yyyy";
            this.txtInicio.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.txtInicio.Location = new System.Drawing.Point(1569, 22);
            this.txtInicio.Name = "txtInicio";
            this.txtInicio.Size = new System.Drawing.Size(95, 20);
            this.txtInicio.TabIndex = 99;
            // 
            // VentasVSGastos
            // 
            this.VentasVSGastos.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.VentasVSGastos.BorderlineColor = System.Drawing.Color.WhiteSmoke;
            chartArea1.Name = "ChartArea1";
            this.VentasVSGastos.ChartAreas.Add(chartArea1);
            legend1.Name = "Legend1";
            this.VentasVSGastos.Legends.Add(legend1);
            this.VentasVSGastos.Location = new System.Drawing.Point(1451, 64);
            this.VentasVSGastos.Name = "VentasVSGastos";
            series1.ChartArea = "ChartArea1";
            series1.Legend = "Legend1";
            series1.Name = "Series1";
            this.VentasVSGastos.Series.Add(series1);
            this.VentasVSGastos.Size = new System.Drawing.Size(452, 99);
            this.VentasVSGastos.TabIndex = 104;
            this.VentasVSGastos.Text = "chart1";
            // 
            // label12
            // 
            this.label12.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label12.BackColor = System.Drawing.Color.DimGray;
            this.label12.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label12.ForeColor = System.Drawing.Color.Gray;
            this.label12.Location = new System.Drawing.Point(1444, 58);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(462, 108);
            this.label12.TabIndex = 106;
            this.label12.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // txtMonto
            // 
            this.txtMonto.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtMonto.Location = new System.Drawing.Point(15, 325);
            this.txtMonto.Name = "txtMonto";
            this.txtMonto.Size = new System.Drawing.Size(317, 21);
            this.txtMonto.TabIndex = 108;
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.BackColor = System.Drawing.Color.WhiteSmoke;
            this.label15.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label15.Location = new System.Drawing.Point(12, 307);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(42, 15);
            this.label15.TabIndex = 107;
            this.label15.Text = "Monto";
            // 
            // comboBox3
            // 
            this.comboBox3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.comboBox3.BackColor = System.Drawing.SystemColors.ActiveBorder;
            this.comboBox3.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox3.FormattingEnabled = true;
            this.comboBox3.Location = new System.Drawing.Point(9, 59);
            this.comboBox3.Name = "comboBox3";
            this.comboBox3.Size = new System.Drawing.Size(215, 21);
            this.comboBox3.TabIndex = 105;
            // 
            // textBox3
            // 
            this.textBox3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox3.Location = new System.Drawing.Point(230, 59);
            this.textBox3.Name = "textBox3";
            this.textBox3.Size = new System.Drawing.Size(179, 20);
            this.textBox3.TabIndex = 106;
            // 
            // dgvDataDeudores
            // 
            this.dgvDataDeudores.AllowUserToAddRows = false;
            this.dgvDataDeudores.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvDataDeudores.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvDataDeudores.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Cliente,
            this.MontoCliente,
            this.Empleado,
            this.FechaDeuda});
            this.dgvDataDeudores.Location = new System.Drawing.Point(10, 85);
            this.dgvDataDeudores.Name = "dgvDataDeudores";
            this.dgvDataDeudores.Size = new System.Drawing.Size(505, 541);
            this.dgvDataDeudores.TabIndex = 0;
            // 
            // Cliente
            // 
            this.Cliente.HeaderText = "Cliente";
            this.Cliente.Name = "Cliente";
            this.Cliente.ReadOnly = true;
            this.Cliente.Width = 180;
            // 
            // MontoCliente
            // 
            this.MontoCliente.HeaderText = "Monto";
            this.MontoCliente.Name = "MontoCliente";
            this.MontoCliente.ReadOnly = true;
            // 
            // Empleado
            // 
            this.Empleado.HeaderText = "Empleado";
            this.Empleado.Name = "Empleado";
            this.Empleado.Width = 180;
            // 
            // FechaDeuda
            // 
            this.FechaDeuda.HeaderText = "Fecha de Deuda";
            this.FechaDeuda.Name = "FechaDeuda";
            this.FechaDeuda.ReadOnly = true;
            this.FechaDeuda.Width = 150;
            // 
            // iconButton6
            // 
            this.iconButton6.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.iconButton6.BackColor = System.Drawing.SystemColors.Control;
            this.iconButton6.Cursor = System.Windows.Forms.Cursors.Hand;
            this.iconButton6.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.iconButton6.ForeColor = System.Drawing.Color.Gray;
            this.iconButton6.IconChar = FontAwesome.Sharp.IconChar.Search;
            this.iconButton6.IconColor = System.Drawing.Color.Black;
            this.iconButton6.IconFont = FontAwesome.Sharp.IconFont.Auto;
            this.iconButton6.IconSize = 24;
            this.iconButton6.Location = new System.Drawing.Point(414, 57);
            this.iconButton6.Margin = new System.Windows.Forms.Padding(2);
            this.iconButton6.Name = "iconButton6";
            this.iconButton6.Size = new System.Drawing.Size(48, 23);
            this.iconButton6.TabIndex = 107;
            this.iconButton6.UseVisualStyleBackColor = false;
            // 
            // lblTotalDeudoresPendientes
            // 
            this.lblTotalDeudoresPendientes.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.lblTotalDeudoresPendientes.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTotalDeudoresPendientes.Location = new System.Drawing.Point(20, 634);
            this.lblTotalDeudoresPendientes.Name = "lblTotalDeudoresPendientes";
            this.lblTotalDeudoresPendientes.Size = new System.Drawing.Size(495, 19);
            this.lblTotalDeudoresPendientes.TabIndex = 1;
            this.lblTotalDeudoresPendientes.Text = "Totales ";
            this.lblTotalDeudoresPendientes.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // iconButton5
            // 
            this.iconButton5.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.iconButton5.BackColor = System.Drawing.SystemColors.Control;
            this.iconButton5.Cursor = System.Windows.Forms.Cursors.Hand;
            this.iconButton5.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.iconButton5.ForeColor = System.Drawing.Color.Gray;
            this.iconButton5.IconChar = FontAwesome.Sharp.IconChar.Broom;
            this.iconButton5.IconColor = System.Drawing.Color.Black;
            this.iconButton5.IconFont = FontAwesome.Sharp.IconFont.Auto;
            this.iconButton5.IconSize = 24;
            this.iconButton5.Location = new System.Drawing.Point(466, 57);
            this.iconButton5.Margin = new System.Windows.Forms.Padding(2);
            this.iconButton5.Name = "iconButton5";
            this.iconButton5.Size = new System.Drawing.Size(48, 23);
            this.iconButton5.TabIndex = 108;
            this.iconButton5.UseVisualStyleBackColor = false;
            // 
            // gpbDeudores
            // 
            this.gpbDeudores.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gpbDeudores.BackColor = System.Drawing.Color.WhiteSmoke;
            this.gpbDeudores.Controls.Add(this.label16);
            this.gpbDeudores.Controls.Add(this.iconButton5);
            this.gpbDeudores.Controls.Add(this.lblTotalDeudoresPendientes);
            this.gpbDeudores.Controls.Add(this.iconButton6);
            this.gpbDeudores.Controls.Add(this.dgvDataDeudores);
            this.gpbDeudores.Controls.Add(this.textBox3);
            this.gpbDeudores.Controls.Add(this.comboBox3);
            this.gpbDeudores.Location = new System.Drawing.Point(1372, 169);
            this.gpbDeudores.Name = "gpbDeudores";
            this.gpbDeudores.Size = new System.Drawing.Size(527, 670);
            this.gpbDeudores.TabIndex = 109;
            this.gpbDeudores.TabStop = false;
            // 
            // label16
            // 
            this.label16.AllowDrop = true;
            this.label16.BackColor = System.Drawing.Color.WhiteSmoke;
            this.label16.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label16.Location = new System.Drawing.Point(7, 21);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(248, 23);
            this.label16.TabIndex = 104;
            this.label16.Text = "Lista de Deudores";
            // 
            // frmFlujoCaja
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1906, 843);
            this.Controls.Add(this.gpbDeudores);
            this.Controls.Add(this.txtMonto);
            this.Controls.Add(this.label15);
            this.Controls.Add(this.VentasVSGastos);
            this.Controls.Add(this.btnBuscar);
            this.Controls.Add(this.label13);
            this.Controls.Add(this.txtFin);
            this.Controls.Add(this.label14);
            this.Controls.Add(this.txtInicio);
            this.Controls.Add(this.btnAgregarFormaPago);
            this.Controls.Add(this.btnAgregarCategoria);
            this.Controls.Add(this.gpbegresos);
            this.Controls.Add(this.gpbIngreso);
            this.Controls.Add(this.gpbutilidad);
            this.Controls.Add(this.cboFormaPago);
            this.Controls.Add(this.cboCategoria);
            this.Controls.Add(this.dtpFechaGasto);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.txtReferencia);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.txtDescripcion);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.btnGuardarGastosOperativos);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.gpbGastosOperativos);
            this.Controls.Add(this.label11);
            this.Controls.Add(this.label12);
            this.Controls.Add(this.label1);
            this.Name = "frmFlujoCaja";
            this.Text = "frmFlujoCaja";
            this.Load += new System.EventHandler(this.frmFlujoCaja_Load);
            this.gpbGastosOperativos.ResumeLayout(false);
            this.gpbGastosOperativos.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvDataGastosOperativos)).EndInit();
            this.gpbutilidad.ResumeLayout(false);
            this.gpbutilidad.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.iconPictureBox1)).EndInit();
            this.gpbIngreso.ResumeLayout(false);
            this.gpbIngreso.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.iconPictureBox2)).EndInit();
            this.gpbegresos.ResumeLayout(false);
            this.gpbegresos.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.iconPictureBox3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.VentasVSGastos)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvDataDeudores)).EndInit();
            this.gpbDeudores.ResumeLayout(false);
            this.gpbDeudores.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.DataGridViewTextBoxColumn Estado;
        private System.Windows.Forms.ComboBox cboFormaPago;
        private System.Windows.Forms.ComboBox cboCategoria;
        private System.Windows.Forms.DateTimePicker dtpFechaGasto;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.TextBox txtReferencia;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox txtDescripcion;
        private System.Windows.Forms.DataGridViewTextBoxColumn IdRol;
        private System.Windows.Forms.DataGridViewTextBoxColumn Clave;
        private System.Windows.Forms.DataGridViewTextBoxColumn Correo;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.DataGridViewTextBoxColumn EstadoValor;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label6;
        private FontAwesome.Sharp.IconButton btnGuardarGastosOperativos;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.DataGridViewTextBoxColumn Rol;
        private System.Windows.Forms.DataGridViewTextBoxColumn IdUsuario;
        private System.Windows.Forms.DataGridViewButtonColumn btnSeleccionar;
        private System.Windows.Forms.DataGridViewTextBoxColumn NombreCompleto;
        private System.Windows.Forms.DataGridViewTextBoxColumn Documento;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.GroupBox gpbGastosOperativos;
        private System.Windows.Forms.Label label21;
        private System.Windows.Forms.DataGridView dgvDataGastosOperativos;
        private System.Windows.Forms.GroupBox gpbutilidad;
        private System.Windows.Forms.Label label22;
        private System.Windows.Forms.Label lblMargenUtilidad;
        private System.Windows.Forms.Label lblUtilidadNeta;
        private FontAwesome.Sharp.IconPictureBox iconPictureBox1;
        private System.Windows.Forms.GroupBox gpbIngreso;
        private System.Windows.Forms.Label label24;
        private System.Windows.Forms.Label lblMargenIngresos;
        private System.Windows.Forms.Label lblCantidadIngresos;
        private FontAwesome.Sharp.IconPictureBox iconPictureBox2;
        private System.Windows.Forms.GroupBox gpbegresos;
        private System.Windows.Forms.Label label27;
        private System.Windows.Forms.Label lblMargenEgresos;
        private System.Windows.Forms.Label lblCantidadEgresos;
        private FontAwesome.Sharp.IconPictureBox iconPictureBox3;
        private System.Windows.Forms.Label lblTotaldgvDataGastosOperativos;
        private System.Windows.Forms.Button btnAgregarCategoria;
        private System.Windows.Forms.Button btnAgregarFormaPago;
        private FontAwesome.Sharp.IconButton btnLimpiarGastosOperativos;
        private FontAwesome.Sharp.IconButton btnBuscarGastosOperativos;
        private System.Windows.Forms.TextBox txtGastosOperativos;
        private System.Windows.Forms.ComboBox cboGastosOperativos;
        private System.Windows.Forms.Label label9;
        private FontAwesome.Sharp.IconButton btnBuscar;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.DateTimePicker txtFin;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.DateTimePicker txtInicio;
        private System.Windows.Forms.DataVisualization.Charting.Chart VentasVSGastos;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.TextBox txtMonto;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.ComboBox comboBox3;
        private System.Windows.Forms.TextBox textBox3;
        private System.Windows.Forms.DataGridView dgvDataDeudores;
        private FontAwesome.Sharp.IconButton iconButton6;
        private System.Windows.Forms.Label lblTotalDeudoresPendientes;
        private FontAwesome.Sharp.IconButton iconButton5;
        private System.Windows.Forms.GroupBox gpbDeudores;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.DataGridViewTextBoxColumn Cliente;
        private System.Windows.Forms.DataGridViewTextBoxColumn MontoCliente;
        private System.Windows.Forms.DataGridViewTextBoxColumn Empleado;
        private System.Windows.Forms.DataGridViewTextBoxColumn FechaDeuda;
        private System.Windows.Forms.DataGridViewTextBoxColumn IdGasto;
        private System.Windows.Forms.DataGridViewTextBoxColumn Categoria;
        private System.Windows.Forms.DataGridViewTextBoxColumn FormaPago;
        private System.Windows.Forms.DataGridViewTextBoxColumn Descripción;
        private System.Windows.Forms.DataGridViewTextBoxColumn Fecha;
        private System.Windows.Forms.DataGridViewTextBoxColumn Referencia;
        private System.Windows.Forms.DataGridViewTextBoxColumn Monto;
        private System.Windows.Forms.DataGridViewButtonColumn btnEliminar;
    }
}