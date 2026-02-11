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
            
            // Initialize services
            var logService = new FileLogService();
            var userRepository = new UserRepository();
            var authService = new AuthenticationService(userRepository, logService);
            var localizationService = new LocalizationService();
            
            // Start with LoginForm
            Application.Run(new LoginForm(authService, logService, localizationService));
        }
    }
}
