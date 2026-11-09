using System.Text.RegularExpressions;

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

    public static LocationCode Parse(string? input)
    {
        string? trimmedCode = input?.Trim();
        if (string.IsNullOrWhiteSpace(trimmedCode))
            throw new BusinessRuleException("Location code cannot be null or empty.");

        var match = Regex.Match(trimmedCode, @"^(?<aisle>[0-9]{1,3})-(?<bay>[0-9]{1,2})-(?<level>[0-9]{1,2})$");
        if (!match.Success)
            throw new BusinessRuleException("Location code must be in the format AAA-BB-LL (aisle-bay-level), using digits only, e.g. \"001-02-04\".");

        int aisle = int.Parse(match.Groups["aisle"].Value);
        int bay = int.Parse(match.Groups["bay"].Value);
        int level = int.Parse(match.Groups["level"].Value);

        return Create(aisle, bay, level);
    }
}
