using WarehouseManagement.Domain.Items;

namespace WarehouseManagement.Domain.UnitTests.Items;

public class UnitOfMeasureExtensionsTests
{
    // BR-24 tests for AllowedDecimals method
    [Theory]
    [InlineData(UnitOfMeasure.Piece, 0)]
    [InlineData(UnitOfMeasure.Pair, 0)]
    [InlineData(UnitOfMeasure.Box, 0)]
    [InlineData(UnitOfMeasure.Kilogram, 3)]
    [InlineData(UnitOfMeasure.Tonne, 3)]
    [InlineData(UnitOfMeasure.Litre, 3)]
    [InlineData(UnitOfMeasure.Metre, 3)]
    public void AllowedDecimals_ShouldReturnExpectedValue(UnitOfMeasure unitOfMeasure, int expectedDecimals)
    {
        // Act
        var allowedDecimals = unitOfMeasure.AllowedDecimals();
        // Assert
        Assert.Equal(expectedDecimals, allowedDecimals);
    }

    // BR-24 tests for UnEceCode method
    [Theory]
    [InlineData(UnitOfMeasure.Piece, "H87")]
    [InlineData(UnitOfMeasure.Pair, "PR")]
    [InlineData(UnitOfMeasure.Box, "BX")]
    [InlineData(UnitOfMeasure.Kilogram, "KGM")]
    [InlineData(UnitOfMeasure.Tonne, "TNE")]
    [InlineData(UnitOfMeasure.Litre, "LTR")]
    [InlineData(UnitOfMeasure.Metre, "MTR")]
    public void UnEceCode_ShouldReturnExpectedValue(UnitOfMeasure unitOfMeasure, string expectedCode)
    {
        // Act
        var code = unitOfMeasure.UnEceCode();
        // Assert
        Assert.Equal(expectedCode, code);
    }

    // Additional tests for invalid enum values
    [Fact]
    public void AllowedDecimals_InvalidUnitOfMeasure_ThrowsArgumentOutOfRangeException()
    {
        // Arrange
        var invalidUnitOfMeasure = (UnitOfMeasure)999;
        // Act & Assert
        Assert.Throws<ArgumentOutOfRangeException>(() => invalidUnitOfMeasure.AllowedDecimals());
    }

    [Fact]
    public void UnEceCode_InvalidUnitOfMeasure_ThrowsArgumentOutOfRangeException()
    {
        // Arrange
        var invalidUnitOfMeasure = (UnitOfMeasure)999;
        // Act & Assert
        Assert.Throws<ArgumentOutOfRangeException>(() => invalidUnitOfMeasure.UnEceCode());
    }
}
