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
            _localizationService = new LocalizationService();
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
            tabRevenueByDate.Text = _localizationService.GetString("Reports.RevenueByDate") ?? "Ingresos por Fecha";
            tabClientProductRanking.Text = _localizationService.GetString("Reports.ClientProductRanking") ?? "Ranking Clientes-Productos";
            tabClientTicketAverage.Text = _localizationService.GetString("Reports.ClientTicketAverage") ?? "Ticket Promedio";
        }

        private void LoadCommonData()
        {
            try
            {
                _products = _productService.GetAllActiveProducts();
                _clients = _clientService.GetAllActiveClients();
                _warehouses = _warehouseService.GetAllActiveWarehouses();
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
            dtpTopProductsStart.Value = today.AddMonths(-1);
            dtpTopProductsEnd.Value = today;
            
            // Report 2: Client Purchases
            dtpClientPurchasesStart.Value = today.AddMonths(-1);
            dtpClientPurchasesEnd.Value = today;
            
            // Report 3: Price Variation
            dtpPriceVariationStart.Value = today.AddMonths(-1);
            dtpPriceVariationEnd.Value = today;
            
            // Report 4: Seller Performance
            dtpSellerPerformanceStart.Value = today.AddMonths(-1);
            dtpSellerPerformanceEnd.Value = today;
            
            // Report 5: Category Sales
            dtpCategorySalesStart.Value = today.AddMonths(-1);
            dtpCategorySalesEnd.Value = today;
            
            // Report 6: Revenue by Date
            dtpRevenueByDateStart.Value = today.AddMonths(-1);
            dtpRevenueByDateEnd.Value = today;
            
            // Report 7: Client Product Ranking
            dtpClientProductRankingStart.Value = today.AddMonths(-1);
            dtpClientProductRankingEnd.Value = today;
            
            // Report 8: Client Ticket Average
            dtpClientTicketAverageStart.Value = today.AddMonths(-1);
            dtpClientTicketAverageEnd.Value = today;
        }

        private void PopulateCategories()
        {
            cboTopProductsCategory.Items.Clear();
            cboTopProductsCategory.Items.Add("-- Todas las Categorías --");
            cboTopProductsCategory.Items.AddRange(_categories.ToArray());
            cboTopProductsCategory.SelectedIndex = 0;

            cboPriceVariationCategory.Items.Clear();
            cboPriceVariationCategory.Items.Add("-- Todas las Categorías --");
            cboPriceVariationCategory.Items.AddRange(_categories.ToArray());
            cboPriceVariationCategory.SelectedIndex = 0;

            cboSellerPerformanceCategory.Items.Clear();
            cboSellerPerformanceCategory.Items.Add("-- Todas las Categorías --");
            cboSellerPerformanceCategory.Items.AddRange(_categories.ToArray());
            cboSellerPerformanceCategory.SelectedIndex = 0;

            cboCategorySalesCategory.Items.Clear();
            cboCategorySalesCategory.Items.Add("-- Todas las Categorías --");
            cboCategorySalesCategory.Items.AddRange(_categories.ToArray());
            cboCategorySalesCategory.SelectedIndex = 0;

            cboClientProductRankingCategory.Items.Clear();
            cboClientProductRankingCategory.Items.Add("-- Todas las Categorías --");
            cboClientProductRankingCategory.Items.AddRange(_categories.ToArray());
            cboClientProductRankingCategory.SelectedIndex = 0;
        }

        private void PopulateClients()
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

            cboClientTicketAverageClient.Items.Clear();
            cboClientTicketAverageClient.Items.Add(new ComboBoxItem { Text = "-- Todos los Clientes --", Value = null });
            foreach (var client in _clients)
            {
                cboClientTicketAverageClient.Items.Add(new ComboBoxItem 
                { 
                    Text = $"{client.Nombre} {client.Apellido} - {client.DNI}", 
                    Value = client.ClientId 
                });
            }
            cboClientTicketAverageClient.DisplayMember = "Text";
            cboClientTicketAverageClient.ValueMember = "Value";
            cboClientTicketAverageClient.SelectedIndex = 0;
        }

        private void PopulateProducts()
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

        private void PopulateWarehouses()
        {
            cboRevenueByDateWarehouse.Items.Clear();
            cboRevenueByDateWarehouse.Items.Add(new ComboBoxItem { Text = "-- Todos los Almacenes --", Value = null });
            foreach (var warehouse in _warehouses)
            {
                cboRevenueByDateWarehouse.Items.Add(new ComboBoxItem 
                { 
                    Text = warehouse.Name, 
                    Value = warehouse.WarehouseId 
                });
            }
            cboRevenueByDateWarehouse.DisplayMember = "Text";
            cboRevenueByDateWarehouse.ValueMember = "Value";
            cboRevenueByDateWarehouse.SelectedIndex = 0;
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
            if (dgvTopProducts.DataSource != null)
            {
                dgvTopProducts.Columns["Ranking"].HeaderText = "Posición";
                dgvTopProducts.Columns["SKU"].HeaderText = "SKU";
                dgvTopProducts.Columns["ProductName"].HeaderText = "Producto";
                dgvTopProducts.Columns["Category"].HeaderText = "Categoría";
                dgvTopProducts.Columns["UnitsSold"].HeaderText = "Unidades Vendidas";
                dgvTopProducts.Columns["Revenue"].HeaderText = "Ingresos";
                dgvTopProducts.Columns["Revenue"].DefaultCellStyle.Format = "C2";
                dgvTopProducts.Columns["ListPrice"].HeaderText = "Precio Lista";
                dgvTopProducts.Columns["ListPrice"].DefaultCellStyle.Format = "C2";
                dgvTopProducts.Columns["AverageSalePrice"].HeaderText = "Precio Promedio Venta";
                dgvTopProducts.Columns["AverageSalePrice"].DefaultCellStyle.Format = "C2";
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
            if (dgvClientPurchases.DataSource != null)
            {
                dgvClientPurchases.Columns["ClientId"].Visible = false;
                dgvClientPurchases.Columns["ClientFullName"].HeaderText = "Cliente";
                dgvClientPurchases.Columns["DNI"].HeaderText = "DNI";
                dgvClientPurchases.Columns["Email"].HeaderText = "Email";
                dgvClientPurchases.Columns["PurchaseCount"].HeaderText = "# Compras";
                dgvClientPurchases.Columns["TotalSpent"].HeaderText = "Total Gastado";
                dgvClientPurchases.Columns["TotalSpent"].DefaultCellStyle.Format = "C2";
                dgvClientPurchases.Columns["TotalUnits"].HeaderText = "Total Unidades";
                dgvClientPurchases.Columns["DistinctProducts"].HeaderText = "Productos Distintos";
                dgvClientPurchases.Columns["AverageTicket"].HeaderText = "Ticket Promedio";
                dgvClientPurchases.Columns["AverageTicket"].DefaultCellStyle.Format = "C2";
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
            if (dgvPriceVariation.DataSource != null)
            {
                dgvPriceVariation.Columns["SKU"].HeaderText = "SKU";
                dgvPriceVariation.Columns["ProductName"].HeaderText = "Producto";
                dgvPriceVariation.Columns["Category"].HeaderText = "Categoría";
                dgvPriceVariation.Columns["ListPrice"].HeaderText = "Precio Lista";
                dgvPriceVariation.Columns["ListPrice"].DefaultCellStyle.Format = "C2";
                dgvPriceVariation.Columns["MinSalePrice"].HeaderText = "Precio Venta Min";
                dgvPriceVariation.Columns["MinSalePrice"].DefaultCellStyle.Format = "C2";
                dgvPriceVariation.Columns["MaxSalePrice"].HeaderText = "Precio Venta Max";
                dgvPriceVariation.Columns["MaxSalePrice"].DefaultCellStyle.Format = "C2";
                dgvPriceVariation.Columns["AverageSalePrice"].HeaderText = "Precio Venta Promedio";
                dgvPriceVariation.Columns["AverageSalePrice"].DefaultCellStyle.Format = "C2";
                dgvPriceVariation.Columns["AbsoluteVariation"].HeaderText = "Variación Absoluta";
                dgvPriceVariation.Columns["AbsoluteVariation"].DefaultCellStyle.Format = "C2";
                dgvPriceVariation.Columns["PercentageVariation"].HeaderText = "Variación %";
                dgvPriceVariation.Columns["PercentageVariation"].DefaultCellStyle.Format = "N2";
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
            if (dgvSellerPerformance.DataSource != null)
            {
                dgvSellerPerformance.Columns["SellerName"].HeaderText = "Vendedor";
                dgvSellerPerformance.Columns["TotalSales"].HeaderText = "Total Ventas";
                dgvSellerPerformance.Columns["TotalUnits"].HeaderText = "Total Unidades";
                dgvSellerPerformance.Columns["TotalRevenue"].HeaderText = "Facturación Total";
                dgvSellerPerformance.Columns["TotalRevenue"].DefaultCellStyle.Format = "C2";
                dgvSellerPerformance.Columns["AverageTicket"].HeaderText = "Ticket Promedio";
                dgvSellerPerformance.Columns["AverageTicket"].DefaultCellStyle.Format = "C2";
                dgvSellerPerformance.Columns["TopProduct"].HeaderText = "Producto Top";
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
            if (dgvCategorySales.DataSource != null)
            {
                dgvCategorySales.Columns["Category"].HeaderText = "Categoría";
                dgvCategorySales.Columns["UnitsSold"].HeaderText = "Unidades Vendidas";
                dgvCategorySales.Columns["TotalRevenue"].HeaderText = "Facturación Total";
                dgvCategorySales.Columns["TotalRevenue"].DefaultCellStyle.Format = "C2";
                dgvCategorySales.Columns["PercentageOfTotal"].HeaderText = "% del Total";
                dgvCategorySales.Columns["PercentageOfTotal"].DefaultCellStyle.Format = "N2";
            }
        }

        private void btnExportCategorySales_Click(object sender, EventArgs e)
        {
            ExportToCSV(dgvCategorySales, "Ventas_Por_Categoria");
        }

        // Report 6: Revenue by Date
        private void btnGenerateRevenueByDate_Click(object sender, EventArgs e)
        {
            try
            {
                var startDate = dtpRevenueByDateStart.Value.Date;
                var endDate = dtpRevenueByDateEnd.Value.Date.AddDays(1).AddSeconds(-1);
                var movementType = cboRevenueByDateMovementType.SelectedIndex > 0 ? cboRevenueByDateMovementType.SelectedItem.ToString() : null;
                var warehouseItem = cboRevenueByDateWarehouse.SelectedItem as ComboBoxItem;
                var warehouseId = warehouseItem?.Value as int?;

                var data = _reportService.GetRevenueByDateReport(startDate, endDate, movementType, warehouseId);
                dgvRevenueByDate.DataSource = data;

                FormatRevenueByDateGrid();
                _logService.Info($"Reporte Ingresos por Fecha generado con {data.Count} registros");
            }
            catch (Exception ex)
            {
                _errorHandler.HandleError(ex, "Error generando reporte de ingresos por fecha");
            }
        }

        private void FormatRevenueByDateGrid()
        {
            if (dgvRevenueByDate.DataSource != null)
            {
                dgvRevenueByDate.Columns["ReportDate"].HeaderText = "Fecha";
                dgvRevenueByDate.Columns["ReportDate"].DefaultCellStyle.Format = "dd/MM/yyyy";
                dgvRevenueByDate.Columns["SalesRevenue"].HeaderText = "Ingresos Ventas";
                dgvRevenueByDate.Columns["SalesRevenue"].DefaultCellStyle.Format = "C2";
                dgvRevenueByDate.Columns["StockInMovements"].HeaderText = "Movimientos Entrada";
                dgvRevenueByDate.Columns["StockInUnits"].HeaderText = "Unidades Entrada";
                dgvRevenueByDate.Columns["StockOutMovements"].HeaderText = "Movimientos Salida";
                dgvRevenueByDate.Columns["StockOutUnits"].HeaderText = "Unidades Salida";
            }
        }

        private void btnExportRevenueByDate_Click(object sender, EventArgs e)
        {
            ExportToCSV(dgvRevenueByDate, "Ingresos_Por_Fecha");
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
            if (dgvClientProductRanking.DataSource != null)
            {
                dgvClientProductRanking.Columns["ClientId"].Visible = false;
                dgvClientProductRanking.Columns["ClientFullName"].HeaderText = "Cliente";
                dgvClientProductRanking.Columns["DNI"].HeaderText = "DNI";
                dgvClientProductRanking.Columns["ProductName"].HeaderText = "Producto";
                dgvClientProductRanking.Columns["SKU"].HeaderText = "SKU";
                dgvClientProductRanking.Columns["Category"].HeaderText = "Categoría";
                dgvClientProductRanking.Columns["UnitsPurchased"].HeaderText = "Unidades Compradas";
                dgvClientProductRanking.Columns["TotalSpent"].HeaderText = "Total Gastado";
                dgvClientProductRanking.Columns["TotalSpent"].DefaultCellStyle.Format = "C2";
                dgvClientProductRanking.Columns["PercentageOfProductSales"].HeaderText = "% del Producto";
                dgvClientProductRanking.Columns["PercentageOfProductSales"].DefaultCellStyle.Format = "N2";
            }
        }

        private void btnExportClientProductRanking_Click(object sender, EventArgs e)
        {
            ExportToCSV(dgvClientProductRanking, "Ranking_Clientes_Productos");
        }

        // Report 8: Client Ticket Average
        private void btnGenerateClientTicketAverage_Click(object sender, EventArgs e)
        {
            try
            {
                var startDate = dtpClientTicketAverageStart.Value.Date;
                var endDate = dtpClientTicketAverageEnd.Value.Date.AddDays(1).AddSeconds(-1);
                var clientItem = cboClientTicketAverageClient.SelectedItem as ComboBoxItem;
                var clientId = clientItem?.Value as int?;
                var minPurchases = chkClientTicketAverageMinPurchases.Checked ? (int?)nudClientTicketAverageMinPurchases.Value : null;

                var data = _reportService.GetClientTicketAverageReport(startDate, endDate, clientId, minPurchases);
                dgvClientTicketAverage.DataSource = data;

                FormatClientTicketAverageGrid();
                _logService.Info($"Reporte Ticket Promedio por Cliente generado con {data.Count} registros");
            }
            catch (Exception ex)
            {
                _errorHandler.HandleError(ex, "Error generando reporte de ticket promedio por cliente");
            }
        }

        private void FormatClientTicketAverageGrid()
        {
            if (dgvClientTicketAverage.DataSource != null)
            {
                dgvClientTicketAverage.Columns["ClientId"].Visible = false;
                dgvClientTicketAverage.Columns["ClientFullName"].HeaderText = "Cliente";
                dgvClientTicketAverage.Columns["DNI"].HeaderText = "DNI";
                dgvClientTicketAverage.Columns["PurchaseCount"].HeaderText = "# Compras";
                dgvClientTicketAverage.Columns["TotalSpent"].HeaderText = "Total Gastado";
                dgvClientTicketAverage.Columns["TotalSpent"].DefaultCellStyle.Format = "C2";
                dgvClientTicketAverage.Columns["AverageTicket"].HeaderText = "Ticket Promedio";
                dgvClientTicketAverage.Columns["AverageTicket"].DefaultCellStyle.Format = "C2";
                dgvClientTicketAverage.Columns["MinTicket"].HeaderText = "Ticket Mínimo";
                dgvClientTicketAverage.Columns["MinTicket"].DefaultCellStyle.Format = "C2";
                dgvClientTicketAverage.Columns["MaxTicket"].HeaderText = "Ticket Máximo";
                dgvClientTicketAverage.Columns["MaxTicket"].DefaultCellStyle.Format = "C2";
                dgvClientTicketAverage.Columns["StdDeviation"].HeaderText = "Desv. Estándar";
                dgvClientTicketAverage.Columns["StdDeviation"].DefaultCellStyle.Format = "C2";
            }
        }

        private void btnExportClientTicketAverage_Click(object sender, EventArgs e)
        {
            ExportToCSV(dgvClientTicketAverage, "Ticket_Promedio_Cliente");
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
