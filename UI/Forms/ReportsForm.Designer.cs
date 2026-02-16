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
            this.SuspendLayout();
            
            // Initialize all controls for each tab
            InitializeTopProductsTab();
            InitializeClientPurchasesTab();
            InitializePriceVariationTab();
            InitializeSellerPerformanceTab();
            InitializeCategorySalesTab();
            InitializeRevenueByDateTab();
            InitializeClientProductRankingTab();
            InitializeClientTicketAverageTab();
            
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
            this.ResumeLayout(false);
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
    }
}

        private void InitializeTopProductsTab()
        {
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

            this.pnlTopProductsFilters.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudTopProductsLimit)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvTopProducts)).BeginInit();

            // pnlTopProductsFilters
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
            this.pnlTopProductsFilters.Location = new System.Drawing.Point(0, 0);
            this.pnlTopProductsFilters.Size = new System.Drawing.Size(1180, 120);

            this.lblTopProductsDateRange.Location = new System.Drawing.Point(10, 10);
            this.lblTopProductsDateRange.Size = new System.Drawing.Size(100, 20);
            this.lblTopProductsDateRange.Text = "Rango de Fechas:";

            this.dtpTopProductsStart.Location = new System.Drawing.Point(120, 10);
            this.dtpTopProductsStart.Size = new System.Drawing.Size(150, 20);

            this.dtpTopProductsEnd.Location = new System.Drawing.Point(280, 10);
            this.dtpTopProductsEnd.Size = new System.Drawing.Size(150, 20);

            this.lblTopProductsCategory.Location = new System.Drawing.Point(10, 40);
            this.lblTopProductsCategory.Size = new System.Drawing.Size(100, 20);
            this.lblTopProductsCategory.Text = "Categoría:";

            this.cboTopProductsCategory.Location = new System.Drawing.Point(120, 40);
            this.cboTopProductsCategory.Size = new System.Drawing.Size(200, 20);
            this.cboTopProductsCategory.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;

            this.chkTopProductsLimit.Location = new System.Drawing.Point(10, 70);
            this.chkTopProductsLimit.Size = new System.Drawing.Size(100, 20);
            this.chkTopProductsLimit.Text = "Limitar a Top:";

            this.nudTopProductsLimit.Location = new System.Drawing.Point(120, 70);
            this.nudTopProductsLimit.Size = new System.Drawing.Size(80, 20);
            this.nudTopProductsLimit.Minimum = 1;
            this.nudTopProductsLimit.Maximum = 1000;
            this.nudTopProductsLimit.Value = 10;

            this.lblTopProductsOrderBy.Location = new System.Drawing.Point(450, 40);
            this.lblTopProductsOrderBy.Size = new System.Drawing.Size(80, 20);
            this.lblTopProductsOrderBy.Text = "Ordenar por:";

            this.cboTopProductsOrderBy.Location = new System.Drawing.Point(540, 40);
            this.cboTopProductsOrderBy.Size = new System.Drawing.Size(150, 20);
            this.cboTopProductsOrderBy.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboTopProductsOrderBy.Items.AddRange(new object[] { "Unidades", "Ingresos" });
            this.cboTopProductsOrderBy.SelectedIndex = 0;

            this.btnGenerateTopProducts.Location = new System.Drawing.Point(750, 40);
            this.btnGenerateTopProducts.Size = new System.Drawing.Size(100, 30);
            this.btnGenerateTopProducts.Text = "Generar";
            this.btnGenerateTopProducts.Click += new System.EventHandler(this.btnGenerateTopProducts_Click);

            this.btnExportTopProducts.Location = new System.Drawing.Point(860, 40);
            this.btnExportTopProducts.Size = new System.Drawing.Size(100, 30);
            this.btnExportTopProducts.Text = "Exportar CSV";
            this.btnExportTopProducts.Click += new System.EventHandler(this.btnExportTopProducts_Click);

            // dgvTopProducts
            this.dgvTopProducts.AllowUserToAddRows = false;
            this.dgvTopProducts.AllowUserToDeleteRows = false;
            this.dgvTopProducts.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvTopProducts.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvTopProducts.Location = new System.Drawing.Point(0, 120);
            this.dgvTopProducts.ReadOnly = true;
            this.dgvTopProducts.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;

            this.tabTopProducts.Controls.Add(this.dgvTopProducts);
            this.tabTopProducts.Controls.Add(this.pnlTopProductsFilters);
            this.tabTopProducts.Location = new System.Drawing.Point(4, 22);
            this.tabTopProducts.Name = "tabTopProducts";
            this.tabTopProducts.Padding = new System.Windows.Forms.Padding(3);
            this.tabTopProducts.Size = new System.Drawing.Size(1192, 674);
            this.tabTopProducts.TabIndex = 0;
            this.tabTopProducts.Text = "Productos Más Vendidos";
            this.tabTopProducts.UseVisualStyleBackColor = true;

            this.pnlTopProductsFilters.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.nudTopProductsLimit)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvTopProducts)).EndInit();
        }
