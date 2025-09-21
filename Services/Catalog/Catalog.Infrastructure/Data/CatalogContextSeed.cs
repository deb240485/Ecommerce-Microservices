using Catalog.Core.Entities;
using MongoDB.Driver;
using System.Reflection;
using System.Text.Json;

namespace Catalog.Infrastructure.Data
{
    public class CatalogContextSeed
    {
        public static void SeedData(IMongoCollection<Product> productCollection)
        {
            bool checkProductsData = productCollection.Find(p => true).Any();
            //var path = Path.Combine("Data", "SeedData", "products.json");
            //var basePath = Directory.GetCurrentDirectoryName();
            //var path = Path.Combine(basePath, "Catalog.Infrastructure", "Data", "SeedData", "brands.json");
            string? basePath = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
            var path = Path.Combine(basePath, "Data", "SeedData", "products.json");

            if (!checkProductsData)
            {
                var productsData = File.ReadAllText(path);
                // var productsData = File.ReadAllText("../Catalog.Infrastructure/Data/SeedData/products.json");
                var products = JsonSerializer.Deserialize<List<Product>>(productsData);

                if(products != null)
                {
                    foreach(var product in products)
                    {
                        productCollection.InsertOneAsync(product);
                    }
                }

            }
        }
    }
}
