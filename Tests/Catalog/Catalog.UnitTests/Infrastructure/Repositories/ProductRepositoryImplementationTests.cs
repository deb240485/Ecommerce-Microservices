using Catalog.Core.Entities;
using Catalog.Core.Repositories;
using Catalog.Core.Specs;
using Catalog.Infrastructure.Data;
using Catalog.Infrastructure.Repositories;
using MongoDB.Driver;
using Moq;
using Xunit;

namespace Catalog.UnitTests.Infrastructure.Repositories
{
    public class ProductRepositoryImplementationTests
    {
        private readonly Mock<ICatalogContext> _mockContext;
        private readonly Mock<IMongoCollection<Product>> _mockProductCollection;
        private readonly Mock<IMongoCollection<ProductBrand>> _mockBrandCollection;
        private readonly Mock<IMongoCollection<ProductType>> _mockTypeCollection;
        private readonly ProductRepository _repository;

        public ProductRepositoryImplementationTests()
        {
            _mockContext = new Mock<ICatalogContext>();
            _mockProductCollection = new Mock<IMongoCollection<Product>>();
            _mockBrandCollection = new Mock<IMongoCollection<ProductBrand>>();
            _mockTypeCollection = new Mock<IMongoCollection<ProductType>>();

            _mockContext.Setup(c => c.Products).Returns(_mockProductCollection.Object);
            _mockContext.Setup(c => c.Brands).Returns(_mockBrandCollection.Object);
            _mockContext.Setup(c => c.Types).Returns(_mockTypeCollection.Object);

            _repository = new ProductRepository(_mockContext.Object);
        }

        [Fact]
        public void Constructor_WithNullContext_ThrowsArgumentNullException()
        {
            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => new ProductRepository(null));
        }

        [Fact]
        public async Task GetProduct_WithValidId_CallsMongoDbCorrectly()
        {
            // Arrange
            var productId = "product-123";
            var expectedProduct = new Product { Id = productId, Name = "Test Product" };

            var mockCursor = new Mock<IAsyncCursor<Product>>();
            mockCursor.Setup(c => c.Current).Returns(new List<Product> { expectedProduct });
            mockCursor.SetupSequence(c => c.MoveNext(It.IsAny<CancellationToken>()))
                .Returns(true)
                .Returns(false);
            mockCursor.SetupSequence(c => c.MoveNextAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(true)
                .ReturnsAsync(false);

            _mockProductCollection
                .Setup(c => c.FindAsync(
                    It.IsAny<FilterDefinition<Product>>(),
                    It.IsAny<FindOptions<Product, Product>>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(mockCursor.Object);

            // Act
            var result = await _repository.GetProduct(productId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(productId, result.Id);
            _mockProductCollection.Verify(
                c => c.FindAsync(
                    It.IsAny<FilterDefinition<Product>>(),
                    It.IsAny<FindOptions<Product, Product>>(),
                    It.IsAny<CancellationToken>()),
                Times.Once);
        }

        [Fact]
        public async Task CreateProduct_WithValidProduct_InsertsProductSuccessfully()
        {
            // Arrange
            var newProduct = new Product
            {
                Name = "New Product",
                Price = 99.99m
            };

            _mockProductCollection
                .Setup(c => c.InsertOneAsync(
                    It.IsAny<Product>(),
                    It.IsAny<InsertOneOptions>(),
                    It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            // Act
            var result = await _repository.CreateProduct(newProduct);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(newProduct.Name, result.Name);
            _mockProductCollection.Verify(
                c => c.InsertOneAsync(
                    It.Is<Product>(p => p.Name == newProduct.Name),
                    It.IsAny<InsertOneOptions>(),
                    It.IsAny<CancellationToken>()),
                Times.Once);
        }

        [Fact]
        public async Task DeleteProduct_WithValidId_ReturnsTrue()
        {
            // Arrange
            var productId = "product-to-delete";
            var deleteResult = new DeleteResult.Acknowledged(1);

            _mockProductCollection
                .Setup(c => c.DeleteOneAsync(
                    It.IsAny<FilterDefinition<Product>>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(deleteResult);

            // Act
            var result = await _repository.DeleteProduct(productId);

            // Assert
            Assert.True(result);
            _mockProductCollection.Verify(
                c => c.DeleteOneAsync(
                    It.IsAny<FilterDefinition<Product>>(),
                    It.IsAny<CancellationToken>()),
                Times.Once);
        }

        [Fact]
        public async Task DeleteProduct_WithNonExistentId_ReturnsFalse()
        {
            // Arrange
            var productId = "non-existent";
            var deleteResult = new DeleteResult.Acknowledged(0);

            _mockProductCollection
                .Setup(c => c.DeleteOneAsync(
                    It.IsAny<FilterDefinition<Product>>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(deleteResult);

            // Act
            var result = await _repository.DeleteProduct(productId);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public async Task UpdateProduct_WithValidProduct_ReturnsTrue()
        {
            // Arrange
            var product = new Product
            {
                Id = "product-123",
                Name = "Updated Product",
                Price = 149.99m
            };

            var replaceResult = new ReplaceOneResult.Acknowledged(1, 1, null);

            _mockProductCollection
                .Setup(c => c.ReplaceOneAsync(
                    It.IsAny<FilterDefinition<Product>>(),
                    It.IsAny<Product>(),
                    It.IsAny<ReplaceOptions>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(replaceResult);

            // Act
            var result = await _repository.UpdateProduct(product);

            // Assert
            Assert.True(result);
            _mockProductCollection.Verify(
                c => c.ReplaceOneAsync(
                    It.IsAny<FilterDefinition<Product>>(),
                    It.Is<Product>(p => p.Id == product.Id && p.Name == product.Name),
                    It.IsAny<ReplaceOptions>(),
                    It.IsAny<CancellationToken>()),
                Times.Once);
        }

        [Fact]
        public async Task UpdateProduct_WithNonExistentProduct_ReturnsFalse()
        {
            // Arrange
            var product = new Product { Id = "non-existent", Name = "Product" };
            var replaceResult = new ReplaceOneResult.Acknowledged(0, 0, null);

            _mockProductCollection
                .Setup(c => c.ReplaceOneAsync(
                    It.IsAny<FilterDefinition<Product>>(),
                    It.IsAny<Product>(),
                    It.IsAny<ReplaceOptions>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(replaceResult);

            // Act
            var result = await _repository.UpdateProduct(product);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public async Task GetAllBrands_CallsBrandCollection()
        {
            // Arrange
            var brands = new List<ProductBrand>
            {
                new ProductBrand { Id = "1", Name = "Brand1" },
                new ProductBrand { Id = "2", Name = "Brand2" }
            };

            var mockCursor = new Mock<IAsyncCursor<ProductBrand>>();
            mockCursor.Setup(c => c.Current).Returns(brands);
            mockCursor.SetupSequence(c => c.MoveNext(It.IsAny<CancellationToken>()))
                .Returns(true)
                .Returns(false);
            mockCursor.SetupSequence(c => c.MoveNextAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(true)
                .ReturnsAsync(false);

            _mockBrandCollection
                .Setup(c => c.FindAsync(
                    It.IsAny<FilterDefinition<ProductBrand>>(),
                    It.IsAny<FindOptions<ProductBrand, ProductBrand>>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(mockCursor.Object);

            // Act
            var result = await _repository.GetAllBrands();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count());
            _mockBrandCollection.Verify(
                c => c.FindAsync(
                    It.IsAny<FilterDefinition<ProductBrand>>(),
                    It.IsAny<FindOptions<ProductBrand, ProductBrand>>(),
                    It.IsAny<CancellationToken>()),
                Times.Once);
        }

        [Fact]
        public async Task GetAllTypes_CallsTypeCollection()
        {
            // Arrange
            var types = new List<ProductType>
            {
                new ProductType { Id = "1", Name = "Type1" },
                new ProductType { Id = "2", Name = "Type2" }
            };

            var mockCursor = new Mock<IAsyncCursor<ProductType>>();
            mockCursor.Setup(c => c.Current).Returns(types);
            mockCursor.SetupSequence(c => c.MoveNext(It.IsAny<CancellationToken>()))
                .Returns(true)
                .Returns(false);
            mockCursor.SetupSequence(c => c.MoveNextAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(true)
                .ReturnsAsync(false);

            _mockTypeCollection
                .Setup(c => c.FindAsync(
                    It.IsAny<FilterDefinition<ProductType>>(),
                    It.IsAny<FindOptions<ProductType, ProductType>>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(mockCursor.Object);

            // Act
            var result = await _repository.GetAllTypes();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count());
            _mockTypeCollection.Verify(
                c => c.FindAsync(
                    It.IsAny<FilterDefinition<ProductType>>(),
                    It.IsAny<FindOptions<ProductType, ProductType>>(),
                    It.IsAny<CancellationToken>()),
                Times.Once);
        }

        [Fact]
        public void Repository_ImplementsAllInterfaces()
        {
            // Assert
            Assert.IsAssignableFrom<IProductRepository>(_repository);
            Assert.IsAssignableFrom<IBrandRepository>(_repository);
            Assert.IsAssignableFrom<ITypesRepository>(_repository);
        }
    }
}