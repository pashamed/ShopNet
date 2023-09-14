using ShopNet.DAL.Helpers;
using System.Text.Json.Serialization;

namespace ShopNet.DAL.Entities
{
    [JsonConverter(typeof(ProductJSONConverter))]
    public class Product : BaseEntity
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public string PictureUrl { get; set; }
        public ProductType ProductType { get; set; }
        public ProductBrand ProductBrand { get; set; }
    }
}