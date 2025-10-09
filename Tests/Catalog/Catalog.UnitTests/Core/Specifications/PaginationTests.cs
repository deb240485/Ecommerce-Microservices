using Catalog.Core.Entities;
using Catalog.Core.Specs;
using Xunit;

namespace Catalog.UnitTests.Core.Specifications;

public class PaginationTests
{
    [Fact]
    public void Pagination_CanBeInstantiatedWithDefaultConstructor()
    {
        // Act
        var pagination = new Pagination<Product>();

        // Assert
        Assert.NotNull(pagination);
    }

    [Fact]
    public void Pagination_CanBeInstantiatedWithParameterizedConstructor()
    {
        // Arrange
        var products = new List<Product>
            {
                new Product { Id = "1", Name = "Product 1" },
                new Product { Id = "2", Name = "Product 2" }
            };

        // Act
        var pagination = new Pagination<Product>(
            pageIndex: 1,
            pageSize: 10,
            count: 2,
            data: products
        );

        // Assert
        Assert.NotNull(pagination);
        Assert.Equal(1, pagination.PageIndex);
        Assert.Equal(10, pagination.PageSize);
        Assert.Equal(2, pagination.Count);
        Assert.Equal(2, pagination.Data.Count);
    }

    [Fact]
    public void Pagination_AllPropertiesCanBeSet()
    {
        // Arrange
        var products = new List<Product>
            {
                new Product { Id = "1", Name = "Product 1" }
            };

        // Act
        var pagination = new Pagination<Product>
        {
            PageIndex = 2,
            PageSize = 20,
            Count = 100,
            Data = products
        };

        // Assert
        Assert.Equal(2, pagination.PageIndex);
        Assert.Equal(20, pagination.PageSize);
        Assert.Equal(100, pagination.Count);
        Assert.Single(pagination.Data);
    }

    [Theory]
    [InlineData(1, 10, 50)]
    [InlineData(2, 20, 100)]
    [InlineData(5, 50, 250)]
    public void Pagination_WithDifferentPageSettings_SetsCorrectly(
        int pageIndex,
        int pageSize,
        long count)
    {
        // Arrange
        var products = new List<Product>();

        // Act
        var pagination = new Pagination<Product>(pageIndex, pageSize, (int)count, products);

        // Assert
        Assert.Equal(pageIndex, pagination.PageIndex);
        Assert.Equal(pageSize, pagination.PageSize);
        Assert.Equal(count, pagination.Count);
    }

    [Fact]
    public void Pagination_WithEmptyData_HandlesEmptyList()
    {
        // Arrange
        var emptyProducts = new List<Product>();

        // Act
        var pagination = new Pagination<Product>(1, 10, 0, emptyProducts);

        // Assert
        Assert.Empty(pagination.Data);
        Assert.Equal(0, pagination.Count);
    }

    [Fact]
    public void Pagination_WithNullData_AllowsNull()
    {
        // Act
        var pagination = new Pagination<Product>
        {
            Data = null
        };

        // Assert
        Assert.Null(pagination.Data);
    }

    [Fact]
    public void Pagination_CountProperty_CanHoldLargeValues()
    {
        // Act
        var pagination = new Pagination<Product>
        {
            Count = 1000000
        };

        // Assert
        Assert.Equal(1000000, pagination.Count);
    }

    [Fact]
    public void Pagination_WithPageIndexZero_AllowsZero()
    {
        // Act
        var pagination = new Pagination<Product>
        {
            PageIndex = 0
        };

        // Assert
        Assert.Equal(0, pagination.PageIndex);
    }

    [Fact]
    public void Pagination_WithNegativePageIndex_AllowsNegative()
    {
        // Act
        var pagination = new Pagination<Product>
        {
            PageIndex = -1
        };

        // Assert
        Assert.Equal(-1, pagination.PageIndex);
    }

    [Fact]
    public void Pagination_WithLargeDataSet_HandlesCorrectly()
    {
        // Arrange
        var largeProductList = Enumerable.Range(1, 100)
            .Select(i => new Product { Id = $"product-{i}", Name = $"Product {i}" })
            .ToList();

        // Act
        var pagination = new Pagination<Product>(1, 100, 1000, largeProductList);

        // Assert
        Assert.Equal(100, pagination.Data.Count);
        Assert.Equal(1000, pagination.Count);
    }

    [Fact]
    public void Pagination_DataProperty_IsReadOnlyList()
    {
        // Arrange
        var products = new List<Product>
            {
                new Product { Id = "1", Name = "Product 1" }
            };

        // Act
        var pagination = new Pagination<Product>(1, 10, 1, products);

        // Assert
        Assert.IsAssignableFrom<IReadOnlyList<Product>>(pagination.Data);
    }

    [Fact]
    public void Pagination_WithDifferentGenericType_WorksCorrectly()
    {
        // Arrange
        var brands = new List<ProductBrand>
            {
                new ProductBrand { Id = "1", Name = "Apple" },
                new ProductBrand { Id = "2", Name = "Samsung" }
            };

        // Act
        var pagination = new Pagination<ProductBrand>(1, 10, 2, brands);

        // Assert
        Assert.Equal(2, pagination.Data.Count);
        Assert.IsType<Pagination<ProductBrand>>(pagination);
    }

    [Fact]
    public void Pagination_WithStringGenericType_WorksCorrectly()
    {
        // Arrange
        var strings = new List<string> { "one", "two", "three" };

        // Act
        var pagination = new Pagination<string>(1, 10, 3, strings);

        // Assert
        Assert.Equal(3, pagination.Data.Count);
        Assert.Equal("one", pagination.Data[0]);
    }
}