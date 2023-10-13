using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopNet.DAL.Entities.OrderAggregate
{
    public class ProductItemOrdered
    {
        public ProductItemOrdered()
        {
        }

        public ProductItemOrdered(int id, string name, string pictureUrl)
        {
            Id = id;
            Name = name;
            PictureUrl = pictureUrl;
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string PictureUrl { get; set; }
    }
}
