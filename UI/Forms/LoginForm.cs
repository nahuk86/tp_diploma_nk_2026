using System;
using System.Windows.Forms;
using SERVICES;
using SERVICES.Interfaces;

namespace UI.Forms
{
    public partial class LoginForm : Form
    {
        private readonly IAuthenticationService _authService;
        private readonly ILogService _logService;
        private readonly ILocalizationService _localizationService;

        public LoginForm(IAuthenticationService authService, ILogService logService, ILocalizationService localizationService)
        {
            InitializeComponent();
            
            _authService = authService ?? throw new ArgumentNullException(nameof(authService));
            _logService = logService ?? throw new ArgumentNullException(nameof(logService));
            _localizationService = localizationService ?? throw new ArgumentNullException(nameof(localizationService));

            ApplyLocalization();
        }

        private void ApplyLocalization()
        {
            this.Text = _localizationService.GetString("Common.Login");
            lblUsername.Text = _localizationService.GetString("Common.Username") + ":";
            lblPassword.Text = _localizationService.GetString("Common.Password") + ":";
            btnLogin.Text = _localizationService.GetString("Common.Login");
            btnCancel.Text = _localizationService.GetString("Common.Cancel");
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            try
            {
                var username = txtUsername.Text.Trim();
                var password = txtPassword.Text;

                if (string.IsNullOrWhiteSpace(username))
                {
                    MessageBox.Show(
                        _localizationService.GetString("Login.UsernameRequired") ?? "Por favor ingrese su usuario.",
                        _localizationService.GetString("Common.Validation") ?? "Validación",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);
                    txtUsername.Focus();
                    return;
                }

                if (string.IsNullOrWhiteSpace(password))
                {
                    MessageBox.Show(
                        _localizationService.GetString("Login.PasswordRequired") ?? "Por favor ingrese su contraseña.",
                        _localizationService.GetString("Common.Validation") ?? "Validación",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);
                    txtPassword.Focus();
                    return;
                }

                // Disable controls during authentication
                SetControlsEnabled(false);
                Cursor = Cursors.WaitCursor;

                var user = _authService.Authenticate(username, password);

                if (user != null)
                {
                    // Set session context
                    SessionContext.CurrentUser = user;
                    
                    _logService.Info($"User {username} logged in successfully");
                    
                    DialogResult = DialogResult.OK;
                    Close();
                }
                else
                {
                    MessageBox.Show(
                        _localizationService.GetString("Login.InvalidCredentials") ?? "Usuario o contraseña incorrectos.",
                        _localizationService.GetString("Login.AuthError") ?? "Error de Autenticación",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                    txtPassword.Clear();
                    txtPassword.Focus();
                }
            }
            catch (Exception ex)
            {
                _logService.Error("Error during login", ex);
                MessageBox.Show(
                    $"{_localizationService.GetString("Login.Error") ?? "Error al iniciar sesión"}: {ex.Message}",
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
            txtUsername.Enabled = enabled;
            txtPassword.Enabled = enabled;
            btnLogin.Enabled = enabled;
            btnCancel.Enabled = enabled;
        }
    }
}
