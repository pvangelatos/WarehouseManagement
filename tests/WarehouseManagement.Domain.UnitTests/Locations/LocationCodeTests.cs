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
}
