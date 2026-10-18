using WarehouseManagement.Domain.Exceptions;
using WarehouseManagement.Domain.Items;

namespace WarehouseManagement.Domain.UnitTests.Items;

public class SkuTests
{
    // BR-20 tests for Sku.Create method
    [Theory]
    [InlineData("AB12")]
    [InlineData("AB-12")]
    [InlineData("A1B")]
    [InlineData("A1B2C3D4E5F6G7H8I9JG")]
    [InlineData("A1B2C3D4E5F6G7H8I9JGH")]
    public void Create_ValidSku_ReturnsSku(string input)
    {
        // Act
        var sku = Sku.Create(input);
        // Assert
        Assert.Equal(input, sku.Value);
    }

    // BR-20 tests for Sku. Validation rules
    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("  ")]
    [InlineData("AB")]
    [InlineData("AB@12")]
    [InlineData("-AB12")]
    [InlineData("AB12-")]
    [InlineData("AB 2")]
    public void Create_InvalidSku_ThrowsBusinessRuleException(string? input)
    {
        // Act & Assert
        Assert.Throws<BusinessRuleException>(() => Sku.Create(input));

    }

    // BR-20 tests for Sku. To UpperInvariant method
    [Theory]
    [InlineData("ab12", "AB12")]
    [InlineData("Ab-12", "AB-12")]
    [InlineData("a1b", "A1B")]
    [InlineData("  AB12", "AB12")]
    [InlineData("AB12  ", "AB12")]
    public void Create_WithLowercaseOrSurroundingSpaces_NormalizesValue(string input, string expected)
    {
        // Act
        var sku = Sku.Create(input);
        // Assert
        Assert.Equal(expected, sku.Value);
    }

    // BR-20 tests for Sku. Validation rules for Latin letters
    [Theory]
    [InlineData("ΑB12")]
    [InlineData("ΑΒ-12")]
    [InlineData("Αβ12")]
    [InlineData("АB1ΓΡIM2NBVCDSWQOI986")]
    public void Create_SkuWithNonLatinLetters_ThrowsBusinessRuleException(string input)
    {
        // Act & Assert
        Assert.Throws<BusinessRuleException>(() => Sku.Create(input));
    }

    //BR-20 tests for Sku. Validation rules for length
    [Theory]
    [InlineData("AB")]
    [InlineData("A")]
    [InlineData("A19087654TAHESGEWSBAH")]
    public void Create_SkuWithInvalidLength_ThrowsBusinessRuleException(string input)
    {
        // Act & Assert
        Assert.Throws<BusinessRuleException>(() => Sku.Create(input));
    }

    //BR-20 tests for Sku. Validation of equality of two Sku instances
    [Fact]
    public void Equals_SameValueDifferentCase_AreEqual()
    {
        // Arrange
        var sku1 = Sku.Create("ab-12");
        var sku2 = Sku.Create("AB-12");
        var sku3 = Sku.Create("ABC-12");
        var sku4 = Sku.Create("abc-12");
        // Act & Assert
        Assert.Equal(sku1, sku2);
        Assert.Equal(sku3, sku4);
        Assert.NotEqual(sku1, sku3);
        Assert.NotEqual(sku2, sku4);
    }
}
