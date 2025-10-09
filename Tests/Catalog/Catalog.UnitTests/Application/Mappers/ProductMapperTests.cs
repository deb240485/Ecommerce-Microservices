using AutoMapper;
using Catalog.Application.Commands;
using Catalog.Application.Mappers;
using Catalog.Application.Responses;
using Catalog.Core.Entities;
using Catalog.Core.Specs;
using Xunit;

namespace Catalog.UnitTests.Application.Mappers
{
    public class ProductMapperTests
    {
        private readonly IMapper _mapper;

        public ProductMapperTests()
        {
            _mapper = ProductMapper.Mapper;
        }

        [Fact]
        public void ProductMapper_IsNotNull()
        {
            // Assert
            Assert.NotNull(_mapper);
        }

        [Fact]
        public void ProductMapper_ConfigurationIsValid()
        {
            // Act & Assert
            _mapper.ConfigurationProvider.AssertConfigurationIsValid();
        }

        [Fact]
        public void Map_ProductToProductResponse_MapsCorrectly()
        {
            // Arrange
            var product = new Product
            {
                Id = "product-123",
                Name = "Test Product",
                Summary = "Test Summary",
                Description = "Test Description",
                ImageFile = "test.jpg",
                Price = 99.99m,
                Brands = new ProductBrand { Id = "brand1", Name = "Brand1" },
                Types = new ProductType { Id = "type1", Name = "Type1" }
            };

            // Act
            var response = _mapper.Map<ProductResponse>(product);

            // Assert
            Assert.NotNull(response);
            Assert.Equal(product.Id, response.Id);
            Assert.Equal(product.Name, response.Name);
            Assert.Equal(product.Summary, response.Summary);
            Assert.Equal(product.Description, response.Description);
            Assert.Equal(product.ImageFile, response.ImageFile);
            Assert.Equal(product.Price, response.Price);
        }

        [Fact]
        public void Map_ProductResponseToProduct_MapsCorrectly()
        {
            // Arrange
            var response = new ProductResponse
            {
                Id = "product-123",
                Name = "Test Product",
                Summary = "Test Summary",
                Description = "Test Description",
                ImageFile = "test.jpg",
                Price = 99.99m
            };

            // Act
            var product = _mapper.Map<Product>(response);

            // Assert
            Assert.NotNull(product);
            Assert.Equal(response.Id, product.Id);
            Assert.Equal(response.Name, product.Name);
            Assert.Equal(response.Summary, product.Summary);
            Assert.Equal(response.Description, product.Description);
            Assert.Equal(response.ImageFile, product.ImageFile);
            Assert.Equal(response.Price, product.Price);
        }

        [Fact]
        public void Map_ProductBrandToBrandResponse_MapsCorrectly()
        {
            // Arrange
            var productBrand = new ProductBrand
            {
                Id = "brand-123",
                Name = "Test Brand"
            };

            // Act
            var brandResponse = _mapper.Map<BrandResponse>(productBrand);

            // Assert
            Assert.NotNull(brandResponse);
            Assert.Equal(productBrand.Id, brandResponse.Id);
            Assert.Equal(productBrand.Name, brandResponse.Name);
        }

        [Fact]
        public void Map_BrandResponseToProductBrand_MapsCorrectly()
        {
            // Arrange
            var brandResponse = new BrandResponse
            {
                Id = "brand-123",
                Name = "Test Brand"
            };

            // Act
            var productBrand = _mapper.Map<ProductBrand>(brandResponse);

            // Assert
            Assert.NotNull(productBrand);
            Assert.Equal(brandResponse.Id, productBrand.Id);
            Assert.Equal(brandResponse.Name, productBrand.Name);
        }

        [Fact]
        public void Map_ProductTypeToTypesResponse_MapsCorrectly()
        {
            // Arrange
            var productType = new ProductType
            {
                Id = "type-123",
                Name = "Test Type"
            };

            // Act
            var typesResponse = _mapper.Map<TypesResponse>(productType);

            // Assert
            Assert.NotNull(typesResponse);
            Assert.Equal(productType.Id, typesResponse.Id);
            Assert.Equal(productType.Name, typesResponse.Name);
        }

        [Fact]
        public void Map_TypesResponseToProductType_MapsCorrectly()
        {
            // Arrange
            var typesResponse = new TypesResponse
            {
                Id = "type-123",
                Name = "Test Type"
            };

            // Act
            var productType = _mapper.Map<ProductType>(typesResponse);

            // Assert
            Assert.NotNull(productType);
            Assert.Equal(typesResponse.Id, productType.Id);
            Assert.Equal(typesResponse.Name, productType.Name);
        }

        [Fact]
        public void Map_CreateProductCommandToProduct_MapsCorrectly()
        {
            // Arrange
            var command = new CreateProductCommand
            {
                Name = "New Product",
                Summary = "New Summary",
                Description = "New Description",
                ImageFile = "new.jpg",
                Price = 149.99m,
                Brands = new ProductBrand { Id = "brand1", Name = "Brand1" },
                Types = new ProductType { Id = "type1", Name = "Type1" }
            };

            // Act
            var product = _mapper.Map<Product>(command);

            // Assert
            Assert.NotNull(product);
            Assert.Equal(command.Name, product.Name);
            Assert.Equal(command.Summary, product.Summary);
            Assert.Equal(command.Description, product.Description);
            Assert.Equal(command.ImageFile, product.ImageFile);
            Assert.Equal(command.Price, product.Price);
            Assert.Equal(command.Brands, product.Brands);
            Assert.Equal(command.Types, product.Types);
        }

        [Fact]
        public void Map_ProductToCreateProductCommand_MapsCorrectly()
        {
            // Arrange
            var product = new Product
            {
                Id = "product-123",
                Name = "Test Product",
                Summary = "Test Summary",
                Description = "Test Description",
                ImageFile = "test.jpg",
                Price = 99.99m,
                Brands = new ProductBrand { Id = "brand1", Name = "Brand1" },
                Types = new ProductType { Id = "type1", Name = "Type1" }
            };

            // Act
            var command = _mapper.Map<CreateProductCommand>(product);

            // Assert
            Assert.NotNull(command);
            Assert.Equal(product.Name, command.Name);
            Assert.Equal(product.Summary, command.Summary);
            Assert.Equal(product.Description, command.Description);
            Assert.Equal(product.ImageFile, command.ImageFile);
            Assert.Equal(product.Price, command.Price);
            Assert.Equal(product.Brands, command.Brands);
            Assert.Equal(product.Types, command.Types);
        }

        [Fact]
        public void Map_PaginationProductToPaginationProductResponse_MapsCorrectly()
        {
            // Arrange
            var products = new List<Product>
            {
                new Product { Id = "1", Name = "Product 1", Price = 100 },
                new Product { Id = "2", Name = "Product 2", Price = 200 }
            };

            var pagination = new Pagination<Product>(
                pageIndex: 1,
                pageSize: 10,
                count: 2,
                data: products
            );

            // Act
            var responsePagination = _mapper.Map<Pagination<ProductResponse>>(pagination);

            // Assert
            Assert.NotNull(responsePagination);
            Assert.Equal(pagination.PageIndex, responsePagination.PageIndex);
            Assert.Equal(pagination.PageSize, responsePagination.PageSize);
            Assert.Equal(pagination.Count, responsePagination.Count);
            Assert.Equal(2, responsePagination.Data.Count);
            Assert.Equal("Product 1", responsePagination.Data[0].Name);
            Assert.Equal("Product 2", responsePagination.Data[1].Name);
        }

        [Fact]
        public void Map_PaginationProductResponseToPaginationProduct_MapsCorrectly()
        {
            // Arrange
            var productResponses = new List<ProductResponse>
            {
                new ProductResponse { Id = "1", Name = "Product 1", Price = 100 },
                new ProductResponse { Id = "2", Name = "Product 2", Price = 200 }
            };

            var responsePagination = new Pagination<ProductResponse>(
                pageIndex: 1,
                pageSize: 10,
                count: 2,
                data: productResponses
            );

            // Act
            var productPagination = _mapper.Map<Pagination<Product>>(responsePagination);

            // Assert
            Assert.NotNull(productPagination);
            Assert.Equal(responsePagination.PageIndex, productPagination.PageIndex);
            Assert.Equal(responsePagination.PageSize, productPagination.PageSize);
            Assert.Equal(responsePagination.Count, productPagination.Count);
            Assert.Equal(2, productPagination.Data.Count);
        }

        [Fact]
        public void Map_ListOfProductBrands_MapsCorrectly()
        {
            // Arrange
            var brands = new List<ProductBrand>
            {
                new ProductBrand { Id = "1", Name = "Apple" },
                new ProductBrand { Id = "2", Name = "Samsung" }
            };

            // Act
            var brandResponses = _mapper.Map<IList<BrandResponse>>(brands);

            // Assert
            Assert.NotNull(brandResponses);
            Assert.Equal(2, brandResponses.Count);
            Assert.Equal("Apple", brandResponses[0].Name);
            Assert.Equal("Samsung", brandResponses[1].Name);
        }

        [Fact]
        public void Map_ListOfProductTypes_MapsCorrectly()
        {
            // Arrange
            var types = new List<ProductType>
            {
                new ProductType { Id = "1", Name = "Electronics" },
                new ProductType { Id = "2", Name = "Clothing" }
            };

            // Act
            var typesResponses = _mapper.Map<IList<TypesResponse>>(types);

            // Assert
            Assert.NotNull(typesResponses);
            Assert.Equal(2, typesResponses.Count);
            Assert.Equal("Electronics", typesResponses[0].Name);
            Assert.Equal("Clothing", typesResponses[1].Name);
        }

        [Fact]
        public void Map_NullProduct_ReturnsNull()
        {
            // Arrange
            Product product = null;

            // Act
            var response = _mapper.Map<ProductResponse>(product);

            // Assert
            Assert.Null(response);
        }

        [Fact]
        public void Map_EmptyProductList_ReturnsEmptyList()
        {
            // Arrange
            var emptyList = new List<Product>();

            // Act
            var responses = _mapper.Map<List<ProductResponse>>(emptyList);

            // Assert
            Assert.NotNull(responses);
            Assert.Empty(responses);
        }

        [Fact]
        public void Map_ProductWithNullBrand_HandlesNull()
        {
            // Arrange
            var product = new Product
            {
                Id = "product-123",
                Name = "Test Product",
                Price = 99.99m,
                Brands = null
            };

            // Act
            var response = _mapper.Map<ProductResponse>(product);

            // Assert
            Assert.NotNull(response);
            Assert.Null(response.Brands);
        }

        [Fact]
        public void Map_ProductWithNullType_HandlesNull()
        {
            // Arrange
            var product = new Product
            {
                Id = "product-123",
                Name = "Test Product",
                Price = 99.99m,
                Types = null
            };

            // Act
            var response = _mapper.Map<ProductResponse>(product);

            // Assert
            Assert.NotNull(response);
            Assert.Null(response.Types);
        }

        [Fact]
        public void Mapper_IsSingleton()
        {
            // Arrange
            var mapper1 = ProductMapper.Mapper;
            var mapper2 = ProductMapper.Mapper;

            // Assert
            Assert.Same(mapper1, mapper2);
        }

        [Fact]
        public void Mapper_LazyInitialization_WorksCorrectly()
        {
            // Act
            var isValueCreated = ProductMapper.Lazy.IsValueCreated;
            var mapper = ProductMapper.Mapper;
            var isValueCreatedAfter = ProductMapper.Lazy.IsValueCreated;

            // Assert
            Assert.True(isValueCreatedAfter);
            Assert.NotNull(mapper);
        }
    }
}