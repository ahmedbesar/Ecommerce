using Catalog.Core.Entities;
using MongoDB.Driver;
using System.Text.Json;

namespace Catalog.Infrastructure.Data.Contexts
{
    public static class BrandContextSeed
    {
        public static async Task SeedDataAsync(IMongoCollection<ProductBrand> brandCollection)
        {
            var hasBrands = await brandCollection.Find(_ => true).AnyAsync();
            if (hasBrands)
                return;

            var filePath = Path.Combine(AppContext.BaseDirectory, "Data", "SeedData", "brands.json");

            if (!File.Exists(filePath))
            {
                Console.WriteLine($"BrandContextSeed Seed file not exists:{filePath}");
                return;
            }
            var brandData = await File.ReadAllTextAsync(filePath);
            var brands=JsonSerializer.Deserialize<List<ProductBrand>>(brandData);
            Console.WriteLine($"brands :{brandData}");
            Console.WriteLine($"BrandContextSeed success");

            if (brands?.Any() is true)
            {
              await brandCollection.InsertManyAsync(brands);
            }
        }
    }
}
