using WarehouseManagement.Domain.Exceptions;
using WarehouseManagement.Domain.StockMovements;

namespace WarehouseManagement.Domain.UnitTests.StockMovements;

public class SupplierDocumentTests
{
    [Fact]
    public void Create_ValidParameters_ReturnsDocumentWithTypeAndNumber()
    {
        // Arrange
        var documentType = SupplierDocumentType.Invoice;
        var documentNumber = "INV-001";

        // Act
        var supplierDocument = SupplierDocument.Create(documentType, documentNumber);

        // Assert
        Assert.Equal(documentType, supplierDocument.Type);
        Assert.Equal(documentNumber, supplierDocument.Number);
    }

    [Theory]
    [InlineData(SupplierDocumentType.Invoice, "  AAA-123  ", "AAA-123")]
    [InlineData(SupplierDocumentType.DeliveryNote, "αβ-125", "ΑΒ-125")]
    [InlineData(SupplierDocumentType.Invoice, "  abc-456  ", "ABC-456")]
    [InlineData(SupplierDocumentType.DeliveryNote, "ΧΥΖ-000124", "ΧΥΖ-000124")]
    [InlineData(SupplierDocumentType.Invoice, "0000012345", "0000012345")]
    public void Create_UnnormalizedNumber_ReturnsNormalizedNumber(SupplierDocumentType documentType, string documentNumber, string expectedNumber)
    {
        // Act
        var supplierDocument = SupplierDocument.Create(documentType, documentNumber);

        // Assert
        Assert.Equal(expectedNumber, supplierDocument.Number);
    }

    [Theory]
    [InlineData(SupplierDocumentType.Invoice, null)]
    [InlineData(SupplierDocumentType.DeliveryNote, "")]
    [InlineData(SupplierDocumentType.Invoice, "   ")]
    public void Create_NullOrEmptyNumber_ThrowsBusinessRuleException(SupplierDocumentType documentType, string? documentNumber)
    {
        // Act & Assert
        var exception = Assert.Throws<BusinessRuleException>(() => SupplierDocument.Create(documentType, documentNumber));
        Assert.Equal("Supplier document number cannot be null or empty.", exception.Message);
    }

    [Fact]
    public void Create_NumberExactMaxLength_ReturnsDocument()
    {
        // Arrange
        var documentType = SupplierDocumentType.Invoice;
        var longNumber = new string('A', SupplierDocument.MaxNumberLength);

        // Act
        var supplierDocument = SupplierDocument.Create(documentType, longNumber);

        // Assert
        Assert.Equal(documentType, supplierDocument.Type);
        Assert.Equal(longNumber, supplierDocument.Number);
    }

    [Fact]
    public void Create_NumberExceedsMaxLength_ThrowsBusinessRuleException()
    {
        // Arrange
        var documentType = SupplierDocumentType.DeliveryNote;
        var longNumber = new string('B', SupplierDocument.MaxNumberLength + 1);

        // Act & Assert
        var exception = Assert.Throws<BusinessRuleException>(() => SupplierDocument.Create(documentType, longNumber));
        Assert.Equal($"Supplier document number cannot have more than {SupplierDocument.MaxNumberLength} characters.", exception.Message);
    }

    [Fact]
    public void Create_UnknownDocumentType_ThrowsArgumentOutOfRangeException()
    {
        // Arrange
        var unknownDocumentType = (SupplierDocumentType)99;

        // Act & Assert
        var exception = Assert.Throws<ArgumentOutOfRangeException>(() => SupplierDocument.Create(unknownDocumentType, "DOC-001"));
        Assert.Equal("type", exception.ParamName);
        Assert.Equal(unknownDocumentType, exception.ActualValue);
    }

    [Fact]
    public void ToString_ValidDocument_ReturnsTypeAndNumber()
    {
        // Arrange
        var documentType = SupplierDocumentType.Invoice;
        var documentNumber = "INV-001";
        var supplierDocument = SupplierDocument.Create(documentType, documentNumber);

        // Act
        var documentToString = supplierDocument.ToString();

        // Assert
        Assert.Equal("Invoice: INV-001", documentToString);
    }
}
