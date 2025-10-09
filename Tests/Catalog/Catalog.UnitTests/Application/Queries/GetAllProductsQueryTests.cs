using Catalog.Application.Queries;
using Catalog.Application.Responses;
using Catalog.Core.Specs;
using MediatR;
using Xunit;

namespace Catalog.UnitTests.Application.Queries;

public class GetAllProductsQueryTests
{
    [Fact]
    public void GetAllProductsQuery_CanBeInstantiated()
    {
        // Arrange
        var catalogParams = new CatalogSpecParams();

        // Act
        var query = new GetAllProductsQuery(catalogParams);

        // Assert
        Assert.NotNull(query);
    }

    [Fact]
    public void GetAllProductsQuery_ImplementsIRequest()
    {
        // Arrange
        var catalogParams = new CatalogSpecParams();

        // Act
        var query = new GetAllProductsQuery(catalogParams);

        // Assert
        Assert.IsAssignableFrom<IRequest<Pagination<ProductResponse>>>(query);
    }

    [Fact]
    public void GetAllProductsQuery_WithCatalogParams_SetsCatalogSpecParams()
    {
        // Arrange
        var catalogParams = new CatalogSpecParams
        {
            PageIndex = 2,
            PageSize = 20,
            BrandId = "brand-1",
            TypeId = "type-1",
            Sort = "priceAsc",
            Search = "laptop"
        };

        // Act
        var query = new GetAllProductsQuery(catalogParams);

        // Assert
        Assert.Equal(catalogParams, query.CatalogSpecParams);
        Assert.Equal(2, query.CatalogSpecParams.PageIndex);
        Assert.Equal(20, query.CatalogSpecParams.PageSize);
        Assert.Equal("brand-1", query.CatalogSpecParams.BrandId);
        Assert.Equal("type-1", query.CatalogSpecParams.TypeId);
        Assert.Equal("priceAsc", query.CatalogSpecParams.Sort);
        Assert.Equal("laptop", query.CatalogSpecParams.Search);
    }

    [Fact]
    public void GetAllProductsQuery_WithNullCatalogParams_AllowsNull()
    {
        // Act
        var query = new GetAllProductsQuery(null);

        // Assert
        Assert.Null(query.CatalogSpecParams);
    }

    [Theory]
    [InlineData(1, 10)]
    [InlineData(2, 20)]
    [InlineData(5, 50)]
    public void GetAllProductsQuery_WithDifferentPageSizes_SetsCorrectly(int pageIndex, int pageSize)
    {
        // Arrange
        var catalogParams = new CatalogSpecParams
        {
            PageIndex = pageIndex,
            PageSize = pageSize
        };

        // Act
        var query = new GetAllProductsQuery(catalogParams);

        // Assert
        Assert.Equal(pageIndex, query.CatalogSpecParams.PageIndex);
        Assert.Equal(pageSize, query.CatalogSpecParams.PageSize);
    }

    [Fact]
    public void GetAllProductsQuery_WithSearchFilter_PreservesSearchTerm()
    {
        // Arrange
        var catalogParams = new CatalogSpecParams { Search = "gaming" };

        // Act
        var query = new GetAllProductsQuery(catalogParams);

        // Assert
        Assert.Equal("gaming", query.CatalogSpecParams.Search);
    }

    [Fact]
    public void GetAllProductsQuery_WithBrandFilter_PreservesBrandId()
    {
        // Arrange
        var catalogParams = new CatalogSpecParams { BrandId = "brand-123" };

        // Act
        var query = new GetAllProductsQuery(catalogParams);

        // Assert
        Assert.Equal("brand-123", query.CatalogSpecParams.BrandId);
    }

    [Fact]
    public void GetAllProductsQuery_WithTypeFilter_PreservesTypeId()
    {
        // Arrange
        var catalogParams = new CatalogSpecParams { TypeId = "type-456" };

        // Act
        var query = new GetAllProductsQuery(catalogParams);

        // Assert
        Assert.Equal("type-456", query.CatalogSpecParams.TypeId);
    }

    [Fact]
    public void GetAllProductsQuery_WithSortParameter_PreservesSort()
    {
        // Arrange
        var catalogParams = new CatalogSpecParams { Sort = "priceDesc" };

        // Act
        var query = new GetAllProductsQuery(catalogParams);

        // Assert
        Assert.Equal("priceDesc", query.CatalogSpecParams.Sort);
    }
}