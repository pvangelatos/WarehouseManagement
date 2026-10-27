using WarehouseManagement.Domain.Exceptions;
using WarehouseManagement.Domain.Items;

namespace WarehouseManagement.Domain.UnitTests.Items;



public class ItemTests
{

    [Fact]
    [Trait("BR", "BR-26")]
    public void Create_NewItem_WithValidParameters_ReturnsItem()
    {
        // Arrange
        string sku = "VALID-SKU";
        string name = "Valid Item Name";
        UnitOfMeasure unitOfMeasure = UnitOfMeasure.Box;
        decimal reorderPoint = 10;
        bool tracksExpiry = false;

        // Act
        Item item = Item.Create(sku, name, unitOfMeasure, reorderPoint, tracksExpiry);

        // Assert
        Assert.Equal(sku.ToUpperInvariant(), item.Sku.Value);
        Assert.Equal(name.Trim(), item.Name);
        Assert.Equal(unitOfMeasure, item.UnitOfMeasure);
        Assert.Equal(reorderPoint, item.ReorderPoint.Value);
        Assert.Equal(tracksExpiry, item.TracksExpiry);
    }

    [Fact]
    [Trait("BR", "BR-26")]
    public void Create_NewItem_WithInvalidName_ThrowBusinessRuleException()
    {
        // Arrange
        string sku = "VALID-SKU";
        string name = ""; // Invalid name
        UnitOfMeasure unitOfMeasure = UnitOfMeasure.Piece;
        decimal reorderPoint = 10;
        bool tracksExpiry = false;

        // Act & Assert
        Assert.Throws<BusinessRuleException>(() => Item.Create(sku, name, unitOfMeasure, reorderPoint, tracksExpiry));
    }

    [Fact]
    [Trait("BR", "BR-26")]
    public void Create_NewItem_WithNameExceedingMaxLength_ThrowBusinessRuleException()
    {
        // Arrange
        string sku = "VALID-SKU";
        string name = new string('A', Item.MaxNameLength + 1); // Exceeds max length
        UnitOfMeasure unitOfMeasure = UnitOfMeasure.Piece;
        decimal reorderPoint = 10;
        bool tracksExpiry = false;

        // Act & Assert
        Assert.Throws<BusinessRuleException>(() => Item.Create(sku, name, unitOfMeasure, reorderPoint, tracksExpiry));
    }

    [Fact]
    [Trait("BR", "BR-26")]
    public void Create_NewItem_WithValidNameMaxLength_ReturnsItem()
    {
        // Arrange
        string sku = "VALID-SKU";
        string name = new string('A', Item.MaxNameLength); // Max length
        UnitOfMeasure unitOfMeasure = UnitOfMeasure.Piece;
        decimal reorderPoint = 10;
        bool tracksExpiry = false;

        // Act
        Item item = Item.Create(sku, name, unitOfMeasure, reorderPoint, tracksExpiry);

        // Assert
        Assert.Equal(name, item.Name);
    }

    [Fact]
    [Trait("BR", "BR-27")]
    public void Create_NewItem_WithStatusActive_IsTrue()
    {
        // Arrange
        string sku = "VALID-SKU";
        string name = "Valid Item Name";
        UnitOfMeasure unitOfMeasure = UnitOfMeasure.Box;
        decimal reorderPoint = 10;
        bool tracksExpiry = false;

        // Act
        Item item = Item.Create(sku, name, unitOfMeasure, reorderPoint, tracksExpiry);

        // Assert
        Assert.Equal(ItemStatus.Active, item.Status);
    }

    [Fact]
    [Trait("BR", "BR-26")]
    public void Create_NewItem_WithReorderPoint_Zero_ReturnsItem()
    {
        // Arrange
        string sku = "VALID-SKU";
        string name = "Valid Item Name";
        UnitOfMeasure unitOfMeasure = UnitOfMeasure.Box;
        decimal reorderPoint = 0; // Zero reorder point
        bool tracksExpiry = false;

        // Act
        Item item = Item.Create(sku, name, unitOfMeasure, reorderPoint, tracksExpiry);

        // Assert
        Assert.Equal(reorderPoint, item.ReorderPoint.Value);
    }

    [Fact]
    [Trait("BR", "BR-22")]
    public void Create_NewItem_WithDecimalReorderPointForUnitWithoutDecimals_ThrowsBusinessRuleException()
    {
        // Arrange
        string sku = "VALID-SKU";
        string name = "Valid Item Name";
        UnitOfMeasure unitOfMeasure = UnitOfMeasure.Piece; // Unit without decimals
        decimal reorderPoint = 1.5m; // Decimal value
        bool tracksExpiry = false;

        // Act & Assert
        Assert.Throws<BusinessRuleException>(() => Item.Create(sku, name, unitOfMeasure, reorderPoint, tracksExpiry));
    }

    [Fact]
    public void Create_NewItem_HasNonEmptyId()
    {
        // Arrange
        string sku = "VALID-SKU";
        string name = "Valid Item Name";
        UnitOfMeasure unitOfMeasure = UnitOfMeasure.Box;
        decimal reorderPoint = 10;
        bool tracksExpiry = false;

        // Act
        Item item = Item.Create(sku, name, unitOfMeasure, reorderPoint, tracksExpiry);

        // Assert
        Assert.NotEqual(Guid.Empty, item.Id);
    }

    [Fact]
    public void Create_SameDataTwice_ReturnsDifferentIds()
    {
        // Arrange
        string sku1 = "VALID-SKU-1";
        string name1 = "Valid Item Name 1";
        UnitOfMeasure unitOfMeasure1 = UnitOfMeasure.Box;
        decimal reorderPoint1 = 10;
        bool tracksExpiry1 = false;


        // Act
        Item item1 = Item.Create(sku1, name1, unitOfMeasure1, reorderPoint1, tracksExpiry1);
        Item item2 = Item.Create(sku1, name1, unitOfMeasure1, reorderPoint1, tracksExpiry1);

        // Assert
        Assert.NotEqual(item1.Id, item2.Id);
    }

}
