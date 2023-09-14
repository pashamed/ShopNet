using System.ComponentModel.DataAnnotations;

namespace ShopNet.DAL.Entities
{
    public class Product
    {
        [Key]
        public int Id { get; set; }

        public string Name { get; set; }
    }
}