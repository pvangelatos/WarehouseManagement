using WarehouseManagement.Domain.Items;
using WarehouseManagement.Domain.StockMovements;

namespace WarehouseManagement.Domain.UnitTests.StockMovements;

public class MovementTypeExtensionsTests
{
    public static TheoryData<MovementType> MovementTypes => new(Enum.GetValues<MovementType>().Where(type => type != MovementType.Reversal));
    public static TheoryData<ItemStatus> ItemStatuses => new(Enum.GetValues<ItemStatus>());

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

    [Theory]
    [InlineData(MovementType.Receipt, ItemStatus.Active, true)]
    [InlineData(MovementType.Issue, ItemStatus.Active, true)]
    [InlineData(MovementType.TransferOut, ItemStatus.Active, true)]
    [InlineData(MovementType.TransferIn, ItemStatus.Active, true)]
    [InlineData(MovementType.AdjustmentIncrease, ItemStatus.Active, true)]
    [InlineData(MovementType.AdjustmentDecrease, ItemStatus.Active, true)]
    [InlineData(MovementType.Reversal, ItemStatus.Active, true)]
    [InlineData(MovementType.Receipt, ItemStatus.Discontinued, false)]
    [InlineData(MovementType.Issue, ItemStatus.Discontinued, true)]
    [InlineData(MovementType.TransferOut, ItemStatus.Discontinued, true)]
    [InlineData(MovementType.TransferIn, ItemStatus.Discontinued, true)]
    [InlineData(MovementType.AdjustmentIncrease, ItemStatus.Discontinued, true)]
    [InlineData(MovementType.AdjustmentDecrease, ItemStatus.Discontinued, true)]
    [InlineData(MovementType.Reversal, ItemStatus.Discontinued, true)]
    [InlineData(MovementType.Receipt, ItemStatus.Inactive, false)]
    [InlineData(MovementType.Issue, ItemStatus.Inactive, false)]
    [InlineData(MovementType.TransferOut, ItemStatus.Inactive, false)]
    [InlineData(MovementType.TransferIn, ItemStatus.Inactive, false)]
    [InlineData(MovementType.AdjustmentIncrease, ItemStatus.Inactive, false)]
    [InlineData(MovementType.AdjustmentDecrease, ItemStatus.Inactive, false)]
    [InlineData(MovementType.Reversal, ItemStatus.Inactive, false)]
    public void IsAllowedFor_TypeAndItemStatus_ReturnsExpectedResult(MovementType movementType, ItemStatus itemStatus, bool expectedResult)
    {
        // Act
        var result = movementType.IsAllowedFor(itemStatus);

        // Assert
        Assert.Equal(expectedResult, result);
    }

    [Fact]
    public void IsAllowedFor_UnknownItemStatus_ThrowsArgumentOutOfRangeException()
    {
        // Arrange
        var unknownItemStatus = (ItemStatus)99;

        // Act & Assert
        var exception = Assert.Throws<ArgumentOutOfRangeException>(() => MovementType.Receipt.IsAllowedFor(unknownItemStatus));
        Assert.Equal("itemStatus", exception.ParamName);
        Assert.Equal(unknownItemStatus, exception.ActualValue);
    }

    [Theory]
    [MemberData(nameof(ItemStatuses))]
    public void IsAllowedFor_AnyItemStatus_DoesNotThrow(ItemStatus itemStatus)
    {
        // Act
        Exception? exception = Record.Exception(() => MovementType.AdjustmentIncrease.IsAllowedFor(itemStatus));

        // Assert
        Assert.Null(exception);
    }
}
