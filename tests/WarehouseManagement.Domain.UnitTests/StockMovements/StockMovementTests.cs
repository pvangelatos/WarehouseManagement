using WarehouseManagement.Domain.Items;
using WarehouseManagement.Domain.Locations;
using WarehouseManagement.Domain.StockMovements;

namespace WarehouseManagement.Domain.UnitTests.StockMovements;

public class StockMovementTests
{
    private static readonly DateOnly DefaultDocumentDate = new DateOnly(2026, 11, 10);
    private static readonly DateTimeOffset DefaultRecordedAt = new DateTimeOffset(2026, 11, 10, 12, 0, 0, TimeSpan.FromHours(2));
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
}
