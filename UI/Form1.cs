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

namespace UI
{
    public partial class Form1 : Form
    {
        private readonly ILocalizationService _localizationService;
        private readonly ILogService _logService;

        public Form1(ILocalizationService localizationService, ILogService logService)
        {
            InitializeComponent();
            
            _localizationService = localizationService ?? throw new ArgumentNullException(nameof(localizationService));
            _logService = logService ?? throw new ArgumentNullException(nameof(logService));
            
            ApplyLocalization();
            InitializeMainForm();
        }

        private void ApplyLocalization()
        {
            this.Text = $"{_localizationService.GetString("App.Title") ?? "Stock Manager"} - {SessionContext.CurrentUsername ?? "User"}";
        }

        private void InitializeMainForm()
        {
            // Set form properties
            this.WindowState = FormWindowState.Maximized;
            this.IsMdiContainer = true;
            
            _logService.Info($"Main form initialized for user: {SessionContext.CurrentUsername ?? "Unknown"}");
        }
    }
}
