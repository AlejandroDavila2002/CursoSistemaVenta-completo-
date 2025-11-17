namespace CapaPresentacion
{
    partial class Inicio
    {
        /// <summary>
        /// Variable del diseñador necesaria.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Limpiar los recursos que se estén usando.
        /// </summary>
        /// <param name="disposing">true si los recursos administrados se deben desechar; false en caso contrario.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Código generado por el Diseñador de Windows Forms

        /// <summary>
        /// Método necesario para admitir el Diseñador. No se puede modificar
        /// el contenido de este método con el editor de código.
        /// </summary>
        private void InitializeComponent()
        {
            this.menu = new System.Windows.Forms.MenuStrip();
            this.menuusuarios = new FontAwesome.Sharp.IconMenuItem();
            this.menumantenedor = new FontAwesome.Sharp.IconMenuItem();
            this.subMenuCategoria = new FontAwesome.Sharp.IconMenuItem();
            this.subMenuProducto = new FontAwesome.Sharp.IconMenuItem();
            this.menuventas = new FontAwesome.Sharp.IconMenuItem();
            this.submenuregistrarVenta = new FontAwesome.Sharp.IconMenuItem();
            this.submenuverdetalleVenta = new FontAwesome.Sharp.IconMenuItem();
            this.menucompras = new FontAwesome.Sharp.IconMenuItem();
            this.submenuregistrarCompra = new FontAwesome.Sharp.IconMenuItem();
            this.submenuverdetalleCompra = new FontAwesome.Sharp.IconMenuItem();
            this.menucliente = new FontAwesome.Sharp.IconMenuItem();
            this.menuproveedores = new FontAwesome.Sharp.IconMenuItem();
            this.menureportes = new FontAwesome.Sharp.IconMenuItem();
            this.menusistema_cambiario = new FontAwesome.Sharp.IconMenuItem();
            this.menuacerdade = new FontAwesome.Sharp.IconMenuItem();
            this.menutitulo = new System.Windows.Forms.MenuStrip();
            this.label1 = new System.Windows.Forms.Label();
            this.contenedor = new System.Windows.Forms.Panel();
            this.label2 = new System.Windows.Forms.Label();
            this.lblUsuario = new System.Windows.Forms.Label();
            this.detalleNegocioToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menu.SuspendLayout();
            this.SuspendLayout();
            // 
            // menu
            // 
            this.menu.BackColor = System.Drawing.Color.White;
            this.menu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuusuarios,
            this.menumantenedor,
            this.menuventas,
            this.menucompras,
            this.menucliente,
            this.menuproveedores,
            this.menureportes,
            this.menusistema_cambiario,
            this.menuacerdade});
            this.menu.Location = new System.Drawing.Point(0, 66);
            this.menu.Name = "menu";
            this.menu.Size = new System.Drawing.Size(1244, 73);
            this.menu.TabIndex = 0;
            this.menu.Text = "menuStrip1";
            // 
            // menuusuarios
            // 
            this.menuusuarios.AutoSize = false;
            this.menuusuarios.IconChar = FontAwesome.Sharp.IconChar.UserCog;
            this.menuusuarios.IconColor = System.Drawing.Color.Black;
            this.menuusuarios.IconFont = FontAwesome.Sharp.IconFont.Auto;
            this.menuusuarios.IconSize = 50;
            this.menuusuarios.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.menuusuarios.Name = "menuusuarios";
            this.menuusuarios.Size = new System.Drawing.Size(80, 69);
            this.menuusuarios.Text = "Usuarios";
            this.menuusuarios.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.menuusuarios.Click += new System.EventHandler(this.menuusuarios_Click);
            // 
            // menumantenedor
            // 
            this.menumantenedor.AutoSize = false;
            this.menumantenedor.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.subMenuCategoria,
            this.subMenuProducto,
            this.detalleNegocioToolStripMenuItem});
            this.menumantenedor.IconChar = FontAwesome.Sharp.IconChar.Tools;
            this.menumantenedor.IconColor = System.Drawing.Color.Black;
            this.menumantenedor.IconFont = FontAwesome.Sharp.IconFont.Auto;
            this.menumantenedor.IconSize = 50;
            this.menumantenedor.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.menumantenedor.Name = "menumantenedor";
            this.menumantenedor.Size = new System.Drawing.Size(80, 69);
            this.menumantenedor.Text = "Mantenedor";
            this.menumantenedor.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            // 
            // subMenuCategoria
            // 
            this.subMenuCategoria.IconChar = FontAwesome.Sharp.IconChar.None;
            this.subMenuCategoria.IconColor = System.Drawing.Color.Black;
            this.subMenuCategoria.IconFont = FontAwesome.Sharp.IconFont.Auto;
            this.subMenuCategoria.Name = "subMenuCategoria";
            this.subMenuCategoria.Size = new System.Drawing.Size(180, 22);
            this.subMenuCategoria.Text = "Categoria";
            this.subMenuCategoria.Click += new System.EventHandler(this.subMenuCategoria_Click);
            // 
            // subMenuProducto
            // 
            this.subMenuProducto.IconChar = FontAwesome.Sharp.IconChar.None;
            this.subMenuProducto.IconColor = System.Drawing.Color.Black;
            this.subMenuProducto.IconFont = FontAwesome.Sharp.IconFont.Auto;
            this.subMenuProducto.Name = "subMenuProducto";
            this.subMenuProducto.Size = new System.Drawing.Size(180, 22);
            this.subMenuProducto.Text = "Producto";
            this.subMenuProducto.Click += new System.EventHandler(this.subMenuProducto_Click);
            // 
            // menuventas
            // 
            this.menuventas.AutoSize = false;
            this.menuventas.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.submenuregistrarVenta,
            this.submenuverdetalleVenta});
            this.menuventas.IconChar = FontAwesome.Sharp.IconChar.Tags;
            this.menuventas.IconColor = System.Drawing.Color.Black;
            this.menuventas.IconFont = FontAwesome.Sharp.IconFont.Auto;
            this.menuventas.IconSize = 50;
            this.menuventas.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.menuventas.Name = "menuventas";
            this.menuventas.Size = new System.Drawing.Size(80, 69);
            this.menuventas.Text = "Ventas";
            this.menuventas.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            // 
            // submenuregistrarVenta
            // 
            this.submenuregistrarVenta.IconChar = FontAwesome.Sharp.IconChar.None;
            this.submenuregistrarVenta.IconColor = System.Drawing.Color.Black;
            this.submenuregistrarVenta.IconFont = FontAwesome.Sharp.IconFont.Auto;
            this.submenuregistrarVenta.Name = "submenuregistrarVenta";
            this.submenuregistrarVenta.Size = new System.Drawing.Size(129, 22);
            this.submenuregistrarVenta.Text = "Registrar";
            this.submenuregistrarVenta.Click += new System.EventHandler(this.submenuregistrarVenta_Click_1);
            // 
            // submenuverdetalleVenta
            // 
            this.submenuverdetalleVenta.IconChar = FontAwesome.Sharp.IconChar.None;
            this.submenuverdetalleVenta.IconColor = System.Drawing.Color.Black;
            this.submenuverdetalleVenta.IconFont = FontAwesome.Sharp.IconFont.Auto;
            this.submenuverdetalleVenta.Name = "submenuverdetalleVenta";
            this.submenuverdetalleVenta.Size = new System.Drawing.Size(129, 22);
            this.submenuverdetalleVenta.Text = "Ver Detalle";
            this.submenuverdetalleVenta.Click += new System.EventHandler(this.submenuverdetalleVenta_Click);
            // 
            // menucompras
            // 
            this.menucompras.AutoSize = false;
            this.menucompras.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.submenuregistrarCompra,
            this.submenuverdetalleCompra});
            this.menucompras.IconChar = FontAwesome.Sharp.IconChar.DollyFlatbed;
            this.menucompras.IconColor = System.Drawing.Color.Black;
            this.menucompras.IconFont = FontAwesome.Sharp.IconFont.Auto;
            this.menucompras.IconSize = 50;
            this.menucompras.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.menucompras.Name = "menucompras";
            this.menucompras.Size = new System.Drawing.Size(80, 69);
            this.menucompras.Text = "Compras";
            this.menucompras.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            // 
            // submenuregistrarCompra
            // 
            this.submenuregistrarCompra.IconChar = FontAwesome.Sharp.IconChar.None;
            this.submenuregistrarCompra.IconColor = System.Drawing.Color.Black;
            this.submenuregistrarCompra.IconFont = FontAwesome.Sharp.IconFont.Auto;
            this.submenuregistrarCompra.Name = "submenuregistrarCompra";
            this.submenuregistrarCompra.Size = new System.Drawing.Size(166, 22);
            this.submenuregistrarCompra.Text = "Registrar Compra";
            this.submenuregistrarCompra.Click += new System.EventHandler(this.submenuregistrarCompra_Click);
            // 
            // submenuverdetalleCompra
            // 
            this.submenuverdetalleCompra.IconChar = FontAwesome.Sharp.IconChar.None;
            this.submenuverdetalleCompra.IconColor = System.Drawing.Color.Black;
            this.submenuverdetalleCompra.IconFont = FontAwesome.Sharp.IconFont.Auto;
            this.submenuverdetalleCompra.Name = "submenuverdetalleCompra";
            this.submenuverdetalleCompra.Size = new System.Drawing.Size(166, 22);
            this.submenuverdetalleCompra.Text = "Ver Detalle";
            this.submenuverdetalleCompra.Click += new System.EventHandler(this.submenuverdetalleCompra_Click);
            // 
            // menucliente
            // 
            this.menucliente.AutoSize = false;
            this.menucliente.IconChar = FontAwesome.Sharp.IconChar.UserFriends;
            this.menucliente.IconColor = System.Drawing.Color.Black;
            this.menucliente.IconFont = FontAwesome.Sharp.IconFont.Auto;
            this.menucliente.IconSize = 50;
            this.menucliente.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.menucliente.Name = "menucliente";
            this.menucliente.Size = new System.Drawing.Size(80, 69);
            this.menucliente.Text = "Clientes";
            this.menucliente.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.menucliente.Click += new System.EventHandler(this.menucliente_Click_1);
            // 
            // menuproveedores
            // 
            this.menuproveedores.AutoSize = false;
            this.menuproveedores.IconChar = FontAwesome.Sharp.IconChar.AddressCard;
            this.menuproveedores.IconColor = System.Drawing.Color.Black;
            this.menuproveedores.IconFont = FontAwesome.Sharp.IconFont.Auto;
            this.menuproveedores.IconSize = 50;
            this.menuproveedores.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.menuproveedores.Name = "menuproveedores";
            this.menuproveedores.Size = new System.Drawing.Size(80, 69);
            this.menuproveedores.Text = "Proveedores";
            this.menuproveedores.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.menuproveedores.Click += new System.EventHandler(this.menuproveedores_Click_1);
            // 
            // menureportes
            // 
            this.menureportes.AutoSize = false;
            this.menureportes.IconChar = FontAwesome.Sharp.IconChar.ChartBar;
            this.menureportes.IconColor = System.Drawing.Color.Black;
            this.menureportes.IconFont = FontAwesome.Sharp.IconFont.Auto;
            this.menureportes.IconSize = 50;
            this.menureportes.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.menureportes.Name = "menureportes";
            this.menureportes.Size = new System.Drawing.Size(80, 69);
            this.menureportes.Text = "Reportes";
            this.menureportes.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.menureportes.Click += new System.EventHandler(this.menureportes_Click);
            // 
            // menusistema_cambiario
            // 
            this.menusistema_cambiario.AutoSize = false;
            this.menusistema_cambiario.IconChar = FontAwesome.Sharp.IconChar.MoneyBillWaveAlt;
            this.menusistema_cambiario.IconColor = System.Drawing.Color.Black;
            this.menusistema_cambiario.IconFont = FontAwesome.Sharp.IconFont.Auto;
            this.menusistema_cambiario.IconSize = 50;
            this.menusistema_cambiario.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.menusistema_cambiario.Name = "menusistema_cambiario";
            this.menusistema_cambiario.Size = new System.Drawing.Size(80, 69);
            this.menusistema_cambiario.Text = "Moneda";
            this.menusistema_cambiario.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.menusistema_cambiario.Click += new System.EventHandler(this.menusistemaCambiario_Click);
            // 
            // menuacerdade
            // 
            this.menuacerdade.AutoSize = false;
            this.menuacerdade.IconChar = FontAwesome.Sharp.IconChar.InfoCircle;
            this.menuacerdade.IconColor = System.Drawing.Color.Black;
            this.menuacerdade.IconFont = FontAwesome.Sharp.IconFont.Auto;
            this.menuacerdade.IconSize = 50;
            this.menuacerdade.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.menuacerdade.Name = "menuacerdade";
            this.menuacerdade.Size = new System.Drawing.Size(80, 69);
            this.menuacerdade.Text = "Acerca de";
            this.menuacerdade.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            // 
            // menutitulo
            // 
            this.menutitulo.AutoSize = false;
            this.menutitulo.BackColor = System.Drawing.Color.SteelBlue;
            this.menutitulo.Location = new System.Drawing.Point(0, 0);
            this.menutitulo.Name = "menutitulo";
            this.menutitulo.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.menutitulo.Size = new System.Drawing.Size(1244, 66);
            this.menutitulo.TabIndex = 1;
            this.menutitulo.Text = "menuStrip2";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.SteelBlue;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 20F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.White;
            this.label1.Location = new System.Drawing.Point(41, 24);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(241, 31);
            this.label1.TabIndex = 2;
            this.label1.Text = "Sistema de Ventas";
            // 
            // contenedor
            // 
            this.contenedor.Dock = System.Windows.Forms.DockStyle.Fill;
            this.contenedor.Location = new System.Drawing.Point(0, 139);
            this.contenedor.Name = "contenedor";
            this.contenedor.Size = new System.Drawing.Size(1244, 520);
            this.contenedor.TabIndex = 3;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.BackColor = System.Drawing.Color.SteelBlue;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.SystemColors.Control;
            this.label2.Location = new System.Drawing.Point(1029, 24);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(65, 16);
            this.label2.TabIndex = 4;
            this.label2.Text = "Usuario:";
            // 
            // lblUsuario
            // 
            this.lblUsuario.AutoSize = true;
            this.lblUsuario.BackColor = System.Drawing.Color.SteelBlue;
            this.lblUsuario.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblUsuario.ForeColor = System.Drawing.SystemColors.Control;
            this.lblUsuario.Location = new System.Drawing.Point(1100, 24);
            this.lblUsuario.Name = "lblUsuario";
            this.lblUsuario.Size = new System.Drawing.Size(78, 16);
            this.lblUsuario.TabIndex = 5;
            this.lblUsuario.Text = "lblUsuario";
            // 
            // detalleNegocioToolStripMenuItem
            // 
            this.detalleNegocioToolStripMenuItem.Name = "detalleNegocioToolStripMenuItem";
            this.detalleNegocioToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.detalleNegocioToolStripMenuItem.Text = "Detalle Negocio";
            this.detalleNegocioToolStripMenuItem.Click += new System.EventHandler(this.detalleNegocioToolStripMenuItem_Click);
            // 
            // Inicio
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1244, 659);
            this.Controls.Add(this.lblUsuario);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.contenedor);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.menu);
            this.Controls.Add(this.menutitulo);
            this.MainMenuStrip = this.menu;
            this.Name = "Inicio";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Show;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "0.0";
            this.Load += new System.EventHandler(this.Inicio_Load);
            this.menu.ResumeLayout(false);
            this.menu.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menu;
        private System.Windows.Forms.MenuStrip menutitulo;
        private FontAwesome.Sharp.IconMenuItem menuacerdade;
        private System.Windows.Forms.Label label1;
        private FontAwesome.Sharp.IconMenuItem menuusuarios;
        private FontAwesome.Sharp.IconMenuItem menumantenedor;
        private FontAwesome.Sharp.IconMenuItem menuventas;
        private FontAwesome.Sharp.IconMenuItem menucompras;
        private FontAwesome.Sharp.IconMenuItem menucliente;
        private FontAwesome.Sharp.IconMenuItem menuproveedores;
        private FontAwesome.Sharp.IconMenuItem menureportes;
        private System.Windows.Forms.Panel contenedor;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label lblUsuario;
        private FontAwesome.Sharp.IconMenuItem subMenuProducto;
        private FontAwesome.Sharp.IconMenuItem subMenuCategoria;
        private FontAwesome.Sharp.IconMenuItem submenuregistrarVenta;
        private FontAwesome.Sharp.IconMenuItem submenuverdetalleVenta;
        private FontAwesome.Sharp.IconMenuItem submenuregistrarCompra;
        private FontAwesome.Sharp.IconMenuItem submenuverdetalleCompra;
        private FontAwesome.Sharp.IconMenuItem menusistema_cambiario;
        private System.Windows.Forms.ToolStripMenuItem detalleNegocioToolStripMenuItem;
    }
}

