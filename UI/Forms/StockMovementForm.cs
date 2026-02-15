using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using BLL.Services;
using DAO.Repositories;
using DOMAIN.Entities;
using DOMAIN.Enums;
using SERVICES;
using SERVICES.Implementations;
using SERVICES.Interfaces;

namespace UI.Forms
{
    public partial class StockMovementForm : Form
    {
        private readonly StockMovementService _movementService;
        private readonly ProductService _productService;
        private readonly WarehouseService _warehouseService;
        private readonly IAuthorizationService _authorizationService;
        private readonly ILocalizationService _localizationService;
        private readonly ILogService _logService;
        private readonly IErrorHandlerService _errorHandler;
        
        private bool _isCreating = false;
        private List<Product> _activeProducts;
        private List<Warehouse> _activeWarehouses;

        public StockMovementForm()
        {
            InitializeComponent();

            // Initialize services
            _logService = new FileLogService();
            var movementRepo = new StockMovementRepository();
            var stockRepo = new StockRepository();
            var productRepo = new ProductRepository();
            var warehouseRepo = new WarehouseRepository();
            var auditRepo = new AuditLogRepository();
            
            _movementService = new StockMovementService(movementRepo, stockRepo, productRepo, warehouseRepo, auditRepo, _logService);
            _productService = new ProductService(productRepo, auditRepo, _logService);
            _warehouseService = new WarehouseService(warehouseRepo, auditRepo, _logService);
            
            var permissionRepo = new PermissionRepository();
            _authorizationService = new AuthorizationService(permissionRepo, _logService);
            _localizationService = new LocalizationService();
            _errorHandler = new ErrorHandlerService(_logService, _localizationService);

            InitializeForm();
        }

        private void InitializeForm()
        {
            ApplyLocalization();
            ConfigurePermissions();
            LoadWarehouses();
            LoadProducts();
            PopulateMovementTypeFilter();
            PopulateMovementTypeCombo();
            LoadMovements();
            EnableForm(false);
        }

        private void ApplyLocalization()
        {
            this.Text = _localizationService.GetString("StockMovement.Title") ?? "Movimientos de Stock";
            
            // Group boxes
            grpList.Text = _localizationService.GetString("StockMovement.List") ?? "Movimientos de Stock";
            grpDetails.Text = _localizationService.GetString("StockMovement.Details") ?? "Detalles del Movimiento";
            grpLines.Text = _localizationService.GetString("StockMovement.Products") ?? "Productos";
            
            // Labels
            lblMovementType.Text = _localizationService.GetString("StockMovement.FilterByType") ?? "Filtrar por tipo:";
            lblType.Text = _localizationService.GetString("StockMovement.Type") ?? "Tipo:";
            lblDate.Text = _localizationService.GetString("StockMovement.Date") ?? "Fecha:";
            lblSourceWarehouse.Text = _localizationService.GetString("StockMovement.SourceWarehouse") ?? "Almacén Origen:";
            lblDestinationWarehouse.Text = _localizationService.GetString("StockMovement.DestinationWarehouse") ?? "Almacén Destino:";
            lblReason.Text = _localizationService.GetString("StockMovement.Reason") ?? "Motivo:";
            lblNotes.Text = _localizationService.GetString("StockMovement.Notes") ?? "Notas:";
            
            // Buttons
            btnNew.Text = _localizationService.GetString("Common.New") ?? "Nuevo";
            btnViewDetails.Text = _localizationService.GetString("StockMovement.ViewDetails") ?? "Ver Detalles";
            btnAddLine.Text = _localizationService.GetString("StockMovement.AddLine") ?? "Agregar Línea";
            btnRemoveLine.Text = _localizationService.GetString("StockMovement.RemoveLine") ?? "Quitar Línea";
            btnSave.Text = _localizationService.GetString("Common.Save") ?? "Guardar";
            btnCancel.Text = _localizationService.GetString("Common.Cancel") ?? "Cancelar";
            
            // DataGridView columns - Movements
            colMovementNumber.HeaderText = _localizationService.GetString("StockMovement.Number") ?? "Número";
            colMovementType.HeaderText = _localizationService.GetString("StockMovement.Type") ?? "Tipo";
            colMovementDate.HeaderText = _localizationService.GetString("StockMovement.Date") ?? "Fecha";
            colSourceWarehouse.HeaderText = _localizationService.GetString("StockMovement.SourceWarehouse") ?? "Almacén Origen";
            colDestinationWarehouse.HeaderText = _localizationService.GetString("StockMovement.DestinationWarehouse") ?? "Almacén Destino";
            
            // DataGridView columns - Lines
            colProduct.HeaderText = _localizationService.GetString("StockMovement.Product") ?? "Producto";
            colQuantity.HeaderText = _localizationService.GetString("StockMovement.Quantity") ?? "Cantidad";
            colUnitPrice.HeaderText = _localizationService.GetString("StockMovement.UnitPrice") ?? "Precio Unitario";
        }

        private void ConfigurePermissions()
        {
            if (!SessionContext.CurrentUserId.HasValue)
                return;

            var userId = SessionContext.CurrentUserId.Value;
            
            // Check permissions based on operation type
            // For simplicity, using Stock.Receive for create operations
            btnNew.Enabled = _authorizationService.HasPermission(userId, "Stock.Receive") ||
                           _authorizationService.HasPermission(userId, "Stock.Issue") ||
                           _authorizationService.HasPermission(userId, "Stock.Transfer") ||
                           _authorizationService.HasPermission(userId, "Stock.Adjust");
        }

        private void LoadWarehouses()
        {
            try
            {
                _activeWarehouses = _warehouseService.GetActiveWarehouses();
                
                // Populate source warehouse combo
                cmbSourceWarehouse.Items.Clear();
                cmbSourceWarehouse.Items.Add(new ComboBoxItem { Text = "(Ninguno)", Value = null });
                foreach (var warehouse in _activeWarehouses)
                {
                    cmbSourceWarehouse.Items.Add(new ComboBoxItem 
                    { 
                        Text = warehouse.Name, 
                        Value = warehouse.WarehouseId 
                    });
                }
                cmbSourceWarehouse.DisplayMember = "Text";
                cmbSourceWarehouse.ValueMember = "Value";
                cmbSourceWarehouse.SelectedIndex = 0;
                
                // Populate destination warehouse combo
                cmbDestinationWarehouse.Items.Clear();
                cmbDestinationWarehouse.Items.Add(new ComboBoxItem { Text = "(Ninguno)", Value = null });
                foreach (var warehouse in _activeWarehouses)
                {
                    cmbDestinationWarehouse.Items.Add(new ComboBoxItem 
                    { 
                        Text = warehouse.Name, 
                        Value = warehouse.WarehouseId 
                    });
                }
                cmbDestinationWarehouse.DisplayMember = "Text";
                cmbDestinationWarehouse.ValueMember = "Value";
                cmbDestinationWarehouse.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                _errorHandler.ShowError(ex, "Error al cargar almacenes");
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
                        DisplayText = $"{product.SKU} - {product.Name}"
                    });
                }
            }
            catch (Exception ex)
            {
                _errorHandler.ShowError(ex, "Error al cargar productos");
            }
        }

        private void PopulateMovementTypeFilter()
        {
            cmbMovementTypeFilter.Items.Clear();
            cmbMovementTypeFilter.Items.Add("(Todos)");
            cmbMovementTypeFilter.Items.Add(MovementType.In);
            cmbMovementTypeFilter.Items.Add(MovementType.Out);
            cmbMovementTypeFilter.Items.Add(MovementType.Transfer);
            cmbMovementTypeFilter.Items.Add(MovementType.Adjustment);
            cmbMovementTypeFilter.SelectedIndex = 0;
        }

        private void PopulateMovementTypeCombo()
        {
            cmbMovementType.Items.Clear();
            cmbMovementType.Items.Add(MovementType.In);
            cmbMovementType.Items.Add(MovementType.Out);
            cmbMovementType.Items.Add(MovementType.Transfer);
            cmbMovementType.Items.Add(MovementType.Adjustment);
            cmbMovementType.SelectedIndex = 0;
        }

        private void LoadMovements()
        {
            try
            {
                List<StockMovement> movements;
                
                if (cmbMovementTypeFilter.SelectedIndex == 0) // All
                {
                    movements = _movementService.GetAllMovements();
                }
                else
                {
                    var movementType = (MovementType)cmbMovementTypeFilter.SelectedItem;
                    movements = _movementService.GetMovementsByType(movementType);
                }
                
                dgvMovements.DataSource = movements;
                
                // Hide unnecessary columns
                if (dgvMovements.Columns["MovementId"] != null)
                    dgvMovements.Columns["MovementId"].Visible = false;
                if (dgvMovements.Columns["SourceWarehouseId"] != null)
                    dgvMovements.Columns["SourceWarehouseId"].Visible = false;
                if (dgvMovements.Columns["DestinationWarehouseId"] != null)
                    dgvMovements.Columns["DestinationWarehouseId"].Visible = false;
                if (dgvMovements.Columns["CreatedAt"] != null)
                    dgvMovements.Columns["CreatedAt"].Visible = false;
                if (dgvMovements.Columns["CreatedBy"] != null)
                    dgvMovements.Columns["CreatedBy"].Visible = false;
                if (dgvMovements.Columns["CreatedByUsername"] != null)
                    dgvMovements.Columns["CreatedByUsername"].Visible = false;
                if (dgvMovements.Columns["Reason"] != null)
                    dgvMovements.Columns["Reason"].Visible = false;
                if (dgvMovements.Columns["Notes"] != null)
                    dgvMovements.Columns["Notes"].Visible = false;
            }
            catch (Exception ex)
            {
                _errorHandler.ShowError(ex, "Error al cargar movimientos");
            }
        }

        private void btnNew_Click(object sender, EventArgs e)
        {
            _isCreating = true;
            ClearForm();
            EnableForm(true);
            cmbMovementType.Focus();
        }

        private void btnViewDetails_Click(object sender, EventArgs e)
        {
            if (dgvMovements.CurrentRow == null)
            {
                MessageBox.Show(
                    "Por favor seleccione un movimiento.",
                    "Validación",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                return;
            }

            var movement = (StockMovement)dgvMovements.CurrentRow.DataBoundItem;
            LoadMovementToForm(movement);
            EnableForm(false); // View only mode
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (!ValidateForm())
                    return;

                // Create movement object
                var movement = new StockMovement
                {
                    MovementType = (MovementType)cmbMovementType.SelectedItem,
                    MovementDate = dtpMovementDate.Value,
                    SourceWarehouseId = GetSelectedWarehouseId(cmbSourceWarehouse),
                    DestinationWarehouseId = GetSelectedWarehouseId(cmbDestinationWarehouse),
                    Reason = txtReason.Text.Trim(),
                    Notes = txtNotes.Text.Trim()
                };

                // Collect lines
                var lines = new List<StockMovementLine>();
                foreach (DataGridViewRow row in dgvLines.Rows)
                {
                    if (row.IsNewRow) continue;
                    
                    var productId = row.Cells[colProduct.Index].Value;
                    var quantity = row.Cells[colQuantity.Index].Value;
                    var unitPrice = row.Cells[colUnitPrice.Index].Value;

                    if (productId == null || quantity == null)
                        continue;

                    lines.Add(new StockMovementLine
                    {
                        ProductId = Convert.ToInt32(productId),
                        Quantity = Convert.ToInt32(quantity),
                        UnitPrice = unitPrice != null ? Convert.ToDecimal(unitPrice) : (decimal?)null
                    });
                }

                if (lines.Count == 0)
                {
                    MessageBox.Show(
                        "Debe agregar al menos un producto al movimiento.",
                        "Validación",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);
                    return;
                }

                // Create movement
                var movementId = _movementService.CreateMovement(movement, lines);

                MessageBox.Show(
                    "Movimiento creado exitosamente.",
                    "Éxito",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);

                LoadMovements();
                EnableForm(false);
                _isCreating = false;
            }
            catch (Exception ex)
            {
                _errorHandler.ShowError(ex, "Error al guardar el movimiento");
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
            // DataGridView allows adding new rows automatically
            dgvLines.Focus();
        }

        private void btnRemoveLine_Click(object sender, EventArgs e)
        {
            if (dgvLines.CurrentRow != null && !dgvLines.CurrentRow.IsNewRow)
            {
                dgvLines.Rows.Remove(dgvLines.CurrentRow);
            }
        }

        private void cmbMovementTypeFilter_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadMovements();
        }

        private void cmbMovementType_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateWarehouseVisibility();
        }

        private void UpdateWarehouseVisibility()
        {
            if (cmbMovementType.SelectedItem == null)
                return;

            var movementType = (MovementType)cmbMovementType.SelectedItem;

            switch (movementType)
            {
                case MovementType.In:
                    lblSourceWarehouse.Enabled = false;
                    cmbSourceWarehouse.Enabled = false;
                    cmbSourceWarehouse.SelectedIndex = 0;
                    lblDestinationWarehouse.Enabled = true;
                    cmbDestinationWarehouse.Enabled = true;
                    break;

                case MovementType.Out:
                    lblSourceWarehouse.Enabled = true;
                    cmbSourceWarehouse.Enabled = true;
                    lblDestinationWarehouse.Enabled = false;
                    cmbDestinationWarehouse.Enabled = false;
                    cmbDestinationWarehouse.SelectedIndex = 0;
                    break;

                case MovementType.Transfer:
                    lblSourceWarehouse.Enabled = true;
                    cmbSourceWarehouse.Enabled = true;
                    lblDestinationWarehouse.Enabled = true;
                    cmbDestinationWarehouse.Enabled = true;
                    break;

                case MovementType.Adjustment:
                    lblSourceWarehouse.Enabled = false;
                    cmbSourceWarehouse.Enabled = false;
                    cmbSourceWarehouse.SelectedIndex = 0;
                    lblDestinationWarehouse.Enabled = true;
                    cmbDestinationWarehouse.Enabled = true;
                    break;
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
            dgvMovements.Enabled = !enable;
            cmbMovementTypeFilter.Enabled = !enable;
        }

        private void ClearForm()
        {
            cmbMovementType.SelectedIndex = 0;
            dtpMovementDate.Value = DateTime.Now;
            cmbSourceWarehouse.SelectedIndex = 0;
            cmbDestinationWarehouse.SelectedIndex = 0;
            txtReason.Clear();
            txtNotes.Clear();
            dgvLines.Rows.Clear();
            UpdateWarehouseVisibility();
        }

        private void LoadMovementToForm(StockMovement movement)
        {
            cmbMovementType.SelectedItem = movement.MovementType;
            dtpMovementDate.Value = movement.MovementDate;
            
            // Set source warehouse
            if (movement.SourceWarehouseId.HasValue)
            {
                for (int i = 0; i < cmbSourceWarehouse.Items.Count; i++)
                {
                    var item = (ComboBoxItem)cmbSourceWarehouse.Items[i];
                    if (item.Value != null && (int)item.Value == movement.SourceWarehouseId.Value)
                    {
                        cmbSourceWarehouse.SelectedIndex = i;
                        break;
                    }
                }
            }
            else
            {
                cmbSourceWarehouse.SelectedIndex = 0;
            }
            
            // Set destination warehouse
            if (movement.DestinationWarehouseId.HasValue)
            {
                for (int i = 0; i < cmbDestinationWarehouse.Items.Count; i++)
                {
                    var item = (ComboBoxItem)cmbDestinationWarehouse.Items[i];
                    if (item.Value != null && (int)item.Value == movement.DestinationWarehouseId.Value)
                    {
                        cmbDestinationWarehouse.SelectedIndex = i;
                        break;
                    }
                }
            }
            else
            {
                cmbDestinationWarehouse.SelectedIndex = 0;
            }
            
            txtReason.Text = movement.Reason ?? "";
            txtNotes.Text = movement.Notes ?? "";
            
            // Load lines
            dgvLines.Rows.Clear();
            var lines = _movementService.GetMovementLines(movement.MovementId);
            foreach (var line in lines)
            {
                var rowIndex = dgvLines.Rows.Add();
                dgvLines.Rows[rowIndex].Cells[colProduct.Index].Value = line.ProductId;
                dgvLines.Rows[rowIndex].Cells[colQuantity.Index].Value = line.Quantity;
                dgvLines.Rows[rowIndex].Cells[colUnitPrice.Index].Value = line.UnitPrice;
            }
            
            UpdateWarehouseVisibility();
        }

        private bool ValidateForm()
        {
            if (cmbMovementType.SelectedItem == null)
            {
                MessageBox.Show("Debe seleccionar un tipo de movimiento.", "Validación", 
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                cmbMovementType.Focus();
                return false;
            }

            var movementType = (MovementType)cmbMovementType.SelectedItem;
            
            // Validate warehouses based on movement type
            switch (movementType)
            {
                case MovementType.In:
                    if (GetSelectedWarehouseId(cmbDestinationWarehouse) == null)
                    {
                        MessageBox.Show("Debe seleccionar un almacén de destino para entradas.", 
                            "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        cmbDestinationWarehouse.Focus();
                        return false;
                    }
                    break;

                case MovementType.Out:
                    if (GetSelectedWarehouseId(cmbSourceWarehouse) == null)
                    {
                        MessageBox.Show("Debe seleccionar un almacén de origen para salidas.", 
                            "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        cmbSourceWarehouse.Focus();
                        return false;
                    }
                    break;

                case MovementType.Transfer:
                    if (GetSelectedWarehouseId(cmbSourceWarehouse) == null)
                    {
                        MessageBox.Show("Debe seleccionar un almacén de origen para transferencias.", 
                            "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        cmbSourceWarehouse.Focus();
                        return false;
                    }
                    if (GetSelectedWarehouseId(cmbDestinationWarehouse) == null)
                    {
                        MessageBox.Show("Debe seleccionar un almacén de destino para transferencias.", 
                            "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        cmbDestinationWarehouse.Focus();
                        return false;
                    }
                    if (GetSelectedWarehouseId(cmbSourceWarehouse) == GetSelectedWarehouseId(cmbDestinationWarehouse))
                    {
                        MessageBox.Show("Los almacenes de origen y destino deben ser diferentes.", 
                            "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        cmbDestinationWarehouse.Focus();
                        return false;
                    }
                    break;

                case MovementType.Adjustment:
                    if (GetSelectedWarehouseId(cmbDestinationWarehouse) == null)
                    {
                        MessageBox.Show("Debe seleccionar un almacén para ajustes.", 
                            "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        cmbDestinationWarehouse.Focus();
                        return false;
                    }
                    if (string.IsNullOrWhiteSpace(txtReason.Text))
                    {
                        MessageBox.Show("Debe especificar un motivo para ajustes de inventario.", 
                            "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        txtReason.Focus();
                        return false;
                    }
                    break;
            }

            return true;
        }

        private int? GetSelectedWarehouseId(ComboBox comboBox)
        {
            if (comboBox.SelectedItem == null)
                return null;

            var item = (ComboBoxItem)comboBox.SelectedItem;
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
        }
    }
}
