using System;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using SERVICES.Interfaces;

namespace UI.Forms
{
    public partial class AdminPasswordInitForm : Form
    {
        private readonly IAuthenticationService _authService;
        private readonly ILogService _logService;
        private readonly ILocalizationService _localizationService;

        public AdminPasswordInitForm(IAuthenticationService authService, ILogService logService, ILocalizationService localizationService)
        {
            InitializeComponent();
            
            _authService = authService ?? throw new ArgumentNullException(nameof(authService));
            _logService = logService ?? throw new ArgumentNullException(nameof(logService));
            _localizationService = localizationService ?? throw new ArgumentNullException(nameof(localizationService));

            ApplyLocalization();
        }

        private void ApplyLocalization()
        {
            // Try to get localized strings, fallback to Spanish
            this.Text = _localizationService.GetString("AdminInit.Title") ?? "Configuración Inicial - Stock Manager";
            lblTitle.Text = _localizationService.GetString("AdminInit.Header") ?? "Configuración Inicial";
            lblMessage.Text = _localizationService.GetString("AdminInit.Message") ?? 
                "Bienvenido al Stock Manager.\n\nPor favor, configure la contraseña del administrador para continuar.";
            lblPassword.Text = (_localizationService.GetString("Common.Password") ?? "Contraseña") + ":";
            lblConfirmPassword.Text = (_localizationService.GetString("AdminInit.ConfirmPassword") ?? "Confirmar Contraseña") + ":";
            lblRequirements.Text = _localizationService.GetString("AdminInit.Requirements") ?? 
                "Requisitos de contraseña:\n• Mínimo 8 caracteres\n• Al menos una letra mayúscula\n• Al menos un número";
            btnInitialize.Text = _localizationService.GetString("AdminInit.Initialize") ?? "Configurar";
            btnCancel.Text = _localizationService.GetString("Common.Cancel") ?? "Cancelar";
        }

        private void btnInitialize_Click(object sender, EventArgs e)
        {
            try
            {
                var password = txtPassword.Text;
                var confirmPassword = txtConfirmPassword.Text;

                // Validate password is not empty
                if (string.IsNullOrWhiteSpace(password))
                {
                    MessageBox.Show(
                        _localizationService.GetString("AdminInit.PasswordRequired") ?? "Por favor ingrese una contraseña.",
                        _localizationService.GetString("Common.Validation") ?? "Validación",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);
                    txtPassword.Focus();
                    return;
                }

                // Validate minimum length
                if (password.Length < 8)
                {
                    MessageBox.Show(
                        _localizationService.GetString("AdminInit.PasswordTooShort") ?? "La contraseña debe tener al menos 8 caracteres.",
                        _localizationService.GetString("Common.Validation") ?? "Validación",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);
                    txtPassword.Focus();
                    return;
                }

                // Validate has uppercase letter
                if (!Regex.IsMatch(password, @"[A-Z]"))
                {
                    MessageBox.Show(
                        _localizationService.GetString("AdminInit.PasswordNeedsUppercase") ?? "La contraseña debe contener al menos una letra mayúscula.",
                        _localizationService.GetString("Common.Validation") ?? "Validación",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);
                    txtPassword.Focus();
                    return;
                }

                // Validate has number
                if (!Regex.IsMatch(password, @"[0-9]"))
                {
                    MessageBox.Show(
                        _localizationService.GetString("AdminInit.PasswordNeedsNumber") ?? "La contraseña debe contener al menos un número.",
                        _localizationService.GetString("Common.Validation") ?? "Validación",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);
                    txtPassword.Focus();
                    return;
                }

                // Validate passwords match
                if (password != confirmPassword)
                {
                    MessageBox.Show(
                        _localizationService.GetString("AdminInit.PasswordsDoNotMatch") ?? "Las contraseñas no coinciden.",
                        _localizationService.GetString("Common.Validation") ?? "Validación",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);
                    txtConfirmPassword.Clear();
                    txtConfirmPassword.Focus();
                    return;
                }

                // Initialize the admin password
                SetControlsEnabled(false);
                Cursor = Cursors.WaitCursor;

                _authService.InitializeAdminPassword("admin", password);
                
                _logService.Info("Admin password initialized successfully");
                
                MessageBox.Show(
                    _localizationService.GetString("AdminInit.Success") ?? "Contraseña configurada exitosamente. Por favor inicie sesión con sus credenciales.",
                    _localizationService.GetString("Common.Success") ?? "Éxito",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);

                DialogResult = DialogResult.OK;
                Close();
            }
            catch (Exception ex)
            {
                _logService.Error("Error initializing admin password", ex);
                MessageBox.Show(
                    $"{_localizationService.GetString("AdminInit.Error") ?? "Error al configurar la contraseña"}: {ex.Message}",
                    _localizationService.GetString("Common.Error") ?? "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
            finally
            {
                SetControlsEnabled(true);
                Cursor = Cursors.Default;
            }
        }

        private void SetControlsEnabled(bool enabled)
        {
            txtPassword.Enabled = enabled;
            txtConfirmPassword.Enabled = enabled;
            btnInitialize.Enabled = enabled;
            btnCancel.Enabled = enabled;
        }
    }
}
