namespace UI.Forms
{
    partial class StockMovementForm
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
            this.dgvMovements = new System.Windows.Forms.DataGridView();
            this.colMovementNumber = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colMovementType = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colMovementDate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colSourceWarehouse = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colDestinationWarehouse = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.lblMovementType = new System.Windows.Forms.Label();
            this.cmbMovementTypeFilter = new System.Windows.Forms.ComboBox();
            this.btnNew = new System.Windows.Forms.Button();
            this.btnViewDetails = new System.Windows.Forms.Button();
            this.grpDetails = new System.Windows.Forms.GroupBox();
            this.lblType = new System.Windows.Forms.Label();
            this.cmbMovementType = new System.Windows.Forms.ComboBox();
            this.lblDate = new System.Windows.Forms.Label();
            this.dtpMovementDate = new System.Windows.Forms.DateTimePicker();
            this.lblSourceWarehouse = new System.Windows.Forms.Label();
            this.cmbSourceWarehouse = new System.Windows.Forms.ComboBox();
            this.lblDestinationWarehouse = new System.Windows.Forms.Label();
            this.cmbDestinationWarehouse = new System.Windows.Forms.ComboBox();
            this.lblReason = new System.Windows.Forms.Label();
            this.txtReason = new System.Windows.Forms.TextBox();
            this.lblNotes = new System.Windows.Forms.Label();
            this.txtNotes = new System.Windows.Forms.TextBox();
            this.grpLines = new System.Windows.Forms.GroupBox();
            this.dgvLines = new System.Windows.Forms.DataGridView();
            this.colProduct = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.colQuantity = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colUnitPrice = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.btnAddLine = new System.Windows.Forms.Button();
            this.btnRemoveLine = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.grpList.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvMovements)).BeginInit();
            this.grpDetails.SuspendLayout();
            this.grpLines.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvLines)).BeginInit();
            this.SuspendLayout();
            // 
            // grpList
            // 
            this.grpList.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.grpList.Controls.Add(this.dgvMovements);
            this.grpList.Controls.Add(this.lblMovementType);
            this.grpList.Controls.Add(this.cmbMovementTypeFilter);
            this.grpList.Controls.Add(this.btnNew);
            this.grpList.Controls.Add(this.btnViewDetails);
            this.grpList.Location = new System.Drawing.Point(12, 12);
            this.grpList.Name = "grpList";
            this.grpList.Size = new System.Drawing.Size(960, 280);
            this.grpList.TabIndex = 0;
            this.grpList.TabStop = false;
            this.grpList.Text = "Movimientos de Stock";
            // 
            // dgvMovements
            // 
            this.dgvMovements.AllowUserToAddRows = false;
            this.dgvMovements.AllowUserToDeleteRows = false;
            this.dgvMovements.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvMovements.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvMovements.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvMovements.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colMovementNumber,
            this.colMovementType,
            this.colMovementDate,
            this.colSourceWarehouse,
            this.colDestinationWarehouse});
            this.dgvMovements.Location = new System.Drawing.Point(6, 73);
            this.dgvMovements.MultiSelect = false;
            this.dgvMovements.Name = "dgvMovements";
            this.dgvMovements.ReadOnly = true;
            this.dgvMovements.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvMovements.Size = new System.Drawing.Size(948, 165);
            this.dgvMovements.TabIndex = 4;
            // 
            // colMovementNumber
            // 
            this.colMovementNumber.DataPropertyName = "MovementNumber";
            this.colMovementNumber.HeaderText = "Número";
            this.colMovementNumber.Name = "colMovementNumber";
            this.colMovementNumber.ReadOnly = true;
            // 
            // colMovementType
            // 
            this.colMovementType.DataPropertyName = "MovementType";
            this.colMovementType.HeaderText = "Tipo";
            this.colMovementType.Name = "colMovementType";
            this.colMovementType.ReadOnly = true;
            // 
            // colMovementDate
            // 
            this.colMovementDate.DataPropertyName = "MovementDate";
            this.colMovementDate.HeaderText = "Fecha";
            this.colMovementDate.Name = "colMovementDate";
            this.colMovementDate.ReadOnly = true;
            // 
            // colSourceWarehouse
            // 
            this.colSourceWarehouse.DataPropertyName = "SourceWarehouseName";
            this.colSourceWarehouse.HeaderText = "Almacén Origen";
            this.colSourceWarehouse.Name = "colSourceWarehouse";
            this.colSourceWarehouse.ReadOnly = true;
            // 
            // colDestinationWarehouse
            // 
            this.colDestinationWarehouse.DataPropertyName = "DestinationWarehouseName";
            this.colDestinationWarehouse.HeaderText = "Almacén Destino";
            this.colDestinationWarehouse.Name = "colDestinationWarehouse";
            this.colDestinationWarehouse.ReadOnly = true;
            // 
            // lblMovementType
            // 
            this.lblMovementType.AutoSize = true;
            this.lblMovementType.Location = new System.Drawing.Point(6, 25);
            this.lblMovementType.Name = "lblMovementType";
            this.lblMovementType.Size = new System.Drawing.Size(65, 13);
            this.lblMovementType.TabIndex = 0;
            this.lblMovementType.Text = "Filtrar por tipo:";
            // 
            // cmbMovementTypeFilter
            // 
            this.cmbMovementTypeFilter.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbMovementTypeFilter.FormattingEnabled = true;
            this.cmbMovementTypeFilter.Location = new System.Drawing.Point(6, 41);
            this.cmbMovementTypeFilter.Name = "cmbMovementTypeFilter";
            this.cmbMovementTypeFilter.Size = new System.Drawing.Size(200, 21);
            this.cmbMovementTypeFilter.TabIndex = 1;
            this.cmbMovementTypeFilter.SelectedIndexChanged += new System.EventHandler(this.cmbMovementTypeFilter_SelectedIndexChanged);
            // 
            // btnNew
            // 
            this.btnNew.Location = new System.Drawing.Point(220, 39);
            this.btnNew.Name = "btnNew";
            this.btnNew.Size = new System.Drawing.Size(100, 23);
            this.btnNew.TabIndex = 2;
            this.btnNew.Text = "Nuevo";
            this.btnNew.UseVisualStyleBackColor = true;
            this.btnNew.Click += new System.EventHandler(this.btnNew_Click);
            // 
            // btnViewDetails
            // 
            this.btnViewDetails.Location = new System.Drawing.Point(326, 39);
            this.btnViewDetails.Name = "btnViewDetails";
            this.btnViewDetails.Size = new System.Drawing.Size(100, 23);
            this.btnViewDetails.TabIndex = 3;
            this.btnViewDetails.Text = "Ver Detalles";
            this.btnViewDetails.UseVisualStyleBackColor = true;
            this.btnViewDetails.Click += new System.EventHandler(this.btnViewDetails_Click);
            // 
            // grpDetails
            // 
            this.grpDetails.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.grpDetails.Controls.Add(this.lblType);
            this.grpDetails.Controls.Add(this.cmbMovementType);
            this.grpDetails.Controls.Add(this.lblDate);
            this.grpDetails.Controls.Add(this.dtpMovementDate);
            this.grpDetails.Controls.Add(this.lblSourceWarehouse);
            this.grpDetails.Controls.Add(this.cmbSourceWarehouse);
            this.grpDetails.Controls.Add(this.lblDestinationWarehouse);
            this.grpDetails.Controls.Add(this.cmbDestinationWarehouse);
            this.grpDetails.Controls.Add(this.lblReason);
            this.grpDetails.Controls.Add(this.txtReason);
            this.grpDetails.Controls.Add(this.lblNotes);
            this.grpDetails.Controls.Add(this.txtNotes);
            this.grpDetails.Location = new System.Drawing.Point(12, 298);
            this.grpDetails.Name = "grpDetails";
            this.grpDetails.Size = new System.Drawing.Size(960, 150);
            this.grpDetails.TabIndex = 1;
            this.grpDetails.TabStop = false;
            this.grpDetails.Text = "Detalles del Movimiento";
            // 
            // lblType
            // 
            this.lblType.AutoSize = true;
            this.lblType.Location = new System.Drawing.Point(6, 25);
            this.lblType.Name = "lblType";
            this.lblType.Size = new System.Drawing.Size(31, 13);
            this.lblType.TabIndex = 0;
            this.lblType.Text = "Tipo:";
            // 
            // cmbMovementType
            // 
            this.cmbMovementType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbMovementType.FormattingEnabled = true;
            this.cmbMovementType.Location = new System.Drawing.Point(120, 22);
            this.cmbMovementType.Name = "cmbMovementType";
            this.cmbMovementType.Size = new System.Drawing.Size(200, 21);
            this.cmbMovementType.TabIndex = 1;
            this.cmbMovementType.SelectedIndexChanged += new System.EventHandler(this.cmbMovementType_SelectedIndexChanged);
            // 
            // lblDate
            // 
            this.lblDate.AutoSize = true;
            this.lblDate.Location = new System.Drawing.Point(340, 25);
            this.lblDate.Name = "lblDate";
            this.lblDate.Size = new System.Drawing.Size(40, 13);
            this.lblDate.TabIndex = 2;
            this.lblDate.Text = "Fecha:";
            // 
            // dtpMovementDate
            // 
            this.dtpMovementDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpMovementDate.Location = new System.Drawing.Point(400, 22);
            this.dtpMovementDate.Name = "dtpMovementDate";
            this.dtpMovementDate.Size = new System.Drawing.Size(200, 20);
            this.dtpMovementDate.TabIndex = 3;
            // 
            // lblSourceWarehouse
            // 
            this.lblSourceWarehouse.AutoSize = true;
            this.lblSourceWarehouse.Location = new System.Drawing.Point(6, 55);
            this.lblSourceWarehouse.Name = "lblSourceWarehouse";
            this.lblSourceWarehouse.Size = new System.Drawing.Size(91, 13);
            this.lblSourceWarehouse.TabIndex = 4;
            this.lblSourceWarehouse.Text = "Almacén Origen:";
            // 
            // cmbSourceWarehouse
            // 
            this.cmbSourceWarehouse.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbSourceWarehouse.FormattingEnabled = true;
            this.cmbSourceWarehouse.Location = new System.Drawing.Point(120, 52);
            this.cmbSourceWarehouse.Name = "cmbSourceWarehouse";
            this.cmbSourceWarehouse.Size = new System.Drawing.Size(200, 21);
            this.cmbSourceWarehouse.TabIndex = 5;
            // 
            // lblDestinationWarehouse
            // 
            this.lblDestinationWarehouse.AutoSize = true;
            this.lblDestinationWarehouse.Location = new System.Drawing.Point(340, 55);
            this.lblDestinationWarehouse.Name = "lblDestinationWarehouse";
            this.lblDestinationWarehouse.Size = new System.Drawing.Size(95, 13);
            this.lblDestinationWarehouse.TabIndex = 6;
            this.lblDestinationWarehouse.Text = "Almacén Destino:";
            // 
            // cmbDestinationWarehouse
            // 
            this.cmbDestinationWarehouse.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbDestinationWarehouse.FormattingEnabled = true;
            this.cmbDestinationWarehouse.Location = new System.Drawing.Point(441, 52);
            this.cmbDestinationWarehouse.Name = "cmbDestinationWarehouse";
            this.cmbDestinationWarehouse.Size = new System.Drawing.Size(200, 21);
            this.cmbDestinationWarehouse.TabIndex = 7;
            // 
            // lblReason
            // 
            this.lblReason.AutoSize = true;
            this.lblReason.Location = new System.Drawing.Point(6, 85);
            this.lblReason.Name = "lblReason";
            this.lblReason.Size = new System.Drawing.Size(41, 13);
            this.lblReason.TabIndex = 8;
            this.lblReason.Text = "Motivo:";
            // 
            // txtReason
            // 
            this.txtReason.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtReason.Location = new System.Drawing.Point(120, 82);
            this.txtReason.MaxLength = 200;
            this.txtReason.Name = "txtReason";
            this.txtReason.Size = new System.Drawing.Size(834, 20);
            this.txtReason.TabIndex = 9;
            // 
            // lblNotes
            // 
            this.lblNotes.AutoSize = true;
            this.lblNotes.Location = new System.Drawing.Point(6, 115);
            this.lblNotes.Name = "lblNotes";
            this.lblNotes.Size = new System.Drawing.Size(38, 13);
            this.lblNotes.TabIndex = 10;
            this.lblNotes.Text = "Notas:";
            // 
            // txtNotes
            // 
            this.txtNotes.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtNotes.Location = new System.Drawing.Point(120, 112);
            this.txtNotes.MaxLength = 500;
            this.txtNotes.Name = "txtNotes";
            this.txtNotes.Size = new System.Drawing.Size(834, 20);
            this.txtNotes.TabIndex = 11;
            // 
            // grpLines
            // 
            this.grpLines.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.grpLines.Controls.Add(this.dgvLines);
            this.grpLines.Controls.Add(this.btnAddLine);
            this.grpLines.Controls.Add(this.btnRemoveLine);
            this.grpLines.Location = new System.Drawing.Point(12, 454);
            this.grpLines.Name = "grpLines";
            this.grpLines.Size = new System.Drawing.Size(960, 220);
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
            this.colUnitPrice});
            this.dgvLines.Location = new System.Drawing.Point(6, 48);
            this.dgvLines.Name = "dgvLines";
            this.dgvLines.Size = new System.Drawing.Size(948, 166);
            this.dgvLines.TabIndex = 2;
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
            this.colUnitPrice.HeaderText = "Precio Unitario";
            this.colUnitPrice.Name = "colUnitPrice";
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
            this.btnSave.Location = new System.Drawing.Point(766, 680);
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
            this.btnCancel.Location = new System.Drawing.Point(872, 680);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(100, 30);
            this.btnCancel.TabIndex = 4;
            this.btnCancel.Text = "Cancelar";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // StockMovementForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(984, 722);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.grpLines);
            this.Controls.Add(this.grpDetails);
            this.Controls.Add(this.grpList);
            this.MinimumSize = new System.Drawing.Size(1000, 760);
            this.Name = "StockMovementForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Movimientos de Stock";
            this.grpList.ResumeLayout(false);
            this.grpList.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvMovements)).EndInit();
            this.grpDetails.ResumeLayout(false);
            this.grpDetails.PerformLayout();
            this.grpLines.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvLines)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox grpList;
        private System.Windows.Forms.DataGridView dgvMovements;
        private System.Windows.Forms.Label lblMovementType;
        private System.Windows.Forms.ComboBox cmbMovementTypeFilter;
        private System.Windows.Forms.Button btnNew;
        private System.Windows.Forms.Button btnViewDetails;
        private System.Windows.Forms.GroupBox grpDetails;
        private System.Windows.Forms.Label lblType;
        private System.Windows.Forms.ComboBox cmbMovementType;
        private System.Windows.Forms.Label lblDate;
        private System.Windows.Forms.DateTimePicker dtpMovementDate;
        private System.Windows.Forms.Label lblSourceWarehouse;
        private System.Windows.Forms.ComboBox cmbSourceWarehouse;
        private System.Windows.Forms.Label lblDestinationWarehouse;
        private System.Windows.Forms.ComboBox cmbDestinationWarehouse;
        private System.Windows.Forms.Label lblReason;
        private System.Windows.Forms.TextBox txtReason;
        private System.Windows.Forms.Label lblNotes;
        private System.Windows.Forms.TextBox txtNotes;
        private System.Windows.Forms.GroupBox grpLines;
        private System.Windows.Forms.DataGridView dgvLines;
        private System.Windows.Forms.Button btnAddLine;
        private System.Windows.Forms.Button btnRemoveLine;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.DataGridViewTextBoxColumn colMovementNumber;
        private System.Windows.Forms.DataGridViewTextBoxColumn colMovementType;
        private System.Windows.Forms.DataGridViewTextBoxColumn colMovementDate;
        private System.Windows.Forms.DataGridViewTextBoxColumn colSourceWarehouse;
        private System.Windows.Forms.DataGridViewTextBoxColumn colDestinationWarehouse;
        private System.Windows.Forms.DataGridViewComboBoxColumn colProduct;
        private System.Windows.Forms.DataGridViewTextBoxColumn colQuantity;
        private System.Windows.Forms.DataGridViewTextBoxColumn colUnitPrice;
    }
}
