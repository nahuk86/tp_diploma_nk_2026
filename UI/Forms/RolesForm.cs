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
    public partial class RolesForm : Form
    {
        private readonly RoleService _roleService;
        private readonly IAuthorizationService _authorizationService;
        private readonly ILocalizationService _localizationService;
        private readonly ILogService _logService;
        private readonly IErrorHandlerService _errorHandler;
        private Role _currentRole;
        private bool _isEditing = false;

        public RolesForm()
        {
            InitializeComponent();

            // Initialize services
            _logService = new FileLogService();
            var roleRepo = new RoleRepository();
            var permissionRepo = new PermissionRepository();
            var auditRepo = new AuditLogRepository();
            _roleService = new RoleService(roleRepo, permissionRepo, auditRepo, _logService);
            
            _authorizationService = new AuthorizationService(permissionRepo, _logService);
            _localizationService = LocalizationService.Instance;
            _errorHandler = new ErrorHandlerService(_logService, _localizationService);

            ApplyLocalization();
            ConfigurePermissions();
            LoadRoles();
        }

        private void ApplyLocalization()
        {
            this.Text = _localizationService.GetString("Roles.Title") ?? "Gestión de Roles";
            
            grpList.Text = _localizationService.GetString("Roles.List") ?? "Lista de Roles";
            grpDetails.Text = _localizationService.GetString("Roles.Details") ?? "Detalles del Rol";
            
            lblRoleName.Text = _localizationService.GetString("Roles.RoleName") ?? "Nombre del Rol:";
            lblDescription.Text = _localizationService.GetString("Roles.Description") ?? "Descripción:";
            
            btnNew.Text = _localizationService.GetString("Common.New") ?? "Nuevo";
            btnEdit.Text = _localizationService.GetString("Common.Edit") ?? "Editar";
            btnDelete.Text = _localizationService.GetString("Common.Delete") ?? "Eliminar";
            btnSave.Text = _localizationService.GetString("Common.Save") ?? "Guardar";
            btnCancel.Text = _localizationService.GetString("Common.Cancel") ?? "Cancelar";
            btnManagePermissions.Text = _localizationService.GetString("Roles.ManagePermissions") ?? "Gestionar Permisos";
            
            colRoleName.HeaderText = _localizationService.GetString("Roles.RoleName") ?? "Nombre del Rol";
            colDescription.HeaderText = _localizationService.GetString("Roles.Description") ?? "Descripción";
        }

        private void ConfigurePermissions()
        {
            if (!SessionContext.CurrentUserId.HasValue)
                return;

            var userId = SessionContext.CurrentUserId.Value;
            
            btnNew.Enabled = _authorizationService.HasPermission(userId, "Roles.Create");
            btnEdit.Enabled = _authorizationService.HasPermission(userId, "Roles.Edit");
            btnDelete.Enabled = _authorizationService.HasPermission(userId, "Roles.Delete");
            btnManagePermissions.Enabled = _authorizationService.HasPermission(userId, "Roles.Edit");
        }

        private void LoadRoles()
        {
            try
            {
                var roles = _roleService.GetActiveRoles();
                dgvRoles.DataSource = roles;
                
                // Hide unnecessary columns
                if (dgvRoles.Columns["RoleId"] != null)
                    dgvRoles.Columns["RoleId"].Visible = false;
                if (dgvRoles.Columns["IsActive"] != null)
                    dgvRoles.Columns["IsActive"].Visible = false;
                if (dgvRoles.Columns["CreatedAt"] != null)
                    dgvRoles.Columns["CreatedAt"].Visible = false;
                if (dgvRoles.Columns["CreatedBy"] != null)
                    dgvRoles.Columns["CreatedBy"].Visible = false;
                if (dgvRoles.Columns["UpdatedAt"] != null)
                    dgvRoles.Columns["UpdatedAt"].Visible = false;
                if (dgvRoles.Columns["UpdatedBy"] != null)
                    dgvRoles.Columns["UpdatedBy"].Visible = false;
                
                EnableForm(false);
            }
            catch (Exception ex)
            {
                _errorHandler.ShowError(ex, _localizationService.GetString("Error.LoadingRoles") ?? "Error al cargar roles");
            }
        }

        private void btnNew_Click(object sender, EventArgs e)
        {
            _currentRole = new Role();
            _isEditing = false;
            ClearForm();
            EnableForm(true);
            txtRoleName.Focus();
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (dgvRoles.CurrentRow == null)
            {
                MessageBox.Show(
                    _localizationService.GetString("Roles.SelectRole") ?? "Por favor seleccione un rol.",
                    _localizationService.GetString("Common.Validation") ?? "Validación",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                return;
            }

            _currentRole = (Role)dgvRoles.CurrentRow.DataBoundItem;
            _isEditing = true;
            LoadRoleToForm(_currentRole);
            EnableForm(true);
            txtDescription.Focus();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (dgvRoles.CurrentRow == null)
            {
                MessageBox.Show(
                    _localizationService.GetString("Roles.SelectRole") ?? "Por favor seleccione un rol.",
                    _localizationService.GetString("Common.Validation") ?? "Validación",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                return;
            }

            var role = (Role)dgvRoles.CurrentRow.DataBoundItem;
            
            var result = MessageBox.Show(
                string.Format(_localizationService.GetString("Roles.ConfirmDelete") ?? "¿Está seguro que desea eliminar el rol '{0}'?", role.RoleName),
                _localizationService.GetString("Common.Confirmation") ?? "Confirmación",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                try
                {
                    _roleService.DeleteRole(role.RoleId);
                    MessageBox.Show(
                        _localizationService.GetString("Roles.DeleteSuccess") ?? "Rol eliminado exitosamente.",
                        _localizationService.GetString("Common.Success") ?? "Éxito",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);
                    LoadRoles();
                }
                catch (Exception ex)
                {
                    _errorHandler.ShowError(ex, _localizationService.GetString("Error.DeletingRole") ?? "Error al eliminar rol");
                }
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (!ValidateForm())
                return;

            try
            {
                GetRoleFromForm();

                if (_isEditing)
                {
                    _roleService.UpdateRole(_currentRole);
                    MessageBox.Show(
                        _localizationService.GetString("Roles.UpdateSuccess") ?? "Rol actualizado exitosamente.",
                        _localizationService.GetString("Common.Success") ?? "Éxito",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);
                }
                else
                {
                    _roleService.CreateRole(_currentRole);
                    MessageBox.Show(
                        _localizationService.GetString("Roles.CreateSuccess") ?? "Rol creado exitosamente.",
                        _localizationService.GetString("Common.Success") ?? "Éxito",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);
                }

                LoadRoles();
            }
            catch (Exception ex)
            {
                _errorHandler.ShowError(ex, _localizationService.GetString("Error.SavingRole") ?? "Error al guardar rol");
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            EnableForm(false);
            ClearForm();
        }

        private void btnManagePermissions_Click(object sender, EventArgs e)
        {
            if (dgvRoles.CurrentRow == null)
            {
                MessageBox.Show(
                    _localizationService.GetString("Roles.SelectRole") ?? "Por favor seleccione un rol.",
                    _localizationService.GetString("Common.Validation") ?? "Validación",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                return;
            }

            var role = (Role)dgvRoles.CurrentRow.DataBoundItem;
            
            var permissionsForm = new RolePermissionsForm(role.RoleId, role.RoleName, _roleService);
            permissionsForm.ShowDialog();
        }

        private bool ValidateForm()
        {
            if (string.IsNullOrWhiteSpace(txtRoleName.Text))
            {
                MessageBox.Show(
                    _localizationService.GetString("Roles.RoleNameRequired") ?? "El nombre del rol es requerido.",
                    _localizationService.GetString("Common.Validation") ?? "Validación",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                txtRoleName.Focus();
                return false;
            }

            return true;
        }

        private void LoadRoleToForm(Role role)
        {
            txtRoleName.Text = role.RoleName;
            txtDescription.Text = role.Description;
        }

        private void GetRoleFromForm()
        {
            _currentRole.RoleName = txtRoleName.Text.Trim();
            _currentRole.Description = txtDescription.Text.Trim();
        }

        private void ClearForm()
        {
            txtRoleName.Clear();
            txtDescription.Clear();
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
            btnManagePermissions.Enabled = !enabled;
            
            txtRoleName.ReadOnly = _isEditing;
        }
    }
}
