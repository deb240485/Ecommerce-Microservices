using Catalog.Application.Handlers;
using Catalog.Application.Queries;
using Catalog.Application.Responses;
using Catalog.Core.Entities;
using Catalog.Core.Repositories;
using Moq;
using Xunit;

namespace Catalog.UnitTests.Application.Handlers;

public class GetProductByNameQueryHandlerTests
    {
        private readonly Mock<IProductRepository> _mockRepository;
        private readonly GetProductByNameQueryHandler _handler;

        public GetProductByNameQueryHandlerTests()
        {
            _mockRepository = new Mock<IProductRepository>();
            _handler = new GetProductByNameQueryHandler(_mockRepository.Object);
        }

        [Fact]
        public async Task Handle_WithValidName_ReturnsProductList()
        {
            // Arrange
            var productName = "Laptop";
            var query = new GetProductByNameQuery(productName);

            var products = new List<Product>
            {
                new Product { Id = "1", Name = "Dell Laptop", Price = 1000 },
                new Product { Id = "2", Name = "HP Laptop", Price = 900 }
            };

            _mockRepository
                .Setup(x => x.GetProductsByName(productName))
                .ReturnsAsync(products);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count);
            _mockRepository.Verify(x => x.GetProductsByName(productName), Times.Once);
        }

        [Fact]
        public async Task Handle_WithNoMatches_ReturnsEmptyList()
        {
            // Arrange
            var query = new GetProductByNameQuery("NonExistent");

            _mockRepository
                .Setup(x => x.GetProductsByName(It.IsAny<string>()))
                .ReturnsAsync(new List<Product>());

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result);
        }
    }