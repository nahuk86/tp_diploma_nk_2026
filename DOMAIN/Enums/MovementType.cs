namespace DOMAIN.Enums
{
    public enum MovementType
    {
        In,          // Stock receipt/incoming
        Out,         // Stock issue/outgoing
        Transfer,    // Transfer between warehouses
        Adjustment   // Stock adjustment (positive or negative)
    }
}
