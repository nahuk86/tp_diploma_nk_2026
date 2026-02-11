namespace UI
{
    partial class Form1
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
            this.menuStrip = new System.Windows.Forms.MenuStrip();
            this.menuFile = new System.Windows.Forms.ToolStripMenuItem();
            this.menuLogout = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.menuExit = new System.Windows.Forms.ToolStripMenuItem();
            this.menuAdmin = new System.Windows.Forms.ToolStripMenuItem();
            this.menuUsers = new System.Windows.Forms.ToolStripMenuItem();
            this.menuRoles = new System.Windows.Forms.ToolStripMenuItem();
            this.menuInventory = new System.Windows.Forms.ToolStripMenuItem();
            this.menuProducts = new System.Windows.Forms.ToolStripMenuItem();
            this.menuWarehouses = new System.Windows.Forms.ToolStripMenuItem();
            this.menuOperations = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStockMovements = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStockQuery = new System.Windows.Forms.ToolStripMenuItem();
            this.menuSettings = new System.Windows.Forms.ToolStripMenuItem();
            this.menuLanguage = new System.Windows.Forms.ToolStripMenuItem();
            this.menuLanguageSpanish = new System.Windows.Forms.ToolStripMenuItem();
            this.menuLanguageEnglish = new System.Windows.Forms.ToolStripMenuItem();
            this.menuHelp = new System.Windows.Forms.ToolStripMenuItem();
            this.menuAbout = new System.Windows.Forms.ToolStripMenuItem();
            this.statusStrip = new System.Windows.Forms.StatusStrip();
            this.statusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.menuStrip.SuspendLayout();
            this.statusStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip
            // 
            this.menuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuFile,
            this.menuAdmin,
            this.menuInventory,
            this.menuOperations,
            this.menuSettings,
            this.menuHelp});
            this.menuStrip.Location = new System.Drawing.Point(0, 0);
            this.menuStrip.Name = "menuStrip";
            this.menuStrip.Size = new System.Drawing.Size(1024, 24);
            this.menuStrip.TabIndex = 1;
            this.menuStrip.Text = "menuStrip";
            // 
            // menuFile
            // 
            this.menuFile.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuLogout,
            this.toolStripSeparator1,
            this.menuExit});
            this.menuFile.Name = "menuFile";
            this.menuFile.Size = new System.Drawing.Size(60, 20);
            this.menuFile.Text = "&Archivo";
            // 
            // menuLogout
            // 
            this.menuLogout.Name = "menuLogout";
            this.menuLogout.Size = new System.Drawing.Size(180, 22);
            this.menuLogout.Text = "Cerrar &Sesión";
            this.menuLogout.Click += new System.EventHandler(this.menuLogout_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(177, 6);
            // 
            // menuExit
            // 
            this.menuExit.Name = "menuExit";
            this.menuExit.Size = new System.Drawing.Size(180, 22);
            this.menuExit.Text = "&Salir";
            this.menuExit.Click += new System.EventHandler(this.menuExit_Click);
            // 
            // menuAdmin
            // 
            this.menuAdmin.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuUsers,
            this.menuRoles});
            this.menuAdmin.Name = "menuAdmin";
            this.menuAdmin.Size = new System.Drawing.Size(100, 20);
            this.menuAdmin.Text = "A&dministración";
            // 
            // menuUsers
            // 
            this.menuUsers.Name = "menuUsers";
            this.menuUsers.Size = new System.Drawing.Size(180, 22);
            this.menuUsers.Text = "&Usuarios";
            this.menuUsers.Click += new System.EventHandler(this.menuUsers_Click);
            // 
            // menuRoles
            // 
            this.menuRoles.Name = "menuRoles";
            this.menuRoles.Size = new System.Drawing.Size(180, 22);
            this.menuRoles.Text = "&Roles";
            this.menuRoles.Click += new System.EventHandler(this.menuRoles_Click);
            // 
            // menuInventory
            // 
            this.menuInventory.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuProducts,
            this.menuWarehouses});
            this.menuInventory.Name = "menuInventory";
            this.menuInventory.Size = new System.Drawing.Size(72, 20);
            this.menuInventory.Text = "&Inventario";
            // 
            // menuProducts
            // 
            this.menuProducts.Name = "menuProducts";
            this.menuProducts.Size = new System.Drawing.Size(180, 22);
            this.menuProducts.Text = "&Productos";
            this.menuProducts.Click += new System.EventHandler(this.menuProducts_Click);
            // 
            // menuWarehouses
            // 
            this.menuWarehouses.Name = "menuWarehouses";
            this.menuWarehouses.Size = new System.Drawing.Size(180, 22);
            this.menuWarehouses.Text = "&Almacenes";
            this.menuWarehouses.Click += new System.EventHandler(this.menuWarehouses_Click);
            // 
            // menuOperations
            // 
            this.menuOperations.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuStockMovements,
            this.menuStockQuery});
            this.menuOperations.Name = "menuOperations";
            this.menuOperations.Size = new System.Drawing.Size(85, 20);
            this.menuOperations.Text = "&Operaciones";
            // 
            // menuStockMovements
            // 
            this.menuStockMovements.Name = "menuStockMovements";
            this.menuStockMovements.Size = new System.Drawing.Size(180, 22);
            this.menuStockMovements.Text = "&Movimientos";
            this.menuStockMovements.Click += new System.EventHandler(this.menuStockMovements_Click);
            // 
            // menuStockQuery
            // 
            this.menuStockQuery.Name = "menuStockQuery";
            this.menuStockQuery.Size = new System.Drawing.Size(180, 22);
            this.menuStockQuery.Text = "&Consultar Stock";
            this.menuStockQuery.Click += new System.EventHandler(this.menuStockQuery_Click);
            // 
            // menuSettings
            // 
            this.menuSettings.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuLanguage});
            this.menuSettings.Name = "menuSettings";
            this.menuSettings.Size = new System.Drawing.Size(95, 20);
            this.menuSettings.Text = "&Configuración";
            // 
            // menuLanguage
            // 
            this.menuLanguage.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuLanguageSpanish,
            this.menuLanguageEnglish});
            this.menuLanguage.Name = "menuLanguage";
            this.menuLanguage.Size = new System.Drawing.Size(180, 22);
            this.menuLanguage.Text = "&Idioma";
            // 
            // menuLanguageSpanish
            // 
            this.menuLanguageSpanish.Checked = true;
            this.menuLanguageSpanish.CheckState = System.Windows.Forms.CheckState.Checked;
            this.menuLanguageSpanish.Name = "menuLanguageSpanish";
            this.menuLanguageSpanish.Size = new System.Drawing.Size(180, 22);
            this.menuLanguageSpanish.Text = "Español";
            this.menuLanguageSpanish.Click += new System.EventHandler(this.menuLanguageSpanish_Click);
            // 
            // menuLanguageEnglish
            // 
            this.menuLanguageEnglish.Name = "menuLanguageEnglish";
            this.menuLanguageEnglish.Size = new System.Drawing.Size(180, 22);
            this.menuLanguageEnglish.Text = "English";
            this.menuLanguageEnglish.Click += new System.EventHandler(this.menuLanguageEnglish_Click);
            // 
            // menuHelp
            // 
            this.menuHelp.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuAbout});
            this.menuHelp.Name = "menuHelp";
            this.menuHelp.Size = new System.Drawing.Size(53, 20);
            this.menuHelp.Text = "Ay&uda";
            // 
            // menuAbout
            // 
            this.menuAbout.Name = "menuAbout";
            this.menuAbout.Size = new System.Drawing.Size(180, 22);
            this.menuAbout.Text = "&Acerca de...";
            this.menuAbout.Click += new System.EventHandler(this.menuAbout_Click);
            // 
            // statusStrip
            // 
            this.statusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.statusLabel});
            this.statusStrip.Location = new System.Drawing.Point(0, 746);
            this.statusStrip.Name = "statusStrip";
            this.statusStrip.Size = new System.Drawing.Size(1024, 22);
            this.statusStrip.TabIndex = 2;
            this.statusStrip.Text = "statusStrip";
            // 
            // statusLabel
            // 
            this.statusLabel.Name = "statusLabel";
            this.statusLabel.Size = new System.Drawing.Size(42, 17);
            this.statusLabel.Text = "Listo";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1024, 768);
            this.Controls.Add(this.statusStrip);
            this.Controls.Add(this.menuStrip);
            this.IsMdiContainer = true;
            this.MainMenuStrip = this.menuStrip;
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Stock Manager";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.menuStrip.ResumeLayout(false);
            this.menuStrip.PerformLayout();
            this.statusStrip.ResumeLayout(false);
            this.statusStrip.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip;
        private System.Windows.Forms.ToolStripMenuItem menuFile;
        private System.Windows.Forms.ToolStripMenuItem menuLogout;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem menuExit;
        private System.Windows.Forms.ToolStripMenuItem menuAdmin;
        private System.Windows.Forms.ToolStripMenuItem menuUsers;
        private System.Windows.Forms.ToolStripMenuItem menuRoles;
        private System.Windows.Forms.ToolStripMenuItem menuInventory;
        private System.Windows.Forms.ToolStripMenuItem menuProducts;
        private System.Windows.Forms.ToolStripMenuItem menuWarehouses;
        private System.Windows.Forms.ToolStripMenuItem menuOperations;
        private System.Windows.Forms.ToolStripMenuItem menuStockMovements;
        private System.Windows.Forms.ToolStripMenuItem menuStockQuery;
        private System.Windows.Forms.ToolStripMenuItem menuSettings;
        private System.Windows.Forms.ToolStripMenuItem menuLanguage;
        private System.Windows.Forms.ToolStripMenuItem menuLanguageSpanish;
        private System.Windows.Forms.ToolStripMenuItem menuLanguageEnglish;
        private System.Windows.Forms.ToolStripMenuItem menuHelp;
        private System.Windows.Forms.ToolStripMenuItem menuAbout;
        private System.Windows.Forms.StatusStrip statusStrip;
        private System.Windows.Forms.ToolStripStatusLabel statusLabel;
    }
}

