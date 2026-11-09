using WarehouseManagement.Domain.Locations;

namespace WarehouseManagement.Domain.UnitTests.Locations;

public class LocationTests
{
    private static Location CreateLocation(LocationCode? code = null) =>
        Location.Create(code ?? LocationCode.Create(
                                LocationCode.MinAisle,
                                LocationCode.MinBay,
                                LocationCode.MinLevel));


    [Fact]
    [Trait("BR", "BR-32")]
    public void Create_ValidCode_ReturnsActiveLocationWithCode()
    {
        // Arrange
        var code = LocationCode.Create(10, 20, 30);

        // Act
        var location = CreateLocation(code);

        // Assert
        Assert.Equal(code, location.Code);
        Assert.Equal(LocationStatus.Active, location.Status);
        Assert.NotEqual(Guid.Empty, location.Id);
    }

    [Fact]
    [Trait("BR", "BR-32")]
    public void Create_NullCode_ThrowsArgumentNullException()
    {
        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => Location.Create(null!));
    }

    [Fact]
    [Trait("BR", "BR-32")]
    public void Create_SameCodeTwice_ReturnsDifferentEntities()
    {
        // Arrange
        var firstCode = LocationCode.Create(1, 1, 0);
        var secondCode = LocationCode.Create(1, 1, 0);

        // Act
        var firstLocation = CreateLocation(firstCode);
        var secondLocation = CreateLocation(secondCode);

        // Assert
        // Value object (LocationCode): equal by value. Entity (Location): equal by identity (Id), so same code ≠ same location.
        Assert.Equal(firstLocation.Code, secondLocation.Code);
        Assert.NotEqual(firstLocation.Id, secondLocation.Id);
        Assert.NotEqual(firstLocation, secondLocation);
    }


}
