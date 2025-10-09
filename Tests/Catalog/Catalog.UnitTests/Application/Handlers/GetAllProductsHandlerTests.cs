using Catalog.Application.Handlers;
using Catalog.Application.Queries;
using Catalog.Application.Responses;
using Catalog.Core.Entities;
using Catalog.Core.Repositories;
using Catalog.Core.Specs;
using Moq;
using Xunit;

namespace Catalog.UnitTests.Application.Handlers
{
    public class GetAllProductsHandlerTests
    {
        private readonly Mock<IProductRepository> _mockRepository;
        private readonly GetAllProductsHandler _handler;

        public GetAllProductsHandlerTests()
        {
            _mockRepository = new Mock<IProductRepository>();
            _handler = new GetAllProductsHandler(_mockRepository.Object);
        }

        [Fact]
        public void Constructor_WithNullRepository_ThrowsArgumentNullException()
        {
            // Act & Assert
            Assert.Throws<ArgumentNullException>(
                () => new GetAllProductsHandler(null)
            );
        }

        [Fact]
        public async Task Handle_WithValidQuery_ReturnsPaginatedProducts()
        {
            // Arrange
            var catalogParams = new CatalogSpecParams
            {
                PageIndex = 1,
                PageSize = 10
            };

            var query = new GetAllProductsQuery(catalogParams);

            var products = new List<Product>
            {
                new Product { Id = "1", Name = "Product 1", Price = 100 },
                new Product { Id = "2", Name = "Product 2", Price = 200 }
            };

            var pagination = new Pagination<Product>(1, 10, 2, products);

            _mockRepository
                .Setup(x => x.GetProducts(It.IsAny<CatalogSpecParams>()))
                .ReturnsAsync(pagination);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<Pagination<ProductResponse>>(result);
            Assert.Equal(2, result.Count);
            Assert.Equal(1, result.PageIndex);
            Assert.Equal(10, result.PageSize);
            _mockRepository.Verify(
                x => x.GetProducts(It.IsAny<CatalogSpecParams>()), 
                Times.Once
            );
        }

        [Fact]
        public async Task Handle_WithEmptyResult_ReturnsEmptyPagination()
        {
            // Arrange
            var catalogParams = new CatalogSpecParams();
            var query = new GetAllProductsQuery(catalogParams);
            var emptyPagination = new Pagination<Product>(1, 10, 0, new List<Product>());

            _mockRepository
                .Setup(x => x.GetProducts(It.IsAny<CatalogSpecParams>()))
                .ReturnsAsync(emptyPagination);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(0, result.Count);
            Assert.Empty(result.Data);
        }

        [Theory]
        [InlineData(1, 10)]
        [InlineData(2, 20)]
        [InlineData(5, 50)]
        public async Task Handle_WithDifferentPageSizes_ReturnsCorrectPagination(
            int pageIndex, 
            int pageSize)
        {
            // Arrange
            var catalogParams = new CatalogSpecParams
            {
                PageIndex = pageIndex,
                PageSize = pageSize
            };

            var query = new GetAllProductsQuery(catalogParams);
            var pagination = new Pagination<Product>(pageIndex, pageSize, 100, new List<Product>());

            _mockRepository
                .Setup(x => x.GetProducts(It.IsAny<CatalogSpecParams>()))
                .ReturnsAsync(pagination);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.Equal(pageIndex, result.PageIndex);
            Assert.Equal(pageSize, result.PageSize);
        }

        [Fact]
        public async Task Handle_WithBrandFilter_PassesCorrectParams()
        {
            // Arrange
            var catalogParams = new CatalogSpecParams
            {
                BrandId = "brand-123"
            };

            var query = new GetAllProductsQuery(catalogParams);
            var pagination = new Pagination<Product>(1, 10, 0, new List<Product>());

            _mockRepository
                .Setup(x => x.GetProducts(It.IsAny<CatalogSpecParams>()))
                .ReturnsAsync(pagination);

            // Act
            await _handler.Handle(query, CancellationToken.None);

            // Assert
            _mockRepository.Verify(
                x => x.GetProducts(It.Is<CatalogSpecParams>(p => 
                    p.BrandId == "brand-123"
                )), 
                Times.Once
            );
        }

        [Fact]
        public async Task Handle_WithTypeFilter_PassesCorrectParams()
        {
            // Arrange
            var catalogParams = new CatalogSpecParams
            {
                TypeId = "type-456"
            };

            var query = new GetAllProductsQuery(catalogParams);
            var pagination = new Pagination<Product>(1, 10, 0, new List<Product>());

            _mockRepository
                .Setup(x => x.GetProducts(It.IsAny<CatalogSpecParams>()))
                .ReturnsAsync(pagination);

            // Act
            await _handler.Handle(query, CancellationToken.None);

            // Assert
            _mockRepository.Verify(
                x => x.GetProducts(It.Is<CatalogSpecParams>(p => 
                    p.TypeId == "type-456"
                )), 
                Times.Once
            );
        }

        [Fact]
        public async Task Handle_WithSearchTerm_PassesCorrectParams()
        {
            // Arrange
            var catalogParams = new CatalogSpecParams
            {
                Search = "laptop"
            };

            var query = new GetAllProductsQuery(catalogParams);
            var pagination = new Pagination<Product>(1, 10, 0, new List<Product>());

            _mockRepository
                .Setup(x => x.GetProducts(It.IsAny<CatalogSpecParams>()))
                .ReturnsAsync(pagination);

            // Act
            await _handler.Handle(query, CancellationToken.None);

            // Assert
            _mockRepository.Verify(
                x => x.GetProducts(It.Is<CatalogSpecParams>(p => 
                    p.Search == "laptop"
                )), 
                Times.Once
            );
        }

        [Fact]
        public async Task Handle_WithSortParameter_PassesCorrectParams()
        {
            // Arrange
            var catalogParams = new CatalogSpecParams
            {
                Sort = "priceAsc"
            };

            var query = new GetAllProductsQuery(catalogParams);
            var pagination = new Pagination<Product>(1, 10, 0, new List<Product>());

            _mockRepository
                .Setup(x => x.GetProducts(It.IsAny<CatalogSpecParams>()))
                .ReturnsAsync(pagination);

            // Act
            await _handler.Handle(query, CancellationToken.None);

            // Assert
            _mockRepository.Verify(
                x => x.GetProducts(It.Is<CatalogSpecParams>(p => 
                    p.Sort == "priceAsc"
                )), 
                Times.Once
            );
        }

        [Fact]
        public async Task Handle_RepositoryThrowsException_PropagatesException()
        {
            // Arrange
            var catalogParams = new CatalogSpecParams();
            var query = new GetAllProductsQuery(catalogParams);

            _mockRepository
                .Setup(x => x.GetProducts(It.IsAny<CatalogSpecParams>()))
                .ThrowsAsync(new Exception("Database error"));

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(
                async () => await _handler.Handle(query, CancellationToken.None)
            );
        }

        [Fact]
        public async Task Handle_WithMultipleFilters_PassesAllFilters()
        {
            // Arrange
            var catalogParams = new CatalogSpecParams
            {
                BrandId = "brand-1",
                TypeId = "type-1",
                Search = "gaming",
                Sort = "priceDesc",
                PageIndex = 2,
                PageSize = 20
            };

            var query = new GetAllProductsQuery(catalogParams);
            var pagination = new Pagination<Product>(2, 20, 50, new List<Product>());

            _mockRepository
                .Setup(x => x.GetProducts(It.IsAny<CatalogSpecParams>()))
                .ReturnsAsync(pagination);

            // Act
            await _handler.Handle(query, CancellationToken.None);

            // Assert
            _mockRepository.Verify(
                x => x.GetProducts(It.Is<CatalogSpecParams>(p => 
                    p.BrandId == "brand-1" &&
                    p.TypeId == "type-1" &&
                    p.Search == "gaming" &&
                    p.Sort == "priceDesc" &&
                    p.PageIndex == 2 &&
                    p.PageSize == 20
                )), 
                Times.Once
            );
        }
    }
}