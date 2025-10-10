using Catalog.Core.Entities;
using Catalog.Core.Specs;
using Catalog.Infrastructure.Data;
using MongoDB.Driver;
using Moq;
using Xunit;

namespace Catalog.UnitTests.Fixtures
{
    /// <summary>
    /// Fixture for creating test Product instances
    /// </summary>
    public class ProductFixture : IDisposable
    {
        public List<Product>? Products { get; private set; }
        public List<ProductBrand>? Brands { get; private set; }
        public List<ProductType>? Types { get; private set; }

        public ProductFixture()
        {
            InitializeBrands();
            InitializeTypes();
            InitializeProducts();
        }

        private void InitializeBrands()
        {
            Brands = new List<ProductBrand>
            {
                new ProductBrand { Id = "brand-1", Name = "Apple" },
                new ProductBrand { Id = "brand-2", Name = "Samsung" },
                new ProductBrand { Id = "brand-3", Name = "Dell" },
                new ProductBrand { Id = "brand-4", Name = "HP" },
                new ProductBrand { Id = "brand-5", Name = "Sony" }
            };
        }

        private void InitializeTypes()
        {
            Types = new List<ProductType>
            {
                new ProductType { Id = "type-1", Name = "Electronics" },
                new ProductType { Id = "type-2", Name = "Computers" },
                new ProductType { Id = "type-3", Name = "Smartphones" },
                new ProductType { Id = "type-4", Name = "Laptops" },
                new ProductType { Id = "type-5", Name = "Tablets" }
            };
        }

        private void InitializeProducts()
        {
            Products = new List<Product>
            {
                new Product
                {
                    Id = "product-1",
                    Name = "iPhone 15 Pro",
                    Summary = "Latest iPhone",
                    Description = "Apple's newest flagship smartphone",
                    ImageFile = "iphone15.jpg",
                    Price = 999.99m,
                    Brands = Brands[0],
                    Types = Types[2]
                },
                new Product
                {
                    Id = "product-2",
                    Name = "Samsung Galaxy S24",
                    Summary = "Samsung flagship",
                    Description = "Samsung's latest smartphone",
                    ImageFile = "galaxys24.jpg",
                    Price = 899.99m,
                    Brands = Brands[1],
                    Types = Types[2]
                },
                new Product
                {
                    Id = "product-3",
                    Name = "Dell XPS 15",
                    Summary = "Premium laptop",
                    Description = "High-performance laptop for professionals",
                    ImageFile = "dellxps15.jpg",
                    Price = 1499.99m,
                    Brands = Brands[2],
                    Types = Types[3]
                },
                new Product
                {
                    Id = "product-4",
                    Name = "HP Spectre x360",
                    Summary = "2-in-1 laptop",
                    Description = "Versatile convertible laptop",
                    ImageFile = "hpspectre.jpg",
                    Price = 1299.99m,
                    Brands = Brands[3],
                    Types = Types[3]
                },
                new Product
                {
                    Id = "product-5",
                    Name = "Sony WH-1000XM5",
                    Summary = "Noise-canceling headphones",
                    Description = "Premium wireless headphones",
                    ImageFile = "sonywh1000.jpg",
                    Price = 399.99m,
                    Brands = Brands[4],
                    Types = Types[0]
                }
            };
        }

        public Product GetProductById(string id)
        {
            return Products.FirstOrDefault(p => p.Id == id);
        }

        public IEnumerable<Product> GetProductsByBrand(string brandName)
        {
            return Products!.Where(p => p.Brands.Name.Equals(brandName, StringComparison.OrdinalIgnoreCase));
        }

        public IEnumerable<Product> GetProductsByType(string typeName)
        {
            return Products!.Where(p => p.Types.Name.Equals(typeName, StringComparison.OrdinalIgnoreCase));
        }

        public IEnumerable<Product> GetProductsByPriceRange(decimal minPrice, decimal maxPrice)
        {
            return Products!.Where(p => p.Price >= minPrice && p.Price <= maxPrice);
        }

        public Product CreateSampleProduct()
        {
            return new Product
            {
                Id = Guid.NewGuid().ToString(),
                Name = "Sample Product",
                Summary = "Sample Summary",
                Description = "Sample Description",
                ImageFile = "sample.jpg",
                Price = 99.99m,
                Brands = Brands[0],
                Types = Types[0]
            };
        }

        public Pagination<Product> GetPaginatedProducts(int pageIndex, int pageSize)
        {
            var totalCount = Products!.Count;
            var pagedProducts = Products
                .Skip((pageIndex - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            return new Pagination<Product>(pageIndex, pageSize, totalCount, pagedProducts);
        }

        public void Dispose()
        {
            Products?.Clear();
            Brands?.Clear();
            Types?.Clear();
        }
    } 
    
}