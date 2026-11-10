namespace WarehouseManagement.Domain.StockMovements;

public static class MovementTypeExtensions
{
    public static MovementDirection Direction(this MovementType movementType)
    {
        return movementType switch
        {
            MovementType.Receipt => MovementDirection.In,
            MovementType.TransferIn => MovementDirection.In,
            MovementType.AdjustmentIncrease => MovementDirection.In,
            MovementType.Issue => MovementDirection.Out,
            MovementType.TransferOut => MovementDirection.Out,
            MovementType.AdjustmentDecrease => MovementDirection.Out,
            MovementType.Reversal => throw new InvalidOperationException("Reversal movement type does not have a direction. The direction is the opposite of the original movement type being reversed."),
            _ => throw new ArgumentOutOfRangeException(nameof(movementType), movementType, $"Unknown movement type: {movementType}."),
        };
    }
}
