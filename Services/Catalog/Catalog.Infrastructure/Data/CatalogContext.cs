using Catalog.Core.Entities;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catalog.Infrastructure.Data
{
    public class CatalogContext : ICatalogContext
    {
        private readonly IConfiguration _configuration;
        public IMongoCollection<Product> Products { get; }
        public IMongoCollection<ProductBrand> Brands { get; }
        public IMongoCollection<ProductType> Types { get; }

        public CatalogContext(IConfiguration configuration)
        {
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            var connectionString = _configuration["DatabaseSettings:ConnectionString"]; 
            var client = new MongoClient(connectionString);
            
            var databaseName = _configuration["DatabaseSettings:DatabaseName"];
            var database = client.GetDatabase(databaseName);

            Brands = database.GetCollection<ProductBrand>(_configuration["DatabaseSettings:BrandsCollection"]);
            Products = database.GetCollection<Product>(_configuration["DatabaseSettings:ProductsCollection"]);
            Types = database.GetCollection<ProductType>(_configuration["DatabaseSettings:TypesCollection"]);

            BrandContextSeed.SeedData(Brands);
            TypeContextSeed.SeedData(Types);
            CatalogContextSeed.SeedData(Products);
        }
    }
}
