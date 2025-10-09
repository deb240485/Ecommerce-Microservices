using Catalog.Application.Commands;
using Catalog.Core.Entities;
using Xunit;

namespace Catalog.UnitTests.Application.Commands;

public class CreateProductCommandTests
{
    [Fact]
    public void CreateProductCommand_CanBeInstantiated()
    {
        // Act
        var command = new CreateProductCommand();

        // Assert
        Assert.NotNull(command);
    }

    [Fact]
    public void CreateProductCommand_PropertiesCanBeSet()
    {
        // Arrange
        var brand = new ProductBrand { Id = "brand1", Name = "TestBrand" };
        var type = new ProductType { Id = "type1", Name = "TestType" };

        // Act
        var command = new CreateProductCommand
        {
            Name = "Test Product",
            Summary = "Test Summary",
            Description = "Test Description",
            ImageFile = "test.jpg",
            Price = 99.99m,
            Brands = brand,
            Types = type
        };

        // Assert
        Assert.Equal("Test Product", command.Name);
        Assert.Equal("Test Summary", command.Summary);
        Assert.Equal("Test Description", command.Description);
        Assert.Equal("test.jpg", command.ImageFile);
        Assert.Equal(99.99m, command.Price);
        Assert.Equal(brand, command.Brands);
        Assert.Equal(type, command.Types);
    }

    [Theory]
    [InlineData("Product 1", 10.50)]
    [InlineData("Product 2", 99.99)]
    [InlineData("Product 3", 1500.00)]
    public void CreateProductCommand_WithDifferentPrices_SetsCorrectly(string name, decimal price)
    {
        // Act
        var command = new CreateProductCommand
        {
            Name = name,
            Price = price
        };

        // Assert
        Assert.Equal(name, command.Name);
        Assert.Equal(price, command.Price);
    }

    [Fact]
    public void CreateProductCommand_WithDefaultBrand_HasNonNullBrand()
    {
        // Act
        var command = new CreateProductCommand
        {
            Name = "Product",
            Brands = new ProductBrand { Id = "default", Name = "Default" }
        };

        // Assert
        Assert.NotNull(command.Brands);
    }

    [Fact]
    public void CreateProductCommand_WithDefaultType_HasNonNullType()
    {
        // Act
        var command = new CreateProductCommand
        {
            Name = "Product",
            Types = new ProductType { Id = "default", Name = "Default" }
        };

        // Assert
        Assert.NotNull(command.Types);
    }

    [Fact]
    public void CreateProductCommand_WithEmptyStrings_AllowsEmpty()
    {
        // Act
        var command = new CreateProductCommand
        {
            Name = string.Empty,
            Summary = string.Empty,
            Description = string.Empty,
            ImageFile = string.Empty
        };

        // Assert
        Assert.Equal(string.Empty, command.Name);
        Assert.Equal(string.Empty, command.Summary);
        Assert.Equal(string.Empty, command.Description);
        Assert.Equal(string.Empty, command.ImageFile);
    }

    [Fact]
    public void CreateProductCommand_WithZeroPrice_AllowsZero()
    {
        // Act
        var command = new CreateProductCommand
        {
            Price = 0m
        };

        // Assert
        Assert.Equal(0m, command.Price);
    }

    [Fact]
    public void CreateProductCommand_WithNegativePrice_AllowsNegative()
    {
        // Act
        var command = new CreateProductCommand
        {
            Price = -10.50m
        };

        // Assert
        Assert.Equal(-10.50m, command.Price);
    }

    [Fact]
    public void CreateProductCommand_WithLargePrice_HandlesLargeValues()
    {
        // Act
        var command = new CreateProductCommand
        {
            Price = 999999.99m
        };

        // Assert
        Assert.Equal(999999.99m, command.Price);
    }

    [Fact]
    public void CreateProductCommand_WithSpecialCharacters_HandlesCorrectly()
    {
        // Act
        var command = new CreateProductCommand
        {
            Name = "Product @#$%",
            Summary = "Summary with special chars: <>",
            Description = "Description with émojis 😊"
        };

        // Assert
        Assert.Contains("@#$%", command.Name);
        Assert.Contains("<>", command.Summary);
        Assert.Contains("😊", command.Description);
    }

    [Fact]
    public void CreateProductCommand_WithLongStrings_HandlesLargeText()
    {
        // Arrange
        var longText = new string('A', 5000);

        // Act
        var command = new CreateProductCommand
        {
            Description = longText
        };

        // Assert
        Assert.Equal(5000, command.Description.Length);
    }
}
