namespace WarehouseManagement.Domain.Items;

public static class UnitOfMeasureExtensions
{
    public static int AllowedDecimals(this UnitOfMeasure unitOfMeasure)
    {
        return unitOfMeasure switch
        {
            UnitOfMeasure.Piece => 0,
            UnitOfMeasure.Pair => 0,
            UnitOfMeasure.Box => 0,
            UnitOfMeasure.Kilogram => 3,
            UnitOfMeasure.Tonne => 3,
            UnitOfMeasure.Litre => 3,
            UnitOfMeasure.Metre => 3,
            _ => throw new ArgumentOutOfRangeException(nameof(unitOfMeasure), unitOfMeasure, null)
        };
    }

    public static string UnEneCode(this UnitOfMeasure unitOfMeasure)
    {
        return unitOfMeasure switch
        {
            UnitOfMeasure.Piece => "H87",
            UnitOfMeasure.Pair => "PR",
            UnitOfMeasure.Box => "BX",
            UnitOfMeasure.Kilogram => "KGM",
            UnitOfMeasure.Tonne => "TNE",
            UnitOfMeasure.Litre => "LTR",
            UnitOfMeasure.Metre => "MTR",
            _ => throw new ArgumentOutOfRangeException(nameof(unitOfMeasure), unitOfMeasure, null)
        };
    }
}
