namespace WarehouseManagement.Domain.StockMovements;

public enum MovementType
{
    Receipt = 1,
    Issue = 2,
    TransferOut = 3,
    TransferIn = 4,
    AdjustmentIncrease = 5,
    AdjustmentDecrease = 6,
    Reversal = 7
}

