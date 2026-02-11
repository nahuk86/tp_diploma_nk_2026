using System;
using System.Collections.Generic;
using System.Windows.Forms;
using BLL.Services;
using DAO.Repositories;
using DOMAIN.Entities;
using SERVICES;
using SERVICES.Implementations;
using SERVICES.Interfaces;

namespace UI.Forms
{
    public partial class ProductsForm : Form
    {
        private readonly ProductService _productService;
        private readonly IAuthorizationService _authorizationService;
        private readonly ILocalizationService _localizationService;
        private readonly ILogService _logService;
        private readonly IErrorHandlerService _errorHandler;
        private Product _currentProduct;
        private bool _isEditing = false;

        public ProductsForm()
        {
            InitializeComponent();

            // Initialize services
            _logService = new FileLogService();
            var productRepo = new ProductRepository();
            var auditRepo = new AuditLogRepository();
            _productService = new ProductService(productRepo, auditRepo, _logService);
            
            var permissionRepo = new PermissionRepository();
            _authorizationService = new AuthorizationService(permissionRepo, _logService);
            _localizationService = new LocalizationService();
            _errorHandler = new ErrorHandlerService(_logService, _localizationService);

            ApplyLocalization();
            ConfigurePermissions();
            LoadProducts();
        }

        private void ApplyLocalization()
        {
            this.Text = _localizationService.GetString("Products.Title") ?? "Gestión de Productos";
            
            // Group boxes
            grpList.Text = _localizationService.GetString("Products.List") ?? "Lista de Productos";
            grpDetails.Text = _localizationService.GetString("Products.Details") ?? "Detalles del Producto";
            
            // Labels
            lblSKU.Text = _localizationService.GetString("Products.SKU") ?? "SKU:";
            lblName.Text = _localizationService.GetString("Products.Name") ?? "Nombre:";
            lblDescription.Text = _localizationService.GetString("Products.Description") ?? "Descripción:";
            lblCategory.Text = _localizationService.GetString("Products.Category") ?? "Categoría:";
            lblPrice.Text = _localizationService.GetString("Products.Price") ?? "Precio:";
            lblMinStock.Text = _localizationService.GetString("Products.MinStock") ?? "Stock Mínimo:";
            lblSearch.Text = _localizationService.GetString("Common.Search") ?? "Buscar:";
            
            // Buttons
            btnNew.Text = _localizationService.GetString("Common.New") ?? "Nuevo";
            btnEdit.Text = _localizationService.GetString("Common.Edit") ?? "Editar";
            btnDelete.Text = _localizationService.GetString("Common.Delete") ?? "Eliminar";
            btnSave.Text = _localizationService.GetString("Common.Save") ?? "Guardar";
            btnCancel.Text = _localizationService.GetString("Common.Cancel") ?? "Cancelar";
            
            // DataGridView columns
            colSKU.HeaderText = _localizationService.GetString("Products.SKU") ?? "SKU";
            colName.HeaderText = _localizationService.GetString("Products.Name") ?? "Nombre";
            colCategory.HeaderText = _localizationService.GetString("Products.Category") ?? "Categoría";
            colPrice.HeaderText = _localizationService.GetString("Products.Price") ?? "Precio";
            colMinStock.HeaderText = _localizationService.GetString("Products.MinStock") ?? "Stock Mín.";
        }

        private void ConfigurePermissions()
        {
            if (!SessionContext.CurrentUserId.HasValue)
                return;

            var userId = SessionContext.CurrentUserId.Value;
            
            btnNew.Enabled = _authorizationService.HasPermission(userId, "Products.Create");
            btnEdit.Enabled = _authorizationService.HasPermission(userId, "Products.Edit");
            btnDelete.Enabled = _authorizationService.HasPermission(userId, "Products.Delete");
        }

        private void LoadProducts()
        {
            try
            {
                var products = _productService.GetActiveProducts();
                dgvProducts.DataSource = products;
                
                // Hide unnecessary columns
                if (dgvProducts.Columns["ProductId"] != null)
                    dgvProducts.Columns["ProductId"].Visible = false;
                if (dgvProducts.Columns["IsActive"] != null)
                    dgvProducts.Columns["IsActive"].Visible = false;
                if (dgvProducts.Columns["CreatedAt"] != null)
                    dgvProducts.Columns["CreatedAt"].Visible = false;
                if (dgvProducts.Columns["CreatedBy"] != null)
                    dgvProducts.Columns["CreatedBy"].Visible = false;
                if (dgvProducts.Columns["UpdatedAt"] != null)
                    dgvProducts.Columns["UpdatedAt"].Visible = false;
                if (dgvProducts.Columns["UpdatedBy"] != null)
                    dgvProducts.Columns["UpdatedBy"].Visible = false;
                if (dgvProducts.Columns["Description"] != null)
                    dgvProducts.Columns["Description"].Visible = false;
                
                EnableForm(false);
            }
            catch (Exception ex)
            {
                _errorHandler.ShowError(ex, _localizationService.GetString("Error.LoadingProducts") ?? "Error al cargar productos");
            }
        }

        private void btnNew_Click(object sender, EventArgs e)
        {
            _currentProduct = new Product();
            _isEditing = false;
            ClearForm();
            EnableForm(true);
            txtSKU.Focus();
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (dgvProducts.CurrentRow == null)
            {
                MessageBox.Show(
                    _localizationService.GetString("Products.SelectProduct") ?? "Por favor seleccione un producto.",
                    _localizationService.GetString("Common.Validation") ?? "Validación",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                return;
            }

            _currentProduct = (Product)dgvProducts.CurrentRow.DataBoundItem;
            _isEditing = true;
            LoadProductToForm(_currentProduct);
            EnableForm(true);
            txtName.Focus();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (dgvProducts.CurrentRow == null)
            {
                MessageBox.Show(
                    _localizationService.GetString("Products.SelectProduct") ?? "Por favor seleccione un producto.",
                    _localizationService.GetString("Common.Validation") ?? "Validación",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                return;
            }

            var product = (Product)dgvProducts.CurrentRow.DataBoundItem;
            
            var result = MessageBox.Show(
                string.Format(_localizationService.GetString("Products.ConfirmDelete") ?? "¿Está seguro que desea eliminar el producto '{0}'?", product.Name),
                _localizationService.GetString("Common.Confirmation") ?? "Confirmación",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                try
                {
                    _productService.DeleteProduct(product.ProductId);
                    MessageBox.Show(
                        _localizationService.GetString("Products.DeleteSuccess") ?? "Producto eliminado exitosamente.",
                        _localizationService.GetString("Common.Success") ?? "Éxito",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);
                    LoadProducts();
                }
                catch (Exception ex)
                {
                    _errorHandler.ShowError(ex, _localizationService.GetString("Error.DeletingProduct") ?? "Error al eliminar producto");
                }
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (!ValidateForm())
                return;

            try
            {
                GetProductFromForm();

                if (_isEditing)
                {
                    _productService.UpdateProduct(_currentProduct);
                    MessageBox.Show(
                        _localizationService.GetString("Products.UpdateSuccess") ?? "Producto actualizado exitosamente.",
                        _localizationService.GetString("Common.Success") ?? "Éxito",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);
                }
                else
                {
                    _productService.CreateProduct(_currentProduct);
                    MessageBox.Show(
                        _localizationService.GetString("Products.CreateSuccess") ?? "Producto creado exitosamente.",
                        _localizationService.GetString("Common.Success") ?? "Éxito",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);
                }

                LoadProducts();
            }
            catch (Exception ex)
            {
                _errorHandler.ShowError(ex, _localizationService.GetString("Error.SavingProduct") ?? "Error al guardar producto");
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            EnableForm(false);
            ClearForm();
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            try
            {
                var searchTerm = txtSearch.Text.Trim();
                var products = string.IsNullOrWhiteSpace(searchTerm) 
                    ? _productService.GetActiveProducts() 
                    : _productService.SearchProducts(searchTerm);
                
                dgvProducts.DataSource = products;
            }
            catch (Exception ex)
            {
                _errorHandler.ShowError(ex, _localizationService.GetString("Error.SearchingProducts") ?? "Error al buscar productos");
            }
        }

        private bool ValidateForm()
        {
            if (string.IsNullOrWhiteSpace(txtSKU.Text))
            {
                MessageBox.Show(
                    _localizationService.GetString("Products.SKURequired") ?? "El SKU es requerido.",
                    _localizationService.GetString("Common.Validation") ?? "Validación",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                txtSKU.Focus();
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtName.Text))
            {
                MessageBox.Show(
                    _localizationService.GetString("Products.NameRequired") ?? "El nombre es requerido.",
                    _localizationService.GetString("Common.Validation") ?? "Validación",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                txtName.Focus();
                return false;
            }

            if (string.IsNullOrWhiteSpace(cmbCategory.Text))
            {
                MessageBox.Show(
                    _localizationService.GetString("Products.CategoryRequired") ?? "La categoría es requerida.",
                    _localizationService.GetString("Common.Validation") ?? "Validación",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                cmbCategory.Focus();
                return false;
            }

            if (!decimal.TryParse(txtPrice.Text, out decimal price) || price < 0)
            {
                MessageBox.Show(
                    _localizationService.GetString("Products.InvalidPrice") ?? "El precio debe ser un número positivo.",
                    _localizationService.GetString("Common.Validation") ?? "Validación",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                txtPrice.Focus();
                return false;
            }

            if (!int.TryParse(txtMinStock.Text, out int minStock) || minStock < 0)
            {
                MessageBox.Show(
                    _localizationService.GetString("Products.InvalidMinStock") ?? "El stock mínimo debe ser un número entero positivo.",
                    _localizationService.GetString("Common.Validation") ?? "Validación",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                txtMinStock.Focus();
                return false;
            }

            return true;
        }

        private void LoadProductToForm(Product product)
        {
            txtSKU.Text = product.SKU;
            txtName.Text = product.Name;
            txtDescription.Text = product.Description;
            cmbCategory.Text = product.Category;
            txtPrice.Text = product.UnitPrice.ToString("F2");
            txtMinStock.Text = product.MinStockLevel.ToString();
        }

        private void GetProductFromForm()
        {
            _currentProduct.SKU = txtSKU.Text.Trim();
            _currentProduct.Name = txtName.Text.Trim();
            _currentProduct.Description = txtDescription.Text.Trim();
            _currentProduct.Category = cmbCategory.Text.Trim();
            _currentProduct.UnitPrice = decimal.Parse(txtPrice.Text);
            _currentProduct.MinStockLevel = int.Parse(txtMinStock.Text);
        }

        private void ClearForm()
        {
            txtSKU.Clear();
            txtName.Clear();
            txtDescription.Clear();
            cmbCategory.SelectedIndex = -1;
            txtPrice.Text = "0.00";
            txtMinStock.Text = "0";
        }

        private void EnableForm(bool enabled)
        {
            grpDetails.Enabled = enabled;
            btnSave.Enabled = enabled;
            btnCancel.Enabled = enabled;
            
            grpList.Enabled = !enabled;
            btnNew.Enabled = !enabled;
            btnEdit.Enabled = !enabled;
            btnDelete.Enabled = !enabled;
            
            txtSKU.ReadOnly = _isEditing;
        }
    }
}
