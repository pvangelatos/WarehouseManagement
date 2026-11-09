using WarehouseManagement.Domain.Exceptions;
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

    [Fact]
    [Trait("BR", "BR-33")]
    public void Deactivate_ActiveAndEmpty_SetsStatusToInactive()
    {
        // Arrange
        var location = CreateLocation();

        // Act
        location.Deactivate(isEmpty: true);

        // Assert
        Assert.Equal(LocationStatus.Inactive, location.Status);
    }

    [Fact]
    [Trait("BR", "BR-33")]
    public void Deactivate_AlreadyInactive_ThrowsBusinessRuleException()
    {
        // Arrange
        var location = CreateLocation();
        location.Deactivate(isEmpty: true);

        // Act & Assert
        Assert.Throws<BusinessRuleException>(() => location.Deactivate(isEmpty: true));
    }

    [Fact]
    [Trait("BR", "BR-18")]
    public void Deactivate_NotEmpty_ThrowsBusinessRuleException()
    {
        // Arrange
        var location = CreateLocation();

        // Act & Assert
        Assert.Throws<BusinessRuleException>(() => location.Deactivate(isEmpty: false));
    }

    [Fact]
    [Trait("BR", "BR-18")]
    public void Deactivate_NotEmpty_StatusRemainsActive()
    {
        // Arrange
        var location = CreateLocation();

        // Act & Assert
        Assert.Throws<BusinessRuleException>(() => location.Deactivate(isEmpty: false));
        Assert.Equal(LocationStatus.Active, location.Status);
    }

    [Fact]
    [Trait("BR", "BR-18")]
    [Trait("BR", "BR-33")]
    public void Deactivate_AlreadyInactiveAndNotEmpty_ThrowsStateErrorFirst()
    {
        // Arrange
        var location = CreateLocation();
        location.Deactivate(isEmpty: true);

        // Act & Assert
        var exception = Assert.Throws<BusinessRuleException>(() => location.Deactivate(isEmpty: false));
        Assert.Contains("already inactive", exception.Message);
    }
}
