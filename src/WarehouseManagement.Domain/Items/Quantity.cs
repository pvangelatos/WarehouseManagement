using WarehouseManagement.Domain.Exceptions;

namespace WarehouseManagement.Domain.Items;

public sealed record Quantity
{
    public decimal Value { get; }

    private Quantity(decimal value) => Value = value;

    public static Quantity Create(decimal value, UnitOfMeasure unitOfMeasure)
    {
        if (value < 0)
            throw new BusinessRuleException("Quantity cannot be negative.");

        int allowedDecimals = unitOfMeasure.AllowedDecimals();

        if (decimal.Round(value, allowedDecimals) != value)
            throw new BusinessRuleException($"Quantity for {unitOfMeasure} cannot have more than {allowedDecimals} decimal places.");

        return new Quantity(value);
    }
}
