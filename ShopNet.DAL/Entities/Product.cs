using System.Text.Json.Serialization;
using ShopNet.DAL.Helpers;

namespace ShopNet.DAL.Entities
{
    [JsonConverter(typeof(ProductConverter))]
    public class Product:BaseEntity
    {
        public string Name { get; set; }
        public  string Description { get; set; }
        public decimal Price { get; set; }
        public string PictureUrl { get; set; }
        public ProductType ProductType { get; set; }
        //public int ProductTypeId { get; set; }
        public ProductBrand ProductBrand { get; set; }
        //public int ProductBrandId { get; set; }
    }
}