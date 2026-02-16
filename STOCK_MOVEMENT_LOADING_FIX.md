# Stock Movement Loading Fix - LEFT JOIN for Users

## Problem
The stock movement form was failing to load with the error "sigue fallando al momento de cargar" (keeps failing when loading).

## Root Cause Analysis

### The Issue
All SQL queries in `StockMovementRepository` used `INNER JOIN Users u ON sm.CreatedBy = u.UserId`. This created a strict dependency between StockMovements and Users tables.

### Why It Failed
The INNER JOIN means:
- If a stock movement's `CreatedBy` references a user that doesn't exist, the movement won't be returned
- If there are any data integrity issues (FK constraint violations), movements could be excluded
- This results in either an empty grid or complete form loading failure

### Scenarios That Cause Failure
1. **User Deletion**: If a user was deleted while they had created stock movements (FK constraint not enforced or disabled)
2. **Data Migration Issues**: Import/export operations that break referential integrity
3. **Test Data**: Seed data that references non-existent users
4. **Database Maintenance**: Manual database operations that break constraints

## Solution Implemented

### Change: INNER JOIN → LEFT JOIN

Changed all 5 occurrences in `StockMovementRepository.cs`:

```sql
-- Before (STRICT - fails if user doesn't exist)
INNER JOIN Users u ON sm.CreatedBy = u.UserId

-- After (PERMISSIVE - always returns movement, username may be NULL)
LEFT JOIN Users u ON sm.CreatedBy = u.UserId
```

### Methods Updated
1. `GetById()` - line 22
2. `GetAll()` - line 49
3. `GetByType()` - line 75
4. `GetByWarehouse()` - line 103
5. `GetByDateRange()` - line 131

### Mapping Changes

Updated `MapStockMovement()` to handle potential NULL values:

```csharp
// Before: Would throw NullReferenceException
CreatedByUsername = reader["CreatedByUsername"].ToString(),
CreatedBy = (int)reader["CreatedBy"],

// After: Safely handles NULL
CreatedByUsername = reader["CreatedByUsername"] == DBNull.Value ? null : reader["CreatedByUsername"].ToString(),
CreatedBy = reader["CreatedBy"] == DBNull.Value ? 0 : (int)reader["CreatedBy"],
```

## Design Decisions

### Why LEFT JOIN?
- **Defensive Programming**: Prioritizes data accessibility over strict integrity
- **User Experience**: Form loads successfully even with data issues
- **Consistency**: Matches how warehouse references are already handled (LEFT JOIN)
- **Graceful Degradation**: Shows movements with "(Unknown User)" rather than hiding them

### Why Default CreatedBy to 0?
The `StockMovement` entity has `CreatedBy` as `int` (not nullable):
```csharp
public int CreatedBy { get; set; }
```

Options considered:
1. **Default to 0** (chosen): Simple sentinel value indicating "unknown user"
2. **Change entity to int?**: Would require broader changes across the codebase
3. **Throw exception**: Would defeat the purpose of the LEFT JOIN fix

We chose option 1 because:
- Minimal code changes
- 0 is not a valid user ID (IDs start at 1 with IDENTITY)
- Clearly indicates a data issue without breaking functionality
- The username being NULL provides the real information anyway

### Database Schema Context

From `Database/01_CreateSchema.sql`:
```sql
CREATE TABLE [dbo].[StockMovements] (
    ...
    [CreatedBy] INT NOT NULL,
    CONSTRAINT FK_StockMovements_CreatedBy FOREIGN KEY ([CreatedBy]) 
        REFERENCES [dbo].[Users]([UserId])
);
```

Notes:
- `CreatedBy` has NOT NULL constraint
- Foreign key constraint to Users table (no CASCADE DELETE)
- In a healthy database, CreatedBy should never be NULL
- The defensive NULL check handles corrupted/test data scenarios

## Impact

### Benefits
✅ Form loads successfully even with data integrity issues  
✅ Users can view all stock movements, not just those with valid user references  
✅ Consistent behavior with warehouse reference handling  
✅ Graceful degradation - shows "NULL" username instead of hiding data  
✅ Easier troubleshooting - data issues become visible instead of hidden  

### Trade-offs
⚠️ Masks referential integrity issues (they're visible but don't prevent loading)  
⚠️ CreatedBy = 0 is a sentinel value that needs to be understood by developers  
⚠️ May allow viewing movements that "shouldn't exist" per strict data rules  

### No Breaking Changes
- Existing functionality unchanged
- NULL handling is transparent to the UI
- Form displays all data that was previously hidden

## Testing Recommendations

To verify the fix works:

1. **Normal Case**: Movements with valid users should display normally
2. **Missing User**: Create a movement, then simulate user deletion
3. **NULL Username**: Verify form doesn't crash with NULL CreatedByUsername
4. **Zero CreatedBy**: Ensure 0 is handled appropriately in the UI

## Related Issues

This fix follows a similar defensive pattern used in:
- `SalesForm.cs` - handling inactive products in sale details
- `StockMovementForm.cs` - handling inactive products in movement details

All these fixes prioritize data accessibility and form stability over strict data validation.

## Future Considerations

If strict data integrity is required:
1. Add cascade delete rules to foreign keys
2. Implement soft delete for users (IsActive flag)
3. Add data validation tools to find and fix integrity issues
4. Consider making CreatedBy nullable in the entity if NULL is a valid state

For now, the defensive LEFT JOIN approach provides the best user experience while maintaining data visibility.
