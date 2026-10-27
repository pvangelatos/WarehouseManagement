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

    public static Quantity Zero { get; } = new(0);

    public static Quantity operator +(Quantity a, Quantity b)
    {
        ArgumentNullException.ThrowIfNull(a);
        ArgumentNullException.ThrowIfNull(b);

        return new Quantity(a.Value + b.Value);
    }

    public static Quantity operator -(Quantity a, Quantity b)
    {
        ArgumentNullException.ThrowIfNull(a);
        ArgumentNullException.ThrowIfNull(b);

        decimal result = a.Value - b.Value;

        if (result < 0)
            throw new BusinessRuleException("Resulting quantity cannot be negative.");

        return new Quantity(result);
    }

    public static bool operator <(Quantity a, Quantity b)
    {
        ArgumentNullException.ThrowIfNull(a);
        ArgumentNullException.ThrowIfNull(b);

        return a.Value < b.Value;
    }

    public static bool operator >(Quantity a, Quantity b)
    {
        ArgumentNullException.ThrowIfNull(a);
        ArgumentNullException.ThrowIfNull(b);

        return a.Value > b.Value;
    }

    public static bool operator <=(Quantity a, Quantity b)
    {
        ArgumentNullException.ThrowIfNull(a);
        ArgumentNullException.ThrowIfNull(b);

        return a.Value <= b.Value;
    }

    public static bool operator >=(Quantity a, Quantity b)
    {
        ArgumentNullException.ThrowIfNull(a);
        ArgumentNullException.ThrowIfNull(b);

        return a.Value >= b.Value;
    }
}
