using WarehouseManagement.Domain.Exceptions;

namespace WarehouseManagement.Domain.Items;

public sealed class Item
{
    public const int MaxNameLength = 50;

    public Guid Id { get; }

    public Sku Sku { get; }

    public string Name { get; }

    public UnitOfMeasure UnitOfMeasure { get; }

    public Quantity ReorderPoint { get; }

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

}
