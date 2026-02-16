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
            
            // Note: Tab initialization methods not yet implemented
            // Will be added in future updates
            
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

        // Note: Tab initialization methods can be added here in the future
        // For now, tabs will need to be manually configured in the designer or at runtime
    }
}
