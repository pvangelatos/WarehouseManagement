namespace WarehouseManagement.Domain.StockMovements;

public static class MovementTypeExtensions
{
    public static MovementDirection Direction(this MovementType movementType)
    {
        return movementType switch
        {
            MovementType.Receipt or MovementType.TransferIn or MovementType.AdjustmentIncrease => MovementDirection.In,
            MovementType.Issue or MovementType.TransferOut or MovementType.AdjustmentDecrease => MovementDirection.Out,
            MovementType.Reversal => throw new InvalidOperationException("Reversal movement type does not have a direction. The direction is the opposite of the original movement type being reversed."),
            _ => throw new ArgumentOutOfRangeException(nameof(movementType), movementType, $"Unknown movement type: {movementType}."),
        };
    }
}
