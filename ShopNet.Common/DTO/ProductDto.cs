using ShopNet.DAL.Entities;

namespace ShopNet.Common.DTO
{
    public class ProductDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public string PictureUrl { get; set; }
        public ProductType ProductType { get; set; }
        public ProductBrand ProductBrand { get; set; }
    }
}