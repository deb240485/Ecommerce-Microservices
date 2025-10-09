using Catalog.Application.Queries;
using Catalog.Application.Responses;
using Catalog.Core.Specs;
using MediatR;
using Xunit;

namespace Catalog.UnitTests.Application.Queries;

public class GetProductByIdQueryTests
{
    [Fact]
    public void GetProductByIdQuery_CanBeInstantiatedWithId()
    {
        // Act
        var query = new GetProductByIdQuery("product-123");

        // Assert
        Assert.NotNull(query);
        Assert.Equal("product-123", query.Id);
    }

    [Fact]
    public void GetProductByIdQuery_ImplementsIRequest()
    {
        // Act
        var query = new GetProductByIdQuery("product-123");

        // Assert
        Assert.IsAssignableFrom<IRequest<ProductResponse>>(query);
    }

    [Theory]
    [InlineData("id-1")]
    [InlineData("id-2")]
    [InlineData("product-abc-123")]
    public void GetProductByIdQuery_WithDifferentIds_SetsCorrectly(string id)
    {
        // Act
        var query = new GetProductByIdQuery(id);

        // Assert
        Assert.Equal(id, query.Id);
    }

    [Fact]
    public void GetProductByIdQuery_WithNullId_AllowsNull()
    {
        // Act
        var query = new GetProductByIdQuery(null);

        // Assert
        Assert.Null(query.Id);
    }

    [Fact]
    public void GetProductByIdQuery_WithEmptyString_AllowsEmpty()
    {
        // Act
        var query = new GetProductByIdQuery(string.Empty);

        // Assert
        Assert.Equal(string.Empty, query.Id);
    }

    [Fact]
    public void GetProductByIdQuery_WithGuidId_HandlesCorrectly()
    {
        // Arrange
        var guidId = Guid.NewGuid().ToString();

        // Act
        var query = new GetProductByIdQuery(guidId);

        // Assert
        Assert.Equal(guidId, query.Id);
    }
}