using Catalog.Core.Entities;
using Catalog.Core.Specs;
using Catalog.Infrastructure.Data;
using MongoDB.Driver;
using Moq;
using Xunit;

namespace Catalog.UnitTests.Fixtures;

/// <summary>
/// Fixture for creating mocked ICatalogContext
/// </summary>
public class CatalogContextFixture : IDisposable
{
    public Mock<ICatalogContext> MockContext { get; private set; }
    public Mock<IMongoCollection<Product>> MockProductCollection { get; private set; }
    public Mock<IMongoCollection<ProductBrand>> MockBrandCollection { get; private set; }
    public Mock<IMongoCollection<ProductType>> MockTypeCollection { get; private set; }
    public ProductFixture ProductFixture { get; private set; }

    public CatalogContextFixture()
    {
        ProductFixture = new ProductFixture();
        InitializeMocks();
    }

    private void InitializeMocks()
    {
        MockContext = new Mock<ICatalogContext>();
        MockProductCollection = new Mock<IMongoCollection<Product>>();
        MockBrandCollection = new Mock<IMongoCollection<ProductBrand>>();
        MockTypeCollection = new Mock<IMongoCollection<ProductType>>();

        MockContext.Setup(c => c.Products).Returns(MockProductCollection.Object);
        MockContext.Setup(c => c.Brands).Returns(MockBrandCollection.Object);
        MockContext.Setup(c => c.Types).Returns(MockTypeCollection.Object);
    }

    public void SetupProductFindAsync(Product product)
    {
        var mockCursor = new Mock<IAsyncCursor<Product>>();
        mockCursor.Setup(c => c.Current).Returns(new List<Product> { product });
        mockCursor.SetupSequence(c => c.MoveNext(It.IsAny<CancellationToken>()))
            .Returns(true)
            .Returns(false);
        mockCursor.SetupSequence(c => c.MoveNextAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(true)
            .ReturnsAsync(false);

        MockProductCollection
            .Setup(c => c.FindAsync(
                It.IsAny<FilterDefinition<Product>>(),
                It.IsAny<FindOptions<Product, Product>>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(mockCursor.Object);
    }

    public void SetupProductListFindAsync(List<Product> products)
    {
        var mockCursor = new Mock<IAsyncCursor<Product>>();
        mockCursor.Setup(c => c.Current).Returns(products);
        mockCursor.SetupSequence(c => c.MoveNext(It.IsAny<CancellationToken>()))
            .Returns(true)
            .Returns(false);
        mockCursor.SetupSequence(c => c.MoveNextAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(true)
            .ReturnsAsync(false);

        MockProductCollection
            .Setup(c => c.FindAsync(
                It.IsAny<FilterDefinition<Product>>(),
                It.IsAny<FindOptions<Product, Product>>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(mockCursor.Object);
    }

    public void SetupBrandListFindAsync(List<ProductBrand> brands)
    {
        var mockCursor = new Mock<IAsyncCursor<ProductBrand>>();
        mockCursor.Setup(c => c.Current).Returns(brands);
        mockCursor.SetupSequence(c => c.MoveNext(It.IsAny<CancellationToken>()))
            .Returns(true)
            .Returns(false);
        mockCursor.SetupSequence(c => c.MoveNextAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(true)
            .ReturnsAsync(false);

        MockBrandCollection
            .Setup(c => c.FindAsync(
                It.IsAny<FilterDefinition<ProductBrand>>(),
                It.IsAny<FindOptions<ProductBrand, ProductBrand>>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(mockCursor.Object);
    }

    public void SetupTypeListFindAsync(List<ProductType> types)
    {
        var mockCursor = new Mock<IAsyncCursor<ProductType>>();
        mockCursor.Setup(c => c.Current).Returns(types);
        mockCursor.SetupSequence(c => c.MoveNext(It.IsAny<CancellationToken>()))
            .Returns(true)
            .Returns(false);
        mockCursor.SetupSequence(c => c.MoveNextAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(true)
            .ReturnsAsync(false);

        MockTypeCollection
            .Setup(c => c.FindAsync(
                It.IsAny<FilterDefinition<ProductType>>(),
                It.IsAny<FindOptions<ProductType, ProductType>>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(mockCursor.Object);
    }

    public void SetupInsertOneAsync()
    {
        MockProductCollection
            .Setup(c => c.InsertOneAsync(
                It.IsAny<Product>(),
                It.IsAny<InsertOneOptions>(),
                It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);
    }

    public void SetupDeleteOneAsync(long deletedCount)
    {
        var deleteResult = new DeleteResult.Acknowledged(deletedCount);
        MockProductCollection
            .Setup(c => c.DeleteOneAsync(
                It.IsAny<FilterDefinition<Product>>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(deleteResult);
    }

    public void SetupReplaceOneAsync(long modifiedCount)
    {
        var replaceResult = new ReplaceOneResult.Acknowledged(modifiedCount, modifiedCount, null);
        MockProductCollection
            .Setup(c => c.ReplaceOneAsync(
                It.IsAny<FilterDefinition<Product>>(),
                It.IsAny<Product>(),
                It.IsAny<ReplaceOptions>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(replaceResult);
    }

    public void SetupCountDocumentsAsync(long count)
    {
        MockProductCollection
            .Setup(c => c.CountDocumentsAsync(
                It.IsAny<FilterDefinition<Product>>(),
                It.IsAny<CountOptions>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(count);
    }

    public void Dispose()
    {
        ProductFixture?.Dispose();
        MockContext = null;
        MockProductCollection = null;
        MockBrandCollection = null;
        MockTypeCollection = null;
    }
}

/// <summary>
/// Example usage of fixtures with xUnit's IClassFixture
/// </summary>
public class ExampleFixtureUsageTests : IClassFixture<ProductFixture>
{
    private readonly ProductFixture _fixture;

    public ExampleFixtureUsageTests(ProductFixture fixture)
    {
        _fixture = fixture;
    }

    [Fact]
    public void UsingFixture_GetProductById_ReturnsProduct()
    {
        // Arrange
        var productId = "product-1";

        // Act
        var product = _fixture.GetProductById(productId);

        // Assert
        Assert.NotNull(product);
        Assert.Equal(productId, product.Id);
    }

    [Fact]
    public void UsingFixture_GetProductsByBrand_ReturnsFilteredProducts()
    {
        // Act
        var appleProducts = _fixture.GetProductsByBrand("Apple");

        // Assert
        Assert.NotEmpty(appleProducts);
        Assert.All(appleProducts, p => Assert.Equal("Apple", p.Brands.Name));
    }

    [Fact]
    public void UsingFixture_CreateSampleProduct_ReturnsNewProduct()
    {
        // Act
        var product = _fixture.CreateSampleProduct();

        // Assert
        Assert.NotNull(product);
        Assert.NotNull(product.Id);
        Assert.Equal("Sample Product", product.Name);
    }
}