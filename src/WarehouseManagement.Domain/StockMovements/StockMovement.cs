using WarehouseManagement.Domain.Exceptions;
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
        ArgumentNullException.ThrowIfNull(item);
        ArgumentNullException.ThrowIfNull(location);
        ArgumentNullException.ThrowIfNull(supplierDocument);
        ArgumentException.ThrowIfNullOrWhiteSpace(recordedBy);

        if (item.Status is not ItemStatus.Active)
            throw new BusinessRuleException($"Cannot receive item {item.Sku.Value} with status {item.Status}.");

        if (location.Status is not LocationStatus.Active)
            throw new BusinessRuleException($"Cannot receive stock to location {location.Code} with status {location.Status}.");

        Quantity quantityValue = Quantity.Create(quantity, item.UnitOfMeasure);

        if (quantityValue.Value <= 0)
            throw new BusinessRuleException("Quantity must be greater than zero.");

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
