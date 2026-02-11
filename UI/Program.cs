using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using DAO.Repositories;
using SERVICES.Implementations;
using UI.Forms;

namespace UI
{
    internal static class Program
    {
        /// <summary>
        /// Punto de entrada principal para la aplicación.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            
            try
            {
                // Initialize services
                var logService = new FileLogService();
                var userRepository = new UserRepository();
                var authService = new AuthenticationService(userRepository, logService);
                var localizationService = new LocalizationService();
                
                // Check if admin password needs initialization
                var adminUser = userRepository.GetByUsername("admin");
                if (adminUser != null && adminUser.PasswordHash == "HASH_PLACEHOLDER_WILL_BE_GENERATED_BY_APP")
                {
                    // Show admin password initialization form
                    var initForm = new AdminPasswordInitForm(authService, logService, localizationService);
                    if (initForm.ShowDialog() != DialogResult.OK)
                    {
                        // User cancelled initialization, exit application
                        return;
                    }
                }
                
                // Show LoginForm as a dialog
                using (var loginForm = new LoginForm(authService, logService, localizationService))
                {
                    if (loginForm.ShowDialog() == DialogResult.OK)
                    {
                        // Login successful, show main form
                        Application.Run(new Form1(localizationService, logService));
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"Error al iniciar la aplicación: {ex.Message}\n\nPor favor, asegúrese de que la base de datos esté configurada correctamente.",
                    "Error de Inicio",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }
    }
}
