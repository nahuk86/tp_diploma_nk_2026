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

        /// <summary>
        /// Inicializa una nueva instancia del formulario de gestión de almacenes
        /// </summary>
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
            _localizationService = LocalizationService.Instance;
            _errorHandler = new ErrorHandlerService(_logService, _localizationService);

            ApplyLocalization();
            ConfigurePermissions();
            LoadWarehouses();
        }

        /// <summary>
        /// Aplica la localización a todos los controles del formulario según el idioma actual
        /// </summary>
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

        /// <summary>
        /// Configura los permisos de los botones según los permisos del usuario actual
        /// </summary>
        private void ConfigurePermissions()
        {
            if (!SessionContext.Instance.CurrentUserId.HasValue)
                return;

            var userId = SessionContext.Instance.CurrentUserId.Value;
            
            btnNew.Enabled = _authorizationService.HasPermission(userId, "Warehouses.Create");
            btnEdit.Enabled = _authorizationService.HasPermission(userId, "Warehouses.Edit");
            btnDelete.Enabled = _authorizationService.HasPermission(userId, "Warehouses.Delete");
        }

        /// <summary>
        /// Carga la lista de almacenes activos en el DataGridView
        /// </summary>
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

        /// <summary>
        /// Maneja el evento Click del botón Nuevo para crear un nuevo almacén
        /// </summary>
        /// <param name="sender">El objeto que generó el evento</param>
        /// <param name="e">Los datos del evento</param>
        private void btnNew_Click(object sender, EventArgs e)
        {
            _currentWarehouse = new Warehouse();
            _isEditing = false;
            ClearForm();
            EnableForm(true);
            txtCode.Focus();
        }

        /// <summary>
        /// Maneja el evento Click del botón Editar para modificar el almacén seleccionado
        /// </summary>
        /// <param name="sender">El objeto que generó el evento</param>
        /// <param name="e">Los datos del evento</param>
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

        /// <summary>
        /// Maneja el evento Click del botón Eliminar para eliminar el almacén seleccionado
        /// </summary>
        /// <param name="sender">El objeto que generó el evento</param>
        /// <param name="e">Los datos del evento</param>
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

        /// <summary>
        /// Maneja el evento Click del botón Guardar para guardar los cambios del almacén
        /// </summary>
        /// <param name="sender">El objeto que generó el evento</param>
        /// <param name="e">Los datos del evento</param>
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

        /// <summary>
        /// Maneja el evento Click del botón Cancelar para cancelar la operación actual
        /// </summary>
        /// <param name="sender">El objeto que generó el evento</param>
        /// <param name="e">Los datos del evento</param>
        private void btnCancel_Click(object sender, EventArgs e)
        {
            EnableForm(false);
            ClearForm();
        }

        /// <summary>
        /// Valida que los campos del formulario contengan datos válidos
        /// </summary>
        /// <returns>True si el formulario es válido; de lo contrario, False</returns>
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

        /// <summary>
        /// Carga los datos del almacén en los controles del formulario
        /// </summary>
        /// <param name="warehouse">El almacén cuyos datos se cargarán en el formulario</param>
        private void LoadWarehouseToForm(Warehouse warehouse)
        {
            txtCode.Text = warehouse.Code;
            txtName.Text = warehouse.Name;
            txtAddress.Text = warehouse.Address;
        }

        /// <summary>
        /// Obtiene los datos del formulario y los asigna al almacén actual
        /// </summary>
        private void GetWarehouseFromForm()
        {
            _currentWarehouse.Code = txtCode.Text.Trim();
            _currentWarehouse.Name = txtName.Text.Trim();
            _currentWarehouse.Address = txtAddress.Text.Trim();
        }

        /// <summary>
        /// Limpia todos los campos del formulario
        /// </summary>
        private void ClearForm()
        {
            txtCode.Clear();
            txtName.Clear();
            txtAddress.Clear();
        }

        /// <summary>
        /// Habilita o deshabilita los controles del formulario según el estado de edición
        /// </summary>
        /// <param name="enabled">True para habilitar los controles de edición; False para deshabilitarlos</param>
        private void EnableForm(bool enabled)
        {
            grpDetails.Enabled = enabled;
            btnSave.Enabled = enabled;
            btnCancel.Enabled = enabled;
            
            grpList.Enabled = !enabled;
            
            // Re-apply permissions when disabling form
            if (!enabled)
            {
                ConfigurePermissions();
            }
            
            txtCode.ReadOnly = _isEditing;
        }
    }
}
