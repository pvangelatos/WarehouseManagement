using WarehouseManagement.Domain.Exceptions;
using WarehouseManagement.Domain.Locations;

namespace WarehouseManagement.Domain.UnitTests.Locations;

[Trait("BR", "BR-31")]
public class LocationCodeTests
{
    [Theory]
    [InlineData(10, 20, 30, "010-20-30")]
    [InlineData(5, 5, 5, "005-05-05")]
    [InlineData(LocationCode.MaxAisle, LocationCode.MaxBay, LocationCode.MaxLevel, "999-99-99")]
    [InlineData(LocationCode.MinAisle, LocationCode.MinBay, LocationCode.MinLevel, "001-01-00")]
    public void Create_ValidValues_ReturnsLocationCode(int aisle, int bay, int level, string expected)
    {
        // Act
        var locationCode = LocationCode.Create(aisle, bay, level);
        // Assert
        Assert.Equal(aisle, locationCode.Aisle);
        Assert.Equal(bay, locationCode.Bay);
        Assert.Equal(level, locationCode.Level);
        Assert.Equal(expected, locationCode.Value);
    }

    [Theory]
    [InlineData(LocationCode.MinAisle - 1, 20, 30)] // Aisle too low
    [InlineData(LocationCode.MaxAisle + 1, 20, 30)] // Aisle too high
    [InlineData(10, LocationCode.MinBay - 1, 30)] // Bay too low
    [InlineData(10, LocationCode.MaxBay + 1, 30)] // Bay too high
    [InlineData(10, 20, LocationCode.MinLevel - 1)] // Level too low
    [InlineData(10, 20, LocationCode.MaxLevel + 1)] // Level too high
    public void Create_ValueOutOfRange_ThrowsBusinessRuleException(int aisle, int bay, int level)
    {
        // Act & Assert
        Assert.Throws<BusinessRuleException>(() => LocationCode.Create(aisle, bay, level));
    }

    [Theory]
    [InlineData(" 1-2-4 ", "001-02-04", 1, 2, 4)]
    [InlineData("10-2-3", "010-02-03", 10, 2, 3)]
    [InlineData("001-02-04", "001-02-04", 1, 2, 4)]
    [InlineData("12-3-45", "012-03-45", 12, 3, 45)]
    public void Parse_ValidInput_ReturnsNormalizedCode(string input, string expected, int expectedAisle, int expectedBay, int expectedLevel)
    {
        // Act
        var locationCode = LocationCode.Parse(input);

        // Assert
        Assert.Equal(expected, locationCode.Value);
        Assert.Equal(expectedAisle, locationCode.Aisle);
        Assert.Equal(expectedBay, locationCode.Bay);
        Assert.Equal(expectedLevel, locationCode.Level);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData("1-2")] // Missing level
    [InlineData("1-2-3-4")] // Extra part
    [InlineData("1--4")] // Missing bay
    [InlineData("1-2-")] // Missing level
    [InlineData("1 -2-4")] // Space before hyphen
    [InlineData("0001-02-04")] // Aisle too long
    [InlineData("001-002-04")] // Bay too long
    [InlineData("001-02-004")] // Level too long
    [InlineData("A-02-04")] // Non-digit character
    [InlineData("+1-02-04")] // Non-digit character
    [InlineData("/-=-!")] // Non-digit character
    [InlineData("1 - 2 - 4")] // Spaces between numbers and hyphens
    [InlineData("١-٢-٤")]   // Arabic-Indic digits
    [InlineData("１-２-４")] // Fullwidth digits
    public void Parse_InvalidFormat_ThrowsBusinessRuleException(string? input)
    {
        // Act & Assert
        Assert.Throws<BusinessRuleException>(() => LocationCode.Parse(input));
    }

    [Theory]
    [InlineData("000-02-04")] // Aisle too low
    [InlineData("001-00-04")] // Bay too low
    public void Parse_ValueOutOfRange_ThrowsBusinessRuleException(string input)
    {
        // Act & Assert
        Assert.Throws<BusinessRuleException>(() => LocationCode.Parse(input));
    }

    [Fact]
    public void ToString_ReturnsCanonicalCode()
    {
        // Arrange
        var locationCode = LocationCode.Create(1, 2, 3);
        // Act
        string result = locationCode.ToString();
        // Assert
        Assert.Equal("001-02-03", result);
    }

    [Fact]
    public void Equals_SameLocationDifferentFormat_AreEqual()
    {
        // Arrange
        var fromNumbers = LocationCode.Create(1, 2, 4);
        var fromShortCode = LocationCode.Parse("1-2-4");
        var fromCanonicalCode = LocationCode.Parse("001-02-04");

        // Act & Assert
        Assert.Equal(fromNumbers, fromShortCode);
        Assert.Equal(fromNumbers, fromCanonicalCode);
        Assert.Equal(fromShortCode, fromCanonicalCode);
    }

    [Fact]
    public void Equals_DifferentLevel_AreNotEqual()
    {
        // Arrange
        var locationCode = LocationCode.Create(1, 2, 4);
        var differentLevel = LocationCode.Create(1, 2, 5);

        // Act & Assert
        Assert.NotEqual(locationCode, differentLevel);
    }
}
