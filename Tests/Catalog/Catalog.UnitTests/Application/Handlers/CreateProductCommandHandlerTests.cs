using Catalog.Application.Commands;
using Catalog.Application.Handlers;
using Catalog.Application.Responses;
using Catalog.Core.Entities;
using Catalog.Core.Repositories;
using Moq;
using Xunit;

namespace Catalog.UnitTests.Application.Handlers
{
    public class CreateProductCommandHandlerTests
    {
        private readonly Mock<IProductRepository> _mockRepository;
        private readonly CreateProductCommandHandler _handler;

        public CreateProductCommandHandlerTests()
        {
            _mockRepository = new Mock<IProductRepository>();
            _handler = new CreateProductCommandHandler(_mockRepository.Object);
        }

        [Fact]
        public async Task Handle_WithValidCommand_ReturnsProductResponse()
        {
            // Arrange
            var command = new CreateProductCommand
            {
                Name = "Test Product",
                Summary = "Test Summary",
                Description = "Test Description",
                ImageFile = "test.jpg",
                Price = 99.99m,
                Brands = new ProductBrand { Id = "brand1", Name = "Brand1" },
                Types = new ProductType { Id = "type1", Name = "Type1" }
            };

            var createdProduct = new Product
            {
                Id = "new-id-123",
                Name = command.Name,
                Summary = command.Summary,
                Description = command.Description,
                ImageFile = command.ImageFile,
                Price = command.Price,
                Brands = command.Brands,
                Types = command.Types
            };

            _mockRepository
                .Setup(x => x.CreateProduct(It.IsAny<Product>()))
                .ReturnsAsync(createdProduct);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<ProductResponse>(result);
            Assert.Equal(command.Name, result.Name);
            Assert.Equal(command.Price, result.Price);
            _mockRepository.Verify(x => x.CreateProduct(It.IsAny<Product>()), Times.Once);
        }

        [Fact]
        public async Task Handle_WithNullCommand_ThrowsArgumentNullException()
        {
            // Act & Assert
            await Assert.ThrowsAsync<ArgumentNullException>(
                async () => await _handler.Handle(null, CancellationToken.None)
            );
        }

        [Fact]
        public async Task Handle_RepositoryThrowsException_PropagatesException()
        {
            // Arrange
            var command = new CreateProductCommand
            {
                Name = "Test Product",
                Price = 99.99m
            };

            _mockRepository
                .Setup(x => x.CreateProduct(It.IsAny<Product>()))
                .ThrowsAsync(new Exception("Database error"));

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(
                async () => await _handler.Handle(command, CancellationToken.None)
            );
        }

        [Fact]
        public async Task Handle_WithCompleteProductData_MapsAllProperties()
        {
            // Arrange
            var command = new CreateProductCommand
            {
                Name = "Complete Product",
                Summary = "Complete Summary",
                Description = "Complete Description",
                ImageFile = "complete.jpg",
                Price = 199.99m,
                Brands = new ProductBrand { Id = "b1", Name = "CompleteBrand" },
                Types = new ProductType { Id = "t1", Name = "CompleteType" }
            };

            var createdProduct = new Product
            {
                Id = "complete-id",
                Name = command.Name,
                Summary = command.Summary,
                Description = command.Description,
                ImageFile = command.ImageFile,
                Price = command.Price,
                Brands = command.Brands,
                Types = command.Types
            };

            _mockRepository
                .Setup(x => x.CreateProduct(It.IsAny<Product>()))
                .ReturnsAsync(createdProduct);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.Equal(command.Name, result.Name);
            Assert.Equal(command.Summary, result.Summary);
            Assert.Equal(command.Description, result.Description);
            Assert.Equal(command.ImageFile, result.ImageFile);
            Assert.Equal(command.Price, result.Price);
        }

        [Theory]
        [InlineData("Product 1", 10.00)]
        [InlineData("Product 2", 20.50)]
        [InlineData("Product 3", 999.99)]
        public async Task Handle_WithDifferentPrices_CreatesProductSuccessfully(
            string name, 
            decimal price)
        {
            // Arrange
            var command = new CreateProductCommand
            {
                Name = name,
                Price = price
            };

            var createdProduct = new Product
            {
                Id = "test-id",
                Name = name,
                Price = price
            };

            _mockRepository
                .Setup(x => x.CreateProduct(It.IsAny<Product>()))
                .ReturnsAsync(createdProduct);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.Equal(name, result.Name);
            Assert.Equal(price, result.Price);
        }

        [Fact]
        public async Task Handle_WithCancellationToken_PassesTokenCorrectly()
        {
            // Arrange
            var command = new CreateProductCommand
            {
                Name = "Test Product",
                Price = 99.99m
            };

            var cancellationTokenSource = new CancellationTokenSource();
            var cancellationToken = cancellationTokenSource.Token;

            var createdProduct = new Product
            {
                Id = "test-id",
                Name = command.Name,
                Price = command.Price
            };

            _mockRepository
                .Setup(x => x.CreateProduct(It.IsAny<Product>()))
                .ReturnsAsync(createdProduct);

            // Act
            var result = await _handler.Handle(command, cancellationToken);

            // Assert
            Assert.NotNull(result);
        }
    }
}