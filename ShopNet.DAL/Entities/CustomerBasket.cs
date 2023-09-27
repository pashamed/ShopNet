using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopNet.DAL.Entities
{
    //redis entity
    public class CustomerBasket
    {
        public string Id { get; set; }
        public List<BasketItem> Items { get; set; } = new List<BasketItem>();

        public CustomerBasket()
        {
        }

        public CustomerBasket(string id)
        {
            Id = id;
        }
    }

    public class BasketItem
    {
        public string Id { get; set; }
        public string ProductName { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public string PictureUrl { get; set; }
        public ProductBrand Brand { get; set; }
        public ProductType Type { get; set; }
    }
}