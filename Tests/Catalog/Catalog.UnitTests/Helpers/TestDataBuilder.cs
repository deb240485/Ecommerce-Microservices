using Catalog.Application.Commands;
using Catalog.Application.Queries;
using Catalog.Application.Responses;
using Catalog.Core.Entities;
using Catalog.Core.Specs;

namespace Catalog.UnitTests.Helpers
{
    /// <summary>
    /// Builder pattern for creating test data objects
    /// </summary>
    public class TestDataBuilder
    {
        // Product Builder
        public class ProductBuilder
        {
            private string _id = Guid.NewGuid().ToString();
            private string _name = "Test Product";
            private string _summary = "Test Summary";
            private string _description = "Test Description";
            private string _imageFile = "test.jpg";
            private decimal _price = 99.99m;
            private ProductBrand _brand = new ProductBrand { Id = "brand-1", Name = "TestBrand" };
            private ProductType _type = new ProductType { Id = "type-1", Name = "TestType" };

            public ProductBuilder WithId(string id)
            {
                _id = id;
                return this;
            }

            public ProductBuilder WithName(string name)
            {
                _name = name;
                return this;
            }

            public ProductBuilder WithSummary(string summary)
            {
                _summary = summary;
                return this;
            }

            public ProductBuilder WithDescription(string description)
            {
                _description = description;
                return this;
            }

            public ProductBuilder WithImageFile(string imageFile)
            {
                _imageFile = imageFile;
                return this;
            }

            public ProductBuilder WithPrice(decimal price)
            {
                _price = price;
                return this;
            }

            public ProductBuilder WithBrand(ProductBrand brand)
            {
                _brand = brand;
                return this;
            }

            public ProductBuilder WithType(ProductType type)
            {
                _type = type;
                return this;
            }

            public Product Build()
            {
                return new Product
                {
                    Id = _id,
                    Name = _name,
                    Summary = _summary,
                    Description = _description,
                    ImageFile = _imageFile,
                    Price = _price,
                    Brands = _brand,
                    Types = _type
                };
            }
        }

        // ProductBrand Builder
        public class ProductBrandBuilder
        {
            private string _id = Guid.NewGuid().ToString();
            private string _name = "Test Brand";

            public ProductBrandBuilder WithId(string id)
            {
                _id = id;
                return this;
            }

            public ProductBrandBuilder WithName(string name)
            {
                _name = name;
                return this;
            }

            public ProductBrand Build()
            {
                return new ProductBrand
                {
                    Id = _id,
                    Name = _name
                };
            }
        }

        // ProductType Builder
        public class ProductTypeBuilder
        {
            private string _id = Guid.NewGuid().ToString();
            private string _name = "Test Type";

            public ProductTypeBuilder WithId(string id)
            {
                _id = id;
                return this;
            }

            public ProductTypeBuilder WithName(string name)
            {
                _name = name;
                return this;
            }

            public ProductType Build()
            {
                return new ProductType
                {
                    Id = _id,
                    Name = _name
                };
            }
        }

        // CreateProductCommand Builder
        public class CreateProductCommandBuilder
        {
            private string _name = "Test Product";
            private string _summary = "Test Summary";
            private string _description = "Test Description";
            private string _imageFile = "test.jpg";
            private decimal _price = 99.99m;
            private ProductBrand _brand = new ProductBrand { Id = "brand-1", Name = "TestBrand" };
            private ProductType _type = new ProductType { Id = "type-1", Name = "TestType" };

            public CreateProductCommandBuilder WithName(string name)
            {
                _name = name;
                return this;
            }

            public CreateProductCommandBuilder WithSummary(string summary)
            {
                _summary = summary;
                return this;
            }

            public CreateProductCommandBuilder WithDescription(string description)
            {
                _description = description;
                return this;
            }

            public CreateProductCommandBuilder WithImageFile(string imageFile)
            {
                _imageFile = imageFile;
                return this;
            }

            public CreateProductCommandBuilder WithPrice(decimal price)
            {
                _price = price;
                return this;
            }

            public CreateProductCommandBuilder WithBrand(ProductBrand brand)
            {
                _brand = brand;
                return this;
            }

            public CreateProductCommandBuilder WithType(ProductType type)
            {
                _type = type;
                return this;
            }

            public CreateProductCommand Build()
            {
                return new CreateProductCommand
                {
                    Name = _name,
                    Summary = _summary,
                    Description = _description,
                    ImageFile = _imageFile,
                    Price = _price,
                    Brands = _brand,
                    Types = _type
                };
            }
        }

        // UpdateProductCommand Builder
        public class UpdateProductCommandBuilder
        {
            private string _id = Guid.NewGuid().ToString();
            private string _name = "Updated Product";
            private string _summary = "Updated Summary";
            private string _description = "Updated Description";
            private string _imageFile = "updated.jpg";
            private decimal _price = 149.99m;
            private ProductBrand _brand = new ProductBrand { Id = "brand-1", Name = "TestBrand" };
            private ProductType _type = new ProductType { Id = "type-1", Name = "TestType" };

            public UpdateProductCommandBuilder WithId(string id)
            {
                _id = id;
                return this;
            }

            public UpdateProductCommandBuilder WithName(string name)
            {
                _name = name;
                return this;
            }

            public UpdateProductCommandBuilder WithSummary(string summary)
            {
                _summary = summary;
                return this;
            }

            public UpdateProductCommandBuilder WithDescription(string description)
            {
                _description = description;
                return this;
            }

            public UpdateProductCommandBuilder WithImageFile(string imageFile)
            {
                _imageFile = imageFile;
                return this;
            }

            public UpdateProductCommandBuilder WithPrice(decimal price)
            {
                _price = price;
                return this;
            }

            public UpdateProductCommandBuilder WithBrand(ProductBrand brand)
            {
                _brand = brand;
                return this;
            }

            public UpdateProductCommandBuilder WithType(ProductType type)
            {
                _type = type;
                return this;
            }

            public UpdateProductCommand Build()
            {
                return new UpdateProductCommand
                {
                    Id = _id,
                    Name = _name,
                    Summary = _summary,
                    Description = _description,
                    ImageFile = _imageFile,
                    Price = _price,
                    Brands = _brand,
                    Types = _type
                };
            }
        }

        // CatalogSpecParams Builder
        public class CatalogSpecParamsBuilder
        {
            private int _pageIndex = 1;
            private int _pageSize = 10;
            private string _brandId = null;
            private string _typeId = null;
            private string _sort = null;
            private string _search = null;

            public CatalogSpecParamsBuilder WithPageIndex(int pageIndex)
            {
                _pageIndex = pageIndex;
                return this;
            }

            public CatalogSpecParamsBuilder WithPageSize(int pageSize)
            {
                _pageSize = pageSize;
                return this;
            }

            public CatalogSpecParamsBuilder WithBrandId(string brandId)
            {
                _brandId = brandId;
                return this;
            }

            public CatalogSpecParamsBuilder WithTypeId(string typeId)
            {
                _typeId = typeId;
                return this;
            }

            public CatalogSpecParamsBuilder WithSort(string sort)
            {
                _sort = sort;
                return this;
            }

            public CatalogSpecParamsBuilder WithSearch(string search)
            {
                _search = search;
                return this;
            }

            public CatalogSpecParams Build()
            {
                return new CatalogSpecParams
                {
                    PageIndex = _pageIndex,
                    PageSize = _pageSize,
                    BrandId = _brandId,
                    TypeId = _typeId,
                    Sort = _sort,
                    Search = _search
                };
            }
        }

        // Pagination Builder
        public class PaginationBuilder<T> where T : class
        {
            private int _pageIndex = 1;
            private int _pageSize = 10;
            private int _count = 0;
            private IReadOnlyList<T> _data = new List<T>();

            public PaginationBuilder<T> WithPageIndex(int pageIndex)
            {
                _pageIndex = pageIndex;
                return this;
            }

            public PaginationBuilder<T> WithPageSize(int pageSize)
            {
                _pageSize = pageSize;
                return this;
            }

            public PaginationBuilder<T> WithCount(int count)
            {
                _count = count;
                return this;
            }

            public PaginationBuilder<T> WithData(IReadOnlyList<T> data)
            {
                _data = data;
                _count = data.Count;
                return this;
            }

            public Pagination<T> Build()
            {
                return new Pagination<T>(_pageIndex, _pageSize, _count, _data);
            }
        }

        // ProductResponse Builder
        public class ProductResponseBuilder
        {
            private string _id = Guid.NewGuid().ToString();
            private string _name = "Test Product Response";
            private string _summary = "Test Summary";
            private string _description = "Test Description";
            private string _imageFile = "test.jpg";
            private decimal _price = 99.99m;

            public ProductResponseBuilder WithId(string id)
            {
                _id = id;
                return this;
            }

            public ProductResponseBuilder WithName(string name)
            {
                _name = name;
                return this;
            }

            public ProductResponseBuilder WithSummary(string summary)
            {
                _summary = summary;
                return this;
            }

            public ProductResponseBuilder WithDescription(string description)
            {
                _description = description;
                return this;
            }

            public ProductResponseBuilder WithImageFile(string imageFile)
            {
                _imageFile = imageFile;
                return this;
            }

            public ProductResponseBuilder WithPrice(decimal price)
            {
                _price = price;
                return this;
            }

            public ProductResponse Build()
            {
                return new ProductResponse
                {
                    Id = _id,
                    Name = _name,
                    Summary = _summary,
                    Description = _description,
                    ImageFile = _imageFile,
                    Price = _price
                };
            }
        }

        // BrandResponse Builder
        public class BrandResponseBuilder
        {
            private string _id = Guid.NewGuid().ToString();
            private string _name = "Test Brand Response";

            public BrandResponseBuilder WithId(string id)
            {
                _id = id;
                return this;
            }

            public BrandResponseBuilder WithName(string name)
            {
                _name = name;
                return this;
            }

            public BrandResponse Build()
            {
                return new BrandResponse
                {
                    Id = _id,
                    Name = _name
                };
            }
        }

        // TypesResponse Builder
        public class TypesResponseBuilder
        {
            private string _id = Guid.NewGuid().ToString();
            private string _name = "Test Type Response";

            public TypesResponseBuilder WithId(string id)
            {
                _id = id;
                return this;
            }

            public TypesResponseBuilder WithName(string name)
            {
                _name = name;
                return this;
            }

            public TypesResponse Build()
            {
                return new TypesResponse
                {
                    Id = _id,
                    Name = _name
                };
            }
        }

        // Static factory methods for easy access
        public static ProductBuilder AProduct() => new ProductBuilder();
        public static ProductBrandBuilder AProductBrand() => new ProductBrandBuilder();
        public static ProductTypeBuilder AProductType() => new ProductTypeBuilder();
        public static CreateProductCommandBuilder ACreateProductCommand() => new CreateProductCommandBuilder();
        public static UpdateProductCommandBuilder AnUpdateProductCommand() => new UpdateProductCommandBuilder();
        public static CatalogSpecParamsBuilder ACatalogSpecParams() => new CatalogSpecParamsBuilder();
        public static PaginationBuilder<T> APagination<T>() where T : class => new PaginationBuilder<T>();
        public static ProductResponseBuilder AProductResponse() => new ProductResponseBuilder();
        public static BrandResponseBuilder ABrandResponse() => new BrandResponseBuilder();
        public static TypesResponseBuilder ATypesResponse() => new TypesResponseBuilder();

        // Helper methods for creating lists
        public static List<Product> CreateProductList(int count)
        {
            var products = new List<Product>();
            for (int i = 1; i <= count; i++)
            {
                products.Add(AProduct()
                    .WithId($"product-{i}")
                    .WithName($"Product {i}")
                    .WithPrice(100m * i)
                    .Build());
            }
            return products;
        }

        public static List<ProductBrand> CreateBrandList(int count)
        {
            var brands = new List<ProductBrand>();
            for (int i = 1; i <= count; i++)
            {
                brands.Add(AProductBrand()
                    .WithId($"brand-{i}")
                    .WithName($"Brand {i}")
                    .Build());
            }
            return brands;
        }

        public static List<ProductType> CreateTypeList(int count)
        {
            var types = new List<ProductType>();
            for (int i = 1; i <= count; i++)
            {
                types.Add(AProductType()
                    .WithId($"type-{i}")
                    .WithName($"Type {i}")
                    .Build());
            }
            return types;
        }

        public static List<ProductResponse> CreateProductResponseList(int count)
        {
            var responses = new List<ProductResponse>();
            for (int i = 1; i <= count; i++)
            {
                responses.Add(AProductResponse()
                    .WithId($"product-{i}")
                    .WithName($"Product {i}")
                    .WithPrice(100m * i)
                    .Build());
            }
            return responses;
        }

        public static List<BrandResponse> CreateBrandResponseList(int count)
        {
            var responses = new List<BrandResponse>();
            for (int i = 1; i <= count; i++)
            {
                responses.Add(ABrandResponse()
                    .WithId($"brand-{i}")
                    .WithName($"Brand {i}")
                    .Build());
            }
            return responses;
        }

        public static List<TypesResponse> CreateTypesResponseList(int count)
        {
            var responses = new List<TypesResponse>();
            for (int i = 1; i <= count; i++)
            {
                responses.Add(ATypesResponse()
                    .WithId($"type-{i}")
                    .WithName($"Type {i}")
                    .Build());
            }
            return responses;
        }
    }

    /// <summary>
    /// Example usage of TestDataBuilder
    /// </summary>
    public class TestDataBuilderExamples
    {
        public void ExampleUsage()
        {
            // Create a single product using builder
            var product = TestDataBuilder.AProduct()
                .WithName("iPhone 15")
                .WithPrice(999.99m)
                .WithBrand(new ProductBrand { Id = "apple", Name = "Apple" })
                .Build();

            // Create a command
            var command = TestDataBuilder.ACreateProductCommand()
                .WithName("New Product")
                .WithPrice(149.99m)
                .Build();

            // Create a list of products
            var products = TestDataBuilder.CreateProductList(10);

            // Create pagination
            var pagination = TestDataBuilder.APagination<Product>()
                .WithPageIndex(1)
                .WithPageSize(10)
                .WithData(products)
                .Build();

            // Create catalog spec params
            var specParams = TestDataBuilder.ACatalogSpecParams()
                .WithPageSize(20)
                .WithBrandId("brand-123")
                .WithSearch("laptop")
                .Build();
        }
    }
}