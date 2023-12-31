﻿using ShopNet.DAL.Entities;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace ShopNet.DAL.Helpers
{
    public class ProductJSONConverter : JsonConverter<Product>

    {
        //Converter to deserialize json's Brands and Types Ids and map them to navigation properties when seeding
        public override Product Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            using JsonDocument doc = JsonDocument.ParseValue(ref reader);
            var root = doc.RootElement;

            var product = new Product
            {
                Name = root.GetProperty("Name").GetString(),
                Description = root.GetProperty("Description").GetString(),
                Price = root.GetProperty("Price").GetDecimal(),
                PictureUrl = root.GetProperty("PictureUrl").GetString(),
                ProductType = new ProductType
                {
                    Id = int.Parse(root.GetProperty("ProductTypeId").GetRawText())
                },
                ProductBrand = new ProductBrand
                {
                    Id = int.Parse(root.GetProperty("ProductBrandId").GetRawText())
                }
            };

            return product;
        }

        public override void Write(Utf8JsonWriter writer, Product value, JsonSerializerOptions options)
        {
            writer.WriteStartObject();
            writer.WriteString("Name", value.Name);
            writer.WriteString("Description", value.Description);
            writer.WriteNumber("Price", value.Price);
            writer.WriteString("PictureUrl", value.PictureUrl);
            writer.WritePropertyName("ProductType");
            JsonSerializer.Serialize(writer, value.ProductType, typeof(ProductType), options);
            writer.WritePropertyName("ProductBrand");
            JsonSerializer.Serialize(writer, value.ProductBrand, typeof(ProductBrand), options);
            writer.WriteEndObject();
        }
    }
}