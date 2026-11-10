using WarehouseManagement.Domain.StockMovements;

namespace WarehouseManagement.Domain.UnitTests.StockMovements;

public class AdjustmentReasonExtensionsTests
{
    public static TheoryData<AdjustmentReason> AllAdjustmentReasons() => new(Enum.GetValues<AdjustmentReason>());

    [Theory]
    [InlineData(AdjustmentReason.Damage, MovementType.AdjustmentDecrease, true)]
    [InlineData(AdjustmentReason.Damage, MovementType.AdjustmentIncrease, false)]
    [InlineData(AdjustmentReason.Loss, MovementType.AdjustmentDecrease, true)]
    [InlineData(AdjustmentReason.Loss, MovementType.AdjustmentIncrease, false)]
    [InlineData(AdjustmentReason.Disposal, MovementType.AdjustmentDecrease, true)]
    [InlineData(AdjustmentReason.Disposal, MovementType.AdjustmentIncrease, false)]
    [InlineData(AdjustmentReason.CountingError, MovementType.AdjustmentDecrease, true)]
    [InlineData(AdjustmentReason.CountingError, MovementType.AdjustmentIncrease, true)]
    public void IsAllowedForAdjustment_ValidReasons_ReturnsExpectedResult(AdjustmentReason adjustmentReason, MovementType movementType, bool expectedResult)
    {
        // Act
        var result = adjustmentReason.IsAllowedForAdjustment(movementType);

        // Assert
        Assert.Equal(expectedResult, result);
    }

    [Fact]
    public void IsAllowedForAdjustment_InvalidMovementType_ThrowsArgumentException()
    {
        // Arrange
        var adjustmentReason = AdjustmentReason.Damage;
        var movementType = MovementType.Receipt;

        // Act & Assert
        var exception = Assert.Throws<ArgumentException>(() => adjustmentReason.IsAllowedForAdjustment(movementType));
        Assert.Equal("movementType", exception.ParamName);
    }

    [Fact]
    public void IsAllowedForAdjustment_UnknownReason_ThrowsArgumentOutOfRangeException()
    {
        // Arrange
        var unknownReason = (AdjustmentReason)99;

        // Act & Assert
        var exception = Assert.Throws<ArgumentOutOfRangeException>(() => unknownReason.IsAllowedForAdjustment(MovementType.AdjustmentIncrease));
        Assert.Equal("adjustmentReason", exception.ParamName);
        Assert.Equal(unknownReason, exception.ActualValue);
    }

    [Theory]
    [MemberData(nameof(AllAdjustmentReasons))]
    public void IsAllowedForAdjustment_AnyDefinedReason_DoesNotThrow(AdjustmentReason adjustmentReason)
    {
        // Act
        Exception? exception = Record.Exception(() => adjustmentReason.IsAllowedForAdjustment(MovementType.AdjustmentIncrease));

        // Assert
        Assert.Null(exception);
    }
}
