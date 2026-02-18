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
    public partial class UsersForm : Form
    {
        private readonly UserService _userService;
        private readonly RoleService _roleService;
        private readonly IAuthorizationService _authorizationService;
        private readonly ILocalizationService _localizationService;
        private readonly ILogService _logService;
        private readonly IErrorHandlerService _errorHandler;
        private User _currentUser;
        private bool _isEditing = false;

        /// <summary>
        /// Inicializa una nueva instancia del formulario de gestión de usuarios
        /// </summary>
        public UsersForm()
        {
            InitializeComponent();

            // Initialize services
            _logService = new FileLogService();
            var userRepo = new UserRepository();
            var auditRepo = new AuditLogRepository();
            var authService = new AuthenticationService(userRepo, _logService);
            _userService = new UserService(userRepo, auditRepo, _logService, authService);
            
            var roleRepo = new RoleRepository();
            var permissionRepo = new PermissionRepository();
            _roleService = new RoleService(roleRepo, permissionRepo, auditRepo, _logService);
            
            _authorizationService = new AuthorizationService(permissionRepo, _logService);
            _localizationService = LocalizationService.Instance;
            _errorHandler = new ErrorHandlerService(_logService, _localizationService);

            ApplyLocalization();
            ConfigurePermissions();
            LoadUsers();
        }

        /// <summary>
        /// Aplica las traducciones localizadas a todos los controles del formulario
        /// </summary>
        private void ApplyLocalization()
        {
            this.Text = _localizationService.GetString("Users.Title") ?? "Gestión de Usuarios";
            
            grpList.Text = _localizationService.GetString("Users.List") ?? "Lista de Usuarios";
            grpDetails.Text = _localizationService.GetString("Users.Details") ?? "Detalles del Usuario";
            
            lblUsername.Text = _localizationService.GetString("Users.Username") ?? "Usuario:";
            lblFullName.Text = _localizationService.GetString("Users.FullName") ?? "Nombre Completo:";
            lblEmail.Text = _localizationService.GetString("Users.Email") ?? "Email:";
            lblPassword.Text = _localizationService.GetString("Users.Password") ?? "Contraseña:";
            
            btnNew.Text = _localizationService.GetString("Common.New") ?? "Nuevo";
            btnEdit.Text = _localizationService.GetString("Common.Edit") ?? "Editar";
            btnDelete.Text = _localizationService.GetString("Common.Delete") ?? "Eliminar";
            btnSave.Text = _localizationService.GetString("Common.Save") ?? "Guardar";
            btnCancel.Text = _localizationService.GetString("Common.Cancel") ?? "Cancelar";
            btnChangePassword.Text = _localizationService.GetString("Users.ChangePassword") ?? "Cambiar Contraseña";
            btnAssignRoles.Text = _localizationService.GetString("Users.AssignRoles") ?? "Asignar Roles";
            
            colUsername.HeaderText = _localizationService.GetString("Users.Username") ?? "Usuario";
            colFullName.HeaderText = _localizationService.GetString("Users.FullName") ?? "Nombre Completo";
            colEmail.HeaderText = _localizationService.GetString("Users.Email") ?? "Email";
        }

        /// <summary>
        /// Configura los permisos de los botones del formulario basándose en los permisos del usuario actual
        /// </summary>
        private void ConfigurePermissions()
        {
            if (!SessionContext.CurrentUserId.HasValue)
                return;

            var userId = SessionContext.CurrentUserId.Value;
            
            btnNew.Enabled = _authorizationService.HasPermission(userId, "Users.Create");
            btnEdit.Enabled = _authorizationService.HasPermission(userId, "Users.Edit");
            btnDelete.Enabled = _authorizationService.HasPermission(userId, "Users.Delete");
            btnAssignRoles.Enabled = _authorizationService.HasPermission(userId, "Users.Edit");
        }

        /// <summary>
        /// Carga la lista de usuarios activos en el DataGridView
        /// </summary>
        private void LoadUsers()
        {
            try
            {
                var users = _userService.GetActiveUsers();
                dgvUsers.DataSource = users;
                
                // Hide unnecessary columns
                if (dgvUsers.Columns["UserId"] != null)
                    dgvUsers.Columns["UserId"].Visible = false;
                if (dgvUsers.Columns["PasswordHash"] != null)
                    dgvUsers.Columns["PasswordHash"].Visible = false;
                if (dgvUsers.Columns["PasswordSalt"] != null)
                    dgvUsers.Columns["PasswordSalt"].Visible = false;
                if (dgvUsers.Columns["IsActive"] != null)
                    dgvUsers.Columns["IsActive"].Visible = false;
                if (dgvUsers.Columns["CreatedAt"] != null)
                    dgvUsers.Columns["CreatedAt"].Visible = false;
                if (dgvUsers.Columns["CreatedBy"] != null)
                    dgvUsers.Columns["CreatedBy"].Visible = false;
                if (dgvUsers.Columns["UpdatedAt"] != null)
                    dgvUsers.Columns["UpdatedAt"].Visible = false;
                if (dgvUsers.Columns["UpdatedBy"] != null)
                    dgvUsers.Columns["UpdatedBy"].Visible = false;
                if (dgvUsers.Columns["Roles"] != null)
                    dgvUsers.Columns["Roles"].Visible = false;
                
                EnableForm(false);
            }
            catch (Exception ex)
            {
                _errorHandler.ShowError(ex, _localizationService.GetString("Error.LoadingUsers") ?? "Error al cargar usuarios");
            }
        }

        /// <summary>
        /// Maneja el evento Click del botón Nuevo para crear un nuevo usuario
        /// </summary>
        /// <param name="sender">El objeto que generó el evento</param>
        /// <param name="e">Los datos del evento</param>
        private void btnNew_Click(object sender, EventArgs e)
        {
            _currentUser = new User();
            _isEditing = false;
            ClearForm();
            EnableForm(true);
            lblPassword.Visible = true;
            txtPassword.Visible = true;
            txtUsername.Focus();
        }

        /// <summary>
        /// Maneja el evento Click del botón Editar para modificar un usuario existente
        /// </summary>
        /// <param name="sender">El objeto que generó el evento</param>
        /// <param name="e">Los datos del evento</param>
        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (dgvUsers.CurrentRow == null)
            {
                MessageBox.Show(
                    _localizationService.GetString("Users.SelectUser") ?? "Por favor seleccione un usuario.",
                    _localizationService.GetString("Common.Validation") ?? "Validación",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                return;
            }

            _currentUser = (User)dgvUsers.CurrentRow.DataBoundItem;
            _isEditing = true;
            LoadUserToForm(_currentUser);
            EnableForm(true);
            lblPassword.Visible = false;
            txtPassword.Visible = false;
            txtFullName.Focus();
        }

        /// <summary>
        /// Maneja el evento Click del botón Eliminar para dar de baja un usuario
        /// </summary>
        /// <param name="sender">El objeto que generó el evento</param>
        /// <param name="e">Los datos del evento</param>
        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (dgvUsers.CurrentRow == null)
            {
                MessageBox.Show(
                    _localizationService.GetString("Users.SelectUser") ?? "Por favor seleccione un usuario.",
                    _localizationService.GetString("Common.Validation") ?? "Validación",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                return;
            }

            var user = (User)dgvUsers.CurrentRow.DataBoundItem;
            
            var result = MessageBox.Show(
                string.Format(_localizationService.GetString("Users.ConfirmDelete") ?? "¿Está seguro que desea eliminar el usuario '{0}'?", user.Username),
                _localizationService.GetString("Common.Confirmation") ?? "Confirmación",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                try
                {
                    _userService.DeleteUser(user.UserId);
                    MessageBox.Show(
                        _localizationService.GetString("Users.DeleteSuccess") ?? "Usuario eliminado exitosamente.",
                        _localizationService.GetString("Common.Success") ?? "Éxito",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);
                    LoadUsers();
                }
                catch (Exception ex)
                {
                    _errorHandler.ShowError(ex, _localizationService.GetString("Error.DeletingUser") ?? "Error al eliminar usuario");
                }
            }
        }

        /// <summary>
        /// Maneja el evento Click del botón Guardar para crear o actualizar un usuario
        /// </summary>
        /// <param name="sender">El objeto que generó el evento</param>
        /// <param name="e">Los datos del evento</param>
        private void btnSave_Click(object sender, EventArgs e)
        {
            if (!ValidateForm())
                return;

            try
            {
                GetUserFromForm();

                if (_isEditing)
                {
                    _userService.UpdateUser(_currentUser);
                    MessageBox.Show(
                        _localizationService.GetString("Users.UpdateSuccess") ?? "Usuario actualizado exitosamente.",
                        _localizationService.GetString("Common.Success") ?? "Éxito",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);
                }
                else
                {
                    var password = txtPassword.Text;
                    _userService.CreateUser(_currentUser, password);
                    MessageBox.Show(
                        _localizationService.GetString("Users.CreateSuccess") ?? "Usuario creado exitosamente.",
                        _localizationService.GetString("Common.Success") ?? "Éxito",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);
                }

                LoadUsers();
            }
            catch (Exception ex)
            {
                _errorHandler.ShowError(ex, _localizationService.GetString("Error.SavingUser") ?? "Error al guardar usuario");
            }
        }

        /// <summary>
        /// Maneja el evento Click del botón Cancelar para descartar los cambios y cerrar el formulario de edición
        /// </summary>
        /// <param name="sender">El objeto que generó el evento</param>
        /// <param name="e">Los datos del evento</param>
        private void btnCancel_Click(object sender, EventArgs e)
        {
            EnableForm(false);
            ClearForm();
        }

        /// <summary>
        /// Maneja el evento Click del botón Cambiar Contraseña para modificar la contraseña de un usuario
        /// </summary>
        /// <param name="sender">El objeto que generó el evento</param>
        /// <param name="e">Los datos del evento</param>
        private void btnChangePassword_Click(object sender, EventArgs e)
        {
            if (dgvUsers.CurrentRow == null)
            {
                MessageBox.Show(
                    _localizationService.GetString("Users.SelectUser") ?? "Por favor seleccione un usuario.",
                    _localizationService.GetString("Common.Validation") ?? "Validación",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                return;
            }

            var user = (User)dgvUsers.CurrentRow.DataBoundItem;
            
            // TODO: Replace InputBox with custom password dialog that masks input
            // Current implementation shows password in plain text which is a security concern
            string newPassword = Microsoft.VisualBasic.Interaction.InputBox(
                _localizationService.GetString("Users.EnterNewPassword") ?? "Ingrese la nueva contraseña (mínimo 8 caracteres, 1 mayúscula, 1 número):",
                _localizationService.GetString("Users.ChangePassword") ?? "Cambiar Contraseña",
                "",
                -1, -1);

            if (!string.IsNullOrWhiteSpace(newPassword))
            {
                try
                {
                    _userService.ChangePassword(user.UserId, newPassword);
                    MessageBox.Show(
                        _localizationService.GetString("Users.PasswordChanged") ?? "Contraseña cambiada exitosamente.",
                        _localizationService.GetString("Common.Success") ?? "Éxito",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    _errorHandler.ShowError(ex, _localizationService.GetString("Error.ChangingPassword") ?? "Error al cambiar contraseña");
                }
            }
        }

        /// <summary>
        /// Maneja el evento Click del botón Asignar Roles para gestionar los roles del usuario seleccionado
        /// </summary>
        /// <param name="sender">El objeto que generó el evento</param>
        /// <param name="e">Los datos del evento</param>
        private void btnAssignRoles_Click(object sender, EventArgs e)
        {
            if (dgvUsers.CurrentRow == null)
            {
                MessageBox.Show(
                    _localizationService.GetString("Users.SelectUser") ?? "Por favor seleccione un usuario.",
                    _localizationService.GetString("Common.Validation") ?? "Validación",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                return;
            }

            var user = (User)dgvUsers.CurrentRow.DataBoundItem;
            
            var userRolesForm = new UserRolesForm(user.UserId, user.Username, _userService, _roleService);
            userRolesForm.ShowDialog();
        }

        /// <summary>
        /// Valida que los campos requeridos del formulario estén completos
        /// </summary>
        /// <returns>True si la validación es exitosa, false en caso contrario</returns>
        private bool ValidateForm()
        {
            if (string.IsNullOrWhiteSpace(txtUsername.Text))
            {
                MessageBox.Show(
                    _localizationService.GetString("Users.UsernameRequired") ?? "El usuario es requerido.",
                    _localizationService.GetString("Common.Validation") ?? "Validación",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                txtUsername.Focus();
                return false;
            }

            if (!_isEditing && string.IsNullOrWhiteSpace(txtPassword.Text))
            {
                MessageBox.Show(
                    _localizationService.GetString("Users.PasswordRequired") ?? "La contraseña es requerida.",
                    _localizationService.GetString("Common.Validation") ?? "Validación",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                txtPassword.Focus();
                return false;
            }

            return true;
        }

        /// <summary>
        /// Carga los datos de un usuario en los controles del formulario
        /// </summary>
        /// <param name="user">El usuario cuyos datos se cargarán en el formulario</param>
        private void LoadUserToForm(User user)
        {
            txtUsername.Text = user.Username;
            txtFullName.Text = user.FullName;
            txtEmail.Text = user.Email;
        }

        /// <summary>
        /// Obtiene los datos del formulario y los asigna al objeto usuario actual
        /// </summary>
        private void GetUserFromForm()
        {
            _currentUser.Username = txtUsername.Text.Trim();
            _currentUser.FullName = txtFullName.Text.Trim();
            _currentUser.Email = txtEmail.Text.Trim();
        }

        /// <summary>
        /// Limpia todos los campos del formulario
        /// </summary>
        private void ClearForm()
        {
            txtUsername.Clear();
            txtFullName.Clear();
            txtEmail.Clear();
            txtPassword.Clear();
        }

        /// <summary>
        /// Habilita o deshabilita los controles del formulario según el modo de edición
        /// </summary>
        /// <param name="enabled">True para habilitar el modo de edición, false para deshabilitarlo</param>
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
            
            txtUsername.ReadOnly = _isEditing;
        }
    }
}
