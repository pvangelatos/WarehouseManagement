using WarehouseManagement.Domain.Items;
using WarehouseManagement.Domain.Locations;

namespace WarehouseManagement.Domain.StockMovements;

public sealed class StockMovement
{
    public Guid Id { get; }
    public Guid ItemId { get; }
    public Guid LocationId { get; }
    public MovementType Type { get; }
    public Quantity Quantity { get; }
    public DateOnly DocumentDate { get; }
    public DateTimeOffset RecordedAt { get; }
    public string RecordedBy { get; }
    public SupplierDocument? SupplierDocument { get; }

    private StockMovement(
        Guid id,
        Guid itemId,
        Guid locationId,
        MovementType type,
        Quantity quantity,
        DateOnly documentDate,
        DateTimeOffset recordedAt,
        string recordedBy,
        SupplierDocument? supplierDocument)
    {
        Id = id;
        ItemId = itemId;
        LocationId = locationId;
        Type = type;
        Quantity = quantity;
        DocumentDate = documentDate;
        RecordedAt = recordedAt;
        RecordedBy = recordedBy;
        SupplierDocument = supplierDocument;
    }

    public static StockMovement CreateReceipt(
        Item item,
        Location location,
        decimal quantity,
        SupplierDocument supplierDocument,
        DateOnly documentDate,
        DateTimeOffset recordedAt,
        string recordedBy)
    {
        ArgumentNullException.ThrowIfNull(item, nameof(item));
        ArgumentNullException.ThrowIfNull(location, nameof(location));
        ArgumentNullException.ThrowIfNull(supplierDocument, nameof(supplierDocument));
        ArgumentException.ThrowIfNullOrWhiteSpace(recordedBy, nameof(recordedBy));

        Quantity quantityValue = Quantity.Create(quantity, item.UnitOfMeasure);

        return new StockMovement(
            Guid.CreateVersion7(),
            item.Id,
            location.Id,
            MovementType.Receipt,
            quantityValue,
            documentDate,
            recordedAt,
            recordedBy,
            supplierDocument);
    }
}
