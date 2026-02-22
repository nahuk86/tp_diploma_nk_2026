using System;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;
using Microsoft.Web.WebView2.Core;

namespace UI.Forms
{
    /// <summary>
    /// Formulario de Guía de Usuario interactiva, basado en WebView2 (Microsoft Edge).
    /// Carga los archivos HTML/CSS/JS de la carpeta Help/ del directorio de la aplicación.
    /// Funciona 100% offline; no requiere conexión a internet.
    /// </summary>
    public partial class UserManualForm : Form
    {
        /// <summary>
        /// Inicializa una nueva instancia del formulario de guía de usuario
        /// </summary>
        public UserManualForm()
        {
            InitializeComponent();
            this.Load += UserManualForm_Load;
        }

        /// <summary>
        /// Al cargar el formulario, inicializa WebView2 de forma asíncrona y navega al index.html local
        /// </summary>
        private async void UserManualForm_Load(object sender, EventArgs e)
        {
            try
            {
                await webView.EnsureCoreWebView2Async(null);

                // Intercept new-window requests: open external links in the system browser
                webView.CoreWebView2.NewWindowRequested += CoreWebView2_NewWindowRequested;

                // Block external http/https navigation inside WebView; open in system browser instead
                webView.CoreWebView2.NavigationStarting += CoreWebView2_NavigationStarting;

                string helpPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Help", "index.html");

                if (!File.Exists(helpPath))
                {
                    ShowMissingFilesError(helpPath);
                    return;
                }

                webView.Source = new Uri(helpPath);
            }
            catch (WebView2RuntimeNotFoundException)
            {
                ShowWebView2MissingError();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Ocurrió un error al inicializar la Guía de Usuario:\n\n" + ex.Message,
                    "Error al abrir la Guía",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                this.Close();
            }
        }

        /// <summary>
        /// Intercepta solicitudes de nueva ventana y las redirige al navegador del sistema
        /// </summary>
        private void CoreWebView2_NewWindowRequested(object sender, CoreWebView2NewWindowRequestedEventArgs e)
        {
            e.Handled = true;
            TryOpenExternalUrl(e.Uri);
        }

        /// <summary>
        /// Bloquea la navegación fuera de la carpeta Help local; abre URLs externas en el navegador del sistema
        /// </summary>
        private void CoreWebView2_NavigationStarting(object sender, CoreWebView2NavigationStartingEventArgs e)
        {
            if (!e.Uri.StartsWith("file://", StringComparison.OrdinalIgnoreCase) &&
                !e.Uri.StartsWith("about:", StringComparison.OrdinalIgnoreCase))
            {
                e.Cancel = true;
                TryOpenExternalUrl(e.Uri);
            }
        }

        /// <summary>
        /// Abre una URL en el navegador predeterminado del sistema de forma segura
        /// </summary>
        /// <param name="url">URL a abrir</param>
        private static void TryOpenExternalUrl(string url)
        {
            try
            {
                Process.Start(new ProcessStartInfo { FileName = url, UseShellExecute = true });
            }
            catch
            {
                // Silently ignore if the URL cannot be opened
            }
        }

        /// <summary>
        /// Muestra un mensaje amigable cuando los archivos de ayuda no se encuentran en disco
        /// </summary>
        /// <param name="expectedPath">Ruta donde se esperaba encontrar index.html</param>
        private void ShowMissingFilesError(string expectedPath)
        {
            MessageBox.Show(
                "No se encontraron los archivos de la Guía de Usuario.\n\n" +
                "Ruta esperada:\n" + expectedPath + "\n\n" +
                "Verifique que la carpeta 'Help' existe junto al ejecutable de la aplicación.\n" +
                "Consulte Documentation/HELP_GUIDE.md para más información.",
                "Archivos de ayuda no encontrados",
                MessageBoxButtons.OK,
                MessageBoxIcon.Warning);
            this.Close();
        }

        /// <summary>
        /// Muestra un mensaje cuando el WebView2 Runtime no está instalado en el equipo
        /// </summary>
        private void ShowWebView2MissingError()
        {
            MessageBox.Show(
                "No se encontró el Microsoft Edge WebView2 Runtime en este equipo.\n\n" +
                "Para usar la Guía de Usuario interactiva, instale el WebView2 Runtime:\n" +
                "  1. Abra su navegador y vaya a:\n" +
                "     microsoft.com/en-us/microsoft-edge/webview2/\n" +
                "  2. Descargue e instale el 'Evergreen Bootstrapper'.\n" +
                "  3. Reinicie Stock Manager.\n\n" +
                "Nota: El resto de la aplicación funciona correctamente sin WebView2.\n" +
                "Solo la Guía de Usuario interactiva requiere este componente.",
                "WebView2 Runtime no disponible",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);
            this.Close();
        }

        /// <summary>
        /// Maneja el evento Click del botón Cerrar para cerrar el formulario
        /// </summary>
        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
