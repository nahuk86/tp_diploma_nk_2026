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

        /// <summary>
        /// Inicializa una nueva instancia del formulario de gestión de ventas
        /// </summary>
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
            _localizationService = LocalizationService.Instance;
            _errorHandler = new ErrorHandlerService(_logService, _localizationService);

            _productStockCache = new Dictionary<int, Dictionary<int, int>>();

            InitializeForm();
        }

        /// <summary>
        /// Inicializa el formulario y carga los datos necesarios
        /// </summary>
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

        /// <summary>
        /// Aplica la localización de textos al formulario según el idioma seleccionado
        /// </summary>
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

        /// <summary>
        /// Configura los permisos del usuario actual para las acciones del formulario
        /// </summary>
        private void ConfigurePermissions()
        {
            if (!SessionContext.Instance.CurrentUserId.HasValue)
                return;

            var userId = SessionContext.Instance.CurrentUserId.Value;
            
            btnNew.Enabled = _authorizationService.HasPermission(userId, "Sales.Create");
        }

        /// <summary>
        /// Carga la lista de clientes activos en el ComboBox
        /// </summary>
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

        /// <summary>
        /// Carga la lista de productos activos en el DataGridView de líneas de venta
        /// </summary>
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

        /// <summary>
        /// Carga la lista de almacenes activos
        /// </summary>
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

        /// <summary>
        /// Carga la lista de ventas con sus detalles en el DataGridView principal
        /// </summary>
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

        /// <summary>
        /// Obtiene el nombre completo de un cliente por su ID
        /// </summary>
        /// <param name="clientId">ID del cliente</param>
        /// <returns>Nombre completo del cliente o "(Desconocido)" si no se encuentra</returns>
        private string GetClientNameById(int clientId)
        {
            var client = _activeClients.FirstOrDefault(c => c.ClientId == clientId);
            return client != null ? $"{client.Nombre} {client.Apellido}" : _localizationService.GetString("Sales.Unknown") ?? "(Desconocido)";
        }

        /// <summary>
        /// Maneja el evento Click del botón Nuevo para crear una nueva venta
        /// </summary>
        /// <param name="sender">Objeto que genera el evento</param>
        /// <param name="e">Argumentos del evento</param>
        private void btnNew_Click(object sender, EventArgs e)
        {
            _isCreating = true;
            ClearForm();
            EnableForm(true);
            cmbClient.Focus();  // Focus on client instead of seller name since it's read-only
        }

        /// <summary>
        /// Maneja el evento Click del botón Ver Detalles para mostrar los detalles de una venta seleccionada
        /// </summary>
        /// <param name="sender">Objeto que genera el evento</param>
        /// <param name="e">Argumentos del evento</param>
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

        /// <summary>
        /// Maneja el evento Click del botón Nuevo Cliente para abrir el formulario de gestión de clientes
        /// </summary>
        /// <param name="sender">Objeto que genera el evento</param>
        /// <param name="e">Argumentos del evento</param>
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

        /// <summary>
        /// Maneja el evento Click del botón Guardar para crear una nueva venta con sus líneas
        /// </summary>
        /// <param name="sender">Objeto que genera el evento</param>
        /// <param name="e">Argumentos del evento</param>
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
                var currentUserId = SessionContext.Instance.CurrentUserId ?? 0;
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

        /// <summary>
        /// Maneja el evento Click del botón Cancelar para descartar los cambios y cerrar el modo de edición
        /// </summary>
        /// <param name="sender">Objeto que genera el evento</param>
        /// <param name="e">Argumentos del evento</param>
        private void btnCancel_Click(object sender, EventArgs e)
        {
            ClearForm();
            EnableForm(false);
            _isCreating = false;
        }

        /// <summary>
        /// Maneja el evento Click del botón Agregar Línea para añadir una nueva línea de producto a la venta
        /// </summary>
        /// <param name="sender">Objeto que genera el evento</param>
        /// <param name="e">Argumentos del evento</param>
        private void btnAddLine_Click(object sender, EventArgs e)
        {
            dgvLines.Rows.Add();
            dgvLines.Focus();
        }

        /// <summary>
        /// Maneja el evento Click del botón Quitar Línea para eliminar una línea de producto de la venta
        /// </summary>
        /// <param name="sender">Objeto que genera el evento</param>
        /// <param name="e">Argumentos del evento</param>
        private void btnRemoveLine_Click(object sender, EventArgs e)
        {
            if (dgvLines.CurrentRow != null && !dgvLines.CurrentRow.IsNewRow)
            {
                dgvLines.Rows.Remove(dgvLines.CurrentRow);
                CalculateTotal();
            }
        }

        /// <summary>
        /// Maneja el evento CellValueChanged del DataGridView de líneas para recalcular totales y actualizar stock
        /// </summary>
        /// <param name="sender">Objeto que genera el evento</param>
        /// <param name="e">Argumentos del evento con información de la celda modificada</param>
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

        /// <summary>
        /// Maneja el cambio de estado de la celda actual para confirmar los cambios inmediatamente
        /// </summary>
        /// <param name="sender">Objeto que genera el evento</param>
        /// <param name="e">Argumentos del evento</param>
        private void dgvLines_CurrentCellDirtyStateChanged(object sender, EventArgs e)
        {
            if (dgvLines.IsCurrentCellDirty)
            {
                dgvLines.CommitEdit(DataGridViewDataErrorContexts.Commit);
            }
        }

        /// <summary>
        /// Calcula el total de una línea de venta multiplicando cantidad por precio unitario
        /// </summary>
        /// <param name="row">Fila del DataGridView que contiene la línea de venta</param>
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

        /// <summary>
        /// Calcula el total de la venta sumando todas las líneas de productos
        /// </summary>
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

        /// <summary>
        /// Actualiza la visualización del stock disponible para un producto en los diferentes almacenes
        /// </summary>
        /// <param name="row">Fila del DataGridView donde se mostrará el stock</param>
        /// <param name="productId">ID del producto para consultar su stock</param>
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

        /// <summary>
        /// Habilita o deshabilita los controles del formulario según el modo de edición
        /// </summary>
        /// <param name="enable">True para habilitar edición, False para deshabilitar</param>
        private void EnableForm(bool enable)
        {
            grpDetails.Enabled = enable;
            grpLines.Enabled = enable;
            btnSave.Enabled = enable;
            btnCancel.Enabled = enable;
            
            dgvSales.Enabled = !enable;
            
            // Re-apply permissions when disabling form
            if (!enable)
            {
                ConfigurePermissions();
            }
            
            btnViewDetails.Enabled = !enable;
        }

        /// <summary>
        /// Limpia todos los campos del formulario y restablece los valores por defecto
        /// </summary>
        private void ClearForm()
        {
            txtSaleNumber.Text = _localizationService.GetString("Sales.AutoGenerated") ?? "(Autogenerado)";
            dtpSaleDate.Value = DateTime.Now;
            
            // Set seller name from current user
            if (SessionContext.Instance.CurrentUser != null)
            {
                txtSellerName.Text = SessionContext.Instance.CurrentUser.FullName ?? SessionContext.Instance.CurrentUsername;
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

        /// <summary>
        /// Carga los datos de una venta existente en el formulario para visualización
        /// </summary>
        /// <param name="sale">Objeto Sale con los datos a mostrar</param>
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

        /// <summary>
        /// Valida que los campos requeridos del formulario estén completos antes de guardar
        /// </summary>
        /// <returns>True si la validación es exitosa, False en caso contrario</returns>
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

        /// <summary>
        /// Obtiene el ID del cliente seleccionado en el ComboBox
        /// </summary>
        /// <returns>ID del cliente seleccionado o null si no hay selección válida</returns>
        private int? GetSelectedClientId()
        {
            if (cmbClient.SelectedItem == null)
                return null;

            var item = (ComboBoxItem)cmbClient.SelectedItem;
            return item.Value;
        }

        // Helper classes
        /// <summary>
        /// Clase auxiliar para representar elementos en un ComboBox con texto y valor
        /// </summary>
        private class ComboBoxItem
        {
            /// <summary>
            /// Obtiene o establece el texto a mostrar en el ComboBox
            /// </summary>
            public string Text { get; set; }
            /// <summary>
            /// Obtiene o establece el valor asociado al elemento
            /// </summary>
            public int? Value { get; set; }
        }

        /// <summary>
        /// Clase auxiliar para representar productos en el DataGridView de líneas de venta
        /// </summary>
        private class ProductItem
        {
            /// <summary>
            /// Obtiene o establece el ID del producto
            /// </summary>
            public int ProductId { get; set; }
            /// <summary>
            /// Obtiene o establece el texto a mostrar para el producto
            /// </summary>
            public string DisplayText { get; set; }
            /// <summary>
            /// Obtiene o establece el precio unitario del producto
            /// </summary>
            public decimal UnitPrice { get; set; }
        }
    }
}
