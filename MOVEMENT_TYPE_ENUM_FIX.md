# MovementType Enum Parsing Fix

## Problem
The application was crashing when trying to load stock movements with the following error:

```
ArgumentException: No se puede encontrar el valor solicitado 'ADJUSTMENT'.
StackTrace: en System.Enum.Parse(Type enumType, String value)
   en DAO.Repositories.StockMovementRepository.MapStockMovement(SqlDataReader reader)
```

## Root Cause Analysis

### The Mismatch
There was a case mismatch between:

1. **Enum Definition** (`DOMAIN/Enums/MovementType.cs`):
```csharp
public enum MovementType
{
    In,          // Mixed case
    Out,         
    Transfer,    
    Adjustment   // Mixed case - 'A' + lowercase
}
```

2. **Database Values** (from `Database/04_ReportsTestData.sql`):
```sql
'IN', 'OUT', 'TRANSFER', 'ADJUSTMENT'  -- All uppercase
```

3. **Schema Comment** (`Database/01_CreateSchema.sql`):
```sql
[MovementType] NVARCHAR(20) NOT NULL, -- 'IN', 'OUT', 'TRANSFER', 'ADJUSTMENT'
```

### Why It Failed
The `Enum.Parse()` method in `StockMovementRepository.MapStockMovement()` was using case-sensitive parsing:

```csharp
// Line 247 (original code)
MovementType = (MovementType)Enum.Parse(typeof(MovementType), reader["MovementType"].ToString()),
```

When the code tried to parse `"ADJUSTMENT"` from the database:
- It looked for an exact match in the enum
- The enum has `Adjustment` (not `ADJUSTMENT`)
- No match found → `ArgumentException` thrown

### Call Stack
```
1. User opens Stock Movement Form
2. Form calls StockMovementService.GetAllMovements()
3. Service calls StockMovementRepository.GetAll()
4. GetAll() reads from database, gets "ADJUSTMENT"
5. Calls MapStockMovement() to convert SqlDataReader to object
6. Enum.Parse("ADJUSTMENT") fails - no exact match
7. Application crashes
```

## Solution Implemented

### The Fix
Changed line 247 in `DAO/Repositories/StockMovementRepository.cs` to use case-insensitive parsing:

```csharp
// Before (case-sensitive)
MovementType = (MovementType)Enum.Parse(typeof(MovementType), reader["MovementType"].ToString()),

// After (case-insensitive)
MovementType = (MovementType)Enum.Parse(typeof(MovementType), reader["MovementType"].ToString(), ignoreCase: true),
```

### Why This Works
The `Enum.Parse()` method has an overload that accepts an `ignoreCase` parameter:
```csharp
public static object Parse(Type enumType, string value, bool ignoreCase)
```

When `ignoreCase: true`:
- `"ADJUSTMENT"` → matches `Adjustment`
- `"Adjustment"` → matches `Adjustment`
- `"adjustment"` → matches `Adjustment`
- `"IN"` → matches `In`
- etc.

## Impact

### Benefits
✅ **Robustness**: Handles any case variation in database  
✅ **Compatibility**: Works with existing test data (all caps)  
✅ **Future-proof**: Works if future code uses mixed case  
✅ **Minimal change**: Single parameter addition  
✅ **No breaking changes**: All existing functionality preserved  

### Test Coverage
The fix handles all MovementType values:
- `IN` / `In` / `in`
- `OUT` / `Out` / `out`
- `TRANSFER` / `Transfer` / `transfer`
- `ADJUSTMENT` / `Adjustment` / `adjustment`

## Related Code

### Where Enum Values Are Written to Database
In `StockMovementRepository.Insert()` (line 160):
```csharp
command.Parameters.Add(DatabaseHelper.CreateParameter("@MovementType", movement.MovementType.ToString()));
```

When C# code creates movements, it uses `.ToString()` which outputs:
- `MovementType.In.ToString()` → `"In"`
- `MovementType.Adjustment.ToString()` → `"Adjustment"`

So the database can contain mixed case values from C# code and all caps from test data scripts.

### Where It's Used
The fix is applied in the single place where enum parsing occurs:
- `MapStockMovement()` - line 247

This method is called by all repository query methods:
- `GetById()`
- `GetAll()`
- `GetByType()`
- `GetByWarehouse()`
- `GetByDateRange()`

## Testing

To verify the fix works:

1. **Normal case**: Movements created by C# code (mixed case) load correctly
2. **Test data**: Movements from `04_ReportsTestData.sql` (all caps) load correctly
3. **Edge cases**: Any case variation in database is handled

## Alternative Solutions Considered

### 1. Change Database Values to Match Enum
**Rejected**: Would require updating existing data and test scripts. More invasive change.

### 2. Change Enum Values to All Caps
**Rejected**: Would break C# naming conventions. Enum values should be PascalCase.

### 3. Add Custom Enum Converter
**Rejected**: Overkill for a simple case-sensitivity issue.

### 4. Use Case-Insensitive Parse (Chosen)
**Selected**: Minimal, robust, follows best practices for parsing external data.

## Lessons Learned

1. **Always use case-insensitive parsing for external data**: Database values, API responses, etc.
2. **Test data should match production patterns**: Test scripts used all caps, but C# code uses mixed case
3. **Enum parsing from strings needs defensive coding**: External data can have unexpected variations

## Related Issues

This fix is part of a series of defensive programming improvements:
- SalesForm: Handling inactive products
- StockMovementForm: Handling inactive products and missing users
- StockMovementRepository: Case-insensitive enum parsing (this fix)

All prioritize robustness and data accessibility over strict validation.
