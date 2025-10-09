using Catalog.Application.Commands;
using Catalog.Application.Handlers;
using Catalog.Core.Entities;
using Catalog.Core.Repositories;
using Moq;
using Xunit;

namespace Catalog.UnitTests.Application.Handlers
{
    public class UpdateProductCommandHandlerTests
    {
        private readonly Mock<IProductRepository> _mockRepository;
        private readonly UpdateProductCommandHandler _handler;

        public UpdateProductCommandHandlerTests()
        {
            _mockRepository = new Mock<IProductRepository>();
            _handler = new UpdateProductCommandHandler(_mockRepository.Object);
        }

        [Fact]
        public async Task Handle_WithValidCommand_ReturnsTrue()
        {
            // Arrange
            var command = new UpdateProductCommand
            {
                Id = "product-123",
                Name = "Updated Product",
                Summary = "Updated Summary",
                Description = "Updated Description",
                ImageFile = "updated.jpg",
                Price = 149.99m,
                Brands = new ProductBrand { Id = "b1", Name = "Brand1" },
                Types = new ProductType { Id = "t1", Name = "Type1" }
            };

            _mockRepository
                .Setup(x => x.UpdateProduct(It.IsAny<Product>()))
                .ReturnsAsync(true);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.True(result);
            _mockRepository.Verify(
                x => x.UpdateProduct(It.Is<Product>(p => 
                    p.Id == command.Id &&
                    p.Name == command.Name &&
                    p.Price == command.Price
                )), 
                Times.Once
            );
        }

        [Fact]
        public async Task Handle_WithNonExistentProduct_ReturnsTrue()
        {
            // Arrange
            var command = new UpdateProductCommand
            {
                Id = "non-existent-id",
                Name = "Product",
                Price = 99.99m
            };

            _mockRepository
                .Setup(x => x.UpdateProduct(It.IsAny<Product>()))
                .ReturnsAsync(false);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.True(result); // Handler always returns true
        }

        [Fact]
        public async Task Handle_RepositoryThrowsException_PropagatesException()
        {
            // Arrange
            var command = new UpdateProductCommand
            {
                Id = "product-123",
                Name = "Product",
                Price = 99.99m
            };

            _mockRepository
                .Setup(x => x.UpdateProduct(It.IsAny<Product>()))
                .ThrowsAsync(new Exception("Database error"));

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(
                async () => await _handler.Handle(command, CancellationToken.None)
            );
        }

        [Theory]
        [InlineData("id-1", "Name 1", 10.00)]
        [InlineData("id-2", "Name 2", 20.50)]
        [InlineData("id-3", "Name 3", 999.99)]
        public async Task Handle_WithDifferentValues_UpdatesCorrectly(
            string id, 
            string name, 
            decimal price)
        {
            // Arrange
            var command = new UpdateProductCommand
            {
                Id = id,
                Name = name,
                Price = price
            };

            _mockRepository
                .Setup(x => x.UpdateProduct(It.IsAny<Product>()))
                .ReturnsAsync(true);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.True(result);
            _mockRepository.Verify(
                x => x.UpdateProduct(It.Is<Product>(p => 
                    p.Id == id && p.Name == name && p.Price == price
                )), 
                Times.Once
            );
        }
    } 
}