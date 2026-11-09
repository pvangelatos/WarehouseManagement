using WarehouseManagement.Domain.Exceptions;

namespace WarehouseManagement.Domain.Locations;

public sealed record LocationCode
{
    public const int MinAisle = 1;
    public const int MaxAisle = 999;
    public const int MinBay = 1;
    public const int MaxBay = 99;
    public const int MinLevel = 0;
    public const int MaxLevel = 99;

    public int Aisle { get; }
    public int Bay { get; }
    public int Level { get; }
    public string Value => $"{Aisle:D3}-{Bay:D2}-{Level:D2}";

    private LocationCode(int aisle, int bay, int level)
    {
        Aisle = aisle;
        Bay = bay;
        Level = level;
    }

    public static LocationCode Create(int aisle, int bay, int level)
    {
        if (aisle < MinAisle || aisle > MaxAisle)
            throw new BusinessRuleException($"Aisle must be between {MinAisle} and {MaxAisle}.");

        if (bay < MinBay || bay > MaxBay)
            throw new BusinessRuleException($"Bay must be between {MinBay} and {MaxBay}.");

        if (level < MinLevel || level > MaxLevel)
            throw new BusinessRuleException($"Level must be between {MinLevel} and {MaxLevel}.");

        return new LocationCode(aisle, bay, level);
    }
}
