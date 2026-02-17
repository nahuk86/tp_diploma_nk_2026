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
    public partial class ClientsForm : Form
    {
        private readonly ClientService _clientService;
        private readonly IAuthorizationService _authorizationService;
        private readonly ILocalizationService _localizationService;
        private readonly ILogService _logService;
        private readonly IErrorHandlerService _errorHandler;
        private Client _currentClient;
        private bool _isEditing = false;

        public ClientsForm()
        {
            InitializeComponent();

            // Initialize services
            _logService = new FileLogService();
            var clientRepo = new ClientRepository();
            var auditRepo = new AuditLogRepository();
            _clientService = new ClientService(clientRepo, auditRepo, _logService);
            
            var permissionRepo = new PermissionRepository();
            _authorizationService = new AuthorizationService(permissionRepo, _logService);
            _localizationService = LocalizationService.Instance;
            _errorHandler = new ErrorHandlerService(_logService, _localizationService);

            ApplyLocalization();
            ConfigurePermissions();
            LoadClients();
        }

        private void ApplyLocalization()
        {
            this.Text = _localizationService.GetString("Clients.Title") ?? "Gestión de Clientes";
            
            grpList.Text = _localizationService.GetString("Clients.List") ?? "Lista de Clientes";
            grpDetails.Text = _localizationService.GetString("Clients.Details") ?? "Detalles del Cliente";
            
            lblNombre.Text = _localizationService.GetString("Clients.Nombre") ?? "Nombre:";
            lblApellido.Text = _localizationService.GetString("Clients.Apellido") ?? "Apellido:";
            lblCorreo.Text = _localizationService.GetString("Clients.Correo") ?? "Correo:";
            lblDNI.Text = _localizationService.GetString("Clients.DNI") ?? "DNI:";
            lblTelefono.Text = _localizationService.GetString("Clients.Telefono") ?? "Teléfono:";
            lblDireccion.Text = _localizationService.GetString("Clients.Direccion") ?? "Dirección:";
            
            btnNew.Text = _localizationService.GetString("Common.New") ?? "Nuevo";
            btnEdit.Text = _localizationService.GetString("Common.Edit") ?? "Editar";
            btnDelete.Text = _localizationService.GetString("Common.Delete") ?? "Eliminar";
            btnSave.Text = _localizationService.GetString("Common.Save") ?? "Guardar";
            btnCancel.Text = _localizationService.GetString("Common.Cancel") ?? "Cancelar";
            
            colNombre.HeaderText = _localizationService.GetString("Clients.Nombre") ?? "Nombre";
            colApellido.HeaderText = _localizationService.GetString("Clients.Apellido") ?? "Apellido";
            colDNI.HeaderText = _localizationService.GetString("Clients.DNI") ?? "DNI";
            colTelefono.HeaderText = _localizationService.GetString("Clients.Telefono") ?? "Teléfono";
        }

        private void ConfigurePermissions()
        {
            if (!SessionContext.CurrentUserId.HasValue)
                return;

            var userId = SessionContext.CurrentUserId.Value;
            
            btnNew.Enabled = _authorizationService.HasPermission(userId, "Clients.Create");
            btnEdit.Enabled = _authorizationService.HasPermission(userId, "Clients.Edit");
            btnDelete.Enabled = _authorizationService.HasPermission(userId, "Clients.Delete");
        }

        private void LoadClients()
        {
            try
            {
                var clients = _clientService.GetActiveClients();
                dgvClients.DataSource = clients;
                
                // Hide unnecessary columns
                if (dgvClients.Columns["ClientId"] != null)
                    dgvClients.Columns["ClientId"].Visible = false;
                if (dgvClients.Columns["Correo"] != null)
                    dgvClients.Columns["Correo"].Visible = false;
                if (dgvClients.Columns["Direccion"] != null)
                    dgvClients.Columns["Direccion"].Visible = false;
                if (dgvClients.Columns["IsActive"] != null)
                    dgvClients.Columns["IsActive"].Visible = false;
                if (dgvClients.Columns["CreatedAt"] != null)
                    dgvClients.Columns["CreatedAt"].Visible = false;
                if (dgvClients.Columns["CreatedBy"] != null)
                    dgvClients.Columns["CreatedBy"].Visible = false;
                if (dgvClients.Columns["UpdatedAt"] != null)
                    dgvClients.Columns["UpdatedAt"].Visible = false;
                if (dgvClients.Columns["UpdatedBy"] != null)
                    dgvClients.Columns["UpdatedBy"].Visible = false;
                
                EnableForm(false);
            }
            catch (Exception ex)
            {
                _errorHandler.ShowError(ex, _localizationService.GetString("Error.LoadingClients") ?? "Error al cargar clientes");
            }
        }

        private void btnNew_Click(object sender, EventArgs e)
        {
            _currentClient = new Client();
            _isEditing = false;
            ClearForm();
            EnableForm(true);
            txtNombre.Focus();
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (dgvClients.CurrentRow == null)
            {
                MessageBox.Show(
                    _localizationService.GetString("Clients.SelectClient") ?? "Por favor seleccione un cliente.",
                    _localizationService.GetString("Common.Validation") ?? "Validación",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                return;
            }

            _currentClient = (Client)dgvClients.CurrentRow.DataBoundItem;
            _isEditing = true;
            LoadClientToForm(_currentClient);
            EnableForm(true);
            txtNombre.Focus();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (dgvClients.CurrentRow == null)
            {
                MessageBox.Show(
                    _localizationService.GetString("Clients.SelectClient") ?? "Por favor seleccione un cliente.",
                    _localizationService.GetString("Common.Validation") ?? "Validación",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                return;
            }

            var client = (Client)dgvClients.CurrentRow.DataBoundItem;
            
            var result = MessageBox.Show(
                string.Format(_localizationService.GetString("Clients.ConfirmDelete") ?? "¿Está seguro que desea eliminar el cliente '{0} {1}'?", client.Nombre, client.Apellido),
                _localizationService.GetString("Common.Confirmation") ?? "Confirmación",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                try
                {
                    _clientService.DeleteClient(client.ClientId);
                    MessageBox.Show(
                        _localizationService.GetString("Clients.DeleteSuccess") ?? "Cliente eliminado exitosamente.",
                        _localizationService.GetString("Common.Success") ?? "Éxito",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);
                    LoadClients();
                }
                catch (Exception ex)
                {
                    _errorHandler.ShowError(ex, _localizationService.GetString("Error.DeletingClient") ?? "Error al eliminar cliente");
                }
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (!ValidateForm())
                return;

            try
            {
                GetClientFromForm();

                if (_isEditing)
                {
                    _clientService.UpdateClient(_currentClient);
                    MessageBox.Show(
                        _localizationService.GetString("Clients.UpdateSuccess") ?? "Cliente actualizado exitosamente.",
                        _localizationService.GetString("Common.Success") ?? "Éxito",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);
                }
                else
                {
                    _clientService.CreateClient(_currentClient);
                    MessageBox.Show(
                        _localizationService.GetString("Clients.CreateSuccess") ?? "Cliente creado exitosamente.",
                        _localizationService.GetString("Common.Success") ?? "Éxito",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);
                }

                LoadClients();
            }
            catch (Exception ex)
            {
                _errorHandler.ShowError(ex, _localizationService.GetString("Error.SavingClient") ?? "Error al guardar cliente");
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            EnableForm(false);
            ClearForm();
        }

        private bool ValidateForm()
        {
            if (string.IsNullOrWhiteSpace(txtNombre.Text))
            {
                MessageBox.Show(
                    _localizationService.GetString("Clients.NombreRequired") ?? "El nombre es requerido.",
                    _localizationService.GetString("Common.Validation") ?? "Validación",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                txtNombre.Focus();
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtApellido.Text))
            {
                MessageBox.Show(
                    _localizationService.GetString("Clients.ApellidoRequired") ?? "El apellido es requerido.",
                    _localizationService.GetString("Common.Validation") ?? "Validación",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                txtApellido.Focus();
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtDNI.Text))
            {
                MessageBox.Show(
                    _localizationService.GetString("Clients.DNIRequired") ?? "El DNI es requerido.",
                    _localizationService.GetString("Common.Validation") ?? "Validación",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                txtDNI.Focus();
                return false;
            }

            return true;
        }

        private void LoadClientToForm(Client client)
        {
            txtNombre.Text = client.Nombre;
            txtApellido.Text = client.Apellido;
            txtCorreo.Text = client.Correo;
            txtDNI.Text = client.DNI;
            txtTelefono.Text = client.Telefono;
            txtDireccion.Text = client.Direccion;
        }

        private void GetClientFromForm()
        {
            _currentClient.Nombre = txtNombre.Text.Trim();
            _currentClient.Apellido = txtApellido.Text.Trim();
            _currentClient.Correo = txtCorreo.Text.Trim();
            _currentClient.DNI = txtDNI.Text.Trim();
            _currentClient.Telefono = txtTelefono.Text.Trim();
            _currentClient.Direccion = txtDireccion.Text.Trim();
        }

        private void ClearForm()
        {
            txtNombre.Clear();
            txtApellido.Clear();
            txtCorreo.Clear();
            txtDNI.Clear();
            txtTelefono.Clear();
            txtDireccion.Clear();
        }

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
            
            txtDNI.ReadOnly = _isEditing;
        }
    }
}
