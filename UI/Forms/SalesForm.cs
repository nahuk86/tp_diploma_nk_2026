using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using BLL.Services;
using DAO.Repositories;
using DOMAIN.Entities;
using SERVICES;
using SERVICES.Implementations;
using SERVICES.Interfaces;

namespace UI.Forms
{
    public partial class SalesForm : Form
    {
        private readonly SaleService _saleService;
        private readonly ClientService _clientService;
        private readonly ProductService _productService;
        private readonly WarehouseService _warehouseService;
        private readonly IAuthorizationService _authorizationService;
        private readonly ILocalizationService _localizationService;
        private readonly ILogService _logService;
        private readonly IErrorHandlerService _errorHandler;
        
        private bool _isCreating = false;
        private List<Product> _activeProducts;
        private List<Client> _activeClients;
        private List<Warehouse> _activeWarehouses;
        private Dictionary<int, Dictionary<int, int>> _productStockCache;

        public SalesForm()
        {
            InitializeComponent();

            // Initialize services
            _logService = new FileLogService();
            var saleRepo = new SaleRepository();
            var clientRepo = new ClientRepository();
            var productRepo = new ProductRepository();
            var stockRepo = new StockRepository();
            var warehouseRepo = new WarehouseRepository();
            var auditRepo = new AuditLogRepository();
            
            _saleService = new SaleService(saleRepo, clientRepo, productRepo, stockRepo, auditRepo, _logService);
            _clientService = new ClientService(clientRepo, auditRepo, _logService);
            _productService = new ProductService(productRepo, auditRepo, _logService);
            _warehouseService = new WarehouseService(warehouseRepo, auditRepo, _logService);
            
            var permissionRepo = new PermissionRepository();
            _authorizationService = new AuthorizationService(permissionRepo, _logService);
            _localizationService = new LocalizationService();
            _errorHandler = new ErrorHandlerService(_logService, _localizationService);

            _productStockCache = new Dictionary<int, Dictionary<int, int>>();

            InitializeForm();
        }

        private void InitializeForm()
        {
            ApplyLocalization();
            ConfigurePermissions();
            LoadClients();
            LoadProducts();
            LoadWarehouses();
            LoadSales();
            EnableForm(false);
        }

        private void ApplyLocalization()
        {
            this.Text = _localizationService.GetString("Sales.Title") ?? "Gestión de Ventas";
            
            // Group boxes
            grpList.Text = _localizationService.GetString("Sales.List") ?? "Ventas";
            grpDetails.Text = _localizationService.GetString("Sales.Details") ?? "Detalles de la Venta";
            grpLines.Text = _localizationService.GetString("Sales.Products") ?? "Productos";
            
            // Labels
            lblSaleNumber.Text = _localizationService.GetString("Sales.SaleNumber") ?? "Número:";
            lblSaleDate.Text = _localizationService.GetString("Sales.SaleDate") ?? "Fecha:";
            lblSellerName.Text = _localizationService.GetString("Sales.SellerName") ?? "Vendedor:";
            lblClient.Text = _localizationService.GetString("Sales.Client") ?? "Cliente:";
            lblNotes.Text = _localizationService.GetString("Sales.Notes") ?? "Notas:";
            lblTotalAmount.Text = _localizationService.GetString("Sales.TotalAmount") ?? "Total:";
            
            // Buttons
            btnNew.Text = _localizationService.GetString("Common.New") ?? "Nuevo";
            btnViewDetails.Text = _localizationService.GetString("Sales.ViewDetails") ?? "Ver Detalles";
            btnNewClient.Text = _localizationService.GetString("Sales.NewClient") ?? "Nuevo Cliente";
            btnAddLine.Text = _localizationService.GetString("Sales.AddLine") ?? "Agregar Línea";
            btnRemoveLine.Text = _localizationService.GetString("Sales.RemoveLine") ?? "Quitar Línea";
            btnSave.Text = _localizationService.GetString("Common.Save") ?? "Guardar";
            btnCancel.Text = _localizationService.GetString("Common.Cancel") ?? "Cancelar";
            
            // DataGridView columns - Sales
            colSaleNumber.HeaderText = _localizationService.GetString("Sales.SaleNumber") ?? "Número";
            colSaleDate.HeaderText = _localizationService.GetString("Sales.SaleDate") ?? "Fecha";
            colSellerName.HeaderText = _localizationService.GetString("Sales.SellerName") ?? "Vendedor";
            colClientName.HeaderText = _localizationService.GetString("Sales.Client") ?? "Cliente";
            colTotalAmount.HeaderText = _localizationService.GetString("Sales.TotalAmount") ?? "Total";
            
            // DataGridView columns - Lines
            colProduct.HeaderText = _localizationService.GetString("Sales.Product") ?? "Producto";
            colQuantity.HeaderText = _localizationService.GetString("Sales.Quantity") ?? "Cantidad";
            colUnitPrice.HeaderText = _localizationService.GetString("Sales.UnitPrice") ?? "Precio Unit.";
            colLineTotal.HeaderText = _localizationService.GetString("Sales.LineTotal") ?? "Subtotal";
            colStock.HeaderText = _localizationService.GetString("Sales.StockAvailable") ?? "Stock Disponible";
        }

        private void ConfigurePermissions()
        {
            if (!SessionContext.CurrentUserId.HasValue)
                return;

            var userId = SessionContext.CurrentUserId.Value;
            
            btnNew.Enabled = _authorizationService.HasPermission(userId, "Sales.Create");
        }

        private void LoadClients()
        {
            try
            {
                _activeClients = _clientService.GetActiveClients();
                
                cmbClient.Items.Clear();
                cmbClient.Items.Add(new ComboBoxItem { Text = _localizationService.GetString("Sales.NoClient") ?? "(Sin cliente)", Value = null });
                foreach (var client in _activeClients)
                {
                    cmbClient.Items.Add(new ComboBoxItem 
                    { 
                        Text = $"{client.Nombre} {client.Apellido} - DNI: {client.DNI}", 
                        Value = client.ClientId 
                    });
                }
                cmbClient.DisplayMember = "Text";
                cmbClient.ValueMember = "Value";
                cmbClient.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                _errorHandler.ShowError(ex, "Error al cargar clientes");
            }
        }

        private void LoadProducts()
        {
            try
            {
                _activeProducts = _productService.GetActiveProducts();
                
                // Configure product column in dgvLines
                colProduct.Items.Clear();
                colProduct.DisplayMember = "DisplayText";
                colProduct.ValueMember = "ProductId";
                foreach (var product in _activeProducts)
                {
                    colProduct.Items.Add(new ProductItem 
                    { 
                        ProductId = product.ProductId,
                        DisplayText = $"{product.SKU} - {product.Name}",
                        UnitPrice = product.UnitPrice
                    });
                }
            }
            catch (Exception ex)
            {
                _errorHandler.ShowError(ex, "Error al cargar productos");
            }
        }

        private void LoadWarehouses()
        {
            try
            {
                _activeWarehouses = _warehouseService.GetActiveWarehouses();
            }
            catch (Exception ex)
            {
                _errorHandler.ShowError(ex, "Error al cargar almacenes");
            }
        }

        private void LoadSales()
        {
            try
            {
                var sales = _saleService.GetAllSalesWithDetails();
                
                // Create display list with client names
                var salesDisplay = sales.Select(s => new
                {
                    s.SaleId,
                    s.SaleNumber,
                    s.SaleDate,
                    s.SellerName,
                    ClientName = s.ClientId.HasValue 
                        ? GetClientNameById(s.ClientId.Value) 
                        : _localizationService.GetString("Sales.NoClient") ?? "(Sin cliente)",
                    s.TotalAmount
                }).ToList();
                
                dgvSales.DataSource = salesDisplay;
                
                // Hide unnecessary columns
                if (dgvSales.Columns["SaleId"] != null)
                    dgvSales.Columns["SaleId"].Visible = false;
                
                // Format currency
                if (dgvSales.Columns["TotalAmount"] != null)
                    dgvSales.Columns["TotalAmount"].DefaultCellStyle.Format = "C2";
            }
            catch (Exception ex)
            {
                _errorHandler.ShowError(ex, "Error al cargar ventas");
            }
        }

        private string GetClientNameById(int clientId)
        {
            var client = _activeClients.FirstOrDefault(c => c.ClientId == clientId);
            return client != null ? $"{client.Nombre} {client.Apellido}" : _localizationService.GetString("Sales.Unknown") ?? "(Desconocido)";
        }

        private void btnNew_Click(object sender, EventArgs e)
        {
            _isCreating = true;
            ClearForm();
            EnableForm(true);
            cmbClient.Focus();  // Focus on client instead of seller name since it's read-only
        }

        private void btnViewDetails_Click(object sender, EventArgs e)
        {
            if (dgvSales.CurrentRow == null)
            {
                MessageBox.Show(
                    _localizationService.GetString("Sales.SelectSale") ?? "Por favor seleccione una venta.",
                    _localizationService.GetString("Common.Validation") ?? "Validación",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                return;
            }

            var saleId = (int)dgvSales.CurrentRow.Cells["SaleId"].Value;
            var sale = _saleService.GetSaleByIdWithLines(saleId);
            
            if (sale != null)
            {
                LoadSaleToForm(sale);
                EnableForm(false);
            }
        }

        private void btnNewClient_Click(object sender, EventArgs e)
        {
            try
            {
                var clientForm = new ClientsForm();
                clientForm.ShowDialog();
                LoadClients();
            }
            catch (Exception ex)
            {
                _errorHandler.ShowError(ex, "Error al abrir el formulario de clientes");
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (!ValidateForm())
                    return;

                // Create sale object
                var sale = new Sale
                {
                    SaleDate = dtpSaleDate.Value,
                    SellerName = txtSellerName.Text.Trim(),
                    ClientId = GetSelectedClientId(),
                    Notes = txtNotes.Text.Trim()
                };

                // Collect lines
                var lines = new List<SaleLine>();
                foreach (DataGridViewRow row in dgvLines.Rows)
                {
                    if (row.IsNewRow) continue;
                    
                    var productId = row.Cells[colProduct.Index].Value;
                    var quantity = row.Cells[colQuantity.Index].Value;
                    var unitPrice = row.Cells[colUnitPrice.Index].Value;

                    if (productId == null || quantity == null || unitPrice == null)
                        continue;

                    var lineTotal = Convert.ToDecimal(quantity) * Convert.ToDecimal(unitPrice);
                    
                    lines.Add(new SaleLine
                    {
                        ProductId = Convert.ToInt32(productId),
                        Quantity = Convert.ToInt32(quantity),
                        UnitPrice = Convert.ToDecimal(unitPrice),
                        LineTotal = lineTotal
                    });
                }

                if (lines.Count == 0)
                {
                    MessageBox.Show(
                        _localizationService.GetString("Sales.AtLeastOneProduct") ?? "Debe agregar al menos un producto a la venta.",
                        _localizationService.GetString("Common.Validation") ?? "Validación",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);
                    return;
                }

                // Create sale
                var currentUserId = SessionContext.CurrentUserId ?? 0;
                var saleId = _saleService.CreateSale(sale, lines, currentUserId);

                MessageBox.Show(
                    _localizationService.GetString("Sales.SaleCreatedSuccess") ?? "Venta creada exitosamente.",
                    _localizationService.GetString("Common.Confirmation") ?? "Confirmación",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);

                LoadSales();
                EnableForm(false);
                _isCreating = false;
            }
            catch (Exception ex)
            {
                _errorHandler.ShowError(ex, "Error al guardar la venta");
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            ClearForm();
            EnableForm(false);
            _isCreating = false;
        }

        private void btnAddLine_Click(object sender, EventArgs e)
        {
            dgvLines.Rows.Add();
            dgvLines.Focus();
        }

        private void btnRemoveLine_Click(object sender, EventArgs e)
        {
            if (dgvLines.CurrentRow != null && !dgvLines.CurrentRow.IsNewRow)
            {
                dgvLines.Rows.Remove(dgvLines.CurrentRow);
                CalculateTotal();
            }
        }

        private void dgvLines_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            var row = dgvLines.Rows[e.RowIndex];

            // When product is selected, set unit price and show stock
            if (e.ColumnIndex == colProduct.Index)
            {
                var productId = row.Cells[colProduct.Index].Value;
                if (productId != null)
                {
                    var product = _activeProducts.FirstOrDefault(p => p.ProductId == Convert.ToInt32(productId));
                    if (product != null)
                    {
                        row.Cells[colUnitPrice.Index].Value = product.UnitPrice;
                        UpdateStockDisplay(row, product.ProductId);
                        CalculateLineTotal(row);
                    }
                }
            }

            // When quantity or price changes, recalculate line total
            if (e.ColumnIndex == colQuantity.Index || e.ColumnIndex == colUnitPrice.Index)
            {
                CalculateLineTotal(row);
            }
        }

        private void dgvLines_CurrentCellDirtyStateChanged(object sender, EventArgs e)
        {
            if (dgvLines.IsCurrentCellDirty)
            {
                dgvLines.CommitEdit(DataGridViewDataErrorContexts.Commit);
            }
        }

        private void CalculateLineTotal(DataGridViewRow row)
        {
            var quantity = row.Cells[colQuantity.Index].Value;
            var unitPrice = row.Cells[colUnitPrice.Index].Value;

            if (quantity != null && unitPrice != null)
            {
                var qty = Convert.ToDecimal(quantity);
                var price = Convert.ToDecimal(unitPrice);
                row.Cells[colLineTotal.Index].Value = qty * price;
                CalculateTotal();
            }
        }

        private void CalculateTotal()
        {
            decimal total = 0;
            foreach (DataGridViewRow row in dgvLines.Rows)
            {
                if (!row.IsNewRow && row.Cells[colLineTotal.Index].Value != null)
                {
                    total += Convert.ToDecimal(row.Cells[colLineTotal.Index].Value);
                }
            }
            txtTotalAmount.Text = total.ToString("C2");
        }

        private void UpdateStockDisplay(DataGridViewRow row, int productId)
        {
            try
            {
                // Get stock by warehouse
                if (!_productStockCache.ContainsKey(productId))
                {
                    _productStockCache[productId] = _saleService.GetAvailableStockByWarehouse(productId);
                }

                var stockByWarehouse = _productStockCache[productId];
                
                if (stockByWarehouse.Count == 0)
                {
                    row.Cells[colStock.Index].Value = _localizationService.GetString("Sales.NoStock") ?? "Sin stock";
                }
                else
                {
                    var stockInfo = string.Join(", ", 
                        stockByWarehouse.Select(kvp => 
                        {
                            var warehouse = _activeWarehouses.FirstOrDefault(w => w.WarehouseId == kvp.Key);
                            var warehouseName = warehouse != null ? warehouse.Name : $"ID:{kvp.Key}";
                            return $"{warehouseName}: {kvp.Value}";
                        }));
                    row.Cells[colStock.Index].Value = stockInfo;
                }
            }
            catch (Exception ex)
            {
                _logService.Error($"Error loading stock for product {productId}", ex);
                row.Cells[colStock.Index].Value = "Error";
            }
        }

        private void EnableForm(bool enable)
        {
            grpDetails.Enabled = enable;
            grpLines.Enabled = enable;
            btnSave.Enabled = enable;
            btnCancel.Enabled = enable;
            
            btnNew.Enabled = !enable;
            btnViewDetails.Enabled = !enable;
            dgvSales.Enabled = !enable;
        }

        private void ClearForm()
        {
            txtSaleNumber.Text = _localizationService.GetString("Sales.AutoGenerated") ?? "(Autogenerado)";
            dtpSaleDate.Value = DateTime.Now;
            
            // Set seller name from current user
            if (SessionContext.CurrentUser != null)
            {
                txtSellerName.Text = SessionContext.CurrentUser.FullName ?? SessionContext.CurrentUsername;
            }
            else
            {
                txtSellerName.Text = "";
            }
            
            cmbClient.SelectedIndex = 0;
            txtNotes.Clear();
            dgvLines.Rows.Clear();
            txtTotalAmount.Text = "$0.00";
            _productStockCache.Clear();
        }

        private void LoadSaleToForm(Sale sale)
        {
            txtSaleNumber.Text = sale.SaleNumber;
            dtpSaleDate.Value = sale.SaleDate;
            txtSellerName.Text = sale.SellerName ?? "";
            
            // Set client
            if (sale.ClientId.HasValue)
            {
                for (int i = 0; i < cmbClient.Items.Count; i++)
                {
                    var item = (ComboBoxItem)cmbClient.Items[i];
                    if (item.Value != null && (int)item.Value == sale.ClientId.Value)
                    {
                        cmbClient.SelectedIndex = i;
                        break;
                    }
                }
            }
            else
            {
                cmbClient.SelectedIndex = 0;
            }
            
            txtNotes.Text = sale.Notes ?? "";
            
            // Load lines
            dgvLines.Rows.Clear();
            if (sale.SaleLines != null)
            {
                foreach (var line in sale.SaleLines)
                {
                    var rowIndex = dgvLines.Rows.Add();
                    var row = dgvLines.Rows[rowIndex];
                    
                    // Check if product exists in active products list
                    var productInList = colProduct.Items.Cast<ProductItem>().Any(p => p.ProductId == line.ProductId);
                    
                    if (!productInList && !string.IsNullOrEmpty(line.ProductName))
                    {
                        // Product is no longer active, add it temporarily for display
                        colProduct.Items.Add(new ProductItem
                        {
                            ProductId = line.ProductId,
                            DisplayText = $"{line.SKU} - {line.ProductName} (Inactivo)",
                            UnitPrice = line.UnitPrice
                        });
                    }
                    
                    row.Cells[colProduct.Index].Value = line.ProductId;
                    row.Cells[colQuantity.Index].Value = line.Quantity;
                    row.Cells[colUnitPrice.Index].Value = line.UnitPrice;
                    row.Cells[colLineTotal.Index].Value = line.LineTotal;
                    UpdateStockDisplay(row, line.ProductId);
                }
            }
            
            txtTotalAmount.Text = sale.TotalAmount.ToString("C2");
        }

        private bool ValidateForm()
        {
            // Seller name is automatically set from current user, no need to validate
            
            // Validate client selection is required
            var clientId = GetSelectedClientId();
            if (!clientId.HasValue)
            {
                MessageBox.Show(
                    _localizationService.GetString("Sales.ClientRequired") ?? "Debe seleccionar un cliente para la venta.",
                    _localizationService.GetString("Common.Validation") ?? "Validación",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                cmbClient.Focus();
                return false;
            }
            
            return true;
        }

        private int? GetSelectedClientId()
        {
            if (cmbClient.SelectedItem == null)
                return null;

            var item = (ComboBoxItem)cmbClient.SelectedItem;
            return item.Value;
        }

        // Helper classes
        private class ComboBoxItem
        {
            public string Text { get; set; }
            public int? Value { get; set; }
        }

        private class ProductItem
        {
            public int ProductId { get; set; }
            public string DisplayText { get; set; }
            public decimal UnitPrice { get; set; }
        }
    }
}
