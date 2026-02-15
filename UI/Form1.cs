using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SERVICES;
using SERVICES.Interfaces;
using DAO.Repositories;
using SERVICES.Implementations;

namespace UI
{
    public partial class Form1 : Form
    {
        private readonly ILocalizationService _localizationService;
        private readonly ILogService _logService;
        private readonly IAuthorizationService _authorizationService;

        public Form1(ILocalizationService localizationService, ILogService logService)
        {
            InitializeComponent();
            
            _localizationService = localizationService ?? throw new ArgumentNullException(nameof(localizationService));
            _logService = logService ?? throw new ArgumentNullException(nameof(logService));
            
            // Initialize authorization service
            var permissionRepo = new PermissionRepository();
            _authorizationService = new AuthorizationService(permissionRepo, _logService);
            
            ApplyLocalization();
            InitializeMainForm();
            ConfigureMenuByPermissions();
        }

        private void ApplyLocalization()
        {
            this.Text = $"{_localizationService.GetString("App.Title") ?? "Stock Manager"} - {SessionContext.CurrentUsername ?? "User"}";
            
            // Menu localization
            menuFile.Text = _localizationService.GetString("Menu.File") ?? "&Archivo";
            menuLogout.Text = _localizationService.GetString("Menu.Logout") ?? "Cerrar &Sesión";
            menuExit.Text = _localizationService.GetString("Menu.Exit") ?? "&Salir";
            
            menuAdmin.Text = _localizationService.GetString("Menu.Administration") ?? "A&dministración";
            menuUsers.Text = _localizationService.GetString("Menu.Users") ?? "&Usuarios";
            menuRoles.Text = _localizationService.GetString("Menu.Roles") ?? "&Roles";
            
            menuInventory.Text = _localizationService.GetString("Menu.Inventory") ?? "&Inventario";
            menuProducts.Text = _localizationService.GetString("Menu.Products") ?? "&Productos";
            menuWarehouses.Text = _localizationService.GetString("Menu.Warehouses") ?? "&Almacenes";
            
            menuOperations.Text = _localizationService.GetString("Menu.Operations") ?? "&Operaciones";
            menuStockMovements.Text = _localizationService.GetString("Menu.StockMovements") ?? "&Movimientos";
            menuStockQuery.Text = _localizationService.GetString("Menu.StockQuery") ?? "&Consultar Stock";
            
            menuSettings.Text = _localizationService.GetString("Menu.Settings") ?? "&Configuración";
            menuLanguage.Text = _localizationService.GetString("Menu.Language") ?? "&Idioma";
            
            menuHelp.Text = _localizationService.GetString("Menu.Help") ?? "Ay&uda";
            menuAbout.Text = _localizationService.GetString("Menu.About") ?? "&Acerca de...";
            
            statusLabel.Text = _localizationService.GetString("Status.Ready") ?? "Listo";
        }

        private void InitializeMainForm()
        {
            // Set form properties
            this.WindowState = FormWindowState.Maximized;
            this.IsMdiContainer = true;
            
            _logService.Info($"Main form initialized for user: {SessionContext.CurrentUsername ?? "Unknown"}");
        }

        private void ConfigureMenuByPermissions()
        {
            if (!SessionContext.CurrentUserId.HasValue)
                return;

            var userId = SessionContext.CurrentUserId.Value;

            // Configure menu visibility based on permissions
            menuUsers.Enabled = _authorizationService.HasPermission(userId, "Users.View");
            menuRoles.Enabled = _authorizationService.HasPermission(userId, "Roles.View");
            menuProducts.Enabled = _authorizationService.HasPermission(userId, "Products.View");
            menuWarehouses.Enabled = _authorizationService.HasPermission(userId, "Warehouses.View");
            menuStockMovements.Enabled = _authorizationService.HasPermission(userId, "Stock.View");
            menuStockQuery.Enabled = _authorizationService.HasPermission(userId, "Stock.View");

            // Hide entire admin menu if user has no admin permissions
            menuAdmin.Visible = menuUsers.Enabled || menuRoles.Enabled;
        }

        private void menuLogout_Click(object sender, EventArgs e)
        {
            var result = MessageBox.Show(
                _localizationService.GetString("Confirm.Logout") ?? "¿Está seguro que desea cerrar sesión?",
                _localizationService.GetString("Common.Confirmation") ?? "Confirmación",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                _logService.Info($"User {SessionContext.CurrentUsername} logged out");
                SessionContext.Clear();
                Application.Restart();
            }
        }

        private void menuExit_Click(object sender, EventArgs e)
        {
            var result = MessageBox.Show(
                _localizationService.GetString("Confirm.Exit") ?? "¿Está seguro que desea salir de la aplicación?",
                _localizationService.GetString("Common.Confirmation") ?? "Confirmación",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                _logService.Info($"User {SessionContext.CurrentUsername} exited application");
                Application.Exit();
            }
        }

        private void menuUsers_Click(object sender, EventArgs e)
        {
            if (!CheckPermission("Users.View", "No tiene permisos para ver usuarios."))
                return;

            var usersForm = new Forms.UsersForm();
            usersForm.MdiParent = this;
            usersForm.Show();
        }

        private void menuRoles_Click(object sender, EventArgs e)
        {
            if (!CheckPermission("Roles.View", "No tiene permisos para ver roles."))
                return;

            var rolesForm = new Forms.RolesForm();
            rolesForm.MdiParent = this;
            rolesForm.Show();
        }

        private void menuProducts_Click(object sender, EventArgs e)
        {
            if (!CheckPermission("Products.View", "No tiene permisos para ver productos."))
                return;

            var productsForm = new Forms.ProductsForm();
            productsForm.MdiParent = this;
            productsForm.Show();
        }

        private void menuWarehouses_Click(object sender, EventArgs e)
        {
            if (!CheckPermission("Warehouses.View", "No tiene permisos para ver almacenes."))
                return;

            var warehousesForm = new Forms.WarehousesForm();
            warehousesForm.MdiParent = this;
            warehousesForm.Show();
        }

        private void menuStockMovements_Click(object sender, EventArgs e)
        {
            if (!CheckPermission("Stock.View", "No tiene permisos para registrar movimientos."))
                return;

            var stockMovementForm = new Forms.StockMovementForm();
            stockMovementForm.MdiParent = this;
            stockMovementForm.Show();
        }

        private void menuStockQuery_Click(object sender, EventArgs e)
        {
            if (!CheckPermission("Stock.View", "No tiene permisos para consultar stock."))
                return;

            var stockQueryForm = new Forms.StockQueryForm();
            stockQueryForm.MdiParent = this;
            stockQueryForm.Show();
        }

        private void menuLanguageSpanish_Click(object sender, EventArgs e)
        {
            _localizationService.SetLanguage("es");
            menuLanguageSpanish.Checked = true;
            menuLanguageEnglish.Checked = false;
            ApplyLocalization();
            _logService.Info("Language changed to Spanish");
        }

        private void menuLanguageEnglish_Click(object sender, EventArgs e)
        {
            _localizationService.SetLanguage("en");
            menuLanguageSpanish.Checked = false;
            menuLanguageEnglish.Checked = true;
            ApplyLocalization();
            _logService.Info("Language changed to English");
        }

        private void menuAbout_Click(object sender, EventArgs e)
        {
            MessageBox.Show(
                "Stock Manager v1.0\n\n" +
                "Sistema de Gestión de Inventario\n" +
                "para Accesorios de Celulares\n\n" +
                "© 2026 - Proyecto Académico",
                _localizationService.GetString("Menu.About") ?? "Acerca de...",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);
        }

        private bool CheckPermission(string permissionCode, string errorMessage)
        {
            if (!SessionContext.CurrentUserId.HasValue)
                return false;

            if (!_authorizationService.HasPermission(SessionContext.CurrentUserId.Value, permissionCode))
            {
                MessageBox.Show(
                    _localizationService.GetString("Error.Unauthorized") ?? errorMessage,
                    _localizationService.GetString("Common.Error") ?? "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                return false;
            }

            return true;
        }
    }
}
