using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using BLL.Services;
using DAO.Repositories;
using DOMAIN.Entities;
using DOMAIN.Entities.Reports;
using SERVICES;
using SERVICES.Implementations;
using SERVICES.Interfaces;

namespace UI.Forms
{
    public partial class ReportsForm : Form
    {
        private readonly ReportService _reportService;
        private readonly ProductService _productService;
        private readonly ClientService _clientService;
        private readonly WarehouseService _warehouseService;
        private readonly ILocalizationService _localizationService;
        private readonly ILogService _logService;
        private readonly IErrorHandlerService _errorHandler;

        private List<Product> _products;
        private List<Client> _clients;
        private List<Warehouse> _warehouses;
        private List<string> _categories;

        public ReportsForm()
        {
            InitializeComponent();

            // Initialize services
            _logService = new FileLogService();
            var reportRepo = new ReportRepository();
            var productRepo = new ProductRepository();
            var clientRepo = new ClientRepository();
            var warehouseRepo = new WarehouseRepository();
            var auditRepo = new AuditLogRepository();

            _reportService = new ReportService(reportRepo, _logService);
            _productService = new ProductService(productRepo, auditRepo, _logService);
            _clientService = new ClientService(clientRepo, auditRepo, _logService);
            _warehouseService = new WarehouseService(warehouseRepo, auditRepo, _logService);
            _localizationService = LocalizationService.Instance;
            _errorHandler = new ErrorHandlerService(_logService, _localizationService);

            InitializeForm();
        }

        private void InitializeForm()
        {
            ApplyLocalization();
            LoadCommonData();
            SetupDateRanges();
        }

        private void ApplyLocalization()
        {
            this.Text = _localizationService.GetString("Reports.Title") ?? "Reportes";
            
            // Tab pages
            tabTopProducts.Text = _localizationService.GetString("Reports.TopProducts") ?? "Productos Más Vendidos";
            tabClientPurchases.Text = _localizationService.GetString("Reports.ClientPurchases") ?? "Compras por Cliente";
            tabPriceVariation.Text = _localizationService.GetString("Reports.PriceVariation") ?? "Variación de Precios";
            tabSellerPerformance.Text = _localizationService.GetString("Reports.SellerPerformance") ?? "Ventas por Vendedor";
            tabCategorySales.Text = _localizationService.GetString("Reports.CategorySales") ?? "Ventas por Categoría";
            tabClientProductRanking.Text = _localizationService.GetString("Reports.ClientProductRanking") ?? "Ranking Clientes-Productos";
        }

        private void LoadCommonData()
        {
            try
            {
                _products = _productService.GetActiveProducts();
                _clients = _clientService.GetActiveClients();
                _warehouses = _warehouseService.GetActiveWarehouses();
                _categories = _products.Select(p => p.Category).Distinct().Where(c => !string.IsNullOrEmpty(c)).OrderBy(c => c).ToList();

                // Populate category dropdowns
                PopulateCategories();
                
                // Populate client dropdowns
                PopulateClients();
                
                // Populate product dropdowns
                PopulateProducts();
                
                // Populate warehouse dropdowns
                PopulateWarehouses();
            }
            catch (Exception ex)
            {
                _errorHandler.HandleError(ex, "Error cargando datos comunes");
            }
        }

        private void SetupDateRanges()
        {
            var today = DateTime.Today;
            
            // Report 1: Top Products
            if (dtpTopProductsStart != null)
                dtpTopProductsStart.Value = today.AddMonths(-1);
            if (dtpTopProductsEnd != null)
                dtpTopProductsEnd.Value = today;
            
            // Report 2: Client Purchases
            if (dtpClientPurchasesStart != null)
                dtpClientPurchasesStart.Value = today.AddMonths(-1);
            if (dtpClientPurchasesEnd != null)
                dtpClientPurchasesEnd.Value = today;
            
            // Report 3: Price Variation
            if (dtpPriceVariationStart != null)
                dtpPriceVariationStart.Value = today.AddMonths(-1);
            if (dtpPriceVariationEnd != null)
                dtpPriceVariationEnd.Value = today;
            
            // Report 4: Seller Performance
            if (dtpSellerPerformanceStart != null)
                dtpSellerPerformanceStart.Value = today.AddMonths(-1);
            if (dtpSellerPerformanceEnd != null)
                dtpSellerPerformanceEnd.Value = today;
            
            // Report 5: Category Sales
            if (dtpCategorySalesStart != null)
                dtpCategorySalesStart.Value = today.AddMonths(-1);
            if (dtpCategorySalesEnd != null)
                dtpCategorySalesEnd.Value = today;
            
            // Report 7: Client Product Ranking
            if (dtpClientProductRankingStart != null)
                dtpClientProductRankingStart.Value = today.AddMonths(-1);
            if (dtpClientProductRankingEnd != null)
                dtpClientProductRankingEnd.Value = today;
        }

        private void PopulateCategories()
        {
            if (cboTopProductsCategory != null)
            {
                cboTopProductsCategory.Items.Clear();
                cboTopProductsCategory.Items.Add("-- Todas las Categorías --");
                cboTopProductsCategory.Items.AddRange(_categories.ToArray());
                cboTopProductsCategory.SelectedIndex = 0;
            }

            if (cboPriceVariationCategory != null)
            {
                cboPriceVariationCategory.Items.Clear();
                cboPriceVariationCategory.Items.Add("-- Todas las Categorías --");
                cboPriceVariationCategory.Items.AddRange(_categories.ToArray());
                cboPriceVariationCategory.SelectedIndex = 0;
            }

            if (cboSellerPerformanceCategory != null)
            {
                cboSellerPerformanceCategory.Items.Clear();
                cboSellerPerformanceCategory.Items.Add("-- Todas las Categorías --");
                cboSellerPerformanceCategory.Items.AddRange(_categories.ToArray());
                cboSellerPerformanceCategory.SelectedIndex = 0;
            }

            if (cboCategorySalesCategory != null)
            {
                cboCategorySalesCategory.Items.Clear();
                cboCategorySalesCategory.Items.Add("-- Todas las Categorías --");
                cboCategorySalesCategory.Items.AddRange(_categories.ToArray());
                cboCategorySalesCategory.SelectedIndex = 0;
            }

            if (cboClientProductRankingCategory != null)
            {
                cboClientProductRankingCategory.Items.Clear();
                cboClientProductRankingCategory.Items.Add("-- Todas las Categorías --");
                cboClientProductRankingCategory.Items.AddRange(_categories.ToArray());
                cboClientProductRankingCategory.SelectedIndex = 0;
            }
        }

        private void PopulateClients()
        {
            if (cboClientPurchasesClient != null)
            {
                cboClientPurchasesClient.Items.Clear();
                cboClientPurchasesClient.Items.Add(new ComboBoxItem { Text = "-- Todos los Clientes --", Value = null });
                foreach (var client in _clients)
                {
                    cboClientPurchasesClient.Items.Add(new ComboBoxItem 
                    { 
                        Text = $"{client.Nombre} {client.Apellido} - {client.DNI}", 
                        Value = client.ClientId 
                    });
                }
                cboClientPurchasesClient.DisplayMember = "Text";
                cboClientPurchasesClient.ValueMember = "Value";
                cboClientPurchasesClient.SelectedIndex = 0;
            }
        }

        private void PopulateProducts()
        {
            if (cboPriceVariationProduct != null)
            {
                cboPriceVariationProduct.Items.Clear();
                cboPriceVariationProduct.Items.Add(new ComboBoxItem { Text = "-- Todos los Productos --", Value = null });
                foreach (var product in _products)
                {
                    cboPriceVariationProduct.Items.Add(new ComboBoxItem 
                    { 
                        Text = $"{product.SKU} - {product.Name}", 
                        Value = product.ProductId 
                    });
                }
                cboPriceVariationProduct.DisplayMember = "Text";
                cboPriceVariationProduct.ValueMember = "Value";
                cboPriceVariationProduct.SelectedIndex = 0;
            }

            if (cboClientProductRankingProduct != null)
            {
                cboClientProductRankingProduct.Items.Clear();
                cboClientProductRankingProduct.Items.Add(new ComboBoxItem { Text = "-- Todos los Productos --", Value = null });
                foreach (var product in _products)
                {
                    cboClientProductRankingProduct.Items.Add(new ComboBoxItem 
                    { 
                        Text = $"{product.SKU} - {product.Name}", 
                        Value = product.ProductId 
                    });
                }
                cboClientProductRankingProduct.DisplayMember = "Text";
                cboClientProductRankingProduct.ValueMember = "Value";
                cboClientProductRankingProduct.SelectedIndex = 0;
            }
        }

        private void PopulateWarehouses()
        {
            // Warehouse population removed - no longer needed
        }

        // Report 1: Top Products
        private void btnGenerateTopProducts_Click(object sender, EventArgs e)
        {
            try
            {
                var startDate = dtpTopProductsStart.Value.Date;
                var endDate = dtpTopProductsEnd.Value.Date.AddDays(1).AddSeconds(-1);
                var category = cboTopProductsCategory.SelectedIndex > 0 ? cboTopProductsCategory.SelectedItem.ToString() : null;
                var topN = chkTopProductsLimit.Checked ? (int?)nudTopProductsLimit.Value : null;
                var orderBy = cboTopProductsOrderBy.SelectedIndex == 0 ? "units" : "revenue";

                var data = _reportService.GetTopProductsReport(startDate, endDate, category, topN, orderBy);
                dgvTopProducts.DataSource = data;

                FormatTopProductsGrid();
                _logService.Info($"Reporte Productos Más Vendidos generado con {data.Count} registros");
            }
            catch (Exception ex)
            {
                _errorHandler.HandleError(ex, "Error generando reporte de productos más vendidos");
            }
        }

        private void FormatTopProductsGrid()
        {
            if (dgvTopProducts.DataSource != null && dgvTopProducts.Columns.Count > 0)
            {
                if (dgvTopProducts.Columns.Contains("Ranking"))
                    dgvTopProducts.Columns["Ranking"].HeaderText = "Posición";
                if (dgvTopProducts.Columns.Contains("SKU"))
                    dgvTopProducts.Columns["SKU"].HeaderText = "SKU";
                if (dgvTopProducts.Columns.Contains("ProductName"))
                    dgvTopProducts.Columns["ProductName"].HeaderText = "Producto";
                if (dgvTopProducts.Columns.Contains("Category"))
                    dgvTopProducts.Columns["Category"].HeaderText = "Categoría";
                if (dgvTopProducts.Columns.Contains("UnitsSold"))
                    dgvTopProducts.Columns["UnitsSold"].HeaderText = "Unidades Vendidas";
                if (dgvTopProducts.Columns.Contains("Revenue"))
                {
                    dgvTopProducts.Columns["Revenue"].HeaderText = "Ingresos";
                    dgvTopProducts.Columns["Revenue"].DefaultCellStyle.Format = "C2";
                }
                if (dgvTopProducts.Columns.Contains("ListPrice"))
                {
                    dgvTopProducts.Columns["ListPrice"].HeaderText = "Precio Lista";
                    dgvTopProducts.Columns["ListPrice"].DefaultCellStyle.Format = "C2";
                }
                if (dgvTopProducts.Columns.Contains("AverageSalePrice"))
                {
                    dgvTopProducts.Columns["AverageSalePrice"].HeaderText = "Precio Promedio Venta";
                    dgvTopProducts.Columns["AverageSalePrice"].DefaultCellStyle.Format = "C2";
                }
            }
        }

        private void btnExportTopProducts_Click(object sender, EventArgs e)
        {
            ExportToCSV(dgvTopProducts, "Productos_Mas_Vendidos");
        }

        // Report 2: Client Purchases
        private void btnGenerateClientPurchases_Click(object sender, EventArgs e)
        {
            try
            {
                var startDate = dtpClientPurchasesStart.Value.Date;
                var endDate = dtpClientPurchasesEnd.Value.Date.AddDays(1).AddSeconds(-1);
                var clientItem = cboClientPurchasesClient.SelectedItem as ComboBoxItem;
                var clientId = clientItem?.Value as int?;
                var topN = chkClientPurchasesLimit.Checked ? (int?)nudClientPurchasesLimit.Value : null;

                var data = _reportService.GetClientPurchasesReport(startDate, endDate, clientId, topN);
                dgvClientPurchases.DataSource = data;

                FormatClientPurchasesGrid();
                _logService.Info($"Reporte Compras por Cliente generado con {data.Count} registros");
            }
            catch (Exception ex)
            {
                _errorHandler.HandleError(ex, "Error generando reporte de compras por cliente");
            }
        }

        private void FormatClientPurchasesGrid()
        {
            if (dgvClientPurchases.DataSource != null && dgvClientPurchases.Columns.Count > 0)
            {
                if (dgvClientPurchases.Columns.Contains("ClientId"))
                    dgvClientPurchases.Columns["ClientId"].Visible = false;
                if (dgvClientPurchases.Columns.Contains("ClientFullName"))
                    dgvClientPurchases.Columns["ClientFullName"].HeaderText = "Cliente";
                if (dgvClientPurchases.Columns.Contains("DNI"))
                    dgvClientPurchases.Columns["DNI"].HeaderText = "DNI";
                if (dgvClientPurchases.Columns.Contains("Email"))
                    dgvClientPurchases.Columns["Email"].HeaderText = "Email";
                if (dgvClientPurchases.Columns.Contains("PurchaseCount"))
                    dgvClientPurchases.Columns["PurchaseCount"].HeaderText = "# Compras";
                if (dgvClientPurchases.Columns.Contains("TotalSpent"))
                {
                    dgvClientPurchases.Columns["TotalSpent"].HeaderText = "Total Gastado";
                    dgvClientPurchases.Columns["TotalSpent"].DefaultCellStyle.Format = "C2";
                }
                if (dgvClientPurchases.Columns.Contains("TotalUnits"))
                    dgvClientPurchases.Columns["TotalUnits"].HeaderText = "Total Unidades";
                if (dgvClientPurchases.Columns.Contains("DistinctProducts"))
                    dgvClientPurchases.Columns["DistinctProducts"].HeaderText = "Productos Distintos";
                if (dgvClientPurchases.Columns.Contains("AverageTicket"))
                {
                    dgvClientPurchases.Columns["AverageTicket"].HeaderText = "Ticket Promedio";
                    dgvClientPurchases.Columns["AverageTicket"].DefaultCellStyle.Format = "C2";
                }
                if (dgvClientPurchases.Columns.Contains("ProductDetails"))
                    dgvClientPurchases.Columns["ProductDetails"].Visible = false;
            }
        }

        private void btnExportClientPurchases_Click(object sender, EventArgs e)
        {
            ExportToCSV(dgvClientPurchases, "Compras_Por_Cliente");
        }

        // Report 3: Price Variation
        private void btnGeneratePriceVariation_Click(object sender, EventArgs e)
        {
            try
            {
                var startDate = dtpPriceVariationStart.Value.Date;
                var endDate = dtpPriceVariationEnd.Value.Date.AddDays(1).AddSeconds(-1);
                var productItem = cboPriceVariationProduct.SelectedItem as ComboBoxItem;
                var productId = productItem?.Value as int?;
                var category = cboPriceVariationCategory.SelectedIndex > 0 ? cboPriceVariationCategory.SelectedItem.ToString() : null;

                var data = _reportService.GetPriceVariationReport(startDate, endDate, productId, category);
                dgvPriceVariation.DataSource = data;

                FormatPriceVariationGrid();
                _logService.Info($"Reporte Variación de Precios generado con {data.Count} registros");
            }
            catch (Exception ex)
            {
                _errorHandler.HandleError(ex, "Error generando reporte de variación de precios");
            }
        }

        private void FormatPriceVariationGrid()
        {
            if (dgvPriceVariation.DataSource != null && dgvPriceVariation.Columns.Count > 0)
            {
                if (dgvPriceVariation.Columns.Contains("SKU"))
                    dgvPriceVariation.Columns["SKU"].HeaderText = "SKU";
                if (dgvPriceVariation.Columns.Contains("ProductName"))
                    dgvPriceVariation.Columns["ProductName"].HeaderText = "Producto";
                if (dgvPriceVariation.Columns.Contains("Category"))
                    dgvPriceVariation.Columns["Category"].HeaderText = "Categoría";
                if (dgvPriceVariation.Columns.Contains("ListPrice"))
                {
                    dgvPriceVariation.Columns["ListPrice"].HeaderText = "Precio Lista";
                    dgvPriceVariation.Columns["ListPrice"].DefaultCellStyle.Format = "C2";
                }
                if (dgvPriceVariation.Columns.Contains("MinSalePrice"))
                {
                    dgvPriceVariation.Columns["MinSalePrice"].HeaderText = "Precio Venta Min";
                    dgvPriceVariation.Columns["MinSalePrice"].DefaultCellStyle.Format = "C2";
                }
                if (dgvPriceVariation.Columns.Contains("MaxSalePrice"))
                {
                    dgvPriceVariation.Columns["MaxSalePrice"].HeaderText = "Precio Venta Max";
                    dgvPriceVariation.Columns["MaxSalePrice"].DefaultCellStyle.Format = "C2";
                }
                if (dgvPriceVariation.Columns.Contains("AverageSalePrice"))
                {
                    dgvPriceVariation.Columns["AverageSalePrice"].HeaderText = "Precio Venta Promedio";
                    dgvPriceVariation.Columns["AverageSalePrice"].DefaultCellStyle.Format = "C2";
                }
                if (dgvPriceVariation.Columns.Contains("AbsoluteVariation"))
                {
                    dgvPriceVariation.Columns["AbsoluteVariation"].HeaderText = "Variación Absoluta";
                    dgvPriceVariation.Columns["AbsoluteVariation"].DefaultCellStyle.Format = "C2";
                }
                if (dgvPriceVariation.Columns.Contains("PercentageVariation"))
                {
                    dgvPriceVariation.Columns["PercentageVariation"].HeaderText = "Variación %";
                    dgvPriceVariation.Columns["PercentageVariation"].DefaultCellStyle.Format = "N2";
                }
            }
        }

        private void btnExportPriceVariation_Click(object sender, EventArgs e)
        {
            ExportToCSV(dgvPriceVariation, "Variacion_Precios");
        }

        // Report 4: Seller Performance
        private void btnGenerateSellerPerformance_Click(object sender, EventArgs e)
        {
            try
            {
                var startDate = dtpSellerPerformanceStart.Value.Date;
                var endDate = dtpSellerPerformanceEnd.Value.Date.AddDays(1).AddSeconds(-1);
                var sellerName = string.IsNullOrWhiteSpace(txtSellerPerformanceSeller.Text) ? null : txtSellerPerformanceSeller.Text;
                var category = cboSellerPerformanceCategory.SelectedIndex > 0 ? cboSellerPerformanceCategory.SelectedItem.ToString() : null;

                var data = _reportService.GetSellerPerformanceReport(startDate, endDate, sellerName, category);
                dgvSellerPerformance.DataSource = data;

                FormatSellerPerformanceGrid();
                _logService.Info($"Reporte Ventas por Vendedor generado con {data.Count} registros");
            }
            catch (Exception ex)
            {
                _errorHandler.HandleError(ex, "Error generando reporte de ventas por vendedor");
            }
        }

        private void FormatSellerPerformanceGrid()
        {
            if (dgvSellerPerformance.DataSource != null && dgvSellerPerformance.Columns.Count > 0)
            {
                if (dgvSellerPerformance.Columns.Contains("SellerName"))
                    dgvSellerPerformance.Columns["SellerName"].HeaderText = "Vendedor";
                if (dgvSellerPerformance.Columns.Contains("TotalSales"))
                    dgvSellerPerformance.Columns["TotalSales"].HeaderText = "Total Ventas";
                if (dgvSellerPerformance.Columns.Contains("TotalUnits"))
                    dgvSellerPerformance.Columns["TotalUnits"].HeaderText = "Total Unidades";
                if (dgvSellerPerformance.Columns.Contains("TotalRevenue"))
                {
                    dgvSellerPerformance.Columns["TotalRevenue"].HeaderText = "Facturación Total";
                    dgvSellerPerformance.Columns["TotalRevenue"].DefaultCellStyle.Format = "C2";
                }
                if (dgvSellerPerformance.Columns.Contains("AverageTicket"))
                {
                    dgvSellerPerformance.Columns["AverageTicket"].HeaderText = "Ticket Promedio";
                    dgvSellerPerformance.Columns["AverageTicket"].DefaultCellStyle.Format = "C2";
                }
                if (dgvSellerPerformance.Columns.Contains("TopProduct"))
                    dgvSellerPerformance.Columns["TopProduct"].HeaderText = "Producto Top";
                if (dgvSellerPerformance.Columns.Contains("TopProductQuantity"))
                    dgvSellerPerformance.Columns["TopProductQuantity"].HeaderText = "Cant. Producto Top";
            }
        }

        private void btnExportSellerPerformance_Click(object sender, EventArgs e)
        {
            ExportToCSV(dgvSellerPerformance, "Ventas_Por_Vendedor");
        }

        // Report 5: Category Sales
        private void btnGenerateCategorySales_Click(object sender, EventArgs e)
        {
            try
            {
                var startDate = dtpCategorySalesStart.Value.Date;
                var endDate = dtpCategorySalesEnd.Value.Date.AddDays(1).AddSeconds(-1);
                var category = cboCategorySalesCategory.SelectedIndex > 0 ? cboCategorySalesCategory.SelectedItem.ToString() : null;

                var data = _reportService.GetCategorySalesReport(startDate, endDate, category);
                dgvCategorySales.DataSource = data;

                FormatCategorySalesGrid();
                _logService.Info($"Reporte Ventas por Categoría generado con {data.Count} registros");
            }
            catch (Exception ex)
            {
                _errorHandler.HandleError(ex, "Error generando reporte de ventas por categoría");
            }
        }

        private void FormatCategorySalesGrid()
        {
            if (dgvCategorySales.DataSource != null && dgvCategorySales.Columns.Count > 0)
            {
                if (dgvCategorySales.Columns.Contains("Category"))
                    dgvCategorySales.Columns["Category"].HeaderText = "Categoría";
                if (dgvCategorySales.Columns.Contains("UnitsSold"))
                    dgvCategorySales.Columns["UnitsSold"].HeaderText = "Unidades Vendidas";
                if (dgvCategorySales.Columns.Contains("TotalRevenue"))
                {
                    dgvCategorySales.Columns["TotalRevenue"].HeaderText = "Facturación Total";
                    dgvCategorySales.Columns["TotalRevenue"].DefaultCellStyle.Format = "C2";
                }
                if (dgvCategorySales.Columns.Contains("PercentageOfTotal"))
                {
                    dgvCategorySales.Columns["PercentageOfTotal"].HeaderText = "% del Total";
                    dgvCategorySales.Columns["PercentageOfTotal"].DefaultCellStyle.Format = "N2";
                }
            }
        }

        private void btnExportCategorySales_Click(object sender, EventArgs e)
        {
            ExportToCSV(dgvCategorySales, "Ventas_Por_Categoria");
        }



        // Report 7: Client Product Ranking
        private void btnGenerateClientProductRanking_Click(object sender, EventArgs e)
        {
            try
            {
                var startDate = dtpClientProductRankingStart.Value.Date;
                var endDate = dtpClientProductRankingEnd.Value.Date.AddDays(1).AddSeconds(-1);
                var productItem = cboClientProductRankingProduct.SelectedItem as ComboBoxItem;
                var productId = productItem?.Value as int?;
                var category = cboClientProductRankingCategory.SelectedIndex > 0 ? cboClientProductRankingCategory.SelectedItem.ToString() : null;
                var topN = chkClientProductRankingLimit.Checked ? (int?)nudClientProductRankingLimit.Value : null;

                var data = _reportService.GetClientProductRankingReport(startDate, endDate, productId, category, topN);
                dgvClientProductRanking.DataSource = data;

                FormatClientProductRankingGrid();
                _logService.Info($"Reporte Ranking Clientes-Productos generado con {data.Count} registros");
            }
            catch (Exception ex)
            {
                _errorHandler.HandleError(ex, "Error generando reporte de ranking clientes-productos");
            }
        }

        private void FormatClientProductRankingGrid()
        {
            if (dgvClientProductRanking.DataSource != null && dgvClientProductRanking.Columns.Count > 0)
            {
                if (dgvClientProductRanking.Columns.Contains("ClientId"))
                    dgvClientProductRanking.Columns["ClientId"].Visible = false;
                if (dgvClientProductRanking.Columns.Contains("ClientFullName"))
                    dgvClientProductRanking.Columns["ClientFullName"].HeaderText = "Cliente";
                if (dgvClientProductRanking.Columns.Contains("DNI"))
                    dgvClientProductRanking.Columns["DNI"].HeaderText = "DNI";
                if (dgvClientProductRanking.Columns.Contains("ProductName"))
                    dgvClientProductRanking.Columns["ProductName"].HeaderText = "Producto";
                if (dgvClientProductRanking.Columns.Contains("SKU"))
                    dgvClientProductRanking.Columns["SKU"].HeaderText = "SKU";
                if (dgvClientProductRanking.Columns.Contains("Category"))
                    dgvClientProductRanking.Columns["Category"].HeaderText = "Categoría";
                if (dgvClientProductRanking.Columns.Contains("UnitsPurchased"))
                    dgvClientProductRanking.Columns["UnitsPurchased"].HeaderText = "Unidades Compradas";
                if (dgvClientProductRanking.Columns.Contains("TotalSpent"))
                {
                    dgvClientProductRanking.Columns["TotalSpent"].HeaderText = "Total Gastado";
                    dgvClientProductRanking.Columns["TotalSpent"].DefaultCellStyle.Format = "C2";
                }
                if (dgvClientProductRanking.Columns.Contains("PercentageOfProductSales"))
                {
                    dgvClientProductRanking.Columns["PercentageOfProductSales"].HeaderText = "% del Producto";
                    dgvClientProductRanking.Columns["PercentageOfProductSales"].DefaultCellStyle.Format = "N2";
                }
            }
        }

        private void btnExportClientProductRanking_Click(object sender, EventArgs e)
        {
            ExportToCSV(dgvClientProductRanking, "Ranking_Clientes_Productos");
        }

        // Helper Methods
        private void ExportToCSV(DataGridView dgv, string fileName)
        {
            try
            {
                if (dgv.Rows.Count == 0)
                {
                    MessageBox.Show("No hay datos para exportar", "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                SaveFileDialog sfd = new SaveFileDialog();
                sfd.Filter = "CSV files (*.csv)|*.csv|All files (*.*)|*.*";
                sfd.FileName = $"{fileName}_{DateTime.Now:yyyyMMdd_HHmmss}.csv";

                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    StringBuilder sb = new StringBuilder();

                    // Headers
                    var headers = new List<string>();
                    foreach (DataGridViewColumn column in dgv.Columns)
                    {
                        if (column.Visible)
                        {
                            headers.Add($"\"{column.HeaderText}\"");
                        }
                    }
                    sb.AppendLine(string.Join(",", headers));

                    // Data
                    foreach (DataGridViewRow row in dgv.Rows)
                    {
                        if (row.IsNewRow) continue;

                        var cells = new List<string>();
                        foreach (DataGridViewColumn column in dgv.Columns)
                        {
                            if (column.Visible)
                            {
                                var value = row.Cells[column.Index].Value?.ToString() ?? "";
                                cells.Add($"\"{value.Replace("\"", "\"\"")}\"");
                            }
                        }
                        sb.AppendLine(string.Join(",", cells));
                    }

                    File.WriteAllText(sfd.FileName, sb.ToString(), Encoding.UTF8);
                    MessageBox.Show($"Reporte exportado exitosamente a:\n{sfd.FileName}", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    _logService.Info($"Reporte exportado a {sfd.FileName}");
                }
            }
            catch (Exception ex)
            {
                _errorHandler.HandleError(ex, "Error exportando reporte");
            }
        }

        private class ComboBoxItem
        {
            public string Text { get; set; }
            public object Value { get; set; }
        }
    }
}
