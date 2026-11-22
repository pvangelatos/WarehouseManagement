using WarehouseManagement.Domain.Exceptions;
using WarehouseManagement.Domain.Items;
using WarehouseManagement.Domain.Locations;
using WarehouseManagement.Domain.StockMovements;

namespace WarehouseManagement.Domain.UnitTests.StockMovements;

public class StockMovementTests
{
    private static readonly DateOnly DefaultDocumentDate = new DateOnly(2026, 11, 10);
    private static readonly DateTimeOffset DefaultRecordedAt = new DateTimeOffset(2026, 11, 10, 12, 0, 0, TimeSpan.FromHours(2));

    private static StockMovement CreateReceipt(
        Item? item = null,
        Location? location = null,
        decimal quantity = 5,
        SupplierDocument? supplierDocument = null,
        DateOnly? documentDate = null,
        DateTimeOffset? recordedAt = null,
        string? recordedBy = null) =>
        StockMovement.CreateReceipt(
            item ?? CreateItem(),
            location ?? CreateLocation(),
            quantity,
            supplierDocument ?? SupplierDocument.Create(SupplierDocumentType.Invoice, "INV-001"),
            documentDate ?? DefaultDocumentDate,
            recordedAt ?? DefaultRecordedAt,
            recordedBy ?? "test user");

    private static StockMovement CreateIssue(
        Item? item = null,
        Location? location = null,
        decimal quantity = 5,
        DateOnly? documentDate = null,
        DateTimeOffset? recordedAt = null,
        string? recordedBy = null) =>
        StockMovement.CreateIssue(
            item ?? CreateItem(),
            location ?? CreateLocation(),
            quantity,
            documentDate ?? DefaultDocumentDate,
            recordedAt ?? DefaultRecordedAt,
            recordedBy ?? "test user");

    private static StockMovement CreateAdjustment(
        Item? item = null,
        Location? location = null,
        MovementType type = MovementType.AdjustmentIncrease,
        AdjustmentReason reason = AdjustmentReason.CountingError,
        decimal quantity = 5,
        DateOnly? documentDate = null,
        DateTimeOffset? recordedAt = null,
        string? recordedBy = null) =>
        StockMovement.CreateAdjustment(
            item ?? CreateItem(),
            location ?? CreateLocation(),
            type,
            reason,
            quantity,
            documentDate ?? DefaultDocumentDate,
            recordedAt ?? DefaultRecordedAt,
            recordedBy ?? "test user");

    private static (StockMovement Out, StockMovement In) CreateTransfer(
        Item? item = null,
        Location? source = null,
        Location? destination = null,
        decimal quantity = 5,
        DateOnly? documentDate = null,
        DateTimeOffset? recordedAt = null,
        string? recordedBy = null) =>
        StockMovement.CreateTransfer(
            item ?? CreateItem(),
            source ?? CreateLocation(),
            destination ?? CreateLocation(LocationCode.Create(
                                LocationCode.MinAisle + 1,
                                LocationCode.MinBay,
                                LocationCode.MinLevel)),
            quantity,
            documentDate ?? DefaultDocumentDate,
            recordedAt ?? DefaultRecordedAt,
            recordedBy ?? "test user");

    private static Item CreateItem(
       string sku = "VALID-SKU",
       string name = "Valid Item Name",
       UnitOfMeasure unitOfMeasure = UnitOfMeasure.Box,
       decimal reorderPoint = 10,
       bool tracksExpiry = false)
       => Item.Create(sku, name, unitOfMeasure, reorderPoint, tracksExpiry);

    private static Location CreateLocation(LocationCode? code = null) =>
        Location.Create(code ?? LocationCode.Create(
                                LocationCode.MinAisle,
                                LocationCode.MinBay,
                                LocationCode.MinLevel));

    [Fact]
    public void CreateReceipt_ValidInput_ReturnsReceiptWithAllData()
    {
        // Arrange
        var item = CreateItem();
        var location = CreateLocation();
        decimal quantity = 5;
        var supplierDocument = SupplierDocument.Create(SupplierDocumentType.Invoice, "INV-001");
        var documentDate = DefaultDocumentDate;
        var recordedAt = DefaultRecordedAt;
        string recordedBy = "test_user";

        // Act
        var stockMovement = StockMovement.CreateReceipt(
            item,
            location,
            quantity,
            supplierDocument,
            documentDate,
            recordedAt,
            recordedBy);

        // Assert
        Assert.NotEqual(Guid.Empty, stockMovement.Id);
        Assert.Equal(item.Id, stockMovement.ItemId);
        Assert.Equal(location.Id, stockMovement.LocationId);
        Assert.Equal(MovementType.Receipt, stockMovement.Type);
        Assert.Equal(quantity, stockMovement.Quantity.Value);
        Assert.Equal(documentDate, stockMovement.DocumentDate);
        Assert.Equal(recordedAt, stockMovement.RecordedAt);
        Assert.Equal(recordedBy, stockMovement.RecordedBy);
        Assert.Equal(supplierDocument, stockMovement.SupplierDocument);
        Assert.Null(stockMovement.TransferId);
    }

    [Fact]
    public void CreateReceipt_NullItem_ThrowsArgumentNullException()
    {
        // Arrange
        Item? item = null;
        var location = CreateLocation();
        decimal quantity = 5;
        var supplierDocument = SupplierDocument.Create(SupplierDocumentType.Invoice, "INV-001");
        var documentDate = DefaultDocumentDate;
        var recordedAt = DefaultRecordedAt;
        string recordedBy = "test_user";

        // Act 
        var exception = Assert.Throws<ArgumentNullException>(() => StockMovement.CreateReceipt(
        item!,
        location,
        quantity,
        supplierDocument,
        documentDate,
        recordedAt,
        recordedBy));

        // Assert
        Assert.Equal("item", exception.ParamName);
    }

    [Fact]
    public void CreateReceipt_NullLocation_ThrowsArgumentNullException()
    {
        // Arrange
        var item = CreateItem();
        Location? location = null;
        decimal quantity = 5;
        var supplierDocument = SupplierDocument.Create(SupplierDocumentType.Invoice, "INV-001");
        var documentDate = DefaultDocumentDate;
        var recordedAt = DefaultRecordedAt;
        string recordedBy = "test_user";

        // Act
        var exception = Assert.Throws<ArgumentNullException>(() => StockMovement.CreateReceipt(
            item,
            location!,
            quantity,
            supplierDocument,
            documentDate,
            recordedAt,
            recordedBy));

        // Assert
        Assert.Equal("location", exception.ParamName);
    }

    [Fact]
    public void CreateReceipt_NullSupplierDocument_ThrowsArgumentNullException()
    {
        // Arrange
        var item = CreateItem();
        var location = CreateLocation();
        decimal quantity = 5;
        SupplierDocument? supplierDocument = null;
        var documentDate = DefaultDocumentDate;
        var recordedAt = DefaultRecordedAt;
        string recordedBy = "test_user";

        // Act
        var exception = Assert.Throws<ArgumentNullException>(() => StockMovement.CreateReceipt(
            item,
            location,
            quantity,
            supplierDocument!,
            documentDate,
            recordedAt,
            recordedBy));

        // Assert
        Assert.Equal("supplierDocument", exception.ParamName);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void CreateReceipt_MissingRecordedBy_ThrowsArgumentException(string? recordedBy)
    {
        // Arrange
        var item = CreateItem();
        var location = CreateLocation();
        decimal quantity = 5;
        var supplierDocument = SupplierDocument.Create(SupplierDocumentType.Invoice, "INV-001");
        var documentDate = DefaultDocumentDate;
        var recordedAt = DefaultRecordedAt;

        // Act
        var exception = Assert.ThrowsAny<ArgumentException>(() => StockMovement.CreateReceipt(
            item,
            location,
            quantity,
            supplierDocument,
            documentDate,
            recordedAt,
            recordedBy!));

        // Assert
        Assert.Equal("recordedBy", exception.ParamName);
    }

    [Fact]
    public void CreateReceipt_DiscontinuedItem_ThrowsBusinessRuleException()
    {
        // Arrange
        var item = CreateItem();
        item.Discontinue();

        // Act
        var exception = Assert.Throws<BusinessRuleException>(() => CreateReceipt(item: item));

        // Assert
        Assert.Equal("Movement type Receipt is not allowed for item VALID-SKU with status Discontinued.", exception.Message);
    }

    [Fact]
    public void CreateReceipt_InactiveItem_ThrowsBusinessRuleException()
    {
        // Arrange
        var item = CreateItem();
        item.Discontinue();
        item.Deactivate(Quantity.Zero);

        // Act
        var exception = Assert.Throws<BusinessRuleException>(() => CreateReceipt(item: item));

        // Assert
        Assert.Equal("Movement type Receipt is not allowed for item VALID-SKU with status Inactive.", exception.Message);
    }

    [Fact]
    public void CreateReceipt_InactiveLocation_ThrowsBusinessRuleException()
    {
        // Arrange
        var location = CreateLocation();
        location.Deactivate(isEmpty: true);

        // Act
        var exception = Assert.Throws<BusinessRuleException>(() => CreateReceipt(location: location));

        // Assert
        Assert.Equal("Location 001-01-00 with status Inactive does not allow stock movements.", exception.Message);
    }

    [Fact]
    public void CreateReceipt_ZeroQuantity_ThrowsBusinessRuleException()
    {
        // Act
        var exception = Assert.Throws<BusinessRuleException>(() => CreateReceipt(quantity: 0));

        // Assert
        Assert.Equal("Quantity must be greater than zero.", exception.Message);
    }

    [Fact]
    public void CreateReceipt_DecimalQuantityForWholeUnit_ThrowsBusinessRuleException()
    {
        // Arrange
        decimal quantity = 1.5m;

        // Act
        var exception = Assert.Throws<BusinessRuleException>(() => CreateReceipt(quantity: quantity));

        // Assert
        Assert.Equal("Quantity for Box cannot have more than 0 decimal places.", exception.Message);

    }

    [Fact]
    public void CreateReceipt_DiscontinuedItemAndZeroQuantity_ThrowsItemStatusErrorFirst()
    {
        // Arrange
        var item = CreateItem();
        item.Discontinue();

        // Act
        var exception = Assert.Throws<BusinessRuleException>(() => CreateReceipt(item: item, quantity: 0));

        // Assert
        Assert.Equal("Movement type Receipt is not allowed for item VALID-SKU with status Discontinued.", exception.Message);
    }

    [Fact]
    public void CreateReceipt_DocumentDateAfterRecordingDate_ThrowsBusinessRuleException()
    {
        // Arrange
        var documentDate = new DateOnly(2026, 11, 11);
        var recordedAt = new DateTimeOffset(2026, 11, 10, 12, 0, 0, TimeSpan.FromHours(2));

        // Act
        var exception = Assert.Throws<BusinessRuleException>(() => CreateReceipt(
            documentDate: documentDate,
            recordedAt: recordedAt));

        // Assert
        Assert.Equal("Document date 2026-11-11 cannot be after recording date 2026-11-10.", exception.Message);
    }

    [Fact]
    public void CreateReceipt_DocumentDateSameAsRecordingDate_ReturnsStockMovement()
    {
        // Arrange
        var documentDate = new DateOnly(2026, 11, 10);
        var recordedAt = new DateTimeOffset(2026, 11, 10, 23, 59, 0, TimeSpan.FromHours(2));

        // Act
        var stockMovement = CreateReceipt(
            documentDate: documentDate,
            recordedAt: recordedAt);

        // Assert
        Assert.Equal(documentDate, stockMovement.DocumentDate);
        Assert.Equal(recordedAt, stockMovement.RecordedAt);
    }

    [Fact]
    public void CreateReceipt_DocumentDateFarInPast_ReturnsStockMovement()
    {
        // Arrange
        var documentDate = new DateOnly(1990, 1, 1);

        // Act
        var stockMovement = CreateReceipt(documentDate: documentDate);

        // Assert
        Assert.Equal(documentDate, stockMovement.DocumentDate);
    }

    [Fact]
    public void CreateReceipt_RecordedAfterMidnightLocalTime_ReturnsStockMovement()
    {
        // Arrange
        var recordedAt = new DateTimeOffset(2026, 11, 11, 0, 30, 0, TimeSpan.FromHours(2));
        var documentDate = new DateOnly(2026, 11, 11);

        // Act
        var stockMovement = CreateReceipt(
            documentDate: documentDate,
            recordedAt: recordedAt);

        // Assert
        Assert.Equal(documentDate, stockMovement.DocumentDate);
        Assert.Equal(recordedAt, stockMovement.RecordedAt);
    }

    [Fact]
    public void CreateIssue_DiscontinuedItem_ReturnsIssue()
    {
        // Arrange
        var item = CreateItem();
        item.Discontinue();

        // Act
        StockMovement stockMovement = CreateIssue(item: item);

        // Assert
        Assert.Equal(MovementType.Issue, stockMovement.Type);
        Assert.Equal(item.Id, stockMovement.ItemId);
    }

    [Fact]
    public void CreateIssue_InactiveItem_ThrowsBusinessRuleException()
    {
        // Arrange
        var item = CreateItem();
        item.Discontinue();
        item.Deactivate(Quantity.Zero);

        // Act
        var exception = Assert.Throws<BusinessRuleException>(() => CreateIssue(item: item));

        // Assert
        Assert.Equal("Movement type Issue is not allowed for item VALID-SKU with status Inactive.", exception.Message);
    }

    [Fact]
    public void CreateIssue_ValidInput_ReturnsIssueWithAllData()
    {
        // Arrange
        var item = CreateItem();
        var location = CreateLocation();
        decimal quantity = 5;
        var documentDate = DefaultDocumentDate;
        var recordedAt = DefaultRecordedAt;
        string recordedBy = "test_user";

        // Act
        var stockMovement = StockMovement.CreateIssue(
            item,
            location,
            quantity,
            documentDate,
            recordedAt,
            recordedBy);

        // Assert
        Assert.NotEqual(Guid.Empty, stockMovement.Id);
        Assert.Equal(item.Id, stockMovement.ItemId);
        Assert.Equal(location.Id, stockMovement.LocationId);
        Assert.Equal(MovementType.Issue, stockMovement.Type);
        Assert.Equal(quantity, stockMovement.Quantity.Value);
        Assert.Equal(documentDate, stockMovement.DocumentDate);
        Assert.Equal(recordedAt, stockMovement.RecordedAt);
        Assert.Equal(recordedBy, stockMovement.RecordedBy);
        Assert.Null(stockMovement.SupplierDocument);
        Assert.Null(stockMovement.TransferId);
    }

    [Fact]
    public void CreateIssue_InactiveLocation_ThrowsBusinessRuleException()
    {
        // Arrange
        var location = CreateLocation();
        location.Deactivate(isEmpty: true);

        // Act
        var exception = Assert.Throws<BusinessRuleException>(() => CreateIssue(location: location));

        // Assert
        Assert.Equal("Location 001-01-00 with status Inactive does not allow stock movements.", exception.Message);
    }

    [Fact]
    public void CreateIssue_ZeroQuantity_ThrowsBusinessRuleException()
    {
        // Act
        var exception = Assert.Throws<BusinessRuleException>(() => CreateIssue(quantity: 0));

        // Assert
        Assert.Equal("Quantity must be greater than zero.", exception.Message);
    }

    [Fact]
    public void CreateIssue_DecimalQuantityForWholeUnit_ThrowsBusinessRuleException()
    {
        // Arrange
        decimal quantity = 1.5m;

        // Act
        var exception = Assert.Throws<BusinessRuleException>(() => CreateIssue(quantity: quantity));

        // Assert
        Assert.Equal("Quantity for Box cannot have more than 0 decimal places.", exception.Message);
    }

    [Fact]
    public void CreateIssue_DocumentDateAfterRecordingDate_ThrowsBusinessRuleException()
    {
        // Arrange
        var documentDate = new DateOnly(2026, 11, 11);
        var recordedAt = new DateTimeOffset(2026, 11, 10, 12, 0, 0, TimeSpan.FromHours(2));

        // Act
        var exception = Assert.Throws<BusinessRuleException>(() => CreateIssue(
            documentDate: documentDate,
            recordedAt: recordedAt));

        // Assert
        Assert.Equal("Document date 2026-11-11 cannot be after recording date 2026-11-10.", exception.Message);
    }

    [Fact]
    public void CreateIssue_NullItem_ThrowsArgumentNullException()
    {
        // Arrange
        Item? item = null;
        var location = CreateLocation();
        decimal quantity = 5;
        var documentDate = DefaultDocumentDate;
        var recordedAt = DefaultRecordedAt;
        string recordedBy = "test_user";

        // Act 
        var exception = Assert.Throws<ArgumentNullException>(() => StockMovement.CreateIssue(
        item!,
        location,
        quantity,
        documentDate,
        recordedAt,
        recordedBy));

        // Assert
        Assert.Equal("item", exception.ParamName);
    }

    [Fact]
    public void CreateIssue_NullLocation_ThrowsArgumentNullException()
    {
        // Arrange
        var item = CreateItem();
        Location? location = null;
        decimal quantity = 5;
        var documentDate = DefaultDocumentDate;
        var recordedAt = DefaultRecordedAt;
        string recordedBy = "test_user";

        // Act
        var exception = Assert.Throws<ArgumentNullException>(() => StockMovement.CreateIssue(
            item,
            location!,
            quantity,
            documentDate,
            recordedAt,
            recordedBy));

        // Assert
        Assert.Equal("location", exception.ParamName);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void CreateIssue_MissingRecordedBy_ThrowsArgumentException(string? recordedBy)
    {
        // Arrange
        var item = CreateItem();
        var location = CreateLocation();
        decimal quantity = 5;
        var documentDate = DefaultDocumentDate;
        var recordedAt = DefaultRecordedAt;

        // Act
        var exception = Assert.ThrowsAny<ArgumentException>(() => StockMovement.CreateIssue(
            item,
            location,
            quantity,
            documentDate,
            recordedAt,
            recordedBy!));

        // Assert
        Assert.Equal("recordedBy", exception.ParamName);
    }

    [Fact]
    public void CreateIssue_InactiveItemAndZeroQuantity_ThrowsItemStatusErrorFirst()
    {
        // Arrange
        var item = CreateItem();
        item.Discontinue();
        item.Deactivate(Quantity.Zero);

        // Act
        var exception = Assert.Throws<BusinessRuleException>(() => CreateIssue(item: item, quantity: 0));

        // Assert
        Assert.Equal("Movement type Issue is not allowed for item VALID-SKU with status Inactive.", exception.Message);
    }

    [Fact]
    public void CreateAdjustment_ValidInput_ReturnsAdjustmentWithAllData()
    {
        // Arrange
        var item = CreateItem();
        var location = CreateLocation();
        var type = MovementType.AdjustmentIncrease;
        var reason = AdjustmentReason.CountingError;
        decimal quantity = 5;
        var documentDate = DefaultDocumentDate;
        var recordedAt = DefaultRecordedAt;
        string recordedBy = "test_user";

        // Act
        var stockMovement = StockMovement.CreateAdjustment(
            item,
            location,
            type,
            reason,
            quantity,
            documentDate,
            recordedAt,
            recordedBy);

        // Assert
        Assert.NotEqual(Guid.Empty, stockMovement.Id);
        Assert.Equal(item.Id, stockMovement.ItemId);
        Assert.Equal(location.Id, stockMovement.LocationId);
        Assert.Equal(type, stockMovement.Type);
        Assert.Equal(reason, stockMovement.AdjustmentReason);
        Assert.Equal(quantity, stockMovement.Quantity.Value);
        Assert.Equal(documentDate, stockMovement.DocumentDate);
        Assert.Equal(recordedAt, stockMovement.RecordedAt);
        Assert.Equal(recordedBy, stockMovement.RecordedBy);
        Assert.Null(stockMovement.SupplierDocument);
        Assert.Null(stockMovement.TransferId);
    }

    [Fact]
    public void CreateAdjustment_NullItem_ThrowsArgumentNullException()
    {
        // Arrange
        var location = CreateLocation();
        var type = MovementType.AdjustmentIncrease;
        var reason = AdjustmentReason.CountingError;
        decimal quantity = 5;
        var documentDate = DefaultDocumentDate;
        var recordedAt = DefaultRecordedAt;
        string recordedBy = "test_user";

        // Act
        var exception = Assert.Throws<ArgumentNullException>(() => StockMovement.CreateAdjustment(
            null!,
            location,
            type,
            reason,
            quantity,
            documentDate,
            recordedAt,
            recordedBy));

        // Assert
        Assert.Equal("item", exception.ParamName);
    }

    [Fact]
    public void CreateAdjustment_NullLocation_ThrowsArgumentNullException()
    {
        // Arrange
        var item = CreateItem();
        var type = MovementType.AdjustmentIncrease;
        var reason = AdjustmentReason.CountingError;
        decimal quantity = 5;
        var documentDate = DefaultDocumentDate;
        var recordedAt = DefaultRecordedAt;
        string recordedBy = "test_user";

        // Act
        var exception = Assert.Throws<ArgumentNullException>(() => StockMovement.CreateAdjustment(
            item,
            null!,
            type,
            reason,
            quantity,
            documentDate,
            recordedAt,
            recordedBy));

        // Assert
        Assert.Equal("location", exception.ParamName);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void CreateAdjustment_MissingRecordedBy_ThrowsArgumentException(string? recordedBy)
    {
        // Arrange
        var item = CreateItem();
        var location = CreateLocation();
        var type = MovementType.AdjustmentIncrease;
        var reason = AdjustmentReason.CountingError;
        decimal quantity = 5;
        var documentDate = DefaultDocumentDate;
        var recordedAt = DefaultRecordedAt;

        // Act
        var exception = Assert.ThrowsAny<ArgumentException>(() => StockMovement.CreateAdjustment(
            item,
            location,
            type,
            reason,
            quantity,
            documentDate,
            recordedAt,
            recordedBy!));

        // Assert
        Assert.Equal("recordedBy", exception.ParamName);
    }

    [Theory]
    [InlineData(MovementType.Receipt)]
    [InlineData(MovementType.Issue)]
    [InlineData(MovementType.TransferIn)]
    [InlineData(MovementType.TransferOut)]
    [InlineData(MovementType.Reversal)]
    public void CreateAdjustment_InvalidMovementType_ThrowsArgumentException(MovementType movementType)
    {
        // Act
        var exception = Assert.Throws<ArgumentException>(() => CreateAdjustment(type: movementType));

        // Assert
        Assert.Equal("type", exception.ParamName);
    }

    [Theory]
    [InlineData(AdjustmentReason.Damage, "Adjustment reason Damage is not allowed for movement type AdjustmentIncrease.")]
    [InlineData(AdjustmentReason.Loss, "Adjustment reason Loss is not allowed for movement type AdjustmentIncrease.")]
    [InlineData(AdjustmentReason.Disposal, "Adjustment reason Disposal is not allowed for movement type AdjustmentIncrease.")]
    public void CreateAdjustment_InvalidReasonForMovementType_ThrowsBusinessRuleException(AdjustmentReason reason, string expectedMessage)
    {
        // Act
        var exception = Assert.Throws<BusinessRuleException>(() => CreateAdjustment(type: MovementType.AdjustmentIncrease, reason: reason));

        // Assert
        Assert.Equal(expectedMessage, exception.Message);
    }

    [Fact]
    public void CreateAdjustment_DecreaseWithAllowedReason_ReturnsAdjustment()
    {
        // Arrange
        var type = MovementType.AdjustmentDecrease;
        var reason = AdjustmentReason.Damage;

        // Act
        var stockMovement = CreateAdjustment(type: type, reason: reason);

        // Assert
        Assert.Equal(type, stockMovement.Type);
        Assert.Equal(reason, stockMovement.AdjustmentReason);
    }

    [Theory]
    [InlineData(MovementType.AdjustmentDecrease)]
    [InlineData(MovementType.AdjustmentIncrease)]
    public void CreateAdjustment_DiscontinuedItem_ReturnsAdjustment(MovementType type)
    {
        // Arrange
        var item = CreateItem();
        item.Discontinue();

        // Act
        StockMovement stockMovement = CreateAdjustment(type: type, item: item);

        // Assert
        Assert.Equal(type, stockMovement.Type);
        Assert.Equal(item.Id, stockMovement.ItemId);
    }

    [Fact]
    public void CreateAdjustment_InactiveItem_ThrowsBusinessRuleException()
    {
        // Arrange
        var item = CreateItem();
        item.Discontinue();
        item.Deactivate(Quantity.Zero);

        // Act
        var exception = Assert.Throws<BusinessRuleException>(() => CreateAdjustment(item: item, type: MovementType.AdjustmentIncrease));

        // Assert
        Assert.Equal("Movement type AdjustmentIncrease is not allowed for item VALID-SKU with status Inactive.", exception.Message);
    }

    [Fact]
    public void CreateAdjustment_InactiveLocation_ThrowsBusinessRuleException()
    {
        // Arrange
        var location = CreateLocation();
        location.Deactivate(isEmpty: true);

        // Act
        var exception = Assert.Throws<BusinessRuleException>(() => CreateAdjustment(location: location));

        // Assert
        Assert.Equal("Location 001-01-00 with status Inactive does not allow stock movements.", exception.Message);
    }

    [Fact]
    public void CreateAdjustment_ZeroQuantity_ThrowsBusinessRuleException()
    {
        // Act
        var exception = Assert.Throws<BusinessRuleException>(() => CreateAdjustment(quantity: 0));

        // Assert
        Assert.Equal("Quantity must be greater than zero.", exception.Message);
    }

    [Fact]
    public void CreateAdjustment_DecimalQuantityForWholeUnit_ThrowsBusinessRuleException()
    {
        // Arrange
        decimal quantity = 1.5m;

        // Act
        var exception = Assert.Throws<BusinessRuleException>(() => CreateAdjustment(quantity: quantity));

        // Assert
        Assert.Equal("Quantity for Box cannot have more than 0 decimal places.", exception.Message);
    }

    [Fact]
    public void CreateAdjustment_DocumentDateAfterRecordingDate_ThrowsBusinessRuleException()
    {
        // Arrange
        var documentDate = new DateOnly(2026, 11, 11);
        var recordedAt = new DateTimeOffset(2026, 11, 10, 12, 0, 0, TimeSpan.FromHours(2));

        // Act
        var exception = Assert.Throws<BusinessRuleException>(() => CreateAdjustment(
            documentDate: documentDate,
            recordedAt: recordedAt));

        // Assert
        Assert.Equal("Document date 2026-11-11 cannot be after recording date 2026-11-10.", exception.Message);
    }

    [Fact]
    public void CreateAdjustment_InactiveItemAndInvalidReason_ThrowsItemStatusErrorFirst()
    {
        // Arrange
        var item = CreateItem();
        item.Discontinue();
        item.Deactivate(Quantity.Zero);

        // Act
        var exception = Assert.Throws<BusinessRuleException>(() => CreateAdjustment(item: item, type: MovementType.AdjustmentIncrease, reason: AdjustmentReason.Damage));

        // Assert
        Assert.Equal("Movement type AdjustmentIncrease is not allowed for item VALID-SKU with status Inactive.", exception.Message);
    }

    [Fact]
    public void CreateTransfer_ValidInput_ReturnsOutAndInWithAllData()
    {
        // Arrange
        var item = CreateItem();
        var source = CreateLocation();
        var destination = CreateLocation(LocationCode.Create(
                                LocationCode.MinAisle + 1,
                                LocationCode.MinBay,
                                LocationCode.MinLevel));
        decimal quantity = 5;
        var documentDate = DefaultDocumentDate;
        var recordedAt = DefaultRecordedAt;
        string recordedBy = "test_user";

        // Act
        var (transferOut, transferIn) = StockMovement.CreateTransfer(
            item,
            source,
            destination,
            quantity,
            documentDate,
            recordedAt,
            recordedBy);

        // Assert
        Assert.NotEqual(Guid.Empty, transferOut.Id);
        Assert.Equal(MovementType.TransferOut, transferOut.Type);
        Assert.Equal(source.Id, transferOut.LocationId);
        Assert.Equal(item.Id, transferOut.ItemId);
        Assert.Equal(quantity, transferOut.Quantity.Value);
        Assert.Equal(documentDate, transferOut.DocumentDate);
        Assert.Equal(recordedAt, transferOut.RecordedAt);
        Assert.Equal(recordedBy, transferOut.RecordedBy);
        Assert.Null(transferOut.AdjustmentReason);
        Assert.Null(transferOut.SupplierDocument);

        Assert.NotEqual(Guid.Empty, transferIn.Id);
        Assert.Equal(MovementType.TransferIn, transferIn.Type);
        Assert.Equal(destination.Id, transferIn.LocationId);
        Assert.Equal(item.Id, transferIn.ItemId);
        Assert.Equal(quantity, transferIn.Quantity.Value);
        Assert.Equal(documentDate, transferIn.DocumentDate);
        Assert.Equal(recordedAt, transferIn.RecordedAt);
        Assert.Equal(recordedBy, transferIn.RecordedBy);
        Assert.Null(transferIn.AdjustmentReason);
        Assert.Null(transferIn.SupplierDocument);

        Assert.NotNull(transferOut.TransferId);
        Assert.Equal(transferOut.TransferId, transferIn.TransferId);
        Assert.NotEqual(transferOut.Id, transferIn.Id);
    }
}
