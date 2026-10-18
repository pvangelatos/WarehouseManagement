using System.Text.RegularExpressions;

using WarehouseManagement.Domain.Exceptions;

namespace WarehouseManagement.Domain.Items;

public sealed record Sku
{
    public string Value { get; }

    private Sku(string value) => Value = value;

    public static Sku Create(string? input)
    {
        string? trimmedInput = input?.Trim();
        if (string.IsNullOrWhiteSpace(trimmedInput))
            throw new BusinessRuleException("SKU cannot be null or empty.");

        if (trimmedInput.Length < 3 || trimmedInput.Length > 20)
            throw new BusinessRuleException("SKU must be between 3 and 20 characters.");

        if (!Regex.IsMatch(trimmedInput, @"^(?!-)[A-Za-z0-9-]+(?<!-)$"))
            throw new BusinessRuleException("SKU may contain only Latin letters, digits and hyphens and cannot start or end with a hyphen.");

        return new Sku(trimmedInput.ToUpperInvariant());
    }
}
