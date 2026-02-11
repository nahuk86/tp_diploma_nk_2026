using System;
using System.Collections.Generic;
using System.Windows.Forms;
using DAO.Repositories;
using DOMAIN.Entities;
using SERVICES;
using SERVICES.Implementations;
using SERVICES.Interfaces;

namespace UI.Forms
{
    public partial class StockQueryForm : Form
    {
        private readonly StockRepository _stockRepo;
        private readonly ProductRepository _productRepo;
        private readonly WarehouseRepository _warehouseRepo;
        private readonly ILocalizationService _localizationService;
        private readonly ILogService _logService;
        private readonly IErrorHandlerService _errorHandler;

        public StockQueryForm()
        {
            InitializeComponent();

            // Initialize services and repositories
            _logService = new FileLogService();
            _stockRepo = new StockRepository();
            _productRepo = new ProductRepository();
            _warehouseRepo = new WarehouseRepository();
            _localizationService = new LocalizationService();
            _errorHandler = new ErrorHandlerService(_logService, _localizationService);

            ApplyLocalization();
            LoadWarehouses();
            LoadAllStock();
        }

        private void ApplyLocalization()
        {
            this.Text = _localizationService.GetString("Stock.QueryTitle") ?? "Consulta de Stock";
            
            grpFilters.Text = _localizationService.GetString("Stock.Filters") ?? "Filtros";
            grpResults.Text = _localizationService.GetString("Stock.Results") ?? "Resultados";
            
            lblWarehouse.Text = _localizationService.GetString("Stock.Warehouse") ?? "Almacén:";
            btnSearch.Text = _localizationService.GetString("Common.Search") ?? "Buscar";
            btnShowAll.Text = _localizationService.GetString("Stock.ShowAll") ?? "Mostrar Todo";
            
            colProductSKU.HeaderText = _localizationService.GetString("Products.SKU") ?? "SKU";
            colProductName.HeaderText = _localizationService.GetString("Products.Name") ?? "Producto";
            colWarehouseName.HeaderText = _localizationService.GetString("Warehouses.Name") ?? "Almacén";
            colQuantity.HeaderText = _localizationService.GetString("Stock.Quantity") ?? "Cantidad";
            colLastUpdated.HeaderText = _localizationService.GetString("Stock.LastUpdated") ?? "Última Actualización";
        }

        private void LoadWarehouses()
        {
            try
            {
                cmbWarehouse.Items.Clear();
                cmbWarehouse.Items.Add(new ComboBoxItem { Text = "-- Todos --", Value = 0 });
                
                var warehouses = _warehouseRepo.GetAllActive();
                foreach (var warehouse in warehouses)
                {
                    cmbWarehouse.Items.Add(new ComboBoxItem 
                    { 
                        Text = warehouse.Name, 
                        Value = warehouse.WarehouseId 
                    });
                }
                
                cmbWarehouse.DisplayMember = "Text";
                cmbWarehouse.ValueMember = "Value";
                cmbWarehouse.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                _errorHandler.ShowError(ex, _localizationService.GetString("Error.LoadingWarehouses") ?? "Error al cargar almacenes");
            }
        }

        private void LoadAllStock()
        {
            try
            {
                var stocks = _stockRepo.GetAll();
                dgvStock.DataSource = stocks;
                ConfigureGrid();
                UpdateStatusBar(stocks.Count);
            }
            catch (Exception ex)
            {
                _errorHandler.ShowError(ex, _localizationService.GetString("Error.LoadingStock") ?? "Error al cargar stock");
            }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            try
            {
                if (cmbWarehouse.SelectedItem == null)
                    return;

                var selectedItem = (ComboBoxItem)cmbWarehouse.SelectedItem;
                var warehouseId = selectedItem.Value;

                List<Stock> stocks;
                if (warehouseId == 0)
                {
                    stocks = _stockRepo.GetAll();
                }
                else
                {
                    stocks = _stockRepo.GetByWarehouse(warehouseId);
                }

                dgvStock.DataSource = stocks;
                ConfigureGrid();
                UpdateStatusBar(stocks.Count);
            }
            catch (Exception ex)
            {
                _errorHandler.ShowError(ex, _localizationService.GetString("Error.SearchingStock") ?? "Error al buscar stock");
            }
        }

        private void btnShowAll_Click(object sender, EventArgs e)
        {
            cmbWarehouse.SelectedIndex = 0;
            LoadAllStock();
        }

        private void ConfigureGrid()
        {
            // Hide unnecessary columns
            if (dgvStock.Columns["StockId"] != null)
                dgvStock.Columns["StockId"].Visible = false;
            if (dgvStock.Columns["ProductId"] != null)
                dgvStock.Columns["ProductId"].Visible = false;
            if (dgvStock.Columns["WarehouseId"] != null)
                dgvStock.Columns["WarehouseId"].Visible = false;
            if (dgvStock.Columns["UpdatedBy"] != null)
                dgvStock.Columns["UpdatedBy"].Visible = false;

            // Format date column
            if (dgvStock.Columns["LastUpdated"] != null)
            {
                dgvStock.Columns["LastUpdated"].DefaultCellStyle.Format = "dd/MM/yyyy HH:mm";
                dgvStock.Columns["LastUpdated"].Width = 150;
            }

            // Format quantity column
            if (dgvStock.Columns["Quantity"] != null)
            {
                dgvStock.Columns["Quantity"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                dgvStock.Columns["Quantity"].Width = 100;
            }

            // Highlight low stock items
            // Note: This uses N+1 queries. For better performance, modify StockRepository 
            // to include MinStockLevel in the Stock query via JOIN with Products table
            foreach (DataGridViewRow row in dgvStock.Rows)
            {
                if (row.DataBoundItem is Stock stock)
                {
                    var product = _productRepo.GetById(stock.ProductId);
                    if (product != null && stock.Quantity <= product.MinStockLevel)
                    {
                        row.DefaultCellStyle.BackColor = System.Drawing.Color.LightCoral;
                    }
                }
            }
        }

        private void UpdateStatusBar(int recordCount)
        {
            statusLabel.Text = string.Format(
                _localizationService.GetString("Stock.RecordsFound") ?? "Registros encontrados: {0}",
                recordCount);
        }

        private class ComboBoxItem
        {
            public string Text { get; set; }
            public int Value { get; set; }
        }
    }
}
