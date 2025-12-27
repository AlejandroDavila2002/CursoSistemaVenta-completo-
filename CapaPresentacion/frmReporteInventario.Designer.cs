namespace CapaPresentacion
{
    partial class frmReporteInventario
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle7 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle8 = new System.Windows.Forms.DataGridViewCellStyle();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.totalproductos = new System.Windows.Forms.Label();
            this.iconPictureBox1 = new FontAwesome.Sharp.IconPictureBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.valorInventario_Costo = new System.Windows.Forms.Label();
            this.iconPictureBox2 = new FontAwesome.Sharp.IconPictureBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.label8 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.valorInventario_Venta = new System.Windows.Forms.Label();
            this.iconPictureBox3 = new FontAwesome.Sharp.IconPictureBox();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.label12 = new System.Windows.Forms.Label();
            this.ProductosBajo = new System.Windows.Forms.Label();
            this.iconPictureBox4 = new FontAwesome.Sharp.IconPictureBox();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.label11 = new System.Windows.Forms.Label();
            this.ProductosAgotados = new System.Windows.Forms.Label();
            this.iconPictureBox5 = new FontAwesome.Sharp.IconPictureBox();
            this.btnExportarPDF = new FontAwesome.Sharp.IconButton();
            this.btnExportarExcel = new FontAwesome.Sharp.IconButton();
            this.dgvDataProducto = new System.Windows.Forms.DataGridView();
            this.IMG = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.CODIGO = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.PRODUCTO = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.STOCK = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dgvDataCosto = new System.Windows.Forms.DataGridView();
            this.COSTOUNT = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.TOTALCOSTO = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dgvDataAccion = new System.Windows.Forms.DataGridView();
            this.ESTADO = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ACCION = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dvgDataVenta = new System.Windows.Forms.DataGridView();
            this.PRECIOVENTA = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.TOTALVENTA = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.label14 = new System.Windows.Forms.Label();
            this.txtBusqueda = new System.Windows.Forms.TextBox();
            this.cboBusquedaCategorias = new System.Windows.Forms.ComboBox();
            this.cboBusquedaPorStock = new System.Windows.Forms.ComboBox();
            this.btnLimpiarCombo = new FontAwesome.Sharp.IconButton();
            this.btnBuscar = new FontAwesome.Sharp.IconButton();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.iconPictureBox1)).BeginInit();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.iconPictureBox2)).BeginInit();
            this.groupBox3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.iconPictureBox3)).BeginInit();
            this.groupBox4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.iconPictureBox4)).BeginInit();
            this.groupBox5.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.iconPictureBox5)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvDataProducto)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvDataCosto)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvDataAccion)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dvgDataVenta)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label1.BackColor = System.Drawing.Color.White;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(4, 5);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(1628, 141);
            this.label1.TabIndex = 0;
            this.label1.Text = "Reporte de Inventario Valorizado";
            // 
            // groupBox1
            // 
            this.groupBox1.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.totalproductos);
            this.groupBox1.Controls.Add(this.iconPictureBox1);
            this.groupBox1.Location = new System.Drawing.Point(12, 35);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(200, 100);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(162, 16);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(32, 13);
            this.label4.TabIndex = 3;
            this.label4.Text = "Items";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(5, 82);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(82, 13);
            this.label3.TabIndex = 2;
            this.label3.Text = "Total Productos";
            // 
            // totalproductos
            // 
            this.totalproductos.AutoSize = true;
            this.totalproductos.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.totalproductos.Location = new System.Drawing.Point(16, 58);
            this.totalproductos.Name = "totalproductos";
            this.totalproductos.Size = new System.Drawing.Size(56, 15);
            this.totalproductos.TabIndex = 1;
            this.totalproductos.Text = "Cantidad";
            // 
            // iconPictureBox1
            // 
            this.iconPictureBox1.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.iconPictureBox1.ForeColor = System.Drawing.Color.RoyalBlue;
            this.iconPictureBox1.IconChar = FontAwesome.Sharp.IconChar.Archive;
            this.iconPictureBox1.IconColor = System.Drawing.Color.RoyalBlue;
            this.iconPictureBox1.IconFont = FontAwesome.Sharp.IconFont.Auto;
            this.iconPictureBox1.Location = new System.Drawing.Point(11, 19);
            this.iconPictureBox1.Name = "iconPictureBox1";
            this.iconPictureBox1.Size = new System.Drawing.Size(32, 32);
            this.iconPictureBox1.TabIndex = 0;
            this.iconPictureBox1.TabStop = false;
            // 
            // groupBox2
            // 
            this.groupBox2.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.groupBox2.Controls.Add(this.label5);
            this.groupBox2.Controls.Add(this.label6);
            this.groupBox2.Controls.Add(this.valorInventario_Costo);
            this.groupBox2.Controls.Add(this.iconPictureBox2);
            this.groupBox2.Location = new System.Drawing.Point(227, 35);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(200, 100);
            this.groupBox2.TabIndex = 4;
            this.groupBox2.TabStop = false;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(162, 16);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(34, 13);
            this.label5.TabIndex = 3;
            this.label5.Text = "Costo";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(5, 82);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(117, 13);
            this.label6.TabIndex = 2;
            this.label6.Text = "Valor Inventario (Costo)";
            // 
            // valorInventario_Costo
            // 
            this.valorInventario_Costo.AutoSize = true;
            this.valorInventario_Costo.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.valorInventario_Costo.Location = new System.Drawing.Point(16, 58);
            this.valorInventario_Costo.Name = "valorInventario_Costo";
            this.valorInventario_Costo.Size = new System.Drawing.Size(56, 15);
            this.valorInventario_Costo.TabIndex = 1;
            this.valorInventario_Costo.Text = "Cantidad";
            // 
            // iconPictureBox2
            // 
            this.iconPictureBox2.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.iconPictureBox2.ForeColor = System.Drawing.Color.Lime;
            this.iconPictureBox2.IconChar = FontAwesome.Sharp.IconChar.Donate;
            this.iconPictureBox2.IconColor = System.Drawing.Color.Lime;
            this.iconPictureBox2.IconFont = FontAwesome.Sharp.IconFont.Auto;
            this.iconPictureBox2.Location = new System.Drawing.Point(11, 19);
            this.iconPictureBox2.Name = "iconPictureBox2";
            this.iconPictureBox2.Size = new System.Drawing.Size(32, 32);
            this.iconPictureBox2.TabIndex = 0;
            this.iconPictureBox2.TabStop = false;
            // 
            // groupBox3
            // 
            this.groupBox3.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.groupBox3.Controls.Add(this.label8);
            this.groupBox3.Controls.Add(this.label9);
            this.groupBox3.Controls.Add(this.valorInventario_Venta);
            this.groupBox3.Controls.Add(this.iconPictureBox3);
            this.groupBox3.Location = new System.Drawing.Point(443, 35);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(200, 100);
            this.groupBox3.TabIndex = 5;
            this.groupBox3.TabStop = false;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(162, 16);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(35, 13);
            this.label8.TabIndex = 3;
            this.label8.Text = "Venta";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(5, 82);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(118, 13);
            this.label9.TabIndex = 2;
            this.label9.Text = "Valor Inventario (Venta)";
            // 
            // valorInventario_Venta
            // 
            this.valorInventario_Venta.AutoSize = true;
            this.valorInventario_Venta.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.valorInventario_Venta.Location = new System.Drawing.Point(16, 58);
            this.valorInventario_Venta.Name = "valorInventario_Venta";
            this.valorInventario_Venta.Size = new System.Drawing.Size(56, 15);
            this.valorInventario_Venta.TabIndex = 1;
            this.valorInventario_Venta.Text = "Cantidad";
            // 
            // iconPictureBox3
            // 
            this.iconPictureBox3.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.iconPictureBox3.ForeColor = System.Drawing.Color.Blue;
            this.iconPictureBox3.IconChar = FontAwesome.Sharp.IconChar.DollyFlatbed;
            this.iconPictureBox3.IconColor = System.Drawing.Color.Blue;
            this.iconPictureBox3.IconFont = FontAwesome.Sharp.IconFont.Auto;
            this.iconPictureBox3.Location = new System.Drawing.Point(11, 19);
            this.iconPictureBox3.Name = "iconPictureBox3";
            this.iconPictureBox3.Size = new System.Drawing.Size(32, 32);
            this.iconPictureBox3.TabIndex = 0;
            this.iconPictureBox3.TabStop = false;
            // 
            // groupBox4
            // 
            this.groupBox4.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.groupBox4.Controls.Add(this.label12);
            this.groupBox4.Controls.Add(this.ProductosBajo);
            this.groupBox4.Controls.Add(this.iconPictureBox4);
            this.groupBox4.Location = new System.Drawing.Point(658, 35);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(200, 100);
            this.groupBox4.TabIndex = 6;
            this.groupBox4.TabStop = false;
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(5, 82);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(104, 13);
            this.label12.TabIndex = 2;
            this.label12.Text = "Producto Stock bajo";
            // 
            // ProductosBajo
            // 
            this.ProductosBajo.AutoSize = true;
            this.ProductosBajo.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ProductosBajo.Location = new System.Drawing.Point(16, 58);
            this.ProductosBajo.Name = "ProductosBajo";
            this.ProductosBajo.Size = new System.Drawing.Size(56, 15);
            this.ProductosBajo.TabIndex = 1;
            this.ProductosBajo.Text = "Cantidad";
            // 
            // iconPictureBox4
            // 
            this.iconPictureBox4.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.iconPictureBox4.Flip = FontAwesome.Sharp.FlipOrientation.Vertical;
            this.iconPictureBox4.ForeColor = System.Drawing.Color.Yellow;
            this.iconPictureBox4.IconChar = FontAwesome.Sharp.IconChar.ExclamationTriangle;
            this.iconPictureBox4.IconColor = System.Drawing.Color.Yellow;
            this.iconPictureBox4.IconFont = FontAwesome.Sharp.IconFont.Auto;
            this.iconPictureBox4.InitialImage = global::CapaPresentacion.Properties.Resources.icons8_alerta_32;
            this.iconPictureBox4.Location = new System.Drawing.Point(11, 19);
            this.iconPictureBox4.Name = "iconPictureBox4";
            this.iconPictureBox4.Size = new System.Drawing.Size(32, 32);
            this.iconPictureBox4.TabIndex = 0;
            this.iconPictureBox4.TabStop = false;
            this.iconPictureBox4.WaitOnLoad = true;
            // 
            // groupBox5
            // 
            this.groupBox5.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.groupBox5.Controls.Add(this.label11);
            this.groupBox5.Controls.Add(this.ProductosAgotados);
            this.groupBox5.Controls.Add(this.iconPictureBox5);
            this.groupBox5.Location = new System.Drawing.Point(864, 35);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(200, 100);
            this.groupBox5.TabIndex = 7;
            this.groupBox5.TabStop = false;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(8, 82);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(102, 13);
            this.label11.TabIndex = 4;
            this.label11.Text = "Productos sin Stock";
            // 
            // ProductosAgotados
            // 
            this.ProductosAgotados.AutoSize = true;
            this.ProductosAgotados.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ProductosAgotados.Location = new System.Drawing.Point(16, 58);
            this.ProductosAgotados.Name = "ProductosAgotados";
            this.ProductosAgotados.Size = new System.Drawing.Size(56, 15);
            this.ProductosAgotados.TabIndex = 1;
            this.ProductosAgotados.Text = "Cantidad";
            // 
            // iconPictureBox5
            // 
            this.iconPictureBox5.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.iconPictureBox5.ForeColor = System.Drawing.Color.Red;
            this.iconPictureBox5.IconChar = FontAwesome.Sharp.IconChar.Exclamation;
            this.iconPictureBox5.IconColor = System.Drawing.Color.Red;
            this.iconPictureBox5.IconFont = FontAwesome.Sharp.IconFont.Auto;
            this.iconPictureBox5.Location = new System.Drawing.Point(11, 19);
            this.iconPictureBox5.Name = "iconPictureBox5";
            this.iconPictureBox5.Size = new System.Drawing.Size(32, 32);
            this.iconPictureBox5.TabIndex = 0;
            this.iconPictureBox5.TabStop = false;
            // 
            // btnExportarPDF
            // 
            this.btnExportarPDF.BackColor = System.Drawing.Color.White;
            this.btnExportarPDF.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnExportarPDF.FlatAppearance.BorderSize = 0;
            this.btnExportarPDF.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.btnExportarPDF.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnExportarPDF.IconChar = FontAwesome.Sharp.IconChar.FilePdf;
            this.btnExportarPDF.IconColor = System.Drawing.Color.Red;
            this.btnExportarPDF.IconFont = FontAwesome.Sharp.IconFont.Auto;
            this.btnExportarPDF.IconSize = 28;
            this.btnExportarPDF.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnExportarPDF.Location = new System.Drawing.Point(1070, 93);
            this.btnExportarPDF.Name = "btnExportarPDF";
            this.btnExportarPDF.Size = new System.Drawing.Size(114, 38);
            this.btnExportarPDF.TabIndex = 64;
            this.btnExportarPDF.Text = "Exportar PDF";
            this.btnExportarPDF.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnExportarPDF.UseVisualStyleBackColor = false;
            this.btnExportarPDF.Click += new System.EventHandler(this.btnDescargarPDF_Click);
            // 
            // btnExportarExcel
            // 
            this.btnExportarExcel.BackColor = System.Drawing.Color.White;
            this.btnExportarExcel.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnExportarExcel.FlatAppearance.BorderSize = 0;
            this.btnExportarExcel.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(255)))), ((int)(((byte)(224)))));
            this.btnExportarExcel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnExportarExcel.IconChar = FontAwesome.Sharp.IconChar.FileExcel;
            this.btnExportarExcel.IconColor = System.Drawing.Color.ForestGreen;
            this.btnExportarExcel.IconFont = FontAwesome.Sharp.IconFont.Auto;
            this.btnExportarExcel.IconSize = 28;
            this.btnExportarExcel.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnExportarExcel.Location = new System.Drawing.Point(1070, 43);
            this.btnExportarExcel.Name = "btnExportarExcel";
            this.btnExportarExcel.Size = new System.Drawing.Size(114, 44);
            this.btnExportarExcel.TabIndex = 63;
            this.btnExportarExcel.Text = "Exportar Excel";
            this.btnExportarExcel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnExportarExcel.UseVisualStyleBackColor = false;
            this.btnExportarExcel.Click += new System.EventHandler(this.btnExportarExcel_Click);
            // 
            // dgvDataProducto
            // 
            this.dgvDataProducto.AllowUserToAddRows = false;
            this.dgvDataProducto.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvDataProducto.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.Padding = new System.Windows.Forms.Padding(2);
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvDataProducto.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dgvDataProducto.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvDataProducto.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.IMG,
            this.CODIGO,
            this.PRODUCTO,
            this.STOCK});
            this.dgvDataProducto.Location = new System.Drawing.Point(4, 207);
            this.dgvDataProducto.Margin = new System.Windows.Forms.Padding(2);
            this.dgvDataProducto.MinimumSize = new System.Drawing.Size(10, 10);
            this.dgvDataProducto.MultiSelect = false;
            this.dgvDataProducto.Name = "dgvDataProducto";
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.Desktop;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvDataProducto.RowHeadersDefaultCellStyle = dataGridViewCellStyle2;
            this.dgvDataProducto.RowTemplate.Height = 28;
            this.dgvDataProducto.Size = new System.Drawing.Size(683, 481);
            this.dgvDataProducto.TabIndex = 65;
            this.dgvDataProducto.Scroll += new System.Windows.Forms.ScrollEventHandler(this.dgvDataProducto_Scroll);
            // 
            // IMG
            // 
            this.IMG.HeaderText = "IMG";
            this.IMG.Name = "IMG";
            // 
            // CODIGO
            // 
            this.CODIGO.HeaderText = "CODIGO";
            this.CODIGO.Name = "CODIGO";
            // 
            // PRODUCTO
            // 
            this.PRODUCTO.HeaderText = "PRODUCTO";
            this.PRODUCTO.Name = "PRODUCTO";
            // 
            // STOCK
            // 
            this.STOCK.HeaderText = "STOCK";
            this.STOCK.Name = "STOCK";
            // 
            // dgvDataCosto
            // 
            this.dgvDataCosto.AllowUserToAddRows = false;
            this.dgvDataCosto.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle3.Padding = new System.Windows.Forms.Padding(2);
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvDataCosto.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle3;
            this.dgvDataCosto.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvDataCosto.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.COSTOUNT,
            this.TOTALCOSTO});
            this.dgvDataCosto.Location = new System.Drawing.Point(690, 207);
            this.dgvDataCosto.Margin = new System.Windows.Forms.Padding(2);
            this.dgvDataCosto.MinimumSize = new System.Drawing.Size(10, 10);
            this.dgvDataCosto.MultiSelect = false;
            this.dgvDataCosto.Name = "dgvDataCosto";
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle4.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle4.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle4.ForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle4.SelectionBackColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle4.SelectionForeColor = System.Drawing.SystemColors.Desktop;
            dataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvDataCosto.RowHeadersDefaultCellStyle = dataGridViewCellStyle4;
            this.dgvDataCosto.RowTemplate.Height = 28;
            this.dgvDataCosto.Size = new System.Drawing.Size(343, 481);
            this.dgvDataCosto.TabIndex = 66;
            // 
            // COSTOUNT
            // 
            this.COSTOUNT.HeaderText = "COSTO UNT";
            this.COSTOUNT.Name = "COSTOUNT";
            this.COSTOUNT.Width = 150;
            // 
            // TOTALCOSTO
            // 
            this.TOTALCOSTO.HeaderText = "TOTAL COSTO";
            this.TOTALCOSTO.Name = "TOTALCOSTO";
            this.TOTALCOSTO.Width = 150;
            // 
            // dgvDataAccion
            // 
            this.dgvDataAccion.AllowUserToAddRows = false;
            this.dgvDataAccion.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvDataAccion.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle5.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle5.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle5.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle5.Padding = new System.Windows.Forms.Padding(2);
            dataGridViewCellStyle5.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle5.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle5.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvDataAccion.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle5;
            this.dgvDataAccion.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvDataAccion.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ESTADO,
            this.ACCION});
            this.dgvDataAccion.Location = new System.Drawing.Point(1385, 207);
            this.dgvDataAccion.Margin = new System.Windows.Forms.Padding(2);
            this.dgvDataAccion.MinimumSize = new System.Drawing.Size(10, 10);
            this.dgvDataAccion.MultiSelect = false;
            this.dgvDataAccion.Name = "dgvDataAccion";
            dataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle6.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle6.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle6.ForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle6.SelectionBackColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle6.SelectionForeColor = System.Drawing.SystemColors.Desktop;
            dataGridViewCellStyle6.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvDataAccion.RowHeadersDefaultCellStyle = dataGridViewCellStyle6;
            this.dgvDataAccion.RowTemplate.Height = 28;
            this.dgvDataAccion.Size = new System.Drawing.Size(247, 481);
            this.dgvDataAccion.TabIndex = 67;
            // 
            // ESTADO
            // 
            this.ESTADO.HeaderText = "ESTADO";
            this.ESTADO.Name = "ESTADO";
            // 
            // ACCION
            // 
            this.ACCION.HeaderText = "ACCION";
            this.ACCION.Name = "ACCION";
            // 
            // dvgDataVenta
            // 
            this.dvgDataVenta.AllowUserToAddRows = false;
            this.dvgDataVenta.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            dataGridViewCellStyle7.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle7.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle7.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle7.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle7.Padding = new System.Windows.Forms.Padding(2);
            dataGridViewCellStyle7.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle7.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle7.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dvgDataVenta.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle7;
            this.dvgDataVenta.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dvgDataVenta.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.PRECIOVENTA,
            this.TOTALVENTA});
            this.dvgDataVenta.Location = new System.Drawing.Point(1037, 207);
            this.dvgDataVenta.Margin = new System.Windows.Forms.Padding(2);
            this.dvgDataVenta.MinimumSize = new System.Drawing.Size(10, 10);
            this.dvgDataVenta.MultiSelect = false;
            this.dvgDataVenta.Name = "dvgDataVenta";
            dataGridViewCellStyle8.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle8.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle8.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle8.ForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle8.SelectionBackColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle8.SelectionForeColor = System.Drawing.SystemColors.Desktop;
            dataGridViewCellStyle8.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dvgDataVenta.RowHeadersDefaultCellStyle = dataGridViewCellStyle8;
            this.dvgDataVenta.RowTemplate.Height = 28;
            this.dvgDataVenta.Size = new System.Drawing.Size(344, 481);
            this.dvgDataVenta.TabIndex = 68;
            // 
            // PRECIOVENTA
            // 
            this.PRECIOVENTA.HeaderText = "PRECIO VENTA";
            this.PRECIOVENTA.Name = "PRECIOVENTA";
            this.PRECIOVENTA.Width = 150;
            // 
            // TOTALVENTA
            // 
            this.TOTALVENTA.HeaderText = "TOTAL VENTA";
            this.TOTALVENTA.Name = "TOTALVENTA";
            this.TOTALVENTA.Width = 150;
            // 
            // label14
            // 
            this.label14.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label14.BackColor = System.Drawing.Color.White;
            this.label14.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label14.Location = new System.Drawing.Point(4, 151);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(1628, 52);
            this.label14.TabIndex = 69;
            // 
            // txtBusqueda
            // 
            this.txtBusqueda.AllowDrop = true;
            this.txtBusqueda.Location = new System.Drawing.Point(8, 171);
            this.txtBusqueda.MaximumSize = new System.Drawing.Size(340, 20);
            this.txtBusqueda.Name = "txtBusqueda";
            this.txtBusqueda.Size = new System.Drawing.Size(290, 20);
            this.txtBusqueda.TabIndex = 70;
            // 
            // cboBusquedaCategorias
            // 
            this.cboBusquedaCategorias.AllowDrop = true;
            this.cboBusquedaCategorias.BackColor = System.Drawing.SystemColors.ActiveBorder;
            this.cboBusquedaCategorias.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboBusquedaCategorias.FormattingEnabled = true;
            this.cboBusquedaCategorias.Location = new System.Drawing.Point(302, 170);
            this.cboBusquedaCategorias.Name = "cboBusquedaCategorias";
            this.cboBusquedaCategorias.Size = new System.Drawing.Size(161, 21);
            this.cboBusquedaCategorias.TabIndex = 71;
            // 
            // cboBusquedaPorStock
            // 
            this.cboBusquedaPorStock.AllowDrop = true;
            this.cboBusquedaPorStock.BackColor = System.Drawing.SystemColors.ActiveBorder;
            this.cboBusquedaPorStock.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboBusquedaPorStock.FormattingEnabled = true;
            this.cboBusquedaPorStock.Location = new System.Drawing.Point(468, 170);
            this.cboBusquedaPorStock.Name = "cboBusquedaPorStock";
            this.cboBusquedaPorStock.Size = new System.Drawing.Size(161, 21);
            this.cboBusquedaPorStock.TabIndex = 72;
            // 
            // btnLimpiarCombo
            // 
            this.btnLimpiarCombo.AllowDrop = true;
            this.btnLimpiarCombo.BackColor = System.Drawing.SystemColors.Control;
            this.btnLimpiarCombo.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnLimpiarCombo.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnLimpiarCombo.ForeColor = System.Drawing.SystemColors.WindowFrame;
            this.btnLimpiarCombo.IconChar = FontAwesome.Sharp.IconChar.Broom;
            this.btnLimpiarCombo.IconColor = System.Drawing.Color.Black;
            this.btnLimpiarCombo.IconFont = FontAwesome.Sharp.IconFont.Auto;
            this.btnLimpiarCombo.IconSize = 24;
            this.btnLimpiarCombo.Location = new System.Drawing.Point(686, 169);
            this.btnLimpiarCombo.Margin = new System.Windows.Forms.Padding(2);
            this.btnLimpiarCombo.Name = "btnLimpiarCombo";
            this.btnLimpiarCombo.Size = new System.Drawing.Size(48, 23);
            this.btnLimpiarCombo.TabIndex = 74;
            this.btnLimpiarCombo.UseVisualStyleBackColor = false;
            this.btnLimpiarCombo.AutoSizeChanged += new System.EventHandler(this.btnLimpiarCombo_AutoSizeChanged);
            this.btnLimpiarCombo.Click += new System.EventHandler(this.btnLimpiarCombo_AutoSizeChanged);
            // 
            // btnBuscar
            // 
            this.btnBuscar.AllowDrop = true;
            this.btnBuscar.BackColor = System.Drawing.SystemColors.Control;
            this.btnBuscar.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnBuscar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnBuscar.ForeColor = System.Drawing.SystemColors.WindowFrame;
            this.btnBuscar.IconChar = FontAwesome.Sharp.IconChar.Search;
            this.btnBuscar.IconColor = System.Drawing.Color.Black;
            this.btnBuscar.IconFont = FontAwesome.Sharp.IconFont.Auto;
            this.btnBuscar.IconSize = 24;
            this.btnBuscar.Location = new System.Drawing.Point(634, 169);
            this.btnBuscar.Margin = new System.Windows.Forms.Padding(2);
            this.btnBuscar.Name = "btnBuscar";
            this.btnBuscar.Size = new System.Drawing.Size(48, 23);
            this.btnBuscar.TabIndex = 73;
            this.btnBuscar.UseVisualStyleBackColor = false;
            this.btnBuscar.AutoSizeChanged += new System.EventHandler(this.btnLimpiarCombo_AutoSizeChanged);
            this.btnBuscar.Click += new System.EventHandler(this.btnBuscar_Click);
            // 
            // frmReporteInventario
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1636, 693);
            this.Controls.Add(this.btnLimpiarCombo);
            this.Controls.Add(this.btnBuscar);
            this.Controls.Add(this.cboBusquedaPorStock);
            this.Controls.Add(this.cboBusquedaCategorias);
            this.Controls.Add(this.txtBusqueda);
            this.Controls.Add(this.label14);
            this.Controls.Add(this.dvgDataVenta);
            this.Controls.Add(this.dgvDataAccion);
            this.Controls.Add(this.dgvDataCosto);
            this.Controls.Add(this.dgvDataProducto);
            this.Controls.Add(this.btnExportarPDF);
            this.Controls.Add(this.btnExportarExcel);
            this.Controls.Add(this.groupBox5);
            this.Controls.Add(this.groupBox4);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.label1);
            this.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.Name = "frmReporteInventario";
            this.Text = "frmReporteInventario";
            this.Load += new System.EventHandler(this.frmReporteInventario_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.iconPictureBox1)).EndInit();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.iconPictureBox2)).EndInit();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.iconPictureBox3)).EndInit();
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.iconPictureBox4)).EndInit();
            this.groupBox5.ResumeLayout(false);
            this.groupBox5.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.iconPictureBox5)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvDataProducto)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvDataCosto)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvDataAccion)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dvgDataVenta)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label totalproductos;
        private FontAwesome.Sharp.IconPictureBox iconPictureBox1;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label valorInventario_Costo;
        private FontAwesome.Sharp.IconPictureBox iconPictureBox2;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label valorInventario_Venta;
        private FontAwesome.Sharp.IconPictureBox iconPictureBox3;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label ProductosBajo;
        private FontAwesome.Sharp.IconPictureBox iconPictureBox4;
        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.Label ProductosAgotados;
        private FontAwesome.Sharp.IconPictureBox iconPictureBox5;
        private System.Windows.Forms.Label label11;
        private FontAwesome.Sharp.IconButton btnExportarPDF;
        private FontAwesome.Sharp.IconButton btnExportarExcel;
        private System.Windows.Forms.DataGridView dgvDataProducto;
        private System.Windows.Forms.DataGridView dgvDataCosto;
        private System.Windows.Forms.DataGridViewTextBoxColumn COSTOUNT;
        private System.Windows.Forms.DataGridViewTextBoxColumn TOTALCOSTO;
        private System.Windows.Forms.DataGridView dgvDataAccion;
        private System.Windows.Forms.DataGridView dvgDataVenta;
        private System.Windows.Forms.DataGridViewTextBoxColumn PRECIOVENTA;
        private System.Windows.Forms.DataGridViewTextBoxColumn TOTALVENTA;
        private System.Windows.Forms.DataGridViewTextBoxColumn ESTADO;
        private System.Windows.Forms.DataGridViewTextBoxColumn ACCION;
        private System.Windows.Forms.DataGridViewTextBoxColumn IMG;
        private System.Windows.Forms.DataGridViewTextBoxColumn CODIGO;
        private System.Windows.Forms.DataGridViewTextBoxColumn PRODUCTO;
        private System.Windows.Forms.DataGridViewTextBoxColumn STOCK;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.TextBox txtBusqueda;
        private System.Windows.Forms.ComboBox cboBusquedaCategorias;
        private System.Windows.Forms.ComboBox cboBusquedaPorStock;
        private FontAwesome.Sharp.IconButton btnLimpiarCombo;
        private FontAwesome.Sharp.IconButton btnBuscar;
    }
}