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
using SERVICES.BLL.Decorators;
using DAO.Repositories;
using SERVICES.Implementations;
using UI.Factories;

namespace UI
{
    /// <summary>
    /// Formulario principal de la aplicación que contiene el menú y maneja las ventanas MDI
    /// </summary>
    public partial class Form1 : Form
    {
        private readonly ILocalizationService _localizationService;
        private readonly ILogService _logService;
        private readonly IAuthorizationService _authorizationService;
        private readonly IModuleFactory _moduleFactory;

        /// <summary>
        /// Inicializa una nueva instancia del formulario principal
        /// </summary>
        /// <param name="localizationService">Servicio de localización</param>
        /// <param name="logService">Servicio de registro de logs</param>
        public Form1(ILocalizationService localizationService, ILogService logService)
        {
            InitializeComponent();
            
            _localizationService = localizationService ?? throw new ArgumentNullException(nameof(localizationService));
            _logService = logService ?? throw new ArgumentNullException(nameof(logService));
            
            // Subscribe to language change event
            _localizationService.LanguageChanged += OnLanguageChanged;
            
            // Initialize authorization service with Decorator (adds logging)
            var permissionRepo = new PermissionRepository();
            var baseAuthService = new AuthorizationService(permissionRepo, _logService);
            _authorizationService = new LoggingAuthorizationDecorator(baseAuthService, _logService);

            // Initialize module factory (AbstractFactory + FactoryMethod pattern)
            _moduleFactory = new DefaultModuleFactory();
            
            ApplyLocalization();
            InitializeMainForm();
            ConfigureMenuByPermissions();
        }

        /// <summary>
        /// Maneja el evento de cambio de idioma y actualiza todos los formularios
        /// </summary>
        private void OnLanguageChanged(object sender, EventArgs e)
        {
            // Refresh the main form
            ApplyLocalization();
            
            // Refresh all open MDI child forms
            RefreshMdiChildren();
        }

        /// <summary>
        /// Actualiza la localización de todos los formularios hijos MDI
        /// </summary>
        private void RefreshMdiChildren()
        {
            foreach (Form childForm in this.MdiChildren)
            {
                // Try to call ApplyLocalization method if it exists
                var method = childForm.GetType().GetMethod("ApplyLocalization", 
                    System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
                if (method != null)
                {
                    method.Invoke(childForm, null);
                }
            }
        }

        /// <summary>
        /// Aplica los textos localizados a todos los menús y controles
        /// </summary>
        private void ApplyLocalization()
        {
            this.Text = $"{_localizationService.GetString("App.Title") ?? "Stock Manager"} - {SessionContext.Instance.CurrentUsername ?? "User"}";
            
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
            menuClients.Text = _localizationService.GetString("Menu.Clients") ?? "&Clientes";
            
            menuOperations.Text = _localizationService.GetString("Menu.Operations") ?? "&Operaciones";
            menuSales.Text = _localizationService.GetString("Menu.Sales") ?? "&Ventas";
            menuStockMovements.Text = _localizationService.GetString("Menu.StockMovements") ?? "&Movimientos";
            menuStockQuery.Text = _localizationService.GetString("Menu.StockQuery") ?? "&Consultar Stock";
            menuReports.Text = _localizationService.GetString("Menu.Reports") ?? "&Reportes";
            
            menuSettings.Text = _localizationService.GetString("Menu.Settings") ?? "&Configuración";
            menuLanguage.Text = _localizationService.GetString("Menu.Language") ?? "&Idioma";
            
            menuHelp.Text = _localizationService.GetString("Menu.Help") ?? "Ay&uda";
            menuUserManual.Text = _localizationService.GetString("Menu.UserManual") ?? "&Manual de Uso";
            menuAbout.Text = _localizationService.GetString("Menu.About") ?? "&Acerca de...";
            
            statusLabel.Text = _localizationService.GetString("Status.Ready") ?? "Listo";
        }

        /// <summary>
        /// Inicializa las propiedades del formulario principal
        /// </summary>
        private void InitializeMainForm()
        {
            // Set form properties
            this.WindowState = FormWindowState.Maximized;
            this.IsMdiContainer = true;
            
            _logService.Info($"Main form initialized for user: {SessionContext.Instance.CurrentUsername ?? "Unknown"}");
        }

        /// <summary>
        /// Configura la visibilidad y estado de los menús según los permisos del usuario
        /// </summary>
        private void ConfigureMenuByPermissions()
        {
            if (!SessionContext.Instance.CurrentUserId.HasValue)
                return;

            var userId = SessionContext.Instance.CurrentUserId.Value;

            // Configure menu visibility based on permissions
            menuUsers.Enabled = _authorizationService.HasPermission(userId, "Users.View");
            menuRoles.Enabled = _authorizationService.HasPermission(userId, "Roles.View");
            menuProducts.Enabled = _authorizationService.HasPermission(userId, "Products.View");
            menuWarehouses.Enabled = _authorizationService.HasPermission(userId, "Warehouses.View");
            menuClients.Enabled = _authorizationService.HasPermission(userId, "Clients.View");
            
            // Enable sales if user has sales view permission
            menuSales.Enabled = _authorizationService.HasPermission(userId, "Sales.View");
            
            // Enable stock movements if user has any stock operation permission
            menuStockMovements.Enabled = _authorizationService.HasPermission(userId, "Stock.View") ||
                                        _authorizationService.HasPermission(userId, "Stock.Receive") ||
                                        _authorizationService.HasPermission(userId, "Stock.Issue") ||
                                        _authorizationService.HasPermission(userId, "Stock.Transfer") ||
                                        _authorizationService.HasPermission(userId, "Stock.Adjust");
            menuStockQuery.Enabled = _authorizationService.HasPermission(userId, "Stock.View");
            
            // Enable reports if user has Reports.View permission
            menuReports.Enabled = _authorizationService.HasPermission(userId, "Reports.View");

            // Hide entire admin menu if user has no admin permissions
            menuAdmin.Visible = menuUsers.Enabled || menuRoles.Enabled;
        }

        /// <summary>
        /// Maneja el evento Click del menú Cerrar Sesión
        /// </summary>
        private void menuLogout_Click(object sender, EventArgs e)
        {
            var result = MessageBox.Show(
                _localizationService.GetString("Confirm.Logout") ?? "¿Está seguro que desea cerrar sesión?",
                _localizationService.GetString("Common.Confirmation") ?? "Confirmación",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                _logService.Info($"User {SessionContext.Instance.CurrentUsername} logged out");
                SessionContext.Instance.Clear();
                Application.Restart();
            }
        }

        /// <summary>
        /// Maneja el evento Click del menú Salir de la aplicación
        /// </summary>
        private void menuExit_Click(object sender, EventArgs e)
        {
            var result = MessageBox.Show(
                _localizationService.GetString("Confirm.Exit") ?? "¿Está seguro que desea salir de la aplicación?",
                _localizationService.GetString("Common.Confirmation") ?? "Confirmación",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                _logService.Info($"User {SessionContext.Instance.CurrentUsername} exited application");
                Application.Exit();
            }
        }

        /// <summary>
        /// Maneja el evento Click del menú Usuarios
        /// </summary>
        private void menuUsers_Click(object sender, EventArgs e)
        {
            if (!CheckPermission("Users.View", "No tiene permisos para ver usuarios."))
                return;

            OpenModule("Users");
        }

        /// <summary>
        /// Maneja el evento Click del menú Roles
        /// </summary>
        private void menuRoles_Click(object sender, EventArgs e)
        {
            if (!CheckPermission("Roles.View", "No tiene permisos para ver roles."))
                return;

            OpenModule("Roles");
        }

        /// <summary>
        /// Maneja el evento Click del menú Productos
        /// </summary>
        private void menuProducts_Click(object sender, EventArgs e)
        {
            if (!CheckPermission("Products.View", "No tiene permisos para ver productos."))
                return;

            OpenModule("Products");
        }

        /// <summary>
        /// Maneja el evento Click del menú Almacenes
        /// </summary>
        private void menuWarehouses_Click(object sender, EventArgs e)
        {
            if (!CheckPermission("Warehouses.View", "No tiene permisos para ver almacenes."))
                return;

            OpenModule("Warehouses");
        }

        /// <summary>
        /// Maneja el evento Click del menú Clientes
        /// </summary>
        private void menuClients_Click(object sender, EventArgs e)
        {
            if (!CheckPermission("Clients.View", "No tiene permisos para ver clientes."))
                return;

            OpenModule("Clients");
        }

        /// <summary>
        /// Maneja el evento Click del menú Ventas
        /// </summary>
        private void menuSales_Click(object sender, EventArgs e)
        {
            if (!CheckPermission("Sales.View", _localizationService.GetString("Error.Unauthorized") ?? "No tiene permisos para realizar esta acción."))
                return;

            OpenModule("Sales");
        }

        /// <summary>
        /// Maneja el evento Click del menú Movimientos de Stock
        /// </summary>
        private void menuStockMovements_Click(object sender, EventArgs e)
        {
            if (!SessionContext.Instance.CurrentUserId.HasValue)
                return;

            var userId = SessionContext.Instance.CurrentUserId.Value;
            
            // Allow access if user has any stock operation permission
            if (!_authorizationService.HasAnyPermission(userId,
                "Stock.View", "Stock.Receive", "Stock.Issue", "Stock.Transfer", "Stock.Adjust"))
            {
                MessageBox.Show(
                    _localizationService.GetString("Error.Unauthorized") ?? "No tiene permisos para registrar movimientos.",
                    _localizationService.GetString("Common.Error") ?? "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                return;
            }

            OpenModule("StockMovements");
        }

        /// <summary>
        /// Maneja el evento Click del menú Consultar Stock
        /// </summary>
        private void menuStockQuery_Click(object sender, EventArgs e)
        {
            if (!CheckPermission("Stock.View", "No tiene permisos para consultar stock."))
                return;

            OpenModule("StockQuery");
        }

        /// <summary>
        /// Maneja el evento Click del menú Reportes
        /// </summary>
        private void menuReports_Click(object sender, EventArgs e)
        {
            if (!SessionContext.Instance.CurrentUserId.HasValue)
                return;

            var userId = SessionContext.Instance.CurrentUserId.Value;
            
            // Allow access if user has sales or stock view permission
            if (!_authorizationService.HasAnyPermission(userId, "Sales.View", "Stock.View"))
            {
                MessageBox.Show(
                    _localizationService.GetString("Error.Unauthorized") ?? "No tiene permisos para ver reportes.",
                    _localizationService.GetString("Common.Error") ?? "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                return;
            }

            OpenModule("Reports");
        }

        /// <summary>
        /// Maneja el evento Click del menú Idioma - Español
        /// </summary>
        private void menuLanguageSpanish_Click(object sender, EventArgs e)
        {
            _localizationService.SetLanguage("es");
            menuLanguageSpanish.Checked = true;
            menuLanguageEnglish.Checked = false;
            _logService.Info("Language changed to Spanish");
        }

        /// <summary>
        /// Maneja el evento Click del menú Idioma - Inglés
        /// </summary>
        private void menuLanguageEnglish_Click(object sender, EventArgs e)
        {
            _localizationService.SetLanguage("en");
            menuLanguageSpanish.Checked = false;
            menuLanguageEnglish.Checked = true;
            _logService.Info("Language changed to English");
        }

        /// <summary>
        /// Maneja el evento Click del menú Acerca de
        /// </summary>
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

        /// <summary>
        /// Maneja el evento Click del menú Manual de Usuario
        /// </summary>
        private void menuUserManual_Click(object sender, EventArgs e)
        {
            OpenModule("UserManual");
        }

        /// <summary>
        /// Abre un módulo de la aplicación usando la fábrica de módulos (patrón Factory Method).
        /// Centraliza la creación y configuración de formularios MDI.
        /// </summary>
        /// <param name="moduleKey">Clave del módulo a abrir</param>
        private void OpenModule(string moduleKey)
        {
            var form = _moduleFactory.CreateForm(moduleKey);
            form.MdiParent = this;
            form.Show();
        }

        /// <summary>
        /// Verifica si el usuario actual tiene un permiso específico
        /// </summary>
        /// <param name="permissionCode">Código del permiso a verificar</param>
        /// <param name="errorMessage">Mensaje de error a mostrar si no tiene permiso</param>
        /// <returns>True si tiene permiso, false en caso contrario</returns>
        private bool CheckPermission(string permissionCode, string errorMessage)
        {
            if (!SessionContext.Instance.CurrentUserId.HasValue)
                return false;

            if (!_authorizationService.HasPermission(SessionContext.Instance.CurrentUserId.Value, permissionCode))
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
