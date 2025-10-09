using Catalog.Core.Entities;
using Xunit;

namespace Catalog.UnitTests.Core.Entities;

    public class ProductBrandTests
    {
        [Fact]
        public void ProductBrand_CanBeInstantiated()
        {
            // Act
            var brand = new ProductBrand();

            // Assert
            Assert.NotNull(brand);
        }

        [Fact]
        public void ProductBrand_AllPropertiesCanBeSet()
        {
            // Act
            var brand = new ProductBrand
            {
                Id = "brand-123",
                Name = "Apple"
            };

            // Assert
            Assert.Equal("brand-123", brand.Id);
            Assert.Equal("Apple", brand.Name);
        }

        [Theory]
        [InlineData("brand-1", "Apple")]
        [InlineData("brand-2", "Samsung")]
        [InlineData("brand-3", "Dell")]
        public void ProductBrand_WithDifferentValues_SetsCorrectly(string id, string name)
        {
            // Act
            var brand = new ProductBrand
            {
                Id = id,
                Name = name
            };

            // Assert
            Assert.Equal(id, brand.Id);
            Assert.Equal(name, brand.Name);
        }

        [Fact]
        public void ProductBrand_IdProperty_CanBeModified()
        {
            // Arrange
            var brand = new ProductBrand { Id = "old-id" };

            // Act
            brand.Id = "new-id";

            // Assert
            Assert.Equal("new-id", brand.Id);
        }

        [Fact]
        public void ProductBrand_NameProperty_CanBeModified()
        {
            // Arrange
            var brand = new ProductBrand { Name = "Old Brand" };

            // Act
            brand.Name = "New Brand";

            // Assert
            Assert.Equal("New Brand", brand.Name);
        }

        [Fact]
        public void ProductBrand_WithSpecialCharacters_HandlesCorrectly()
        {
            // Act
            var brand = new ProductBrand
            {
                Name = "L'Oréal"
            };

            // Assert
            Assert.Contains("'", brand.Name);
        }

        [Fact]
        public void ProductBrand_WithLongName_HandlesCorrectly()
        {
            // Arrange
            var longName = new string('B', 200);

            // Act
            var brand = new ProductBrand
            {
                Name = longName
            };

            // Assert
            Assert.Equal(200, brand.Name.Length);
        }

        [Fact]
        public void ProductBrand_WithGuidId_HandlesCorrectly()
        {
            // Arrange
            var guidId = Guid.NewGuid().ToString();

            // Act
            var brand = new ProductBrand
            {
                Id = guidId
            };

            // Assert
            Assert.Equal(guidId, brand.Id);
        }

        [Fact]
        public void ProductBrand_TwoInstancesWithSameValues_AreNotSame()
        {
            // Arrange & Act
            var brand1 = new ProductBrand { Id = "1", Name = "Apple" };
            var brand2 = new ProductBrand { Id = "1", Name = "Apple" };

            // Assert
            Assert.NotSame(brand1, brand2);
            Assert.Equal(brand1.Id, brand2.Id);
            Assert.Equal(brand1.Name, brand2.Name);
        }
    }