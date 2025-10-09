using Catalog.Application.Commands;
using Catalog.Core.Entities;
using Xunit;

namespace Catalog.UnitTests.Application.Commands;

public class UpdateProductCommandTests
{
    [Fact]
    public void UpdateProductCommand_CanBeInstantiated()
    {
        // Act
        var command = new UpdateProductCommand();

        // Assert
        Assert.NotNull(command);
    }

    [Fact]
    public void UpdateProductCommand_AllPropertiesCanBeSet()
    {
        // Arrange
        var brand = new ProductBrand { Id = "brand1", Name = "UpdatedBrand" };
        var type = new ProductType { Id = "type1", Name = "UpdatedType" };

        // Act
        var command = new UpdateProductCommand
        {
            Id = "product-123",
            Name = "Updated Product",
            Summary = "Updated Summary",
            Description = "Updated Description",
            ImageFile = "updated.jpg",
            Price = 149.99m,
            Brands = brand,
            Types = type
        };

        // Assert
        Assert.Equal("product-123", command.Id);
        Assert.Equal("Updated Product", command.Name);
        Assert.Equal("Updated Summary", command.Summary);
        Assert.Equal("Updated Description", command.Description);
        Assert.Equal("updated.jpg", command.ImageFile);
        Assert.Equal(149.99m, command.Price);
        Assert.Equal(brand, command.Brands);
        Assert.Equal(type, command.Types);
    }

    [Theory]
    [InlineData("id-1", "Name 1", 10.00)]
    [InlineData("id-2", "Name 2", 50.50)]
    [InlineData("id-3", "Name 3", 999.99)]
    public void UpdateProductCommand_WithDifferentValues_SetsCorrectly(
        string id,
        string name,
        decimal price)
    {
        // Act
        var command = new UpdateProductCommand
        {
            Id = id,
            Name = name,
            Price = price
        };

        // Assert
        Assert.Equal(id, command.Id);
        Assert.Equal(name, command.Name);
        Assert.Equal(price, command.Price);
    }

    [Fact]
    public void UpdateProductCommand_WithNullId_AllowsNull()
    {
        // Act
        var command = new UpdateProductCommand
        {
            Id = null,
            Name = "Product"
        };

        // Assert
        Assert.Null(command.Id);
    }

    [Fact]
    public void UpdateProductCommand_IdProperty_CanBeModified()
    {
        // Arrange
        var command = new UpdateProductCommand { Id = "old-id" };

        // Act
        command.Id = "new-id";

        // Assert
        Assert.Equal("new-id", command.Id);
    }
}