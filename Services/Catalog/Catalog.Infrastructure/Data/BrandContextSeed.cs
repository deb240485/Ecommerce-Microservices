using Catalog.Core.Entities;
using MongoDB.Driver;
using System.Reflection;
using System.Text.Json;

namespace Catalog.Infrastructure.Data
{
    public class BrandContextSeed
    {
        public static void SeedData(IMongoCollection<ProductBrand> brandCollection)
        {
            bool checkBrandsData = brandCollection.Find(b=> true).Any();
            //var path = Path.Combine("Data", "SeedData", "brands.json");
            string? basePath = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
            var path = Path.Combine(basePath, "Data", "SeedData", "brands.json");
            if (!checkBrandsData)
            {
                var brandsData = File.ReadAllText(path);
                //var brandsData = File.ReadAllText("../Catalog.Infrastructure/Data/SeedData/brands.json");
                var brands = JsonSerializer.Deserialize<List<ProductBrand>>(brandsData);

                if(brands != null)
                {
                    foreach (var brand in brands)
                    {
                        brandCollection.InsertOneAsync(brand);
                    }
                }
            }

        }
    }
}
