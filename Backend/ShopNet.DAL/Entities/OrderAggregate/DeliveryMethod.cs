using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopNet.DAL.Entities.OrderAggregate
{
    public class DeliveryMethod:BaseEntity
    {
        public string ShortName { get; set; } = string.Empty;
        public string DeliveryTime { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public decimal Price { get; set; }
    }
}
