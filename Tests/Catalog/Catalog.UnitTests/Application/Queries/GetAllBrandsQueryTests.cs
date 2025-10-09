using Catalog.Application.Queries;
using Catalog.Application.Responses;
using Catalog.Core.Specs;
using MediatR;
using Xunit;

namespace Catalog.UnitTests.Application.Queries;

    public class GetAllBrandsQueryTests
    {
        [Fact]
        public void GetAllBrandsQuery_CanBeInstantiated()
        {
            // Act
            var query = new GetAllBrandsQuery();

            // Assert
            Assert.NotNull(query);
        }

        [Fact]
        public void GetAllBrandsQuery_ImplementsIRequest()
        {
            // Act
            var query = new GetAllBrandsQuery();

            // Assert
            Assert.IsAssignableFrom<IRequest<IList<BrandResponse>>>(query);
        }

        [Fact]
        public void GetAllBrandsQuery_HasNoParameters()
        {
            // Act
            var query = new GetAllBrandsQuery();

            // Assert
            Assert.NotNull(query);
            // This query typically has no parameters
        }

        [Fact]
        public void GetAllBrandsQuery_MultipleInstances_AreIndependent()
        {
            // Act
            var query1 = new GetAllBrandsQuery();
            var query2 = new GetAllBrandsQuery();

            // Assert
            Assert.NotSame(query1, query2);
        }
    }

    public class GetProductByNameQueryTests
    {
        [Fact]
        public void GetProductByNameQuery_CanBeInstantiatedWithName()
        {
            // Act
            var query = new GetProductByNameQuery("Laptop");

            // Assert
            Assert.NotNull(query);
            Assert.Equal("Laptop", query.Name);
        }

        [Fact]
        public void GetProductByNameQuery_ImplementsIRequest()
        {
            // Act
            var query = new GetProductByNameQuery("Product");

            // Assert
            Assert.IsAssignableFrom<IRequest<IList<ProductResponse>>>(query);
        }

        [Theory]
        [InlineData("Laptop")]
        [InlineData("Phone")]
        [InlineData("Gaming Mouse")]
        public void GetProductByNameQuery_WithDifferentNames_SetsCorrectly(string name)
        {
            // Act
            var query = new GetProductByNameQuery(name);

            // Assert
            Assert.Equal(name, query.Name);
        }

        [Fact]
        public void GetProductByNameQuery_WithNullName_AllowsNull()
        {
            // Act
            var query = new GetProductByNameQuery(null);

            // Assert
            Assert.Null(query.Name);
        }

        [Fact]
        public void GetProductByNameQuery_WithEmptyString_AllowsEmpty()
        {
            // Act
            var query = new GetProductByNameQuery(string.Empty);

            // Assert
            Assert.Equal(string.Empty, query.Name);
        }

        [Fact]
        public void GetProductByNameQuery_WithSpecialCharacters_HandlesCorrectly()
        {
            // Arrange
            var name = "Product @#$ 123";

            // Act
            var query = new GetProductByNameQuery(name);

            // Assert
            Assert.Equal(name, query.Name);
        }
    }

    public class GetProductByBrandQueryTests
    {
        [Fact]
        public void GetProductByBrandQuery_CanBeInstantiatedWithBrandName()
        {
            // Act
            var query = new GetProductByBrandQuery("Apple");

            // Assert
            Assert.NotNull(query);
            Assert.Equal("Apple", query.BrandName);
        }

        [Fact]
        public void GetProductByBrandQuery_ImplementsIRequest()
        {
            // Act
            var query = new GetProductByBrandQuery("Samsung");

            // Assert
            Assert.IsAssignableFrom<IRequest<IList<ProductResponse>>>(query);
        }

        [Theory]
        [InlineData("Apple")]
        [InlineData("Samsung")]
        [InlineData("Dell")]
        public void GetProductByBrandQuery_WithDifferentBrands_SetsCorrectly(string brandName)
        {
            // Act
            var query = new GetProductByBrandQuery(brandName);

            // Assert
            Assert.Equal(brandName, query.BrandName);
        }

        [Fact]
        public void GetProductByBrandQuery_WithNullBrandName_AllowsNull()
        {
            // Act
            var query = new GetProductByBrandQuery(null);

            // Assert
            Assert.Null(query.BrandName);
        }

        [Fact]
        public void GetProductByBrandQuery_WithEmptyString_AllowsEmpty()
        {
            // Act
            var query = new GetProductByBrandQuery(string.Empty);

            // Assert
            Assert.Equal(string.Empty, query.BrandName);
        }
    }