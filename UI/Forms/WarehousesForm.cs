using System;
using System.Windows.Forms;
using BLL.Services;
using DAO.Repositories;
using DOMAIN.Entities;
using SERVICES;
using SERVICES.Implementations;
using SERVICES.Interfaces;

namespace UI.Forms
{
    public partial class WarehousesForm : Form
    {
        private readonly WarehouseService _warehouseService;
        private readonly IAuthorizationService _authorizationService;
        private readonly ILocalizationService _localizationService;
        private readonly ILogService _logService;
        private readonly IErrorHandlerService _errorHandler;
        private Warehouse _currentWarehouse;
        private bool _isEditing = false;

        public WarehousesForm()
        {
            InitializeComponent();

            // Initialize services
            _logService = new FileLogService();
            var warehouseRepo = new WarehouseRepository();
            var auditRepo = new AuditLogRepository();
            _warehouseService = new WarehouseService(warehouseRepo, auditRepo, _logService);
            
            var permissionRepo = new PermissionRepository();
            _authorizationService = new AuthorizationService(permissionRepo, _logService);
            _localizationService = new LocalizationService();
            _errorHandler = new ErrorHandlerService(_localizationService, _logService);

            ApplyLocalization();
            ConfigurePermissions();
            LoadWarehouses();
        }

        private void ApplyLocalization()
        {
            this.Text = _localizationService.GetString("Warehouses.Title") ?? "Gestión de Almacenes";
            
            grpList.Text = _localizationService.GetString("Warehouses.List") ?? "Lista de Almacenes";
            grpDetails.Text = _localizationService.GetString("Warehouses.Details") ?? "Detalles del Almacén";
            
            lblCode.Text = _localizationService.GetString("Warehouses.Code") ?? "Código:";
            lblName.Text = _localizationService.GetString("Warehouses.Name") ?? "Nombre:";
            lblAddress.Text = _localizationService.GetString("Warehouses.Address") ?? "Dirección:";
            
            btnNew.Text = _localizationService.GetString("Common.New") ?? "Nuevo";
            btnEdit.Text = _localizationService.GetString("Common.Edit") ?? "Editar";
            btnDelete.Text = _localizationService.GetString("Common.Delete") ?? "Eliminar";
            btnSave.Text = _localizationService.GetString("Common.Save") ?? "Guardar";
            btnCancel.Text = _localizationService.GetString("Common.Cancel") ?? "Cancelar";
            
            colCode.HeaderText = _localizationService.GetString("Warehouses.Code") ?? "Código";
            colName.HeaderText = _localizationService.GetString("Warehouses.Name") ?? "Nombre";
            colAddress.HeaderText = _localizationService.GetString("Warehouses.Address") ?? "Dirección";
        }

        private void ConfigurePermissions()
        {
            if (!SessionContext.CurrentUserId.HasValue)
                return;

            var userId = SessionContext.CurrentUserId.Value;
            
            btnNew.Enabled = _authorizationService.HasPermission(userId, "Warehouses.Create");
            btnEdit.Enabled = _authorizationService.HasPermission(userId, "Warehouses.Edit");
            btnDelete.Enabled = _authorizationService.HasPermission(userId, "Warehouses.Delete");
        }

        private void LoadWarehouses()
        {
            try
            {
                var warehouses = _warehouseService.GetActiveWarehouses();
                dgvWarehouses.DataSource = warehouses;
                
                // Hide unnecessary columns
                if (dgvWarehouses.Columns["WarehouseId"] != null)
                    dgvWarehouses.Columns["WarehouseId"].Visible = false;
                if (dgvWarehouses.Columns["IsActive"] != null)
                    dgvWarehouses.Columns["IsActive"].Visible = false;
                if (dgvWarehouses.Columns["CreatedAt"] != null)
                    dgvWarehouses.Columns["CreatedAt"].Visible = false;
                if (dgvWarehouses.Columns["CreatedBy"] != null)
                    dgvWarehouses.Columns["CreatedBy"].Visible = false;
                if (dgvWarehouses.Columns["UpdatedAt"] != null)
                    dgvWarehouses.Columns["UpdatedAt"].Visible = false;
                if (dgvWarehouses.Columns["UpdatedBy"] != null)
                    dgvWarehouses.Columns["UpdatedBy"].Visible = false;
                
                EnableForm(false);
            }
            catch (Exception ex)
            {
                _errorHandler.ShowError(ex, _localizationService.GetString("Error.LoadingWarehouses") ?? "Error al cargar almacenes");
            }
        }

        private void btnNew_Click(object sender, EventArgs e)
        {
            _currentWarehouse = new Warehouse();
            _isEditing = false;
            ClearForm();
            EnableForm(true);
            txtCode.Focus();
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (dgvWarehouses.CurrentRow == null)
            {
                MessageBox.Show(
                    _localizationService.GetString("Warehouses.SelectWarehouse") ?? "Por favor seleccione un almacén.",
                    _localizationService.GetString("Common.Validation") ?? "Validación",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                return;
            }

            _currentWarehouse = (Warehouse)dgvWarehouses.CurrentRow.DataBoundItem;
            _isEditing = true;
            LoadWarehouseToForm(_currentWarehouse);
            EnableForm(true);
            txtName.Focus();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (dgvWarehouses.CurrentRow == null)
            {
                MessageBox.Show(
                    _localizationService.GetString("Warehouses.SelectWarehouse") ?? "Por favor seleccione un almacén.",
                    _localizationService.GetString("Common.Validation") ?? "Validación",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                return;
            }

            var warehouse = (Warehouse)dgvWarehouses.CurrentRow.DataBoundItem;
            
            var result = MessageBox.Show(
                string.Format(_localizationService.GetString("Warehouses.ConfirmDelete") ?? "¿Está seguro que desea eliminar el almacén '{0}'?", warehouse.Name),
                _localizationService.GetString("Common.Confirmation") ?? "Confirmación",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                try
                {
                    _warehouseService.DeleteWarehouse(warehouse.WarehouseId);
                    MessageBox.Show(
                        _localizationService.GetString("Warehouses.DeleteSuccess") ?? "Almacén eliminado exitosamente.",
                        _localizationService.GetString("Common.Success") ?? "Éxito",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);
                    LoadWarehouses();
                }
                catch (Exception ex)
                {
                    _errorHandler.ShowError(ex, _localizationService.GetString("Error.DeletingWarehouse") ?? "Error al eliminar almacén");
                }
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (!ValidateForm())
                return;

            try
            {
                GetWarehouseFromForm();

                if (_isEditing)
                {
                    _warehouseService.UpdateWarehouse(_currentWarehouse);
                    MessageBox.Show(
                        _localizationService.GetString("Warehouses.UpdateSuccess") ?? "Almacén actualizado exitosamente.",
                        _localizationService.GetString("Common.Success") ?? "Éxito",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);
                }
                else
                {
                    _warehouseService.CreateWarehouse(_currentWarehouse);
                    MessageBox.Show(
                        _localizationService.GetString("Warehouses.CreateSuccess") ?? "Almacén creado exitosamente.",
                        _localizationService.GetString("Common.Success") ?? "Éxito",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);
                }

                LoadWarehouses();
            }
            catch (Exception ex)
            {
                _errorHandler.ShowError(ex, _localizationService.GetString("Error.SavingWarehouse") ?? "Error al guardar almacén");
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            EnableForm(false);
            ClearForm();
        }

        private bool ValidateForm()
        {
            if (string.IsNullOrWhiteSpace(txtCode.Text))
            {
                MessageBox.Show(
                    _localizationService.GetString("Warehouses.CodeRequired") ?? "El código es requerido.",
                    _localizationService.GetString("Common.Validation") ?? "Validación",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                txtCode.Focus();
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtName.Text))
            {
                MessageBox.Show(
                    _localizationService.GetString("Warehouses.NameRequired") ?? "El nombre es requerido.",
                    _localizationService.GetString("Common.Validation") ?? "Validación",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                txtName.Focus();
                return false;
            }

            return true;
        }

        private void LoadWarehouseToForm(Warehouse warehouse)
        {
            txtCode.Text = warehouse.Code;
            txtName.Text = warehouse.Name;
            txtAddress.Text = warehouse.Address;
        }

        private void GetWarehouseFromForm()
        {
            _currentWarehouse.Code = txtCode.Text.Trim();
            _currentWarehouse.Name = txtName.Text.Trim();
            _currentWarehouse.Address = txtAddress.Text.Trim();
        }

        private void ClearForm()
        {
            txtCode.Clear();
            txtName.Clear();
            txtAddress.Clear();
        }

        private void EnableForm(bool enabled)
        {
            grpDetails.Enabled = enabled;
            btnSave.Enabled = enabled;
            btnCancel.Enabled = enabled;
            
            grpList.Enabled = !enabled;
            btnNew.Enabled = !enabled;
            btnEdit.Enabled = !enabled;
            btnDelete.Enabled = !enabled;
            
            txtCode.ReadOnly = _isEditing;
        }
    }
}
