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
            this.tabRevenueByDate = new System.Windows.Forms.TabPage();
            this.tabClientProductRanking = new System.Windows.Forms.TabPage();
            this.tabClientTicketAverage = new System.Windows.Forms.TabPage();
            
            this.tabControl1.SuspendLayout();
            this.tabTopProducts.SuspendLayout();
            this.SuspendLayout();
            
            // Initialize Top Products tab with essential controls
            InitializeTopProductsTab();
            
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabTopProducts);
            this.tabControl1.Controls.Add(this.tabClientPurchases);
            this.tabControl1.Controls.Add(this.tabPriceVariation);
            this.tabControl1.Controls.Add(this.tabSellerPerformance);
            this.tabControl1.Controls.Add(this.tabCategorySales);
            this.tabControl1.Controls.Add(this.tabRevenueByDate);
            this.tabControl1.Controls.Add(this.tabClientProductRanking);
            this.tabControl1.Controls.Add(this.tabClientTicketAverage);
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

        #endregion

        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabTopProducts;
        private System.Windows.Forms.TabPage tabClientPurchases;
        private System.Windows.Forms.TabPage tabPriceVariation;
        private System.Windows.Forms.TabPage tabSellerPerformance;
        private System.Windows.Forms.TabPage tabCategorySales;
        private System.Windows.Forms.TabPage tabRevenueByDate;
        private System.Windows.Forms.TabPage tabClientProductRanking;
        private System.Windows.Forms.TabPage tabClientTicketAverage;

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

        // Tab 6: Revenue by Date
        private System.Windows.Forms.Panel pnlRevenueByDateFilters;
        private System.Windows.Forms.Label lblRevenueByDateDateRange;
        private System.Windows.Forms.DateTimePicker dtpRevenueByDateStart;
        private System.Windows.Forms.DateTimePicker dtpRevenueByDateEnd;
        private System.Windows.Forms.Label lblRevenueByDateMovementType;
        private System.Windows.Forms.ComboBox cboRevenueByDateMovementType;
        private System.Windows.Forms.Label lblRevenueByDateWarehouse;
        private System.Windows.Forms.ComboBox cboRevenueByDateWarehouse;
        private System.Windows.Forms.Button btnGenerateRevenueByDate;
        private System.Windows.Forms.Button btnExportRevenueByDate;
        private System.Windows.Forms.DataGridView dgvRevenueByDate;

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

        // Tab 8: Client Ticket Average
        private System.Windows.Forms.Panel pnlClientTicketAverageFilters;
        private System.Windows.Forms.Label lblClientTicketAverageDateRange;
        private System.Windows.Forms.DateTimePicker dtpClientTicketAverageStart;
        private System.Windows.Forms.DateTimePicker dtpClientTicketAverageEnd;
        private System.Windows.Forms.Label lblClientTicketAverageClient;
        private System.Windows.Forms.ComboBox cboClientTicketAverageClient;
        private System.Windows.Forms.CheckBox chkClientTicketAverageMinPurchases;
        private System.Windows.Forms.NumericUpDown nudClientTicketAverageMinPurchases;
        private System.Windows.Forms.Button btnGenerateClientTicketAverage;
        private System.Windows.Forms.Button btnExportClientTicketAverage;
        private System.Windows.Forms.DataGridView dgvClientTicketAverage;

        // Note: Tab initialization methods can be added here in the future
        // For now, tabs will need to be manually configured in the designer or at runtime
    }
}
