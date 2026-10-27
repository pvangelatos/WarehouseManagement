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

    [Fact]
    public void Addition_ValidQuantities_ReturnsCorrectSum()
    {
        // Arrange
        var quantity1 = Quantity.Create(2.5m, UnitOfMeasure.Kilogram);
        var quantity2 = Quantity.Create(3.0m, UnitOfMeasure.Kilogram);

        // Act
        var result = quantity1 + quantity2;

        // Assert
        Assert.Equal(5.5m, result.Value);
    }

    [Fact]
    public void Subtraction_ValidQuantities_ReturnsCorrectDifference()
    {
        // Arrange
        var quantity1 = Quantity.Create(5.0m, UnitOfMeasure.Kilogram);
        var quantity2 = Quantity.Create(3.0m, UnitOfMeasure.Kilogram);

        // Act
        var result = quantity1 - quantity2;

        // Assert
        Assert.Equal(2.0m, result.Value);
    }

    [Fact]
    [Trait("BR", "BR-21")]
    public void Subtraction_EqualQuantities_ReturnsZeroQuantity()
    {
        // Arrange
        var quantity1 = Quantity.Create(3.0m, UnitOfMeasure.Kilogram);
        var quantity2 = Quantity.Create(3.0m, UnitOfMeasure.Kilogram);

        // Act
        var result = quantity1 - quantity2;

        // Assert
        Assert.Equal(0.0m, result.Value);
    }

    [Fact]
    [Trait("BR", "BR-21")]
    public void Subtraction_ResultingNegativeQuantity_ThrowsBusinessRuleException()
    {
        // Arrange
        var quantity1 = Quantity.Create(2.0m, UnitOfMeasure.Kilogram);
        var quantity2 = Quantity.Create(3.0m, UnitOfMeasure.Kilogram);

        // Act & Assert
        Assert.Throws<BusinessRuleException>(() => { _ = quantity1 - quantity2; });
    }

    [Fact]
    public void Comparison_ValidQuantities_ReturnsCorrectComparison()
    {
        // Arrange
        var quantity1 = Quantity.Create(2.0m, UnitOfMeasure.Kilogram);
        var quantity2 = Quantity.Create(3.0m, UnitOfMeasure.Kilogram);

        // Act & Assert
        Assert.True(quantity1 < quantity2);
        Assert.True(quantity2 > quantity1);
        Assert.True(quantity1 <= quantity2);
        Assert.True(quantity2 >= quantity1);
    }

    [Fact]
    public void Comparison_EqualQuantities_ReturnsCorrectComparison()
    {
        // Arrange
        var quantity1 = Quantity.Create(2.0m, UnitOfMeasure.Kilogram);
        var quantity2 = Quantity.Create(2.0m, UnitOfMeasure.Kilogram);

        // Act & Assert
        Assert.False(quantity1 < quantity2);
        Assert.False(quantity1 > quantity2);
        Assert.True(quantity1 <= quantity2);
        Assert.True(quantity1 >= quantity2);
    }

    [Fact]
    public void Addition_NullQuantity_ThrowsArgumentNullException()
    {
        // Arrange
        Quantity quantity1 = null!;
        var quantity2 = Quantity.Create(3.0m, UnitOfMeasure.Kilogram);

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => { _ = quantity1 + quantity2; });
    }

    [Fact]
    public void Subtraction_NullQuantity_ThrowsArgumentNullException()
    {
        // Arrange
        Quantity quantity1 = null!;
        var quantity2 = Quantity.Create(3.0m, UnitOfMeasure.Kilogram);

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => { _ = quantity1 - quantity2; });
    }

    [Fact]
    public void Comparison_NullQuantity_ThrowsArgumentNullException()
    {
        // Arrange
        Quantity quantity1 = null!;
        var quantity2 = Quantity.Create(3.0m, UnitOfMeasure.Kilogram);

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => { _ = quantity1 < quantity2; });
        Assert.Throws<ArgumentNullException>(() => { _ = quantity1 > quantity2; });
        Assert.Throws<ArgumentNullException>(() => { _ = quantity1 <= quantity2; });
        Assert.Throws<ArgumentNullException>(() => { _ = quantity1 >= quantity2; });
    }

    [Fact]
    public void Equality_SameValueDifferentScale_ReturnsTrue()
    {
        // Arrange
        var quantity1 = Quantity.Create(2.5m, UnitOfMeasure.Kilogram);
        var quantity2 = Quantity.Create(2.500m, UnitOfMeasure.Kilogram);

        // Act & Assert
        Assert.Equal(quantity1, quantity2);
        Assert.True(quantity1 == quantity2);
        Assert.False(quantity1 != quantity2);
    }

    [Fact]
    public void Zero_ComparedToCreatedZero_IsEqual()
    {
        // Arrange
        var zeroQuantity = Quantity.Zero;
        var createdZeroQuantity = Quantity.Create(0, UnitOfMeasure.Kilogram);

        // Act & Assert
        Assert.Equal(zeroQuantity, createdZeroQuantity);
    }
}
