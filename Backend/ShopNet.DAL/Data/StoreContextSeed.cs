﻿using ShopNet.DAL.Entities;
using ShopNet.DAL.Entities.OrderAggregate;
using System.Reflection;
using System.Text.Json;

namespace ShopNet.DAL.Data
{
    public class StoreContextSeed
    {
        public static async Task SeedAsync(StoreContext context)
        {
            var path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

            if (!context.ProductBrands.Any())
            {
                var brandsData = await File.ReadAllTextAsync(path + @"/SeedData/brands.json");
                var brands = JsonSerializer.Deserialize<List<ProductBrand>>(brandsData);
                await context.ProductBrands.AddRangeAsync(brands);
            }
            if (!context.ProductTypes.Any())
            {
                var typesData = await File.ReadAllTextAsync(path + @"/SeedData/types.json");
                var types = JsonSerializer.Deserialize<List<ProductType>>(typesData);
                await context.ProductTypes.AddRangeAsync(types);
            }
            if (!context.Products.Any())
            {
                var productsData = await File.ReadAllTextAsync(path + @"/SeedData/products.json");
                var products = JsonSerializer.Deserialize<List<Product>>(productsData);
                foreach (var product in products)
                {
                    product.ProductType = await context.ProductTypes.FindAsync(product.ProductType.Id);
                    product.ProductBrand = await context.ProductBrands.FindAsync(product.ProductBrand.Id);
                }
                await context.Products.AddRangeAsync(products);
            }

            if (!context.DeliveryMethods.Any())
            {
                var deliveryData = await File.ReadAllTextAsync(path + @"/SeedData/delivery.json");
                var methods = JsonSerializer.Deserialize<List<DeliveryMethod>>(deliveryData);
                await context.DeliveryMethods.AddRangeAsync(methods);
            }

            if (context.ChangeTracker.HasChanges()) await context.SaveChangesAsync();
        }
    }
}