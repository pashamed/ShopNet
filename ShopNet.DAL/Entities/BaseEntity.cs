using System.ComponentModel.DataAnnotations;

namespace ShopNet.DAL.Entities
{
    public class BaseEntity
    {
        [Key]
        public int Id { get; set; }
    }
}
