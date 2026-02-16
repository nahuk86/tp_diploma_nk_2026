namespace UI.Forms
{
    partial class ReportsForm
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabTopProducts = new System.Windows.Forms.TabPage();
            this.tabClientPurchases = new System.Windows.Forms.TabPage();
            this.tabPriceVariation = new System.Windows.Forms.TabPage();
            this.tabSellerPerformance = new System.Windows.Forms.TabPage();
            this.tabCategorySales = new System.Windows.Forms.TabPage();
            this.tabClientProductRanking = new System.Windows.Forms.TabPage();
            
            this.tabControl1.SuspendLayout();
            this.tabTopProducts.SuspendLayout();
            this.tabClientPurchases.SuspendLayout();
            this.tabPriceVariation.SuspendLayout();
            this.tabSellerPerformance.SuspendLayout();
            this.tabCategorySales.SuspendLayout();
            this.tabClientProductRanking.SuspendLayout();
            this.SuspendLayout();
            
            // Initialize all tabs with essential controls
            InitializeTopProductsTab();
            InitializeClientPurchasesTab();
            InitializePriceVariationTab();
            InitializeSellerPerformanceTab();
            InitializeCategorySalesTab();
            InitializeClientProductRankingTab();
            
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabTopProducts);
            this.tabControl1.Controls.Add(this.tabClientPurchases);
            this.tabControl1.Controls.Add(this.tabPriceVariation);
            this.tabControl1.Controls.Add(this.tabSellerPerformance);
            this.tabControl1.Controls.Add(this.tabCategorySales);
            this.tabControl1.Controls.Add(this.tabClientProductRanking);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(1200, 700);
            this.tabControl1.TabIndex = 0;
            
            // 
            // tabTopProducts
            //
            this.tabTopProducts.Location = new System.Drawing.Point(4, 22);
            this.tabTopProducts.Name = "tabTopProducts";
            this.tabTopProducts.Padding = new System.Windows.Forms.Padding(3);
            this.tabTopProducts.Size = new System.Drawing.Size(1192, 674);
            this.tabTopProducts.TabIndex = 0;
            this.tabTopProducts.Text = "Productos Más Vendidos";
            this.tabTopProducts.UseVisualStyleBackColor = true;
            
            // 
            // ReportsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1200, 700);
            this.Controls.Add(this.tabControl1);
            this.Name = "ReportsForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Reportes";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.tabControl1.ResumeLayout(false);
            this.tabTopProducts.ResumeLayout(false);
            this.tabClientPurchases.ResumeLayout(false);
            this.tabPriceVariation.ResumeLayout(false);
            this.tabSellerPerformance.ResumeLayout(false);
            this.tabCategorySales.ResumeLayout(false);
            this.tabClientProductRanking.ResumeLayout(false);
            this.ResumeLayout(false);
        }

        private void InitializeTopProductsTab()
        {
            // Initialize controls
            this.pnlTopProductsFilters = new System.Windows.Forms.Panel();
            this.lblTopProductsDateRange = new System.Windows.Forms.Label();
            this.dtpTopProductsStart = new System.Windows.Forms.DateTimePicker();
            this.dtpTopProductsEnd = new System.Windows.Forms.DateTimePicker();
            this.lblTopProductsCategory = new System.Windows.Forms.Label();
            this.cboTopProductsCategory = new System.Windows.Forms.ComboBox();
            this.chkTopProductsLimit = new System.Windows.Forms.CheckBox();
            this.nudTopProductsLimit = new System.Windows.Forms.NumericUpDown();
            this.lblTopProductsOrderBy = new System.Windows.Forms.Label();
            this.cboTopProductsOrderBy = new System.Windows.Forms.ComboBox();
            this.btnGenerateTopProducts = new System.Windows.Forms.Button();
            this.btnExportTopProducts = new System.Windows.Forms.Button();
            this.dgvTopProducts = new System.Windows.Forms.DataGridView();
            
            ((System.ComponentModel.ISupportInitialize)(this.nudTopProductsLimit)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvTopProducts)).BeginInit();
            
            // 
            // pnlTopProductsFilters
            // 
            this.pnlTopProductsFilters.Controls.Add(this.lblTopProductsDateRange);
            this.pnlTopProductsFilters.Controls.Add(this.dtpTopProductsStart);
            this.pnlTopProductsFilters.Controls.Add(this.dtpTopProductsEnd);
            this.pnlTopProductsFilters.Controls.Add(this.lblTopProductsCategory);
            this.pnlTopProductsFilters.Controls.Add(this.cboTopProductsCategory);
            this.pnlTopProductsFilters.Controls.Add(this.chkTopProductsLimit);
            this.pnlTopProductsFilters.Controls.Add(this.nudTopProductsLimit);
            this.pnlTopProductsFilters.Controls.Add(this.lblTopProductsOrderBy);
            this.pnlTopProductsFilters.Controls.Add(this.cboTopProductsOrderBy);
            this.pnlTopProductsFilters.Controls.Add(this.btnGenerateTopProducts);
            this.pnlTopProductsFilters.Controls.Add(this.btnExportTopProducts);
            this.pnlTopProductsFilters.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlTopProductsFilters.Location = new System.Drawing.Point(3, 3);
            this.pnlTopProductsFilters.Name = "pnlTopProductsFilters";
            this.pnlTopProductsFilters.Size = new System.Drawing.Size(1186, 100);
            this.pnlTopProductsFilters.TabIndex = 0;
            
            // 
            // lblTopProductsDateRange
            // 
            this.lblTopProductsDateRange.AutoSize = true;
            this.lblTopProductsDateRange.Location = new System.Drawing.Point(10, 15);
            this.lblTopProductsDateRange.Name = "lblTopProductsDateRange";
            this.lblTopProductsDateRange.Size = new System.Drawing.Size(100, 13);
            this.lblTopProductsDateRange.TabIndex = 0;
            this.lblTopProductsDateRange.Text = "Rango de Fechas:";
            
            // 
            // dtpTopProductsStart
            // 
            this.dtpTopProductsStart.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpTopProductsStart.Location = new System.Drawing.Point(120, 12);
            this.dtpTopProductsStart.Name = "dtpTopProductsStart";
            this.dtpTopProductsStart.Size = new System.Drawing.Size(100, 20);
            this.dtpTopProductsStart.TabIndex = 1;
            
            // 
            // dtpTopProductsEnd
            // 
            this.dtpTopProductsEnd.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpTopProductsEnd.Location = new System.Drawing.Point(230, 12);
            this.dtpTopProductsEnd.Name = "dtpTopProductsEnd";
            this.dtpTopProductsEnd.Size = new System.Drawing.Size(100, 20);
            this.dtpTopProductsEnd.TabIndex = 2;
            
            // 
            // lblTopProductsCategory
            // 
            this.lblTopProductsCategory.AutoSize = true;
            this.lblTopProductsCategory.Location = new System.Drawing.Point(350, 15);
            this.lblTopProductsCategory.Name = "lblTopProductsCategory";
            this.lblTopProductsCategory.Size = new System.Drawing.Size(60, 13);
            this.lblTopProductsCategory.TabIndex = 3;
            this.lblTopProductsCategory.Text = "Categoría:";
            
            // 
            // cboTopProductsCategory
            // 
            this.cboTopProductsCategory.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboTopProductsCategory.FormattingEnabled = true;
            this.cboTopProductsCategory.Location = new System.Drawing.Point(420, 12);
            this.cboTopProductsCategory.Name = "cboTopProductsCategory";
            this.cboTopProductsCategory.Size = new System.Drawing.Size(200, 21);
            this.cboTopProductsCategory.TabIndex = 4;
            
            // 
            // chkTopProductsLimit
            // 
            this.chkTopProductsLimit.AutoSize = true;
            this.chkTopProductsLimit.Location = new System.Drawing.Point(10, 45);
            this.chkTopProductsLimit.Name = "chkTopProductsLimit";
            this.chkTopProductsLimit.Size = new System.Drawing.Size(100, 17);
            this.chkTopProductsLimit.TabIndex = 5;
            this.chkTopProductsLimit.Text = "Limitar a Top:";
            this.chkTopProductsLimit.UseVisualStyleBackColor = true;
            
            // 
            // nudTopProductsLimit
            // 
            this.nudTopProductsLimit.Location = new System.Drawing.Point(120, 43);
            this.nudTopProductsLimit.Maximum = new decimal(new int[] { 1000, 0, 0, 0 });
            this.nudTopProductsLimit.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
            this.nudTopProductsLimit.Name = "nudTopProductsLimit";
            this.nudTopProductsLimit.Size = new System.Drawing.Size(80, 20);
            this.nudTopProductsLimit.TabIndex = 6;
            this.nudTopProductsLimit.Value = new decimal(new int[] { 10, 0, 0, 0 });
            
            // 
            // lblTopProductsOrderBy
            // 
            this.lblTopProductsOrderBy.AutoSize = true;
            this.lblTopProductsOrderBy.Location = new System.Drawing.Point(220, 47);
            this.lblTopProductsOrderBy.Name = "lblTopProductsOrderBy";
            this.lblTopProductsOrderBy.Size = new System.Drawing.Size(70, 13);
            this.lblTopProductsOrderBy.TabIndex = 7;
            this.lblTopProductsOrderBy.Text = "Ordenar por:";
            
            // 
            // cboTopProductsOrderBy
            // 
            this.cboTopProductsOrderBy.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboTopProductsOrderBy.FormattingEnabled = true;
            this.cboTopProductsOrderBy.Items.AddRange(new object[] { "Unidades", "Ingresos" });
            this.cboTopProductsOrderBy.Location = new System.Drawing.Point(300, 44);
            this.cboTopProductsOrderBy.Name = "cboTopProductsOrderBy";
            this.cboTopProductsOrderBy.Size = new System.Drawing.Size(120, 21);
            this.cboTopProductsOrderBy.TabIndex = 8;
            this.cboTopProductsOrderBy.SelectedIndex = 0;
            
            // 
            // btnGenerateTopProducts
            // 
            this.btnGenerateTopProducts.Location = new System.Drawing.Point(450, 42);
            this.btnGenerateTopProducts.Name = "btnGenerateTopProducts";
            this.btnGenerateTopProducts.Size = new System.Drawing.Size(80, 25);
            this.btnGenerateTopProducts.TabIndex = 9;
            this.btnGenerateTopProducts.Text = "Generar";
            this.btnGenerateTopProducts.UseVisualStyleBackColor = true;
            this.btnGenerateTopProducts.Click += new System.EventHandler(this.btnGenerateTopProducts_Click);
            
            // 
            // btnExportTopProducts
            // 
            this.btnExportTopProducts.Location = new System.Drawing.Point(540, 42);
            this.btnExportTopProducts.Name = "btnExportTopProducts";
            this.btnExportTopProducts.Size = new System.Drawing.Size(100, 25);
            this.btnExportTopProducts.TabIndex = 10;
            this.btnExportTopProducts.Text = "Exportar CSV";
            this.btnExportTopProducts.UseVisualStyleBackColor = true;
            this.btnExportTopProducts.Click += new System.EventHandler(this.btnExportTopProducts_Click);
            
            // 
            // dgvTopProducts
            // 
            this.dgvTopProducts.AllowUserToAddRows = false;
            this.dgvTopProducts.AllowUserToDeleteRows = false;
            this.dgvTopProducts.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvTopProducts.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvTopProducts.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvTopProducts.Location = new System.Drawing.Point(3, 103);
            this.dgvTopProducts.Name = "dgvTopProducts";
            this.dgvTopProducts.ReadOnly = true;
            this.dgvTopProducts.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvTopProducts.Size = new System.Drawing.Size(1186, 568);
            this.dgvTopProducts.TabIndex = 11;
            
            // Add controls to tab
            this.tabTopProducts.Controls.Add(this.dgvTopProducts);
            this.tabTopProducts.Controls.Add(this.pnlTopProductsFilters);
            
            ((System.ComponentModel.ISupportInitialize)(this.nudTopProductsLimit)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvTopProducts)).EndInit();
        }

        private void InitializeClientPurchasesTab()
        {
            // Initialize controls
            this.pnlClientPurchasesFilters = new System.Windows.Forms.Panel();
            this.lblClientPurchasesDateRange = new System.Windows.Forms.Label();
            this.dtpClientPurchasesStart = new System.Windows.Forms.DateTimePicker();
            this.dtpClientPurchasesEnd = new System.Windows.Forms.DateTimePicker();
            this.lblClientPurchasesClient = new System.Windows.Forms.Label();
            this.cboClientPurchasesClient = new System.Windows.Forms.ComboBox();
            this.chkClientPurchasesLimit = new System.Windows.Forms.CheckBox();
            this.nudClientPurchasesLimit = new System.Windows.Forms.NumericUpDown();
            this.btnGenerateClientPurchases = new System.Windows.Forms.Button();
            this.btnExportClientPurchases = new System.Windows.Forms.Button();
            this.dgvClientPurchases = new System.Windows.Forms.DataGridView();
            
            ((System.ComponentModel.ISupportInitialize)(this.nudClientPurchasesLimit)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvClientPurchases)).BeginInit();
            
            // pnlClientPurchasesFilters
            this.pnlClientPurchasesFilters.Controls.Add(this.lblClientPurchasesDateRange);
            this.pnlClientPurchasesFilters.Controls.Add(this.dtpClientPurchasesStart);
            this.pnlClientPurchasesFilters.Controls.Add(this.dtpClientPurchasesEnd);
            this.pnlClientPurchasesFilters.Controls.Add(this.lblClientPurchasesClient);
            this.pnlClientPurchasesFilters.Controls.Add(this.cboClientPurchasesClient);
            this.pnlClientPurchasesFilters.Controls.Add(this.chkClientPurchasesLimit);
            this.pnlClientPurchasesFilters.Controls.Add(this.nudClientPurchasesLimit);
            this.pnlClientPurchasesFilters.Controls.Add(this.btnGenerateClientPurchases);
            this.pnlClientPurchasesFilters.Controls.Add(this.btnExportClientPurchases);
            this.pnlClientPurchasesFilters.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlClientPurchasesFilters.Location = new System.Drawing.Point(3, 3);
            this.pnlClientPurchasesFilters.Name = "pnlClientPurchasesFilters";
            this.pnlClientPurchasesFilters.Size = new System.Drawing.Size(1186, 80);
            this.pnlClientPurchasesFilters.TabIndex = 0;
            
            // lblClientPurchasesDateRange
            this.lblClientPurchasesDateRange.AutoSize = true;
            this.lblClientPurchasesDateRange.Location = new System.Drawing.Point(10, 15);
            this.lblClientPurchasesDateRange.Name = "lblClientPurchasesDateRange";
            this.lblClientPurchasesDateRange.Size = new System.Drawing.Size(100, 13);
            this.lblClientPurchasesDateRange.TabIndex = 0;
            this.lblClientPurchasesDateRange.Text = "Rango de Fechas:";
            
            // dtpClientPurchasesStart
            this.dtpClientPurchasesStart.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpClientPurchasesStart.Location = new System.Drawing.Point(120, 12);
            this.dtpClientPurchasesStart.Name = "dtpClientPurchasesStart";
            this.dtpClientPurchasesStart.Size = new System.Drawing.Size(100, 20);
            this.dtpClientPurchasesStart.TabIndex = 1;
            
            // dtpClientPurchasesEnd
            this.dtpClientPurchasesEnd.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpClientPurchasesEnd.Location = new System.Drawing.Point(230, 12);
            this.dtpClientPurchasesEnd.Name = "dtpClientPurchasesEnd";
            this.dtpClientPurchasesEnd.Size = new System.Drawing.Size(100, 20);
            this.dtpClientPurchasesEnd.TabIndex = 2;
            
            // lblClientPurchasesClient
            this.lblClientPurchasesClient.AutoSize = true;
            this.lblClientPurchasesClient.Location = new System.Drawing.Point(350, 15);
            this.lblClientPurchasesClient.Name = "lblClientPurchasesClient";
            this.lblClientPurchasesClient.Size = new System.Drawing.Size(45, 13);
            this.lblClientPurchasesClient.TabIndex = 3;
            this.lblClientPurchasesClient.Text = "Cliente:";
            
            // cboClientPurchasesClient
            this.cboClientPurchasesClient.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboClientPurchasesClient.FormattingEnabled = true;
            this.cboClientPurchasesClient.Location = new System.Drawing.Point(405, 12);
            this.cboClientPurchasesClient.Name = "cboClientPurchasesClient";
            this.cboClientPurchasesClient.Size = new System.Drawing.Size(250, 21);
            this.cboClientPurchasesClient.TabIndex = 4;
            
            // chkClientPurchasesLimit
            this.chkClientPurchasesLimit.AutoSize = true;
            this.chkClientPurchasesLimit.Location = new System.Drawing.Point(10, 45);
            this.chkClientPurchasesLimit.Name = "chkClientPurchasesLimit";
            this.chkClientPurchasesLimit.Size = new System.Drawing.Size(100, 17);
            this.chkClientPurchasesLimit.TabIndex = 5;
            this.chkClientPurchasesLimit.Text = "Limitar a Top:";
            this.chkClientPurchasesLimit.UseVisualStyleBackColor = true;
            
            // nudClientPurchasesLimit
            this.nudClientPurchasesLimit.Location = new System.Drawing.Point(120, 43);
            this.nudClientPurchasesLimit.Maximum = new decimal(new int[] { 1000, 0, 0, 0 });
            this.nudClientPurchasesLimit.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
            this.nudClientPurchasesLimit.Name = "nudClientPurchasesLimit";
            this.nudClientPurchasesLimit.Size = new System.Drawing.Size(80, 20);
            this.nudClientPurchasesLimit.TabIndex = 6;
            this.nudClientPurchasesLimit.Value = new decimal(new int[] { 10, 0, 0, 0 });
            
            // btnGenerateClientPurchases
            this.btnGenerateClientPurchases.Location = new System.Drawing.Point(220, 42);
            this.btnGenerateClientPurchases.Name = "btnGenerateClientPurchases";
            this.btnGenerateClientPurchases.Size = new System.Drawing.Size(80, 25);
            this.btnGenerateClientPurchases.TabIndex = 7;
            this.btnGenerateClientPurchases.Text = "Generar";
            this.btnGenerateClientPurchases.UseVisualStyleBackColor = true;
            this.btnGenerateClientPurchases.Click += new System.EventHandler(this.btnGenerateClientPurchases_Click);
            
            // btnExportClientPurchases
            this.btnExportClientPurchases.Location = new System.Drawing.Point(310, 42);
            this.btnExportClientPurchases.Name = "btnExportClientPurchases";
            this.btnExportClientPurchases.Size = new System.Drawing.Size(100, 25);
            this.btnExportClientPurchases.TabIndex = 8;
            this.btnExportClientPurchases.Text = "Exportar CSV";
            this.btnExportClientPurchases.UseVisualStyleBackColor = true;
            this.btnExportClientPurchases.Click += new System.EventHandler(this.btnExportClientPurchases_Click);
            
            // dgvClientPurchases
            this.dgvClientPurchases.AllowUserToAddRows = false;
            this.dgvClientPurchases.AllowUserToDeleteRows = false;
            this.dgvClientPurchases.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvClientPurchases.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvClientPurchases.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvClientPurchases.Location = new System.Drawing.Point(3, 83);
            this.dgvClientPurchases.Name = "dgvClientPurchases";
            this.dgvClientPurchases.ReadOnly = true;
            this.dgvClientPurchases.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvClientPurchases.Size = new System.Drawing.Size(1186, 588);
            this.dgvClientPurchases.TabIndex = 9;
            
            // tabClientPurchases
            this.tabClientPurchases.Controls.Add(this.dgvClientPurchases);
            this.tabClientPurchases.Controls.Add(this.pnlClientPurchasesFilters);
            this.tabClientPurchases.Location = new System.Drawing.Point(4, 22);
            this.tabClientPurchases.Name = "tabClientPurchases";
            this.tabClientPurchases.Padding = new System.Windows.Forms.Padding(3);
            this.tabClientPurchases.Size = new System.Drawing.Size(1192, 674);
            this.tabClientPurchases.TabIndex = 1;
            this.tabClientPurchases.Text = "Compras por Cliente";
            this.tabClientPurchases.UseVisualStyleBackColor = true;
            
            ((System.ComponentModel.ISupportInitialize)(this.nudClientPurchasesLimit)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvClientPurchases)).EndInit();
        }

        private void InitializePriceVariationTab()
        {
            // Initialize controls
            this.pnlPriceVariationFilters = new System.Windows.Forms.Panel();
            this.lblPriceVariationDateRange = new System.Windows.Forms.Label();
            this.dtpPriceVariationStart = new System.Windows.Forms.DateTimePicker();
            this.dtpPriceVariationEnd = new System.Windows.Forms.DateTimePicker();
            this.lblPriceVariationProduct = new System.Windows.Forms.Label();
            this.cboPriceVariationProduct = new System.Windows.Forms.ComboBox();
            this.lblPriceVariationCategory = new System.Windows.Forms.Label();
            this.cboPriceVariationCategory = new System.Windows.Forms.ComboBox();
            this.btnGeneratePriceVariation = new System.Windows.Forms.Button();
            this.btnExportPriceVariation = new System.Windows.Forms.Button();
            this.dgvPriceVariation = new System.Windows.Forms.DataGridView();
            
            ((System.ComponentModel.ISupportInitialize)(this.dgvPriceVariation)).BeginInit();
            
            // pnlPriceVariationFilters
            this.pnlPriceVariationFilters.Controls.Add(this.lblPriceVariationDateRange);
            this.pnlPriceVariationFilters.Controls.Add(this.dtpPriceVariationStart);
            this.pnlPriceVariationFilters.Controls.Add(this.dtpPriceVariationEnd);
            this.pnlPriceVariationFilters.Controls.Add(this.lblPriceVariationProduct);
            this.pnlPriceVariationFilters.Controls.Add(this.cboPriceVariationProduct);
            this.pnlPriceVariationFilters.Controls.Add(this.lblPriceVariationCategory);
            this.pnlPriceVariationFilters.Controls.Add(this.cboPriceVariationCategory);
            this.pnlPriceVariationFilters.Controls.Add(this.btnGeneratePriceVariation);
            this.pnlPriceVariationFilters.Controls.Add(this.btnExportPriceVariation);
            this.pnlPriceVariationFilters.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlPriceVariationFilters.Location = new System.Drawing.Point(3, 3);
            this.pnlPriceVariationFilters.Name = "pnlPriceVariationFilters";
            this.pnlPriceVariationFilters.Size = new System.Drawing.Size(1186, 80);
            this.pnlPriceVariationFilters.TabIndex = 0;
            
            // lblPriceVariationDateRange
            this.lblPriceVariationDateRange.AutoSize = true;
            this.lblPriceVariationDateRange.Location = new System.Drawing.Point(10, 15);
            this.lblPriceVariationDateRange.Name = "lblPriceVariationDateRange";
            this.lblPriceVariationDateRange.Size = new System.Drawing.Size(100, 13);
            this.lblPriceVariationDateRange.TabIndex = 0;
            this.lblPriceVariationDateRange.Text = "Rango de Fechas:";
            
            // dtpPriceVariationStart
            this.dtpPriceVariationStart.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpPriceVariationStart.Location = new System.Drawing.Point(120, 12);
            this.dtpPriceVariationStart.Name = "dtpPriceVariationStart";
            this.dtpPriceVariationStart.Size = new System.Drawing.Size(100, 20);
            this.dtpPriceVariationStart.TabIndex = 1;
            
            // dtpPriceVariationEnd
            this.dtpPriceVariationEnd.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpPriceVariationEnd.Location = new System.Drawing.Point(230, 12);
            this.dtpPriceVariationEnd.Name = "dtpPriceVariationEnd";
            this.dtpPriceVariationEnd.Size = new System.Drawing.Size(100, 20);
            this.dtpPriceVariationEnd.TabIndex = 2;
            
            // lblPriceVariationProduct
            this.lblPriceVariationProduct.AutoSize = true;
            this.lblPriceVariationProduct.Location = new System.Drawing.Point(350, 15);
            this.lblPriceVariationProduct.Name = "lblPriceVariationProduct";
            this.lblPriceVariationProduct.Size = new System.Drawing.Size(53, 13);
            this.lblPriceVariationProduct.TabIndex = 3;
            this.lblPriceVariationProduct.Text = "Producto:";
            
            // cboPriceVariationProduct
            this.cboPriceVariationProduct.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboPriceVariationProduct.FormattingEnabled = true;
            this.cboPriceVariationProduct.Location = new System.Drawing.Point(410, 12);
            this.cboPriceVariationProduct.Name = "cboPriceVariationProduct";
            this.cboPriceVariationProduct.Size = new System.Drawing.Size(250, 21);
            this.cboPriceVariationProduct.TabIndex = 4;
            
            // lblPriceVariationCategory
            this.lblPriceVariationCategory.AutoSize = true;
            this.lblPriceVariationCategory.Location = new System.Drawing.Point(10, 47);
            this.lblPriceVariationCategory.Name = "lblPriceVariationCategory";
            this.lblPriceVariationCategory.Size = new System.Drawing.Size(60, 13);
            this.lblPriceVariationCategory.TabIndex = 5;
            this.lblPriceVariationCategory.Text = "Categoría:";
            
            // cboPriceVariationCategory
            this.cboPriceVariationCategory.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboPriceVariationCategory.FormattingEnabled = true;
            this.cboPriceVariationCategory.Location = new System.Drawing.Point(120, 44);
            this.cboPriceVariationCategory.Name = "cboPriceVariationCategory";
            this.cboPriceVariationCategory.Size = new System.Drawing.Size(200, 21);
            this.cboPriceVariationCategory.TabIndex = 6;
            
            // btnGeneratePriceVariation
            this.btnGeneratePriceVariation.Location = new System.Drawing.Point(340, 42);
            this.btnGeneratePriceVariation.Name = "btnGeneratePriceVariation";
            this.btnGeneratePriceVariation.Size = new System.Drawing.Size(80, 25);
            this.btnGeneratePriceVariation.TabIndex = 7;
            this.btnGeneratePriceVariation.Text = "Generar";
            this.btnGeneratePriceVariation.UseVisualStyleBackColor = true;
            this.btnGeneratePriceVariation.Click += new System.EventHandler(this.btnGeneratePriceVariation_Click);
            
            // btnExportPriceVariation
            this.btnExportPriceVariation.Location = new System.Drawing.Point(430, 42);
            this.btnExportPriceVariation.Name = "btnExportPriceVariation";
            this.btnExportPriceVariation.Size = new System.Drawing.Size(100, 25);
            this.btnExportPriceVariation.TabIndex = 8;
            this.btnExportPriceVariation.Text = "Exportar CSV";
            this.btnExportPriceVariation.UseVisualStyleBackColor = true;
            this.btnExportPriceVariation.Click += new System.EventHandler(this.btnExportPriceVariation_Click);
            
            // dgvPriceVariation
            this.dgvPriceVariation.AllowUserToAddRows = false;
            this.dgvPriceVariation.AllowUserToDeleteRows = false;
            this.dgvPriceVariation.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvPriceVariation.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvPriceVariation.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvPriceVariation.Location = new System.Drawing.Point(3, 83);
            this.dgvPriceVariation.Name = "dgvPriceVariation";
            this.dgvPriceVariation.ReadOnly = true;
            this.dgvPriceVariation.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvPriceVariation.Size = new System.Drawing.Size(1186, 588);
            this.dgvPriceVariation.TabIndex = 9;
            
            // tabPriceVariation
            this.tabPriceVariation.Controls.Add(this.dgvPriceVariation);
            this.tabPriceVariation.Controls.Add(this.pnlPriceVariationFilters);
            this.tabPriceVariation.Location = new System.Drawing.Point(4, 22);
            this.tabPriceVariation.Name = "tabPriceVariation";
            this.tabPriceVariation.Padding = new System.Windows.Forms.Padding(3);
            this.tabPriceVariation.Size = new System.Drawing.Size(1192, 674);
            this.tabPriceVariation.TabIndex = 2;
            this.tabPriceVariation.Text = "Variación de Precios";
            this.tabPriceVariation.UseVisualStyleBackColor = true;
            
            ((System.ComponentModel.ISupportInitialize)(this.dgvPriceVariation)).EndInit();
        }


        private void InitializeSellerPerformanceTab()
        {
            // Initialize controls
            this.pnlSellerPerformanceFilters = new System.Windows.Forms.Panel();
            this.lblSellerPerformanceDateRange = new System.Windows.Forms.Label();
            this.dtpSellerPerformanceStart = new System.Windows.Forms.DateTimePicker();
            this.dtpSellerPerformanceEnd = new System.Windows.Forms.DateTimePicker();
            this.lblSellerPerformanceSeller = new System.Windows.Forms.Label();
            this.txtSellerPerformanceSeller = new System.Windows.Forms.TextBox();
            this.lblSellerPerformanceCategory = new System.Windows.Forms.Label();
            this.cboSellerPerformanceCategory = new System.Windows.Forms.ComboBox();
            this.btnGenerateSellerPerformance = new System.Windows.Forms.Button();
            this.btnExportSellerPerformance = new System.Windows.Forms.Button();
            this.dgvSellerPerformance = new System.Windows.Forms.DataGridView();
            
            ((System.ComponentModel.ISupportInitialize)(this.dgvSellerPerformance)).BeginInit();
            
            // pnlSellerPerformanceFilters
            this.pnlSellerPerformanceFilters.Controls.Add(this.lblSellerPerformanceDateRange);
            this.pnlSellerPerformanceFilters.Controls.Add(this.dtpSellerPerformanceStart);
            this.pnlSellerPerformanceFilters.Controls.Add(this.dtpSellerPerformanceEnd);
            this.pnlSellerPerformanceFilters.Controls.Add(this.lblSellerPerformanceSeller);
            this.pnlSellerPerformanceFilters.Controls.Add(this.txtSellerPerformanceSeller);
            this.pnlSellerPerformanceFilters.Controls.Add(this.lblSellerPerformanceCategory);
            this.pnlSellerPerformanceFilters.Controls.Add(this.cboSellerPerformanceCategory);
            this.pnlSellerPerformanceFilters.Controls.Add(this.btnGenerateSellerPerformance);
            this.pnlSellerPerformanceFilters.Controls.Add(this.btnExportSellerPerformance);
            this.pnlSellerPerformanceFilters.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlSellerPerformanceFilters.Location = new System.Drawing.Point(3, 3);
            this.pnlSellerPerformanceFilters.Name = "pnlSellerPerformanceFilters";
            this.pnlSellerPerformanceFilters.Size = new System.Drawing.Size(1186, 80);
            this.pnlSellerPerformanceFilters.TabIndex = 0;
            
            // lblSellerPerformanceDateRange
            this.lblSellerPerformanceDateRange.AutoSize = true;
            this.lblSellerPerformanceDateRange.Location = new System.Drawing.Point(10, 15);
            this.lblSellerPerformanceDateRange.Name = "lblSellerPerformanceDateRange";
            this.lblSellerPerformanceDateRange.Size = new System.Drawing.Size(100, 13);
            this.lblSellerPerformanceDateRange.TabIndex = 0;
            this.lblSellerPerformanceDateRange.Text = "Rango de Fechas:";
            
            // dtpSellerPerformanceStart
            this.dtpSellerPerformanceStart.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpSellerPerformanceStart.Location = new System.Drawing.Point(120, 12);
            this.dtpSellerPerformanceStart.Name = "dtpSellerPerformanceStart";
            this.dtpSellerPerformanceStart.Size = new System.Drawing.Size(100, 20);
            this.dtpSellerPerformanceStart.TabIndex = 1;
            
            // dtpSellerPerformanceEnd
            this.dtpSellerPerformanceEnd.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpSellerPerformanceEnd.Location = new System.Drawing.Point(230, 12);
            this.dtpSellerPerformanceEnd.Name = "dtpSellerPerformanceEnd";
            this.dtpSellerPerformanceEnd.Size = new System.Drawing.Size(100, 20);
            this.dtpSellerPerformanceEnd.TabIndex = 2;
            
            // lblSellerPerformanceSeller
            this.lblSellerPerformanceSeller.AutoSize = true;
            this.lblSellerPerformanceSeller.Location = new System.Drawing.Point(350, 15);
            this.lblSellerPerformanceSeller.Name = "lblSellerPerformanceSeller";
            this.lblSellerPerformanceSeller.Size = new System.Drawing.Size(100, 13);
            this.lblSellerPerformanceSeller.TabIndex = 3;
            this.lblSellerPerformanceSeller.Text = "Vendedor:";
            
            // txtSellerPerformanceSeller
            this.txtSellerPerformanceSeller.Location = new System.Drawing.Point(415, 12);
            this.txtSellerPerformanceSeller.Name = "txtSellerPerformanceSeller";
            this.txtSellerPerformanceSeller.Size = new System.Drawing.Size(200, 20);
            this.txtSellerPerformanceSeller.TabIndex = 4;
            
            // lblSellerPerformanceCategory
            this.lblSellerPerformanceCategory.AutoSize = true;
            this.lblSellerPerformanceCategory.Location = new System.Drawing.Point(10, 47);
            this.lblSellerPerformanceCategory.Name = "lblSellerPerformanceCategory";
            this.lblSellerPerformanceCategory.Size = new System.Drawing.Size(100, 13);
            this.lblSellerPerformanceCategory.TabIndex = 5;
            this.lblSellerPerformanceCategory.Text = "Categoría:";
            
            // cboSellerPerformanceCategory
            this.cboSellerPerformanceCategory.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboSellerPerformanceCategory.FormattingEnabled = true;
            this.cboSellerPerformanceCategory.Location = new System.Drawing.Point(120, 44);
            this.cboSellerPerformanceCategory.Name = "cboSellerPerformanceCategory";
            this.cboSellerPerformanceCategory.Size = new System.Drawing.Size(200, 21);
            this.cboSellerPerformanceCategory.TabIndex = 6;
            
            // btnGenerateSellerPerformance
            this.btnGenerateSellerPerformance.Location = new System.Drawing.Point(340, 42);
            this.btnGenerateSellerPerformance.Name = "btnGenerateSellerPerformance";
            this.btnGenerateSellerPerformance.Size = new System.Drawing.Size(80, 25);
            this.btnGenerateSellerPerformance.TabIndex = 7;
            this.btnGenerateSellerPerformance.Text = "Generar";
            this.btnGenerateSellerPerformance.UseVisualStyleBackColor = true;
            this.btnGenerateSellerPerformance.Click += new System.EventHandler(this.btnGenerateSellerPerformance_Click);
            
            // btnExportSellerPerformance
            this.btnExportSellerPerformance.Location = new System.Drawing.Point(430, 42);
            this.btnExportSellerPerformance.Name = "btnExportSellerPerformance";
            this.btnExportSellerPerformance.Size = new System.Drawing.Size(100, 25);
            this.btnExportSellerPerformance.TabIndex = 8;
            this.btnExportSellerPerformance.Text = "Exportar CSV";
            this.btnExportSellerPerformance.UseVisualStyleBackColor = true;
            this.btnExportSellerPerformance.Click += new System.EventHandler(this.btnExportSellerPerformance_Click);
            
            // dgvSellerPerformance
            this.dgvSellerPerformance.AllowUserToAddRows = false;
            this.dgvSellerPerformance.AllowUserToDeleteRows = false;
            this.dgvSellerPerformance.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvSellerPerformance.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvSellerPerformance.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvSellerPerformance.Location = new System.Drawing.Point(3, 83);
            this.dgvSellerPerformance.Name = "dgvSellerPerformance";
            this.dgvSellerPerformance.ReadOnly = true;
            this.dgvSellerPerformance.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvSellerPerformance.Size = new System.Drawing.Size(1186, 588);
            this.dgvSellerPerformance.TabIndex = 9;
            
            // tabSellerPerformance
            this.tabSellerPerformance.Controls.Add(this.dgvSellerPerformance);
            this.tabSellerPerformance.Controls.Add(this.pnlSellerPerformanceFilters);
            this.tabSellerPerformance.Location = new System.Drawing.Point(4, 22);
            this.tabSellerPerformance.Name = "tabSellerPerformance";
            this.tabSellerPerformance.Padding = new System.Windows.Forms.Padding(3);
            this.tabSellerPerformance.Size = new System.Drawing.Size(1192, 674);
            this.tabSellerPerformance.TabIndex = 3;
            this.tabSellerPerformance.Text = "Ventas por Vendedor";
            this.tabSellerPerformance.UseVisualStyleBackColor = true;
            
            ((System.ComponentModel.ISupportInitialize)(this.dgvSellerPerformance)).EndInit();
        }

        private void InitializeCategorySalesTab()
        {
            // Initialize controls
            this.pnlCategorySalesFilters = new System.Windows.Forms.Panel();
            this.lblCategorySalesDateRange = new System.Windows.Forms.Label();
            this.dtpCategorySalesStart = new System.Windows.Forms.DateTimePicker();
            this.dtpCategorySalesEnd = new System.Windows.Forms.DateTimePicker();
            this.lblCategorySalesCategory = new System.Windows.Forms.Label();
            this.cboCategorySalesCategory = new System.Windows.Forms.ComboBox();
            this.btnGenerateCategorySales = new System.Windows.Forms.Button();
            this.btnExportCategorySales = new System.Windows.Forms.Button();
            this.dgvCategorySales = new System.Windows.Forms.DataGridView();
            
            ((System.ComponentModel.ISupportInitialize)(this.dgvCategorySales)).BeginInit();
            
            // pnlCategorySalesFilters
            this.pnlCategorySalesFilters.Controls.Add(this.lblCategorySalesDateRange);
            this.pnlCategorySalesFilters.Controls.Add(this.dtpCategorySalesStart);
            this.pnlCategorySalesFilters.Controls.Add(this.dtpCategorySalesEnd);
            this.pnlCategorySalesFilters.Controls.Add(this.lblCategorySalesCategory);
            this.pnlCategorySalesFilters.Controls.Add(this.cboCategorySalesCategory);
            this.pnlCategorySalesFilters.Controls.Add(this.btnGenerateCategorySales);
            this.pnlCategorySalesFilters.Controls.Add(this.btnExportCategorySales);
            this.pnlCategorySalesFilters.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlCategorySalesFilters.Location = new System.Drawing.Point(3, 3);
            this.pnlCategorySalesFilters.Name = "pnlCategorySalesFilters";
            this.pnlCategorySalesFilters.Size = new System.Drawing.Size(1186, 80);
            this.pnlCategorySalesFilters.TabIndex = 0;
            
            // lblCategorySalesDateRange
            this.lblCategorySalesDateRange.AutoSize = true;
            this.lblCategorySalesDateRange.Location = new System.Drawing.Point(10, 15);
            this.lblCategorySalesDateRange.Name = "lblCategorySalesDateRange";
            this.lblCategorySalesDateRange.Size = new System.Drawing.Size(100, 13);
            this.lblCategorySalesDateRange.TabIndex = 0;
            this.lblCategorySalesDateRange.Text = "Rango de Fechas:";
            
            // dtpCategorySalesStart
            this.dtpCategorySalesStart.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpCategorySalesStart.Location = new System.Drawing.Point(120, 12);
            this.dtpCategorySalesStart.Name = "dtpCategorySalesStart";
            this.dtpCategorySalesStart.Size = new System.Drawing.Size(100, 20);
            this.dtpCategorySalesStart.TabIndex = 1;
            
            // dtpCategorySalesEnd
            this.dtpCategorySalesEnd.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpCategorySalesEnd.Location = new System.Drawing.Point(230, 12);
            this.dtpCategorySalesEnd.Name = "dtpCategorySalesEnd";
            this.dtpCategorySalesEnd.Size = new System.Drawing.Size(100, 20);
            this.dtpCategorySalesEnd.TabIndex = 2;
            
            // lblCategorySalesCategory
            this.lblCategorySalesCategory.AutoSize = true;
            this.lblCategorySalesCategory.Location = new System.Drawing.Point(350, 15);
            this.lblCategorySalesCategory.Name = "lblCategorySalesCategory";
            this.lblCategorySalesCategory.Size = new System.Drawing.Size(100, 13);
            this.lblCategorySalesCategory.TabIndex = 3;
            this.lblCategorySalesCategory.Text = "Categoría:";
            
            // cboCategorySalesCategory
            this.cboCategorySalesCategory.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboCategorySalesCategory.FormattingEnabled = true;
            this.cboCategorySalesCategory.Location = new System.Drawing.Point(420, 12);
            this.cboCategorySalesCategory.Name = "cboCategorySalesCategory";
            this.cboCategorySalesCategory.Size = new System.Drawing.Size(200, 21);
            this.cboCategorySalesCategory.TabIndex = 4;
            
            // btnGenerateCategorySales
            this.btnGenerateCategorySales.Location = new System.Drawing.Point(340, 42);
            this.btnGenerateCategorySales.Name = "btnGenerateCategorySales";
            this.btnGenerateCategorySales.Size = new System.Drawing.Size(80, 25);
            this.btnGenerateCategorySales.TabIndex = 5;
            this.btnGenerateCategorySales.Text = "Generar";
            this.btnGenerateCategorySales.UseVisualStyleBackColor = true;
            this.btnGenerateCategorySales.Click += new System.EventHandler(this.btnGenerateCategorySales_Click);
            
            // btnExportCategorySales
            this.btnExportCategorySales.Location = new System.Drawing.Point(430, 42);
            this.btnExportCategorySales.Name = "btnExportCategorySales";
            this.btnExportCategorySales.Size = new System.Drawing.Size(100, 25);
            this.btnExportCategorySales.TabIndex = 6;
            this.btnExportCategorySales.Text = "Exportar CSV";
            this.btnExportCategorySales.UseVisualStyleBackColor = true;
            this.btnExportCategorySales.Click += new System.EventHandler(this.btnExportCategorySales_Click);
            
            // dgvCategorySales
            this.dgvCategorySales.AllowUserToAddRows = false;
            this.dgvCategorySales.AllowUserToDeleteRows = false;
            this.dgvCategorySales.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvCategorySales.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvCategorySales.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvCategorySales.Location = new System.Drawing.Point(3, 83);
            this.dgvCategorySales.Name = "dgvCategorySales";
            this.dgvCategorySales.ReadOnly = true;
            this.dgvCategorySales.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvCategorySales.Size = new System.Drawing.Size(1186, 588);
            this.dgvCategorySales.TabIndex = 7;
            
            // tabCategorySales
            this.tabCategorySales.Controls.Add(this.dgvCategorySales);
            this.tabCategorySales.Controls.Add(this.pnlCategorySalesFilters);
            this.tabCategorySales.Location = new System.Drawing.Point(4, 22);
            this.tabCategorySales.Name = "tabCategorySales";
            this.tabCategorySales.Padding = new System.Windows.Forms.Padding(3);
            this.tabCategorySales.Size = new System.Drawing.Size(1192, 674);
            this.tabCategorySales.TabIndex = 4;
            this.tabCategorySales.Text = "Ventas por Categoría";
            this.tabCategorySales.UseVisualStyleBackColor = true;
            
            ((System.ComponentModel.ISupportInitialize)(this.dgvCategorySales)).EndInit();
        }


        private void InitializeClientProductRankingTab()
        {
            // Initialize controls
            this.pnlClientProductRankingFilters = new System.Windows.Forms.Panel();
            this.lblClientProductRankingDateRange = new System.Windows.Forms.Label();
            this.dtpClientProductRankingStart = new System.Windows.Forms.DateTimePicker();
            this.dtpClientProductRankingEnd = new System.Windows.Forms.DateTimePicker();
            this.lblClientProductRankingProduct = new System.Windows.Forms.Label();
            this.cboClientProductRankingProduct = new System.Windows.Forms.ComboBox();
            this.lblClientProductRankingCategory = new System.Windows.Forms.Label();
            this.cboClientProductRankingCategory = new System.Windows.Forms.ComboBox();
            this.chkClientProductRankingLimit = new System.Windows.Forms.CheckBox();
            this.nudClientProductRankingLimit = new System.Windows.Forms.NumericUpDown();
            this.btnGenerateClientProductRanking = new System.Windows.Forms.Button();
            this.btnExportClientProductRanking = new System.Windows.Forms.Button();
            this.dgvClientProductRanking = new System.Windows.Forms.DataGridView();
            
            ((System.ComponentModel.ISupportInitialize)(this.dgvClientProductRanking)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudClientProductRankingLimit)).BeginInit();
            
            // pnlClientProductRankingFilters
            this.pnlClientProductRankingFilters.Controls.Add(this.lblClientProductRankingDateRange);
            this.pnlClientProductRankingFilters.Controls.Add(this.dtpClientProductRankingStart);
            this.pnlClientProductRankingFilters.Controls.Add(this.dtpClientProductRankingEnd);
            this.pnlClientProductRankingFilters.Controls.Add(this.lblClientProductRankingProduct);
            this.pnlClientProductRankingFilters.Controls.Add(this.cboClientProductRankingProduct);
            this.pnlClientProductRankingFilters.Controls.Add(this.lblClientProductRankingCategory);
            this.pnlClientProductRankingFilters.Controls.Add(this.cboClientProductRankingCategory);
            this.pnlClientProductRankingFilters.Controls.Add(this.chkClientProductRankingLimit);
            this.pnlClientProductRankingFilters.Controls.Add(this.nudClientProductRankingLimit);
            this.pnlClientProductRankingFilters.Controls.Add(this.btnGenerateClientProductRanking);
            this.pnlClientProductRankingFilters.Controls.Add(this.btnExportClientProductRanking);
            this.pnlClientProductRankingFilters.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlClientProductRankingFilters.Location = new System.Drawing.Point(3, 3);
            this.pnlClientProductRankingFilters.Name = "pnlClientProductRankingFilters";
            this.pnlClientProductRankingFilters.Size = new System.Drawing.Size(1186, 80);
            this.pnlClientProductRankingFilters.TabIndex = 0;
            
            // lblClientProductRankingDateRange
            this.lblClientProductRankingDateRange.AutoSize = true;
            this.lblClientProductRankingDateRange.Location = new System.Drawing.Point(10, 15);
            this.lblClientProductRankingDateRange.Name = "lblClientProductRankingDateRange";
            this.lblClientProductRankingDateRange.Size = new System.Drawing.Size(100, 13);
            this.lblClientProductRankingDateRange.TabIndex = 0;
            this.lblClientProductRankingDateRange.Text = "Rango de Fechas:";
            
            // dtpClientProductRankingStart
            this.dtpClientProductRankingStart.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpClientProductRankingStart.Location = new System.Drawing.Point(120, 12);
            this.dtpClientProductRankingStart.Name = "dtpClientProductRankingStart";
            this.dtpClientProductRankingStart.Size = new System.Drawing.Size(100, 20);
            this.dtpClientProductRankingStart.TabIndex = 1;
            
            // dtpClientProductRankingEnd
            this.dtpClientProductRankingEnd.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpClientProductRankingEnd.Location = new System.Drawing.Point(230, 12);
            this.dtpClientProductRankingEnd.Name = "dtpClientProductRankingEnd";
            this.dtpClientProductRankingEnd.Size = new System.Drawing.Size(100, 20);
            this.dtpClientProductRankingEnd.TabIndex = 2;
            
            // lblClientProductRankingProduct
            this.lblClientProductRankingProduct.AutoSize = true;
            this.lblClientProductRankingProduct.Location = new System.Drawing.Point(350, 15);
            this.lblClientProductRankingProduct.Name = "lblClientProductRankingProduct";
            this.lblClientProductRankingProduct.Size = new System.Drawing.Size(100, 13);
            this.lblClientProductRankingProduct.TabIndex = 3;
            this.lblClientProductRankingProduct.Text = "Producto:";
            
            // cboClientProductRankingProduct
            this.cboClientProductRankingProduct.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboClientProductRankingProduct.FormattingEnabled = true;
            this.cboClientProductRankingProduct.Location = new System.Drawing.Point(410, 12);
            this.cboClientProductRankingProduct.Name = "cboClientProductRankingProduct";
            this.cboClientProductRankingProduct.Size = new System.Drawing.Size(250, 21);
            this.cboClientProductRankingProduct.TabIndex = 4;
            
            // lblClientProductRankingCategory
            this.lblClientProductRankingCategory.AutoSize = true;
            this.lblClientProductRankingCategory.Location = new System.Drawing.Point(10, 47);
            this.lblClientProductRankingCategory.Name = "lblClientProductRankingCategory";
            this.lblClientProductRankingCategory.Size = new System.Drawing.Size(100, 13);
            this.lblClientProductRankingCategory.TabIndex = 5;
            this.lblClientProductRankingCategory.Text = "Categoría:";
            
            // cboClientProductRankingCategory
            this.cboClientProductRankingCategory.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboClientProductRankingCategory.FormattingEnabled = true;
            this.cboClientProductRankingCategory.Location = new System.Drawing.Point(120, 44);
            this.cboClientProductRankingCategory.Name = "cboClientProductRankingCategory";
            this.cboClientProductRankingCategory.Size = new System.Drawing.Size(200, 21);
            this.cboClientProductRankingCategory.TabIndex = 6;
            
            // chkClientProductRankingLimit
            this.chkClientProductRankingLimit.AutoSize = true;
            this.chkClientProductRankingLimit.Location = new System.Drawing.Point(340, 45);
            this.chkClientProductRankingLimit.Name = "chkClientProductRankingLimit";
            this.chkClientProductRankingLimit.Size = new System.Drawing.Size(100, 17);
            this.chkClientProductRankingLimit.TabIndex = 7;
            this.chkClientProductRankingLimit.Text = "Limitar a Top:";
            this.chkClientProductRankingLimit.UseVisualStyleBackColor = true;
            
            // nudClientProductRankingLimit
            this.nudClientProductRankingLimit.Location = new System.Drawing.Point(450, 43);
            this.nudClientProductRankingLimit.Maximum = new decimal(new int[] { 1000, 0, 0, 0 });
            this.nudClientProductRankingLimit.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
            this.nudClientProductRankingLimit.Name = "nudClientProductRankingLimit";
            this.nudClientProductRankingLimit.Size = new System.Drawing.Size(80, 20);
            this.nudClientProductRankingLimit.TabIndex = 8;
            this.nudClientProductRankingLimit.Value = new decimal(new int[] { 10, 0, 0, 0 });
            
            // btnGenerateClientProductRanking
            this.btnGenerateClientProductRanking.Location = new System.Drawing.Point(550, 42);
            this.btnGenerateClientProductRanking.Name = "btnGenerateClientProductRanking";
            this.btnGenerateClientProductRanking.Size = new System.Drawing.Size(80, 25);
            this.btnGenerateClientProductRanking.TabIndex = 9;
            this.btnGenerateClientProductRanking.Text = "Generar";
            this.btnGenerateClientProductRanking.UseVisualStyleBackColor = true;
            this.btnGenerateClientProductRanking.Click += new System.EventHandler(this.btnGenerateClientProductRanking_Click);
            
            // btnExportClientProductRanking
            this.btnExportClientProductRanking.Location = new System.Drawing.Point(640, 42);
            this.btnExportClientProductRanking.Name = "btnExportClientProductRanking";
            this.btnExportClientProductRanking.Size = new System.Drawing.Size(100, 25);
            this.btnExportClientProductRanking.TabIndex = 10;
            this.btnExportClientProductRanking.Text = "Exportar CSV";
            this.btnExportClientProductRanking.UseVisualStyleBackColor = true;
            this.btnExportClientProductRanking.Click += new System.EventHandler(this.btnExportClientProductRanking_Click);
            
            // dgvClientProductRanking
            this.dgvClientProductRanking.AllowUserToAddRows = false;
            this.dgvClientProductRanking.AllowUserToDeleteRows = false;
            this.dgvClientProductRanking.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvClientProductRanking.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvClientProductRanking.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvClientProductRanking.Location = new System.Drawing.Point(3, 83);
            this.dgvClientProductRanking.Name = "dgvClientProductRanking";
            this.dgvClientProductRanking.ReadOnly = true;
            this.dgvClientProductRanking.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvClientProductRanking.Size = new System.Drawing.Size(1186, 588);
            this.dgvClientProductRanking.TabIndex = 11;
            
            // tabClientProductRanking
            this.tabClientProductRanking.Controls.Add(this.dgvClientProductRanking);
            this.tabClientProductRanking.Controls.Add(this.pnlClientProductRankingFilters);
            this.tabClientProductRanking.Location = new System.Drawing.Point(4, 22);
            this.tabClientProductRanking.Name = "tabClientProductRanking";
            this.tabClientProductRanking.Padding = new System.Windows.Forms.Padding(3);
            this.tabClientProductRanking.Size = new System.Drawing.Size(1192, 674);
            this.tabClientProductRanking.TabIndex = 6;
            this.tabClientProductRanking.Text = "Ranking Clientes-Productos";
            this.tabClientProductRanking.UseVisualStyleBackColor = true;
            
            ((System.ComponentModel.ISupportInitialize)(this.nudClientProductRankingLimit)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvClientProductRanking)).EndInit();
        }
        #endregion

        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabTopProducts;
        private System.Windows.Forms.TabPage tabClientPurchases;
        private System.Windows.Forms.TabPage tabPriceVariation;
        private System.Windows.Forms.TabPage tabSellerPerformance;
        private System.Windows.Forms.TabPage tabCategorySales;
        private System.Windows.Forms.TabPage tabClientProductRanking;

        // Tab 1: Top Products
        private System.Windows.Forms.Panel pnlTopProductsFilters;
        private System.Windows.Forms.Label lblTopProductsDateRange;
        private System.Windows.Forms.DateTimePicker dtpTopProductsStart;
        private System.Windows.Forms.DateTimePicker dtpTopProductsEnd;
        private System.Windows.Forms.Label lblTopProductsCategory;
        private System.Windows.Forms.ComboBox cboTopProductsCategory;
        private System.Windows.Forms.CheckBox chkTopProductsLimit;
        private System.Windows.Forms.NumericUpDown nudTopProductsLimit;
        private System.Windows.Forms.Label lblTopProductsOrderBy;
        private System.Windows.Forms.ComboBox cboTopProductsOrderBy;
        private System.Windows.Forms.Button btnGenerateTopProducts;
        private System.Windows.Forms.Button btnExportTopProducts;
        private System.Windows.Forms.DataGridView dgvTopProducts;

        // Tab 2: Client Purchases
        private System.Windows.Forms.Panel pnlClientPurchasesFilters;
        private System.Windows.Forms.Label lblClientPurchasesDateRange;
        private System.Windows.Forms.DateTimePicker dtpClientPurchasesStart;
        private System.Windows.Forms.DateTimePicker dtpClientPurchasesEnd;
        private System.Windows.Forms.Label lblClientPurchasesClient;
        private System.Windows.Forms.ComboBox cboClientPurchasesClient;
        private System.Windows.Forms.CheckBox chkClientPurchasesLimit;
        private System.Windows.Forms.NumericUpDown nudClientPurchasesLimit;
        private System.Windows.Forms.Button btnGenerateClientPurchases;
        private System.Windows.Forms.Button btnExportClientPurchases;
        private System.Windows.Forms.DataGridView dgvClientPurchases;

        // Tab 3: Price Variation
        private System.Windows.Forms.Panel pnlPriceVariationFilters;
        private System.Windows.Forms.Label lblPriceVariationDateRange;
        private System.Windows.Forms.DateTimePicker dtpPriceVariationStart;
        private System.Windows.Forms.DateTimePicker dtpPriceVariationEnd;
        private System.Windows.Forms.Label lblPriceVariationProduct;
        private System.Windows.Forms.ComboBox cboPriceVariationProduct;
        private System.Windows.Forms.Label lblPriceVariationCategory;
        private System.Windows.Forms.ComboBox cboPriceVariationCategory;
        private System.Windows.Forms.Button btnGeneratePriceVariation;
        private System.Windows.Forms.Button btnExportPriceVariation;
        private System.Windows.Forms.DataGridView dgvPriceVariation;

        // Tab 4: Seller Performance
        private System.Windows.Forms.Panel pnlSellerPerformanceFilters;
        private System.Windows.Forms.Label lblSellerPerformanceDateRange;
        private System.Windows.Forms.DateTimePicker dtpSellerPerformanceStart;
        private System.Windows.Forms.DateTimePicker dtpSellerPerformanceEnd;
        private System.Windows.Forms.Label lblSellerPerformanceSeller;
        private System.Windows.Forms.TextBox txtSellerPerformanceSeller;
        private System.Windows.Forms.Label lblSellerPerformanceCategory;
        private System.Windows.Forms.ComboBox cboSellerPerformanceCategory;
        private System.Windows.Forms.Button btnGenerateSellerPerformance;
        private System.Windows.Forms.Button btnExportSellerPerformance;
        private System.Windows.Forms.DataGridView dgvSellerPerformance;

        // Tab 5: Category Sales
        private System.Windows.Forms.Panel pnlCategorySalesFilters;
        private System.Windows.Forms.Label lblCategorySalesDateRange;
        private System.Windows.Forms.DateTimePicker dtpCategorySalesStart;
        private System.Windows.Forms.DateTimePicker dtpCategorySalesEnd;
        private System.Windows.Forms.Label lblCategorySalesCategory;
        private System.Windows.Forms.ComboBox cboCategorySalesCategory;
        private System.Windows.Forms.Button btnGenerateCategorySales;
        private System.Windows.Forms.Button btnExportCategorySales;
        private System.Windows.Forms.DataGridView dgvCategorySales;

        // Tab 7: Client Product Ranking
        private System.Windows.Forms.Panel pnlClientProductRankingFilters;
        private System.Windows.Forms.Label lblClientProductRankingDateRange;
        private System.Windows.Forms.DateTimePicker dtpClientProductRankingStart;
        private System.Windows.Forms.DateTimePicker dtpClientProductRankingEnd;
        private System.Windows.Forms.Label lblClientProductRankingProduct;
        private System.Windows.Forms.ComboBox cboClientProductRankingProduct;
        private System.Windows.Forms.Label lblClientProductRankingCategory;
        private System.Windows.Forms.ComboBox cboClientProductRankingCategory;
        private System.Windows.Forms.CheckBox chkClientProductRankingLimit;
        private System.Windows.Forms.NumericUpDown nudClientProductRankingLimit;
        private System.Windows.Forms.Button btnGenerateClientProductRanking;
        private System.Windows.Forms.Button btnExportClientProductRanking;
        private System.Windows.Forms.DataGridView dgvClientProductRanking;

        // Note: Tab initialization methods can be added here in the future
        // For now, tabs will need to be manually configured in the designer or at runtime
    }
}
