using Catalog.Application.Handlers;
using Catalog.Application.Queries;
using Catalog.Application.Responses;
using Catalog.Core.Entities;
using Catalog.Core.Repositories;
using Moq;
using Xunit;

namespace Catalog.UnitTests.Application.Handlers;

public class GetProductByBrandHandlerTests
    {
        private readonly Mock<IProductRepository> _mockRepository;
        private readonly GetProductByBrandHandler _handler;

        public GetProductByBrandHandlerTests()
        {
            _mockRepository = new Mock<IProductRepository>();
            _handler = new GetProductByBrandHandler(_mockRepository.Object);
        }

        [Fact]
        public async Task Handle_WithValidBrand_ReturnsProductList()
        {
            // Arrange
            var brandName = "Apple";
            var query = new GetProductByBrandQuery(brandName);

            var products = new List<Product>
            {
                new Product { Id = "1", Name = "iPhone", Price = 999 },
                new Product { Id = "2", Name = "MacBook", Price = 1999 }
            };

            _mockRepository
                .Setup(x => x.GetProductsByBrand(brandName))
                .ReturnsAsync(products);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count);
            _mockRepository.Verify(x => x.GetProductsByBrand(brandName), Times.Once);
        }

        [Fact]
        public async Task Handle_WithNoMatches_ReturnsEmptyList()
        {
            // Arrange
            var query = new GetProductByBrandQuery("NonExistentBrand");

            _mockRepository
                .Setup(x => x.GetProductsByBrand(It.IsAny<string>()))
                .ReturnsAsync(new List<Product>());

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result);
        }
    }
