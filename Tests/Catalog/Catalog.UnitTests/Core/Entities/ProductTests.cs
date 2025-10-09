using Catalog.Core.Entities;
using Xunit;

namespace Catalog.UnitTests.Core.Entities;

public class ProductTests
{
    [Fact]
    public void Product_CanBeInstantiated()
    {
        // Act
        var product = new Product();

        // Assert
        Assert.NotNull(product);
    }

    [Fact]
    public void Product_AllPropertiesCanBeSet()
    {
        // Arrange
        var brand = new ProductBrand { Id = "brand1", Name = "TestBrand" };
        var type = new ProductType { Id = "type1", Name = "TestType" };

        // Act
        var product = new Product
        {
            Id = "product-123",
            Name = "Test Product",
            Summary = "Test Summary",
            Description = "Test Description",
            ImageFile = "test.jpg",
            Price = 99.99m,
            Brands = brand,
            Types = type
        };

        // Assert
        Assert.Equal("product-123", product.Id);
        Assert.Equal("Test Product", product.Name);
        Assert.Equal("Test Summary", product.Summary);
        Assert.Equal("Test Description", product.Description);
        Assert.Equal("test.jpg", product.ImageFile);
        Assert.Equal(99.99m, product.Price);
        Assert.Equal(brand, product.Brands);
        Assert.Equal(type, product.Types);
    }

    [Theory]
    [InlineData("Product 1", 10.50)]
    [InlineData("Product 2", 99.99)]
    [InlineData("Product 3", 1500.00)]
    public void Product_WithDifferentPrices_SetsCorrectly(string name, decimal price)
    {
        // Act
        var product = new Product
        {
            Name = name,
            Price = price
        };

        // Assert
        Assert.Equal(name, product.Name);
        Assert.Equal(price, product.Price);
    }

    [Fact]
    public void Product_WithZeroPrice_AllowsZero()
    {
        // Act
        var product = new Product
        {
            Price = 0m
        };

        // Assert
        Assert.Equal(0m, product.Price);
    }

    [Fact]
    public void Product_WithNegativePrice_AllowsNegative()
    {
        // Act
        var product = new Product
        {
            Price = -10.50m
        };

        // Assert
        Assert.Equal(-10.50m, product.Price);
    }

    [Fact]
    public void Product_WithVeryLargePrice_HandlesLargeValues()
    {
        // Act
        var product = new Product
        {
            Price = 999999999.99m
        };

        // Assert
        Assert.Equal(999999999.99m, product.Price);
    }

    [Fact]
    public void Product_IdProperty_CanBeModified()
    {
        // Arrange
        var product = new Product { Id = "old-id" };

        // Act
        product.Id = "new-id";

        // Assert
        Assert.Equal("new-id", product.Id);
    }

    [Fact]
    public void Product_NameProperty_CanBeModified()
    {
        // Arrange
        var product = new Product { Name = "Old Name" };

        // Act
        product.Name = "New Name";

        // Assert
        Assert.Equal("New Name", product.Name);
    }

    [Fact]
    public void Product_WithSpecialCharactersInName_HandlesCorrectly()
    {
        // Act
        var product = new Product
        {
            Name = "Product @#$% 123"
        };

        // Assert
        Assert.Contains("@#$%", product.Name);
    }

    [Fact]
    public void Product_WithLongDescription_HandlesLargeText()
    {
        // Arrange
        var longText = new string('A', 5000);

        // Act
        var product = new Product
        {
            Description = longText
        };

        // Assert
        Assert.Equal(5000, product.Description.Length);
    }

    [Fact]
    public void Product_BrandCanBeSet()
    {
        // Arrange
        var brand = new ProductBrand { Id = "brand1", Name = "Apple" };
        var product = new Product();

        // Act
        product.Brands = brand;

        // Assert
        Assert.Equal(brand, product.Brands);
        Assert.Equal("Apple", product.Brands.Name);
    }

    [Fact]
    public void Product_TypeCanBeSet()
    {
        // Arrange
        var type = new ProductType { Id = "type1", Name = "Electronics" };
        var product = new Product();

        // Act
        product.Types = type;

        // Assert
        Assert.Equal(type, product.Types);
        Assert.Equal("Electronics", product.Types.Name);
    }

    [Fact]
    public void Product_WithCompleteData_AllPropertiesAreSet()
    {
        // Act
        var product = new Product
        {
            Id = "prod-001",
            Name = "iPhone 15",
            Summary = "Latest iPhone",
            Description = "Apple's newest smartphone with advanced features",
            ImageFile = "iphone15.jpg",
            Price = 999.99m,
            Brands = new ProductBrand { Id = "b1", Name = "Apple" },
            Types = new ProductType { Id = "t1", Name = "Smartphone" }
        };

        // Assert
        Assert.NotNull(product.Id);
        Assert.NotNull(product.Name);
        Assert.NotNull(product.Summary);
        Assert.NotNull(product.Description);
        Assert.NotNull(product.ImageFile);
        Assert.True(product.Price > 0);
        Assert.NotNull(product.Brands);
        Assert.NotNull(product.Types);
    }
}