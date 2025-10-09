using Catalog.Application.Commands;
using Catalog.Application.Handlers;
using Catalog.Application.Queries;
using Catalog.Application.Responses;
using Catalog.Core.Entities;
using Catalog.Core.Repositories;
using Moq;
using Xunit;

namespace Catalog.UnitTests.Application.Handlers;

public class GetProductByIdQueryHandlerTests
    {
        private readonly Mock<IProductRepository> _mockRepository;
        private readonly GetProductByIdQueryHandler _handler;

        public GetProductByIdQueryHandlerTests()
        {
            _mockRepository = new Mock<IProductRepository>();
            _handler = new GetProductByIdQueryHandler(_mockRepository.Object);
        }

        [Fact]
        public async Task Handle_WithValidId_ReturnsProductResponse()
        {
            // Arrange
            var productId = "product-123";
            var query = new GetProductByIdQuery(productId);

            var product = new Product
            {
                Id = productId,
                Name = "Test Product",
                Price = 99.99m
            };

            _mockRepository
                .Setup(x => x.GetProduct(productId))
                .ReturnsAsync(product);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<ProductResponse>(result);
            Assert.Equal(productId, result.Id);
            Assert.Equal("Test Product", result.Name);
            _mockRepository.Verify(x => x.GetProduct(productId), Times.Once);
        }

        [Fact]
        public async Task Handle_WithNonExistentId_ReturnsNull()
        {
            // Arrange
            var query = new GetProductByIdQuery("non-existent-id");

            _mockRepository
                .Setup(x => x.GetProduct(It.IsAny<string>()))
                .ReturnsAsync((Product)null);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.Null(result);
        }
    }