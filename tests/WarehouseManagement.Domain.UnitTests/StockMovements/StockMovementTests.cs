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
        Assert.Equal("Cannot receive item VALID-SKU with status Discontinued.", exception.Message);
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
        Assert.Equal("Cannot receive item VALID-SKU with status Inactive.", exception.Message);
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
        Assert.Equal($"Cannot receive stock to location 001-01-00 with status Inactive.", exception.Message);
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
        Assert.Equal("Cannot receive item VALID-SKU with status Discontinued.", exception.Message);
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
}
