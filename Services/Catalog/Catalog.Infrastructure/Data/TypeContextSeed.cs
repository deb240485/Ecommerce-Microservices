using Catalog.Core.Entities;
using MongoDB.Driver;
using System.Reflection;
using System.Text.Json;

namespace Catalog.Infrastructure.Data
{
    public class TypeContextSeed
    {
        public static void SeedData(IMongoCollection<ProductType> typeCollection)
        {
            bool checkTypesData = typeCollection.Find(t => true).Any();
            //var path = Path.Combine("Data", "SeedData", "types.json");
            string? basePath = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
            var path = Path.Combine(basePath, "Data", "SeedData", "types.json");

            if (!checkTypesData)
            {
                var typesData = File.ReadAllText(path);
                //var typesData = File.ReadAllText("../Catalog.Infrastructure/Data/SeedData/types.json");
                var types = JsonSerializer.Deserialize<List<ProductType>>(typesData);

                if(types != null)
                {
                    foreach (var type in types)
                    {
                        typeCollection.InsertOneAsync(type);
                    }
                }
            }
        }
    }
}
