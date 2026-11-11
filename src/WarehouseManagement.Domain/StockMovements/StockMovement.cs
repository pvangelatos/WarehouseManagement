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
    public AdjustmentReason? AdjustmentReason { get; }

    private StockMovement(
        Guid id,
        Guid itemId,
        Guid locationId,
        MovementType type,
        Quantity quantity,
        DateOnly documentDate,
        DateTimeOffset recordedAt,
        string recordedBy,
        SupplierDocument? supplierDocument,
        AdjustmentReason? adjustmentReason)
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
        AdjustmentReason = adjustmentReason;
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

        ValidateItemStatus(item, MovementType.Receipt);

        ValidateLocationStatus(location);

        Quantity quantityValue = CreatePositiveQuantity(quantity, item.UnitOfMeasure);

        ValidateDocumentDate(documentDate, recordedAt);

        return new StockMovement(
            Guid.CreateVersion7(),
            item.Id,
            location.Id,
            MovementType.Receipt,
            quantityValue,
            documentDate,
            recordedAt,
            recordedBy,
            supplierDocument,
            adjustmentReason: null);
    }

    public static StockMovement CreateIssue(
        Item item,
        Location location,
        decimal quantity,
        DateOnly documentDate,
        DateTimeOffset recordedAt,
        string recordedBy)
    {
        ArgumentNullException.ThrowIfNull(item);
        ArgumentNullException.ThrowIfNull(location);
        ArgumentException.ThrowIfNullOrWhiteSpace(recordedBy);

        ValidateItemStatus(item, MovementType.Issue);

        ValidateLocationStatus(location);

        Quantity quantityValue = CreatePositiveQuantity(quantity, item.UnitOfMeasure);

        ValidateDocumentDate(documentDate, recordedAt);

        return new StockMovement(
            Guid.CreateVersion7(),
            item.Id,
            location.Id,
            MovementType.Issue,
            quantityValue,
            documentDate,
            recordedAt,
            recordedBy,
            supplierDocument: null,
            adjustmentReason: null);
    }

    public static StockMovement CreateAdjustment(
        Item item,
        Location location,
        MovementType type,
        AdjustmentReason adjustmentReason,
        decimal quantity,
        DateOnly documentDate,
        DateTimeOffset recordedAt,
        string recordedBy)
    {
        ArgumentNullException.ThrowIfNull(item);
        ArgumentNullException.ThrowIfNull(location);
        ArgumentException.ThrowIfNullOrWhiteSpace(recordedBy);

        if (type is not (MovementType.AdjustmentIncrease or MovementType.AdjustmentDecrease))
            throw new ArgumentException($"Movement type must be AdjustmentIncrease or AdjustmentDecrease. Provided: {type}", nameof(type));

        ValidateItemStatus(item, type);

        ValidateLocationStatus(location);

        Quantity quantityValue = CreatePositiveQuantity(quantity, item.UnitOfMeasure);

        ValidateAdjustmentReason(adjustmentReason, type);

        ValidateDocumentDate(documentDate, recordedAt);

        return new StockMovement(
            Guid.CreateVersion7(),
            item.Id,
            location.Id,
            type,
            quantityValue,
            documentDate,
            recordedAt,
            recordedBy,
            supplierDocument: null,
            adjustmentReason: adjustmentReason);
    }

    private static void ValidateItemStatus(Item item, MovementType movementType)
    {
        if (!movementType.IsAllowedFor(item.Status))
            throw new BusinessRuleException($"Movement type {movementType} is not allowed for item {item.Sku.Value} with status {item.Status}.");
    }

    private static void ValidateLocationStatus(Location location)
    {
        if (location.Status is not LocationStatus.Active)
            throw new BusinessRuleException($"Location {location.Code} with status {location.Status} does not allow stock movements.");
    }

    private static Quantity CreatePositiveQuantity(decimal quantity, UnitOfMeasure unitOfMeasure)
    {
        Quantity quantityValue = Quantity.Create(quantity, unitOfMeasure);

        if (quantityValue.Value <= 0)
            throw new BusinessRuleException("Quantity must be greater than zero.");

        return quantityValue;
    }

    private static void ValidateAdjustmentReason(AdjustmentReason adjustmentReason, MovementType movementType)
    {
        if (!adjustmentReason.IsAllowedForAdjustment(movementType))
            throw new BusinessRuleException($"Adjustment reason {adjustmentReason} is not allowed for movement type {movementType}.");
    }

    private static void ValidateDocumentDate(DateOnly documentDate, DateTimeOffset recordedAt)
    {
        DateOnly recordedDate = DateOnly.FromDateTime(recordedAt.DateTime);

        if (documentDate > recordedDate)
            throw new BusinessRuleException(
                $"Document date {documentDate:yyyy-MM-dd} cannot be after recording date {recordedDate:yyyy-MM-dd}.");
    }
}
