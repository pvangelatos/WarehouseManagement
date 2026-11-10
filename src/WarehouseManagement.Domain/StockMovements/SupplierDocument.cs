using WarehouseManagement.Domain.Exceptions;

namespace WarehouseManagement.Domain.StockMovements;

public sealed record SupplierDocument
{
    public const int MaxNumberLength = 30;

    public SupplierDocumentType Type { get; }

    public string Number { get; }

    private SupplierDocument(SupplierDocumentType type, string number)
    {
        Type = type;

        Number = number;
    }

    public static SupplierDocument Create(SupplierDocumentType type, string? number)
    {
        if (!Enum.IsDefined(type))
            throw new ArgumentOutOfRangeException(nameof(type), type, $"Unknown document type: {type}.");

        string? trimmedNumber = number?.Trim();

        if (string.IsNullOrWhiteSpace(trimmedNumber))
            throw new BusinessRuleException("Supplier document number cannot be null or empty.");

        if (trimmedNumber.Length > MaxNumberLength)
            throw new BusinessRuleException($"Supplier document number cannot have more than {MaxNumberLength} characters.");

        return new SupplierDocument(type, trimmedNumber.ToUpperInvariant());
    }

    public override string ToString()
    {
        return $"{Type}: {Number}";
    }
}
