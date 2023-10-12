using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopNet.DAL.Entities.OrderAggregate
{
    public class OrderItem:BaseEntity
    {
        public OrderItem()
        {
        }

        public OrderItem(ProductItemOredered itemOredered, decimal price, int quantity)
        {
            ItemOredered = itemOredered;
            Price = price;
            Quantity = quantity;
        }

        public ProductItemOredered ItemOredered { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }

    }
}
