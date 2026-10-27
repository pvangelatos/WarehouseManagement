using WarehouseManagement.Domain.Exceptions;
using WarehouseManagement.Domain.Items;

namespace WarehouseManagement.Domain.UnitTests.Items;

public class QuantityTests
{
    public static TheoryData<decimal, UnitOfMeasure> QuantitiesWithMoreThanThreeDecimals => new()
    {
        {0.1234m, UnitOfMeasure.Kilogram },
        {0.1234m, UnitOfMeasure.Litre },
    };

    public static TheoryData<decimal, UnitOfMeasure> QuantitiesWithTrailingZeros => new()
    {
        { 2.3400m, UnitOfMeasure.Litre },
        { 3.1500m, UnitOfMeasure.Metre }
    };

    [Theory]
    [Trait("BR", "BR-21")]
    [InlineData(-1, UnitOfMeasure.Piece)]
    [InlineData(-2.5, UnitOfMeasure.Pair)]
    [InlineData(-3, UnitOfMeasure.Box)]
    [InlineData(-0.1, UnitOfMeasure.Kilogram)]
    [InlineData(-1.523, UnitOfMeasure.Tonne)]
    [InlineData(-0.123, UnitOfMeasure.Litre)]
    [InlineData(-3.588, UnitOfMeasure.Metre)]
    public void Create_NegativeValue_ThrowsBusinessRuleException(decimal value, UnitOfMeasure unitOfMeasure)
    {
        // Act & Assert
        Assert.Throws<BusinessRuleException>(() => Quantity.Create(value, unitOfMeasure));
    }

    [Theory]
    [Trait("BR", "BR-21")]
    [InlineData(0, UnitOfMeasure.Kilogram)]
    [InlineData(0, UnitOfMeasure.Piece)]
    [InlineData(0, UnitOfMeasure.Pair)]
    [InlineData(0, UnitOfMeasure.Box)]
    [InlineData(0, UnitOfMeasure.Tonne)]
    [InlineData(0, UnitOfMeasure.Litre)]
    [InlineData(0, UnitOfMeasure.Metre)]
    public void Create_Zero_ReturnsQuantity(decimal value, UnitOfMeasure unitOfMeasure)
    {
        // Act
        var quantity = Quantity.Create(value, unitOfMeasure);
        // Assert
        Assert.Equal(value, quantity.Value);
    }

    [Theory]
    [Trait("BR", "BR-22")]
    [InlineData(1.5, UnitOfMeasure.Piece)]
    [InlineData(2.5, UnitOfMeasure.Pair)]
    [InlineData(3.5, UnitOfMeasure.Box)]
    public void Create_FractionalValueForUnitWithoutDecimals_ThrowsBusinessRuleException(decimal value, UnitOfMeasure unitOfMeasure)
    {
        // Act & Assert
        Assert.Throws<BusinessRuleException>(() => Quantity.Create(value, unitOfMeasure));
    }

    [Theory]
    [Trait("BR", "BR-22")]
    [InlineData(1, UnitOfMeasure.Piece)]
    [InlineData(2, UnitOfMeasure.Pair)]
    [InlineData(3, UnitOfMeasure.Box)]
    public void Create_WholeValueForUnitWithoutDecimals_ReturnsQuantity(decimal value, UnitOfMeasure unitOfMeasure)
    {
        // Act
        var quantity = Quantity.Create(value, unitOfMeasure);
        // Assert
        Assert.Equal(value, quantity.Value);
    }

    [Theory]
    [Trait("BR", "BR-23")]
    [MemberData(nameof(QuantitiesWithMoreThanThreeDecimals))]
    public void Create_MoreThanThreeDecimals_ThrowsBusinessRuleException(decimal value, UnitOfMeasure unitOfMeasure)
    {
        // Act & Assert
        Assert.Throws<BusinessRuleException>(() => Quantity.Create(value, unitOfMeasure));
    }

    [Theory]
    [Trait("BR", "BR-23")]
    [InlineData(1.123, UnitOfMeasure.Kilogram)]
    [InlineData(2.123, UnitOfMeasure.Tonne)]
    [InlineData(3.123, UnitOfMeasure.Litre)]
    [InlineData(4.123, UnitOfMeasure.Metre)]
    [InlineData(36, UnitOfMeasure.Kilogram)]
    [MemberData(nameof(QuantitiesWithTrailingZeros))]
    public void Create_UpToThreeDecimals_ReturnsQuantity(decimal value, UnitOfMeasure unitOfMeasure)
    {
        // Act
        var quantity = Quantity.Create(value, unitOfMeasure);
        // Assert
        Assert.Equal(value, quantity.Value);
    }
}
