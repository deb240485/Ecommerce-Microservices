using Catalog.Application.Commands;
using Catalog.Application.Handlers;
using Catalog.Core.Repositories;
using Moq;
using Xunit;

namespace Catalog.UnitTests.Application.Handlers
{
    public class DeleteProductByIdCommandHandlerTests
    {
        private readonly Mock<IProductRepository> _mockProductRepository;
        private readonly DeleteProductByIdCommandHandler _handler;

        public DeleteProductByIdCommandHandlerTests()
        {
            _mockProductRepository = new Mock<IProductRepository>();
            _handler = new DeleteProductByIdCommandHandler(_mockProductRepository.Object);
        }

        [Fact]
        public async Task Handle_WithValidId_ReturnsTrue()
        {
            // Arrange
            var productId = "product-123";
            var command = new DeleteProductByIdCommand(productId);

            _mockProductRepository
                .Setup(repo => repo.DeleteProduct(productId))
                .ReturnsAsync(true);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.True(result);
            _mockProductRepository.Verify(
                repo => repo.DeleteProduct(productId), 
                Times.Once
            );
        }

        [Fact]
        public async Task Handle_WithNonExistentId_ReturnsFalse()
        {
            // Arrange
            var productId = "non-existent-id";
            var command = new DeleteProductByIdCommand(productId);

            _mockProductRepository
                .Setup(repo => repo.DeleteProduct(productId))
                .ReturnsAsync(false);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.False(result);
            _mockProductRepository.Verify(
                repo => repo.DeleteProduct(productId), 
                Times.Once
            );
        }

        [Fact]
        public async Task Handle_WithNullId_CallsRepositoryWithNull()
        {
            // Arrange
            var command = new DeleteProductByIdCommand(null);

            _mockProductRepository
                .Setup(repo => repo.DeleteProduct(null))
                .ReturnsAsync(false);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.False(result);
            _mockProductRepository.Verify(
                repo => repo.DeleteProduct(null), 
                Times.Once
            );
        }

        [Fact]
        public async Task Handle_WithEmptyStringId_CallsRepositoryWithEmptyString()
        {
            // Arrange
            var command = new DeleteProductByIdCommand(string.Empty);

            _mockProductRepository
                .Setup(repo => repo.DeleteProduct(string.Empty))
                .ReturnsAsync(false);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.False(result);
            _mockProductRepository.Verify(
                repo => repo.DeleteProduct(string.Empty), 
                Times.Once
            );
        }

        [Theory]
        [InlineData("product-1")]
        [InlineData("product-2")]
        [InlineData("product-abc-123")]
        [InlineData("very-long-product-id-12345678901234567890")]
        public async Task Handle_WithDifferentIds_DeletesCorrectly(string productId)
        {
            // Arrange
            var command = new DeleteProductByIdCommand(productId);

            _mockProductRepository
                .Setup(repo => repo.DeleteProduct(productId))
                .ReturnsAsync(true);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.True(result);
            _mockProductRepository.Verify(
                repo => repo.DeleteProduct(productId), 
                Times.Once
            );
        }

        [Fact]
        public async Task Handle_WhenRepositoryThrowsException_PropagatesException()
        {
            // Arrange
            var productId = "product-123";
            var command = new DeleteProductByIdCommand(productId);

            _mockProductRepository
                .Setup(repo => repo.DeleteProduct(productId))
                .ThrowsAsync(new Exception("Database connection error"));

            // Act & Assert
            var exception = await Assert.ThrowsAsync<Exception>(
                async () => await _handler.Handle(command, CancellationToken.None)
            );

            Assert.Equal("Database connection error", exception.Message);
            _mockProductRepository.Verify(
                repo => repo.DeleteProduct(productId), 
                Times.Once
            );
        }

        [Fact]
        public async Task Handle_WhenRepositoryThrowsArgumentException_PropagatesException()
        {
            // Arrange
            var productId = "invalid-id";
            var command = new DeleteProductByIdCommand(productId);

            _mockProductRepository
                .Setup(repo => repo.DeleteProduct(productId))
                .ThrowsAsync(new ArgumentException("Invalid product ID format"));

            // Act & Assert
            var exception = await Assert.ThrowsAsync<ArgumentException>(
                async () => await _handler.Handle(command, CancellationToken.None)
            );

            Assert.Equal("Invalid product ID format", exception.Message);
        }

        [Fact]
        public async Task Handle_WithCancellationToken_PassesTokenCorrectly()
        {
            // Arrange
            var productId = "product-123";
            var command = new DeleteProductByIdCommand(productId);
            var cancellationTokenSource = new CancellationTokenSource();
            var cancellationToken = cancellationTokenSource.Token;

            _mockProductRepository
                .Setup(repo => repo.DeleteProduct(productId))
                .ReturnsAsync(true);

            // Act
            var result = await _handler.Handle(command, cancellationToken);

            // Assert
            Assert.True(result);
            _mockProductRepository.Verify(
                repo => repo.DeleteProduct(productId), 
                Times.Once
            );
        }

        [Fact]
        public async Task Handle_CalledMultipleTimes_EachCallInvokesRepository()
        {
            // Arrange
            var productId1 = "product-1";
            var productId2 = "product-2";
            var command1 = new DeleteProductByIdCommand(productId1);
            var command2 = new DeleteProductByIdCommand(productId2);

            _mockProductRepository
                .Setup(repo => repo.DeleteProduct(It.IsAny<string>()))
                .ReturnsAsync(true);

            // Act
            await _handler.Handle(command1, CancellationToken.None);
            await _handler.Handle(command2, CancellationToken.None);

            // Assert
            _mockProductRepository.Verify(
                repo => repo.DeleteProduct(productId1), 
                Times.Once
            );
            _mockProductRepository.Verify(
                repo => repo.DeleteProduct(productId2), 
                Times.Once
            );
            _mockProductRepository.Verify(
                repo => repo.DeleteProduct(It.IsAny<string>()), 
                Times.Exactly(2)
            );
        }

        [Fact]
        public async Task Handle_WithGuidFormattedId_HandlesCorrectly()
        {
            // Arrange
            var productId = Guid.NewGuid().ToString();
            var command = new DeleteProductByIdCommand(productId);

            _mockProductRepository
                .Setup(repo => repo.DeleteProduct(productId))
                .ReturnsAsync(true);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.True(result);
            _mockProductRepository.Verify(
                repo => repo.DeleteProduct(productId), 
                Times.Once
            );
        }

        [Fact]
        public async Task Handle_WithSpecialCharactersInId_HandlesCorrectly()
        {
            // Arrange
            var productId = "product-@#$-123";
            var command = new DeleteProductByIdCommand(productId);

            _mockProductRepository
                .Setup(repo => repo.DeleteProduct(productId))
                .ReturnsAsync(true);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.True(result);
            _mockProductRepository.Verify(
                repo => repo.DeleteProduct(productId), 
                Times.Once
            );
        }

        [Fact]
        public async Task Handle_WhenProductDoesNotExist_ReturnsAppropriateResult()
        {
            // Arrange
            var productId = "deleted-already-product-123";
            var command = new DeleteProductByIdCommand(productId);

            _mockProductRepository
                .Setup(repo => repo.DeleteProduct(productId))
                .ReturnsAsync(false);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.False(result);
            _mockProductRepository.Verify(
                repo => repo.DeleteProduct(productId), 
                Times.Once
            );
        }

        [Fact]
        public void Constructor_WithNullRepository_ThrowsArgumentNullException()
        {
            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => 
                new DeleteProductByIdCommandHandler(null)
            );
        }

        [Fact]
        public async Task Handle_WithWhitespaceId_CallsRepositoryWithWhitespace()
        {
            // Arrange
            var productId = "   ";
            var command = new DeleteProductByIdCommand(productId);

            _mockProductRepository
                .Setup(repo => repo.DeleteProduct(productId))
                .ReturnsAsync(false);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.False(result);
            _mockProductRepository.Verify(
                repo => repo.DeleteProduct(productId), 
                Times.Once
            );
        }

        [Fact]
        public async Task Handle_VerifiesNoOtherRepositoryMethodsCalled()
        {
            // Arrange
            var productId = "product-123";
            var command = new DeleteProductByIdCommand(productId);

            _mockProductRepository
                .Setup(repo => repo.DeleteProduct(productId))
                .ReturnsAsync(true);

            // Act
            await _handler.Handle(command, CancellationToken.None);

            // Assert
            _mockProductRepository.Verify(
                repo => repo.DeleteProduct(productId), 
                Times.Once
            );
            _mockProductRepository.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task Handle_WithCancelledToken_ThrowsOperationCanceledException()
        {
            // Arrange
            var productId = "product-123";
            var command = new DeleteProductByIdCommand(productId);
            var cancellationTokenSource = new CancellationTokenSource();
            cancellationTokenSource.Cancel();

            _mockProductRepository
                .Setup(repo => repo.DeleteProduct(productId))
                .ThrowsAsync(new OperationCanceledException());

            // Act & Assert
            await Assert.ThrowsAsync<OperationCanceledException>(
                async () => await _handler.Handle(command, cancellationTokenSource.Token)
            );
        }
    }
}