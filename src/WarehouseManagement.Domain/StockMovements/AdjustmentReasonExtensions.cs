namespace WarehouseManagement.Domain.StockMovements;

public static class AdjustmentReasonExtensions
{
    public static bool IsAllowedForAdjustment(this AdjustmentReason adjustmentReason, MovementType movementType)
    {
        if (movementType is not (MovementType.AdjustmentIncrease or MovementType.AdjustmentDecrease))
            throw new ArgumentException($"Movement type must be either AdjustmentIncrease or AdjustmentDecrease. Provided movement type: {movementType}.", nameof(movementType));

        return adjustmentReason switch
        {
            AdjustmentReason.Damage or AdjustmentReason.Loss or AdjustmentReason.Disposal => movementType == MovementType.AdjustmentDecrease,
            AdjustmentReason.CountingError => true,
            _ => throw new ArgumentOutOfRangeException(nameof(adjustmentReason), adjustmentReason, $"Unknown adjustment reason: {adjustmentReason}."),
        };
    }
}
