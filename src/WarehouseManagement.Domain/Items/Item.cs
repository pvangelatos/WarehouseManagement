using WarehouseManagement.Domain.Exceptions;

namespace WarehouseManagement.Domain.Items;

public sealed class Item
{
    public const int MaxNameLength = 50;

    public Guid Id { get; }

    public Sku Sku { get; }

    public string Name { get; }

    public UnitOfMeasure UnitOfMeasure { get; }

    public Quantity ReorderPoint { get; private set; }

    public bool TracksExpiry { get; }

    public ItemStatus Status { get; private set; } = ItemStatus.Active;

    private Item(
        Guid id,
        Sku sku,
        string name,
        UnitOfMeasure unitOfMeasure,
        Quantity reorderPoint,
        bool tracksExpiry)
    {
        Id = id;
        Sku = sku;
        Name = name;
        UnitOfMeasure = unitOfMeasure;
        ReorderPoint = reorderPoint;
        TracksExpiry = tracksExpiry;
    }
    public static Item Create(
        string sku,
        string? name,
        UnitOfMeasure unitOfMeasure,
        decimal reorderPoint,
        bool tracksExpiry)
    {
        string? trimmedName = name?.Trim();
        Guid guid = Guid.CreateVersion7();
        Quantity quantity = Quantity.Create(reorderPoint, unitOfMeasure);
        Sku skuValue = Sku.Create(sku);

        if (string.IsNullOrWhiteSpace(trimmedName))
            throw new BusinessRuleException("Name cannot be empty or whitespace.");

        if (trimmedName.Length > MaxNameLength)
            throw new BusinessRuleException($"Name cannot exceed {MaxNameLength} characters.");

        return new Item(
            guid,
            skuValue,
            trimmedName,
            unitOfMeasure,
            quantity,
            tracksExpiry
        );
    }

    public void Discontinue()
    {
        if (Status is not ItemStatus.Active)
            throw new BusinessRuleException($"Item is already {Status}.");

        Status = ItemStatus.Discontinued;
    }

    public void ChangeReorderPoint(decimal newReorderPoint)
    {
        if (Status is not ItemStatus.Active)
            throw new BusinessRuleException($"Cannot change reorder point for item with status {Status}.");

        Quantity newQuantity = Quantity.Create(newReorderPoint, UnitOfMeasure);

        ReorderPoint = newQuantity;
    }

    public void Deactivate(Quantity currentStock)
    {
        ArgumentNullException.ThrowIfNull(currentStock);

        if (Status is not ItemStatus.Discontinued)
            throw new BusinessRuleException($"Cannot deactivate item with status {Status}.");

        if (currentStock != Quantity.Zero)
            throw new BusinessRuleException($"Cannot deactivate item with {currentStock.Value} {UnitOfMeasure.UnEceCode()} stock. You must have zero stock to deactivate an item.");

        Status = ItemStatus.Inactive;
    }
}
