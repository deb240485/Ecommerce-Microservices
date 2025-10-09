using Catalog.Core.Specs;
using Xunit;

namespace Catalog.UnitTests.Core.Specifications
{
    public class CatalogSpecParamsTests
    {
        [Fact]
        public void CatalogSpecParams_DefaultValues_AreSetCorrectly()
        {
            // Act
            var specParams = new CatalogSpecParams();

            // Assert
            Assert.Equal(1, specParams.PageIndex);
            Assert.Equal(10, specParams.PageSize);
            Assert.Null(specParams.BrandId);
            Assert.Null(specParams.TypeId);
            Assert.Null(specParams.Sort);
            Assert.Null(specParams.Search);
        }

        [Fact]
        public void PageSize_WhenSetBelowMaxPageSize_ReturnsSetValue()
        {
            // Arrange
            var specParams = new CatalogSpecParams();

            // Act
            specParams.PageSize = 20;

            // Assert
            Assert.Equal(20, specParams.PageSize);
        }

        [Fact]
        public void PageSize_WhenSetAboveMaxPageSize_ReturnsMaxPageSize()
        {
            // Arrange
            var specParams = new CatalogSpecParams();

            // Act
            specParams.PageSize = 100; // Above max of 70

            // Assert
            Assert.Equal(70, specParams.PageSize);
        }

        [Fact]
        public void PageSize_WhenSetToExactlyMaxPageSize_ReturnsMaxPageSize()
        {
            // Arrange
            var specParams = new CatalogSpecParams();

            // Act
            specParams.PageSize = 70;

            // Assert
            Assert.Equal(70, specParams.PageSize);
        }

        [Theory]
        [InlineData(5)]
        [InlineData(10)]
        [InlineData(25)]
        [InlineData(50)]
        [InlineData(70)]
        public void PageSize_WithValidValues_SetsCorrectly(int pageSize)
        {
            // Arrange
            var specParams = new CatalogSpecParams();

            // Act
            specParams.PageSize = pageSize;

            // Assert
            Assert.Equal(pageSize, specParams.PageSize);
        }

        [Theory]
        [InlineData(71, 70)]
        [InlineData(100, 70)]
        [InlineData(1000, 70)]
        public void PageSize_WithValuesAboveMax_ClampsToMaxPageSize(int input, int expected)
        {
            // Arrange
            var specParams = new CatalogSpecParams();

            // Act
            specParams.PageSize = input;

            // Assert
            Assert.Equal(expected, specParams.PageSize);
        }

        [Fact]
        public void PageIndex_CanBeSet()
        {
            // Arrange
            var specParams = new CatalogSpecParams();

            // Act
            specParams.PageIndex = 5;

            // Assert
            Assert.Equal(5, specParams.PageIndex);
        }

        [Fact]
        public void BrandId_CanBeSetAndRetrieved()
        {
            // Arrange
            var specParams = new CatalogSpecParams();

            // Act
            specParams.BrandId = "brand-123";

            // Assert
            Assert.Equal("brand-123", specParams.BrandId);
        }

        [Fact]
        public void TypeId_CanBeSetAndRetrieved()
        {
            // Arrange
            var specParams = new CatalogSpecParams();

            // Act
            specParams.TypeId = "type-456";

            // Assert
            Assert.Equal("type-456", specParams.TypeId);
        }

        [Fact]
        public void Sort_CanBeSetAndRetrieved()
        {
            // Arrange
            var specParams = new CatalogSpecParams();

            // Act
            specParams.Sort = "priceAsc";

            // Assert
            Assert.Equal("priceAsc", specParams.Sort);
        }

        [Fact]
        public void Search_CanBeSetAndRetrieved()
        {
            // Arrange
            var specParams = new CatalogSpecParams();

            // Act
            specParams.Search = "laptop";

            // Assert
            Assert.Equal("laptop", specParams.Search);
        }

        [Fact]
        public void AllProperties_CanBeSetTogether()
        {
            // Arrange & Act
            var specParams = new CatalogSpecParams
            {
                PageIndex = 2,
                PageSize = 25,
                BrandId = "brand-1",
                TypeId = "type-1",
                Sort = "priceDesc",
                Search = "gaming"
            };

            // Assert
            Assert.Equal(2, specParams.PageIndex);
            Assert.Equal(25, specParams.PageSize);
            Assert.Equal("brand-1", specParams.BrandId);
            Assert.Equal("type-1", specParams.TypeId);
            Assert.Equal("priceDesc", specParams.Sort);
            Assert.Equal("gaming", specParams.Search);
        }

        [Fact]
        public void PageSize_WhenSetToZero_SetsToZero()
        {
            // Arrange
            var specParams = new CatalogSpecParams();

            // Act
            specParams.PageSize = 0;

            // Assert
            Assert.Equal(0, specParams.PageSize);
        }

        [Fact]
        public void PageSize_WhenSetToNegative_SetsToNegative()
        {
            // Arrange
            var specParams = new CatalogSpecParams();

            // Act
            specParams.PageSize = -1;

            // Assert
            Assert.Equal(-1, specParams.PageSize);
        }
    }
}