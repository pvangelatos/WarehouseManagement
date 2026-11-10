using WarehouseManagement.Domain.StockMovements;

namespace WarehouseManagement.Domain.UnitTests.StockMovements;

public class MovementTypeExtensionsTests
{
    public static TheoryData<MovementType> MovementTypes => new(Enum.GetValues<MovementType>().Where(type => type != MovementType.Reversal));

    [Theory]
    [InlineData(MovementType.Receipt, MovementDirection.In)]
    [InlineData(MovementType.TransferIn, MovementDirection.In)]
    [InlineData(MovementType.AdjustmentIncrease, MovementDirection.In)]
    [InlineData(MovementType.Issue, MovementDirection.Out)]
    [InlineData(MovementType.TransferOut, MovementDirection.Out)]
    [InlineData(MovementType.AdjustmentDecrease, MovementDirection.Out)]
    public void Direction_NonReversalType_ReturnsExpectedDirection(MovementType movementType, MovementDirection expectedDirection)
    {
        // Act
        var direction = movementType.Direction();

        // Assert
        Assert.Equal(expectedDirection, direction);
    }

    [Fact]
    public void Direction_ReversalMovementType_ThrowsInvalidOperationException()
    {
        // Arrange
        var movementType = MovementType.Reversal;

        // Act & Assert
        var exception = Assert.Throws<InvalidOperationException>(() => movementType.Direction());
        Assert.Equal("Reversal movement type does not have a direction. The direction is the opposite of the original movement type being reversed.", exception.Message);
    }

    [Fact]
    public void Direction_UnknownMovementType_ThrowsArgumentOutOfRangeException()
    {
        // Arrange
        var unknownMovementType = (MovementType)99;

        // Act & Assert
        var exception = Assert.Throws<ArgumentOutOfRangeException>(() => unknownMovementType.Direction());
        Assert.Equal("movementType", exception.ParamName);
        Assert.Equal(unknownMovementType, exception.ActualValue);
    }

    [Theory]
    [MemberData(nameof(MovementTypes))]
    public void Direction_AnyNonReversalType_ReturnsDefinedDirection(MovementType movementType)
    {
        // Act
        MovementDirection direction = movementType.Direction();

        // Assert
        Assert.True(Enum.IsDefined(direction));
    }
}
