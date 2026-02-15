# Stock Movement Form - User Guide

## Overview

The Stock Movement Form allows users to register stock movements in the system. It supports four types of movements:

1. **IN (Entrada)** - Stock receipt/incoming
2. **OUT (Salida)** - Stock issue/outgoing
3. **TRANSFER (Transferencia)** - Transfer between warehouses
4. **ADJUSTMENT (Ajuste)** - Stock adjustment

## Accessing the Form

From the main menu:
- Navigate to **Operaciones > Movimientos**

**Required Permission**: `Stock.View` (or specific permissions: `Stock.Receive`, `Stock.Issue`, `Stock.Transfer`, `Stock.Adjust`)

## Form Layout

The form is divided into three main sections:

### 1. Movements List (Top Section)
- **Filter by Type**: Dropdown to filter movements by type or view all
- **Movement Grid**: Displays all registered movements with:
  - Movement Number
  - Movement Type
  - Movement Date
  - Source Warehouse (if applicable)
  - Destination Warehouse (if applicable)
- **Buttons**:
  - **Nuevo**: Create a new movement
  - **Ver Detalles**: View details of the selected movement (read-only)

### 2. Movement Details (Middle Section)
Displays/edits the header information of the movement:
- **Type**: Select the movement type (In, Out, Transfer, Adjustment)
- **Date**: Movement date (defaults to current date)
- **Source Warehouse**: Warehouse from which stock is removed (for OUT and TRANSFER)
- **Destination Warehouse**: Warehouse to which stock is added (for IN, TRANSFER, and ADJUSTMENT)
- **Reason**: Brief description of the movement reason (required for ADJUSTMENT)
- **Notes**: Additional notes or comments (optional)

### 3. Products (Bottom Section)
Grid to add/edit product lines:
- **Product**: Select from active products dropdown
- **Quantity**: Number of units to move (must be > 0)
- **Unit Price**: Optional price per unit
- **Buttons**:
  - **Agregar Línea**: Add a new product line
  - **Quitar Línea**: Remove the selected product line

### Save/Cancel Buttons
- **Guardar**: Save the movement and update stock
- **Cancelar**: Cancel the operation and return to view mode

## Movement Types and Warehouse Selection

The form automatically enables/disables warehouse fields based on the selected movement type:

### IN (Stock Receipt)
- **Source Warehouse**: Disabled (not applicable)
- **Destination Warehouse**: **Required** - where stock will be added
- **Use Case**: Receiving stock from suppliers, returns, production

### OUT (Stock Issue)
- **Source Warehouse**: **Required** - where stock will be removed from
- **Destination Warehouse**: Disabled (not applicable)
- **Use Case**: Sales, shipments, consumption

### TRANSFER (Between Warehouses)
- **Source Warehouse**: **Required** - where stock will be removed from
- **Destination Warehouse**: **Required** - where stock will be added to
- **Note**: Source and destination must be different
- **Use Case**: Moving stock between locations

### ADJUSTMENT (Stock Correction)
- **Source Warehouse**: Disabled (not applicable)
- **Destination Warehouse**: **Required** - warehouse to adjust
- **Reason**: **Required** - must specify why adjustment is needed
- **Use Case**: Physical inventory corrections, damaged goods, shrinkage

## Creating a New Movement

1. Click **Nuevo** button
2. Select the **Movement Type**
3. Set the **Movement Date** (defaults to today)
4. Select appropriate warehouse(s) based on movement type
5. Enter **Reason** (required for adjustments) and **Notes** (optional)
6. Add products:
   - Click in the Product column dropdown and select a product
   - Enter the quantity
   - Optionally enter unit price
   - Repeat for additional products
7. Click **Guardar** to save

## Validation Rules

The system validates the following before saving:

### Movement Header
- Movement type must be selected
- Date is required
- Warehouses must be selected according to movement type
- For TRANSFER: source and destination warehouses must be different
- For ADJUSTMENT: reason is required

### Product Lines
- At least one product line is required
- Product must be selected
- Quantity must be greater than zero
- Product must exist and be active

### Stock Availability (for OUT and TRANSFER)
- The system verifies that sufficient stock exists before allowing the operation
- If insufficient stock is detected, an error message will display showing:
  - Product name
  - Available quantity
  - Required quantity

## Stock Updates

When a movement is saved, the system automatically updates stock levels:

### IN Movement
- **Adds** quantity to destination warehouse

### OUT Movement  
- **Subtracts** quantity from source warehouse
- Validates sufficient stock exists before saving

### TRANSFER Movement
- **Subtracts** quantity from source warehouse
- **Adds** quantity to destination warehouse  
- Validates sufficient stock exists in source before saving

### ADJUSTMENT Movement
- **Adds** quantity to the warehouse (positive adjustment)
- For negative adjustments, use OUT movement type instead

## Auto-Generated Movement Number

Each movement receives an auto-generated unique number with the format:
```
{TYPE}{YYYYMMDD}{NNNN}
```

Examples:
- `IN202402150001` - First IN movement on Feb 15, 2024
- `OUT202402150001` - First OUT movement on Feb 15, 2024
- `TRA202402150001` - First TRANSFER movement on Feb 15, 2024
- `ADJ202402150001` - First ADJUSTMENT movement on Feb 15, 2024

The sequence resets daily per movement type.

## Audit Trail

All movements are automatically logged in the audit system:
- User who created the movement
- Timestamp of creation
- Movement details and changes

Stock updates are also audited:
- Product and warehouse affected
- Previous and new stock levels
- User who performed the update

## Best Practices

1. **Always verify stock availability** before creating OUT or TRANSFER movements
2. **Provide clear reasons** for adjustments to maintain audit trail integrity
3. **Use notes field** to document special circumstances or reference external documents
4. **Double-check quantities** before saving, especially for large movements
5. **Review movements list regularly** to track stock flow patterns
6. **For inventory corrections**, use ADJUSTMENT type with detailed reason

## Error Messages

Common error messages and their meanings:

- **"Debe seleccionar un tipo de movimiento"**: Movement type not selected
- **"Debe seleccionar un almacén..."**: Required warehouse not selected
- **"Los almacenes de origen y destino deben ser diferentes"**: Same warehouse selected for source and destination in transfer
- **"Debe especificar un motivo para ajustes"**: Reason required for adjustments
- **"Debe agregar al menos un producto"**: No product lines added
- **"Insufficient stock for product..."**: Not enough stock available for the operation
- **"Product with ID X does not exist or is inactive"**: Selected product is invalid

## Permissions

Different permissions control access to movement creation:

- `Stock.View` - View movements and access the form
- `Stock.Receive` - Create IN movements
- `Stock.Issue` - Create OUT movements  
- `Stock.Transfer` - Create TRANSFER movements
- `Stock.Adjust` - Create ADJUSTMENT movements

Users must have the appropriate permission for the type of movement they want to create.

## Tips

- Use the **filter dropdown** to quickly find movements by type
- Click **Ver Detalles** to review a past movement without editing
- The form displays movements in reverse chronological order (newest first)
- Product dropdown shows format: `{SKU} - {Name}` for easy identification
- All price fields are optional - useful for transfers where pricing isn't relevant

## Support

For issues or questions about the Stock Movement Form, contact your system administrator.
