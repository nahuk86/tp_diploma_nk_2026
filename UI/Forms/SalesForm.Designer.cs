namespace UI.Forms
{
    partial class SalesForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.grpList = new System.Windows.Forms.GroupBox();
            this.dgvSales = new System.Windows.Forms.DataGridView();
            this.colSaleNumber = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colSaleDate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colSellerName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colClientName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colTotalAmount = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.btnNew = new System.Windows.Forms.Button();
            this.btnViewDetails = new System.Windows.Forms.Button();
            this.grpDetails = new System.Windows.Forms.GroupBox();
            this.lblSaleNumber = new System.Windows.Forms.Label();
            this.txtSaleNumber = new System.Windows.Forms.TextBox();
            this.lblSaleDate = new System.Windows.Forms.Label();
            this.dtpSaleDate = new System.Windows.Forms.DateTimePicker();
            this.lblSellerName = new System.Windows.Forms.Label();
            this.txtSellerName = new System.Windows.Forms.TextBox();
            this.lblClient = new System.Windows.Forms.Label();
            this.cmbClient = new System.Windows.Forms.ComboBox();
            this.btnNewClient = new System.Windows.Forms.Button();
            this.lblNotes = new System.Windows.Forms.Label();
            this.txtNotes = new System.Windows.Forms.TextBox();
            this.lblTotalAmount = new System.Windows.Forms.Label();
            this.txtTotalAmount = new System.Windows.Forms.TextBox();
            this.grpLines = new System.Windows.Forms.GroupBox();
            this.dgvLines = new System.Windows.Forms.DataGridView();
            this.colProduct = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.colQuantity = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colUnitPrice = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colLineTotal = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colStock = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.btnAddLine = new System.Windows.Forms.Button();
            this.btnRemoveLine = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.grpList.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvSales)).BeginInit();
            this.grpDetails.SuspendLayout();
            this.grpLines.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvLines)).BeginInit();
            this.SuspendLayout();
            // 
            // grpList
            // 
            this.grpList.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.grpList.Controls.Add(this.dgvSales);
            this.grpList.Controls.Add(this.btnNew);
            this.grpList.Controls.Add(this.btnViewDetails);
            this.grpList.Location = new System.Drawing.Point(12, 12);
            this.grpList.Name = "grpList";
            this.grpList.Size = new System.Drawing.Size(1060, 250);
            this.grpList.TabIndex = 0;
            this.grpList.TabStop = false;
            this.grpList.Text = "Ventas";
            // 
            // dgvSales
            // 
            this.dgvSales.AllowUserToAddRows = false;
            this.dgvSales.AllowUserToDeleteRows = false;
            this.dgvSales.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvSales.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvSales.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvSales.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colSaleNumber,
            this.colSaleDate,
            this.colSellerName,
            this.colClientName,
            this.colTotalAmount});
            this.dgvSales.Location = new System.Drawing.Point(6, 48);
            this.dgvSales.MultiSelect = false;
            this.dgvSales.Name = "dgvSales";
            this.dgvSales.ReadOnly = true;
            this.dgvSales.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvSales.Size = new System.Drawing.Size(1048, 196);
            this.dgvSales.TabIndex = 2;
            // 
            // colSaleNumber
            // 
            this.colSaleNumber.DataPropertyName = "SaleNumber";
            this.colSaleNumber.HeaderText = "Número";
            this.colSaleNumber.Name = "colSaleNumber";
            this.colSaleNumber.ReadOnly = true;
            // 
            // colSaleDate
            // 
            this.colSaleDate.DataPropertyName = "SaleDate";
            this.colSaleDate.HeaderText = "Fecha";
            this.colSaleDate.Name = "colSaleDate";
            this.colSaleDate.ReadOnly = true;
            // 
            // colSellerName
            // 
            this.colSellerName.DataPropertyName = "SellerName";
            this.colSellerName.HeaderText = "Vendedor";
            this.colSellerName.Name = "colSellerName";
            this.colSellerName.ReadOnly = true;
            // 
            // colClientName
            // 
            this.colClientName.DataPropertyName = "ClientName";
            this.colClientName.HeaderText = "Cliente";
            this.colClientName.Name = "colClientName";
            this.colClientName.ReadOnly = true;
            // 
            // colTotalAmount
            // 
            this.colTotalAmount.DataPropertyName = "TotalAmount";
            this.colTotalAmount.HeaderText = "Total";
            this.colTotalAmount.Name = "colTotalAmount";
            this.colTotalAmount.ReadOnly = true;
            // 
            // btnNew
            // 
            this.btnNew.Location = new System.Drawing.Point(6, 19);
            this.btnNew.Name = "btnNew";
            this.btnNew.Size = new System.Drawing.Size(100, 23);
            this.btnNew.TabIndex = 0;
            this.btnNew.Text = "Nuevo";
            this.btnNew.UseVisualStyleBackColor = true;
            this.btnNew.Click += new System.EventHandler(this.btnNew_Click);
            // 
            // btnViewDetails
            // 
            this.btnViewDetails.Location = new System.Drawing.Point(112, 19);
            this.btnViewDetails.Name = "btnViewDetails";
            this.btnViewDetails.Size = new System.Drawing.Size(100, 23);
            this.btnViewDetails.TabIndex = 1;
            this.btnViewDetails.Text = "Ver Detalles";
            this.btnViewDetails.UseVisualStyleBackColor = true;
            this.btnViewDetails.Click += new System.EventHandler(this.btnViewDetails_Click);
            // 
            // grpDetails
            // 
            this.grpDetails.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.grpDetails.Controls.Add(this.lblSaleNumber);
            this.grpDetails.Controls.Add(this.txtSaleNumber);
            this.grpDetails.Controls.Add(this.lblSaleDate);
            this.grpDetails.Controls.Add(this.dtpSaleDate);
            this.grpDetails.Controls.Add(this.lblSellerName);
            this.grpDetails.Controls.Add(this.txtSellerName);
            this.grpDetails.Controls.Add(this.lblClient);
            this.grpDetails.Controls.Add(this.cmbClient);
            this.grpDetails.Controls.Add(this.btnNewClient);
            this.grpDetails.Controls.Add(this.lblNotes);
            this.grpDetails.Controls.Add(this.txtNotes);
            this.grpDetails.Controls.Add(this.lblTotalAmount);
            this.grpDetails.Controls.Add(this.txtTotalAmount);
            this.grpDetails.Location = new System.Drawing.Point(12, 268);
            this.grpDetails.Name = "grpDetails";
            this.grpDetails.Size = new System.Drawing.Size(1060, 130);
            this.grpDetails.TabIndex = 1;
            this.grpDetails.TabStop = false;
            this.grpDetails.Text = "Detalles de la Venta";
            // 
            // lblSaleNumber
            // 
            this.lblSaleNumber.AutoSize = true;
            this.lblSaleNumber.Location = new System.Drawing.Point(6, 25);
            this.lblSaleNumber.Name = "lblSaleNumber";
            this.lblSaleNumber.Size = new System.Drawing.Size(50, 13);
            this.lblSaleNumber.TabIndex = 0;
            this.lblSaleNumber.Text = "Número:";
            // 
            // txtSaleNumber
            // 
            this.txtSaleNumber.Location = new System.Drawing.Point(100, 22);
            this.txtSaleNumber.Name = "txtSaleNumber";
            this.txtSaleNumber.ReadOnly = true;
            this.txtSaleNumber.Size = new System.Drawing.Size(200, 20);
            this.txtSaleNumber.TabIndex = 1;
            this.txtSaleNumber.TabStop = false;
            this.txtSaleNumber.Text = "(Autogenerado)";
            // 
            // lblSaleDate
            // 
            this.lblSaleDate.AutoSize = true;
            this.lblSaleDate.Location = new System.Drawing.Point(320, 25);
            this.lblSaleDate.Name = "lblSaleDate";
            this.lblSaleDate.Size = new System.Drawing.Size(40, 13);
            this.lblSaleDate.TabIndex = 2;
            this.lblSaleDate.Text = "Fecha:";
            // 
            // dtpSaleDate
            // 
            this.dtpSaleDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpSaleDate.Location = new System.Drawing.Point(380, 22);
            this.dtpSaleDate.Name = "dtpSaleDate";
            this.dtpSaleDate.Size = new System.Drawing.Size(200, 20);
            this.dtpSaleDate.TabIndex = 3;
            // 
            // lblSellerName
            // 
            this.lblSellerName.AutoSize = true;
            this.lblSellerName.Location = new System.Drawing.Point(6, 55);
            this.lblSellerName.Name = "lblSellerName";
            this.lblSellerName.Size = new System.Drawing.Size(59, 13);
            this.lblSellerName.TabIndex = 4;
            this.lblSellerName.Text = "Vendedor:";
            // 
            // txtSellerName
            // 
            this.txtSellerName.Location = new System.Drawing.Point(100, 52);
            this.txtSellerName.MaxLength = 100;
            this.txtSellerName.Name = "txtSellerName";
            this.txtSellerName.Size = new System.Drawing.Size(200, 20);
            this.txtSellerName.TabIndex = 5;
            // 
            // lblClient
            // 
            this.lblClient.AutoSize = true;
            this.lblClient.Location = new System.Drawing.Point(320, 55);
            this.lblClient.Name = "lblClient";
            this.lblClient.Size = new System.Drawing.Size(42, 13);
            this.lblClient.TabIndex = 6;
            this.lblClient.Text = "Cliente:";
            // 
            // cmbClient
            // 
            this.cmbClient.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbClient.FormattingEnabled = true;
            this.cmbClient.Location = new System.Drawing.Point(380, 52);
            this.cmbClient.Name = "cmbClient";
            this.cmbClient.Size = new System.Drawing.Size(300, 21);
            this.cmbClient.TabIndex = 7;
            // 
            // btnNewClient
            // 
            this.btnNewClient.Location = new System.Drawing.Point(686, 50);
            this.btnNewClient.Name = "btnNewClient";
            this.btnNewClient.Size = new System.Drawing.Size(120, 23);
            this.btnNewClient.TabIndex = 8;
            this.btnNewClient.Text = "Nuevo Cliente";
            this.btnNewClient.UseVisualStyleBackColor = true;
            this.btnNewClient.Click += new System.EventHandler(this.btnNewClient_Click);
            // 
            // lblNotes
            // 
            this.lblNotes.AutoSize = true;
            this.lblNotes.Location = new System.Drawing.Point(6, 85);
            this.lblNotes.Name = "lblNotes";
            this.lblNotes.Size = new System.Drawing.Size(38, 13);
            this.lblNotes.TabIndex = 9;
            this.lblNotes.Text = "Notas:";
            // 
            // txtNotes
            // 
            this.txtNotes.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtNotes.Location = new System.Drawing.Point(100, 82);
            this.txtNotes.MaxLength = 500;
            this.txtNotes.Multiline = true;
            this.txtNotes.Name = "txtNotes";
            this.txtNotes.Size = new System.Drawing.Size(706, 40);
            this.txtNotes.TabIndex = 10;
            // 
            // lblTotalAmount
            // 
            this.lblTotalAmount.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblTotalAmount.AutoSize = true;
            this.lblTotalAmount.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTotalAmount.Location = new System.Drawing.Point(812, 95);
            this.lblTotalAmount.Name = "lblTotalAmount";
            this.lblTotalAmount.Size = new System.Drawing.Size(51, 17);
            this.lblTotalAmount.TabIndex = 11;
            this.lblTotalAmount.Text = "Total:";
            // 
            // txtTotalAmount
            // 
            this.txtTotalAmount.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.txtTotalAmount.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtTotalAmount.Location = new System.Drawing.Point(869, 92);
            this.txtTotalAmount.Name = "txtTotalAmount";
            this.txtTotalAmount.ReadOnly = true;
            this.txtTotalAmount.Size = new System.Drawing.Size(185, 23);
            this.txtTotalAmount.TabIndex = 12;
            this.txtTotalAmount.TabStop = false;
            this.txtTotalAmount.Text = "$0.00";
            this.txtTotalAmount.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // grpLines
            // 
            this.grpLines.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.grpLines.Controls.Add(this.dgvLines);
            this.grpLines.Controls.Add(this.btnAddLine);
            this.grpLines.Controls.Add(this.btnRemoveLine);
            this.grpLines.Location = new System.Drawing.Point(12, 404);
            this.grpLines.Name = "grpLines";
            this.grpLines.Size = new System.Drawing.Size(1060, 270);
            this.grpLines.TabIndex = 2;
            this.grpLines.TabStop = false;
            this.grpLines.Text = "Productos";
            // 
            // dgvLines
            // 
            this.dgvLines.AllowUserToDeleteRows = false;
            this.dgvLines.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvLines.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvLines.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvLines.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colProduct,
            this.colQuantity,
            this.colUnitPrice,
            this.colLineTotal,
            this.colStock});
            this.dgvLines.Location = new System.Drawing.Point(6, 48);
            this.dgvLines.Name = "dgvLines";
            this.dgvLines.Size = new System.Drawing.Size(1048, 216);
            this.dgvLines.TabIndex = 2;
            this.dgvLines.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvLines_CellValueChanged);
            this.dgvLines.CurrentCellDirtyStateChanged += new System.EventHandler(this.dgvLines_CurrentCellDirtyStateChanged);
            // 
            // colProduct
            // 
            this.colProduct.HeaderText = "Producto";
            this.colProduct.Name = "colProduct";
            // 
            // colQuantity
            // 
            this.colQuantity.HeaderText = "Cantidad";
            this.colQuantity.Name = "colQuantity";
            // 
            // colUnitPrice
            // 
            this.colUnitPrice.HeaderText = "Precio Unit.";
            this.colUnitPrice.Name = "colUnitPrice";
            this.colUnitPrice.ReadOnly = true;
            // 
            // colLineTotal
            // 
            this.colLineTotal.HeaderText = "Subtotal";
            this.colLineTotal.Name = "colLineTotal";
            this.colLineTotal.ReadOnly = true;
            // 
            // colStock
            // 
            this.colStock.HeaderText = "Stock Disponible";
            this.colStock.Name = "colStock";
            this.colStock.ReadOnly = true;
            // 
            // btnAddLine
            // 
            this.btnAddLine.Location = new System.Drawing.Point(6, 19);
            this.btnAddLine.Name = "btnAddLine";
            this.btnAddLine.Size = new System.Drawing.Size(100, 23);
            this.btnAddLine.TabIndex = 0;
            this.btnAddLine.Text = "Agregar Línea";
            this.btnAddLine.UseVisualStyleBackColor = true;
            this.btnAddLine.Click += new System.EventHandler(this.btnAddLine_Click);
            // 
            // btnRemoveLine
            // 
            this.btnRemoveLine.Location = new System.Drawing.Point(112, 19);
            this.btnRemoveLine.Name = "btnRemoveLine";
            this.btnRemoveLine.Size = new System.Drawing.Size(100, 23);
            this.btnRemoveLine.TabIndex = 1;
            this.btnRemoveLine.Text = "Quitar Línea";
            this.btnRemoveLine.UseVisualStyleBackColor = true;
            this.btnRemoveLine.Click += new System.EventHandler(this.btnRemoveLine_Click);
            // 
            // btnSave
            // 
            this.btnSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSave.Location = new System.Drawing.Point(866, 680);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(100, 30);
            this.btnSave.TabIndex = 3;
            this.btnSave.Text = "Guardar";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.Location = new System.Drawing.Point(972, 680);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(100, 30);
            this.btnCancel.TabIndex = 4;
            this.btnCancel.Text = "Cancelar";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // SalesForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1084, 722);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.grpLines);
            this.Controls.Add(this.grpDetails);
            this.Controls.Add(this.grpList);
            this.MinimumSize = new System.Drawing.Size(1100, 760);
            this.Name = "SalesForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Gestión de Ventas";
            this.grpList.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvSales)).EndInit();
            this.grpDetails.ResumeLayout(false);
            this.grpDetails.PerformLayout();
            this.grpLines.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvLines)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox grpList;
        private System.Windows.Forms.DataGridView dgvSales;
        private System.Windows.Forms.Button btnNew;
        private System.Windows.Forms.Button btnViewDetails;
        private System.Windows.Forms.GroupBox grpDetails;
        private System.Windows.Forms.Label lblSaleNumber;
        private System.Windows.Forms.TextBox txtSaleNumber;
        private System.Windows.Forms.Label lblSaleDate;
        private System.Windows.Forms.DateTimePicker dtpSaleDate;
        private System.Windows.Forms.Label lblSellerName;
        private System.Windows.Forms.TextBox txtSellerName;
        private System.Windows.Forms.Label lblClient;
        private System.Windows.Forms.ComboBox cmbClient;
        private System.Windows.Forms.Button btnNewClient;
        private System.Windows.Forms.Label lblNotes;
        private System.Windows.Forms.TextBox txtNotes;
        private System.Windows.Forms.Label lblTotalAmount;
        private System.Windows.Forms.TextBox txtTotalAmount;
        private System.Windows.Forms.GroupBox grpLines;
        private System.Windows.Forms.DataGridView dgvLines;
        private System.Windows.Forms.Button btnAddLine;
        private System.Windows.Forms.Button btnRemoveLine;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.DataGridViewTextBoxColumn colSaleNumber;
        private System.Windows.Forms.DataGridViewTextBoxColumn colSaleDate;
        private System.Windows.Forms.DataGridViewTextBoxColumn colSellerName;
        private System.Windows.Forms.DataGridViewTextBoxColumn colClientName;
        private System.Windows.Forms.DataGridViewTextBoxColumn colTotalAmount;
        private System.Windows.Forms.DataGridViewComboBoxColumn colProduct;
        private System.Windows.Forms.DataGridViewTextBoxColumn colQuantity;
        private System.Windows.Forms.DataGridViewTextBoxColumn colUnitPrice;
        private System.Windows.Forms.DataGridViewTextBoxColumn colLineTotal;
        private System.Windows.Forms.DataGridViewTextBoxColumn colStock;
    }
}
