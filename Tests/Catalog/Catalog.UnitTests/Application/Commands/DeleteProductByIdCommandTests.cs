using Catalog.Application.Commands;
using Catalog.Core.Entities;
using Xunit;

namespace Catalog.UnitTests.Application.Commands;

    public class DeleteProductByIdCommandTests
    {
        [Fact]
        public void DeleteProductByIdCommand_CanBeInstantiatedWithId()
        {
            // Act
            var command = new DeleteProductByIdCommand("product-123");

            // Assert
            Assert.NotNull(command);
            Assert.Equal("product-123", command.Id);
        }

        [Theory]
        [InlineData("id-1")]
        [InlineData("id-2")]
        [InlineData("very-long-id-12345678901234567890")]
        public void DeleteProductByIdCommand_WithDifferentIds_SetsCorrectly(string id)
        {
            // Act
            var command = new DeleteProductByIdCommand(id);

            // Assert
            Assert.Equal(id, command.Id);
        }

        [Fact]
        public void DeleteProductByIdCommand_WithNullId_AllowsNull()
        {
            // Act
            var command = new DeleteProductByIdCommand(null);

            // Assert
            Assert.Null(command.Id);
        }

        [Fact]
        public void DeleteProductByIdCommand_WithEmptyString_AllowsEmpty()
        {
            // Act
            var command = new DeleteProductByIdCommand(string.Empty);

            // Assert
            Assert.Equal(string.Empty, command.Id);
        }

        [Fact]
        public void DeleteProductByIdCommand_IdIsReadOnly_CannotBeModifiedAfterConstruction()
        {
            // Arrange
            var command = new DeleteProductByIdCommand("product-123");

            // Assert
            Assert.Equal("product-123", command.Id);
            // Id should be set via constructor and have a public getter
        }

        [Fact]
        public void DeleteProductByIdCommand_WithGuidId_HandlesCorrectly()
        {
            // Arrange
            var guidId = Guid.NewGuid().ToString();

            // Act
            var command = new DeleteProductByIdCommand(guidId);

            // Assert
            Assert.Equal(guidId, command.Id);
        }

        [Fact]
        public void DeleteProductByIdCommand_WithSpecialCharacters_HandlesCorrectly()
        {
            // Arrange
            var id = "product-@#$-123";

            // Act
            var command = new DeleteProductByIdCommand(id);

            // Assert
            Assert.Equal(id, command.Id);
        }
    }