using System.Collections.Generic;
using Catalog.API.Entities;
using MongoDB.Driver;

namespace Catalog.API.Data
{
    public class CatalogContextSeed
    {
        public static void SeedData(IMongoCollection<Product> productsCollection)
        {
            bool existProduct = productsCollection.Find(p => true).Any();
            if (!existProduct)
            {
                productsCollection.InsertManyAsync(GetPreconfiguredProducts());
            }
        }

        private static IEnumerable<Product> GetPreconfiguredProducts()
        {
            return new List<Product>()
            {
                new Product()
                {
                    Id = "602d2149e773f2a3990b47f5",
                    Name = "Iphone X",
                    Summary = "This is an iPhone",
                    Description = "Lorem ipsum",
                    ImageFile = "product-1.png",
                    Price = 950.00M,
                    Category = "Smart Phone"
                },
                new Product()
                {
                    Id = "602d2149e773f2a3990b47f6",
                    Name = "Iphone XR",
                    Summary = "This is an iPhone XR",
                    Description = "Lorem ipsum",
                    ImageFile = "product-2.png",
                    Price = 1100.00M,
                    Category = "Smart Phone"
                }
            };
        }
    }
}