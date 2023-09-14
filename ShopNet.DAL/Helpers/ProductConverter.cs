using ShopNet.DAL.Entities;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace ShopNet.DAL.Helpers
{
    public class ProductConverter : JsonConverter<Product>

    {
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
            throw new NotImplementedException();
        }
    }
}