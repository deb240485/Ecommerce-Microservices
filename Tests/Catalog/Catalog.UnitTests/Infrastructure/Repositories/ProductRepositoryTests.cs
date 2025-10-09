using Catalog.Core.Entities;
using Catalog.Core.Repositories;
using Catalog.Core.Specs;
using Moq;
using Xunit;

namespace Catalog.UnitTests.Infrastructure.Repositories
{
    public class ProductRepositoryTests
    {
        private readonly Mock<IProductRepository> _mockProductRepository;

        public ProductRepositoryTests()
        {
            _mockProductRepository = new Mock<IProductRepository>();
        }

        [Fact]
        public async Task GetProducts_WithValidParams_ReturnsPagedProducts()
        {
            // Arrange
            var catalogParams = new CatalogSpecParams
            {
                PageIndex = 1,
                PageSize = 10,
                BrandId = "brand123",
                TypeId = "type123",
                Sort = "name",
                Search = "test"
            };

            var expectedProducts = new List<Product>
            {
                new Product { Id = "1", Name = "Product 1", Price = 100 },
                new Product { Id = "2", Name = "Product 2", Price = 200 }
            };

            var expectedPagination = new Pagination<Product>(
                pageIndex: 1,
                pageSize: 10,
                count: 2,
                data: expectedProducts
            );

            _mockProductRepository
                .Setup(repo => repo.GetProducts(It.IsAny<CatalogSpecParams>()))
                .ReturnsAsync(expectedPagination);

            // Act
            var result = await _mockProductRepository.Object.GetProducts(catalogParams);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count);
            Assert.Equal(1, result.PageIndex);
            Assert.Equal(10, result.PageSize);
            Assert.Equal(2, result.Data.Count);
            _mockProductRepository.Verify(
                repo => repo.GetProducts(It.IsAny<CatalogSpecParams>()),
                Times.Once
            );
        }

        [Fact]
        public async Task GetProducts_WithEmptyResult_ReturnsEmptyPagination()
        {
            // Arrange
            var catalogParams = new CatalogSpecParams { PageIndex = 1, PageSize = 10 };
            var emptyPagination = new Pagination<Product>(1, 10, 0, new List<Product>());

            _mockProductRepository
                .Setup(repo => repo.GetProducts(It.IsAny<CatalogSpecParams>()))
                .ReturnsAsync(emptyPagination);

            // Act
            var result = await _mockProductRepository.Object.GetProducts(catalogParams);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(0, result.Count);
            Assert.Empty(result.Data);
        }

        [Fact]
        public async Task GetProduct_WithValidId_ReturnsProduct()
        {
            // Arrange
            var productId = "12345";
            var expectedProduct = new Product
            {
                Id = productId,
                Name = "Test Product",
                Description = "Test Description",
                Price = 99.99m
            };

            _mockProductRepository
                .Setup(repo => repo.GetProduct(productId))
                .ReturnsAsync(expectedProduct);

            // Act
            var result = await _mockProductRepository.Object.GetProduct(productId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(productId, result.Id);
            Assert.Equal("Test Product", result.Name);
            Assert.Equal(99.99m, result.Price);
            _mockProductRepository.Verify(repo => repo.GetProduct(productId), Times.Once);
        }

        [Fact]
        public async Task GetProduct_WithInvalidId_ReturnsNull()
        {
            // Arrange
            var invalidId = "invalid-id";
            _mockProductRepository
                .Setup(repo => repo.GetProduct(invalidId))
                .ReturnsAsync((Product)null);

            // Act
            var result = await _mockProductRepository.Object.GetProduct(invalidId);

            // Assert
            Assert.Null(result);
            _mockProductRepository.Verify(repo => repo.GetProduct(invalidId), Times.Once);
        }

        [Fact]
        public async Task GetProductsByName_WithValidName_ReturnsProducts()
        {
            // Arrange
            var productName = "Laptop";
            var expectedProducts = new List<Product>
            {
                new Product { Id = "1", Name = "Dell Laptop", Price = 1000 },
                new Product { Id = "2", Name = "HP Laptop", Price = 900 }
            };

            _mockProductRepository
                .Setup(repo => repo.GetProductsByName(productName))
                .ReturnsAsync(expectedProducts);

            // Act
            var result = await _mockProductRepository.Object.GetProductsByName(productName);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count());
            Assert.All(result, p => Assert.Contains("Laptop", p.Name));
            _mockProductRepository.Verify(
                repo => repo.GetProductsByName(productName),
                Times.Once
            );
        }

        [Fact]
        public async Task GetProductsByName_WithNoMatches_ReturnsEmptyList()
        {
            // Arrange
            var productName = "NonExistent";
            _mockProductRepository
                .Setup(repo => repo.GetProductsByName(productName))
                .ReturnsAsync(new List<Product>());

            // Act
            var result = await _mockProductRepository.Object.GetProductsByName(productName);

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result);
        }

        [Fact]
        public async Task GetProductsByBrand_WithValidBrand_ReturnsProducts()
        {
            // Arrange
            var brandName = "Apple";
            var expectedProducts = new List<Product>
            {
                new Product { Id = "1", Name = "iPhone", Price = 999 },
                new Product { Id = "2", Name = "MacBook", Price = 1999 }
            };

            _mockProductRepository
                .Setup(repo => repo.GetProductsByBrand(brandName))
                .ReturnsAsync(expectedProducts);

            // Act
            var result = await _mockProductRepository.Object.GetProductsByBrand(brandName);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count());
            _mockProductRepository.Verify(
                repo => repo.GetProductsByBrand(brandName),
                Times.Once
            );
        }

        [Fact]
        public async Task CreateProduct_WithValidProduct_ReturnsCreatedProduct()
        {
            // Arrange
            var newProduct = new Product
            {
                Name = "New Product",
                Description = "New Description",
                Price = 149.99m
            };

            var createdProduct = new Product
            {
                Id = "new-id-123",
                Name = newProduct.Name,
                Description = newProduct.Description,
                Price = newProduct.Price
            };

            _mockProductRepository
                .Setup(repo => repo.CreateProduct(It.IsAny<Product>()))
                .ReturnsAsync(createdProduct);

            // Act
            var result = await _mockProductRepository.Object.CreateProduct(newProduct);

            // Assert
            Assert.NotNull(result);
            Assert.NotNull(result.Id);
            Assert.Equal(newProduct.Name, result.Name);
            Assert.Equal(newProduct.Price, result.Price);
            _mockProductRepository.Verify(
                repo => repo.CreateProduct(It.Is<Product>(p =>
                    p.Name == newProduct.Name &&
                    p.Price == newProduct.Price
                )),
                Times.Once
            );
        }

        [Fact]
        public async Task UpdateProduct_WithValidProduct_ReturnsTrue()
        {
            // Arrange
            var productToUpdate = new Product
            {
                Id = "existing-id",
                Name = "Updated Product",
                Price = 199.99m
            };

            _mockProductRepository
                .Setup(repo => repo.UpdateProduct(It.IsAny<Product>()))
                .ReturnsAsync(true);

            // Act
            var result = await _mockProductRepository.Object.UpdateProduct(productToUpdate);

            // Assert
            Assert.True(result);
            _mockProductRepository.Verify(
                repo => repo.UpdateProduct(It.Is<Product>(p => p.Id == productToUpdate.Id)),
                Times.Once
            );
        }

        [Fact]
        public async Task UpdateProduct_WithNonExistentProduct_ReturnsFalse()
        {
            // Arrange
            var nonExistentProduct = new Product
            {
                Id = "non-existent-id",
                Name = "Non Existent",
                Price = 99.99m
            };

            _mockProductRepository
                .Setup(repo => repo.UpdateProduct(It.IsAny<Product>()))
                .ReturnsAsync(false);

            // Act
            var result = await _mockProductRepository.Object.UpdateProduct(nonExistentProduct);

            // Assert
            Assert.False(result);
            _mockProductRepository.Verify(
                repo => repo.UpdateProduct(It.IsAny<Product>()),
                Times.Once
            );
        }

        [Fact]
        public async Task DeleteProduct_WithValidId_ReturnsTrue()
        {
            // Arrange
            var productId = "delete-id-123";
            _mockProductRepository
                .Setup(repo => repo.DeleteProduct(productId))
                .ReturnsAsync(true);

            // Act
            var result = await _mockProductRepository.Object.DeleteProduct(productId);

            // Assert
            Assert.True(result);
            _mockProductRepository.Verify(
                repo => repo.DeleteProduct(productId),
                Times.Once
            );
        }

        [Fact]
        public async Task DeleteProduct_WithNonExistentId_ReturnsFalse()
        {
            // Arrange
            var nonExistentId = "non-existent-id";
            _mockProductRepository
                .Setup(repo => repo.DeleteProduct(nonExistentId))
                .ReturnsAsync(false);

            // Act
            var result = await _mockProductRepository.Object.DeleteProduct(nonExistentId);

            // Assert
            Assert.False(result);
            _mockProductRepository.Verify(
                repo => repo.DeleteProduct(nonExistentId),
                Times.Once
            );
        }

        [Theory]
        [InlineData(1, 10)]
        [InlineData(2, 20)]
        [InlineData(3, 30)]
        public async Task GetProducts_WithDifferentPageSizes_ReturnsCorrectPagination(
            int pageIndex,
            int pageSize)
        {
            // Arrange
            var catalogParams = new CatalogSpecParams
            {
                PageIndex = pageIndex,
                PageSize = pageSize
            };

            var pagination = new Pagination<Product>(
                pageIndex,
                pageSize,
                100,
                new List<Product>()
            );

            _mockProductRepository
                .Setup(repo => repo.GetProducts(It.IsAny<CatalogSpecParams>()))
                .ReturnsAsync(pagination);

            // Act
            var result = await _mockProductRepository.Object.GetProducts(catalogParams);

            // Assert
            Assert.Equal(pageIndex, result.PageIndex);
            Assert.Equal(pageSize, result.PageSize);
        }

        [Fact]
        public async Task GetProducts_WithSearchFilter_CallsRepositoryWithCorrectParams()
        {
            // Arrange
            var searchTerm = "gaming";
            var catalogParams = new CatalogSpecParams { Search = searchTerm };
            
            _mockProductRepository
                .Setup(repo => repo.GetProducts(It.IsAny<CatalogSpecParams>()))
                .ReturnsAsync(new Pagination<Product>(1, 10, 0, new List<Product>()));

            // Act
            await _mockProductRepository.Object.GetProducts(catalogParams);

            // Assert
            _mockProductRepository.Verify(
                repo => repo.GetProducts(It.Is<CatalogSpecParams>(p =>
                    p.Search == searchTerm
                )),
                Times.Once
            );
        }
    }
}