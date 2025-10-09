using Catalog.Core.Entities;
using Xunit;

namespace Catalog.UnitTests.Core.Entities;

    public class ProductTypeTests
    {
        [Fact]
        public void ProductType_CanBeInstantiated()
        {
            // Act
            var type = new ProductType();

            // Assert
            Assert.NotNull(type);
        }

        [Fact]
        public void ProductType_AllPropertiesCanBeSet()
        {
            // Act
            var type = new ProductType
            {
                Id = "type-123",
                Name = "Electronics"
            };

            // Assert
            Assert.Equal("type-123", type.Id);
            Assert.Equal("Electronics", type.Name);
        }

        [Theory]
        [InlineData("type-1", "Electronics")]
        [InlineData("type-2", "Clothing")]
        [InlineData("type-3", "Books")]
        public void ProductType_WithDifferentValues_SetsCorrectly(string id, string name)
        {
            // Act
            var type = new ProductType
            {
                Id = id,
                Name = name
            };

            // Assert
            Assert.Equal(id, type.Id);
            Assert.Equal(name, type.Name);
        }

        [Fact]
        public void ProductType_IdProperty_CanBeModified()
        {
            // Arrange
            var type = new ProductType { Id = "old-id" };

            // Act
            type.Id = "new-id";

            // Assert
            Assert.Equal("new-id", type.Id);
        }

        [Fact]
        public void ProductType_NameProperty_CanBeModified()
        {
            // Arrange
            var type = new ProductType { Name = "Old Type" };

            // Act
            type.Name = "New Type";

            // Assert
            Assert.Equal("New Type", type.Name);
        }

        [Fact]
        public void ProductType_WithSpecialCharacters_HandlesCorrectly()
        {
            // Act
            var type = new ProductType
            {
                Name = "Home & Garden"
            };

            // Assert
            Assert.Contains("&", type.Name);
        }

        [Fact]
        public void ProductType_WithLongName_HandlesCorrectly()
        {
            // Arrange
            var longName = new string('T', 200);

            // Act
            var type = new ProductType
            {
                Name = longName
            };

            // Assert
            Assert.Equal(200, type.Name.Length);
        }

        [Fact]
        public void ProductType_WithGuidId_HandlesCorrectly()
        {
            // Arrange
            var guidId = Guid.NewGuid().ToString();

            // Act
            var type = new ProductType
            {
                Id = guidId
            };

            // Assert
            Assert.Equal(guidId, type.Id);
        }

        [Fact]
        public void ProductType_TwoInstancesWithSameValues_AreNotSame()
        {
            // Arrange & Act
            var type1 = new ProductType { Id = "1", Name = "Electronics" };
            var type2 = new ProductType { Id = "1", Name = "Electronics" };

            // Assert
            Assert.NotSame(type1, type2);
            Assert.Equal(type1.Id, type2.Id);
            Assert.Equal(type1.Name, type2.Name);
        }

        [Fact]
        public void ProductType_WithEmptyName_AllowsEmpty()
        {
            // Act
            var type = new ProductType
            {
                Name = string.Empty
            };

            // Assert
            Assert.Equal(string.Empty, type.Name);
        }
    }