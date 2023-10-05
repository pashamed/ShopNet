namespace ShopNet.DAL.Entities
{
    //redis entity
    public class Basket
    {
        public string Id { get; set; }
        public List<BasketItem> Items { get; set; } = new List<BasketItem>();

        public Basket()
        {
        }

        public Basket(string id)
        {
            Id = id;
        }
    }

    public class BasketItem
    {
        public int Id { get; set; }
        public string ProductName { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public string PictureUrl { get; set; }
        public ProductBrand Brand { get; set; }
        public ProductType Type { get; set; }
    }
}