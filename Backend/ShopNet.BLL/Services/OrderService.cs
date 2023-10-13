using ShopNet.BLL.Interfaces;
using ShopNet.BLL.Services.Abstract;
using ShopNet.DAL.Data;
using ShopNet.DAL.Entities;
using ShopNet.DAL.Entities.OrderAggregate;

namespace ShopNet.BLL.Services
{
    public class OrderService : IOrderService
    {
        private readonly IBasketRepository basketRepo;
        private readonly IGenericRepository<Order> orderRepo;
        private readonly IGenericRepository<DeliveryMethod> deliveryRepo;
        private readonly IGenericRepository<Product> productRepo;

        public OrderService(IBasketRepository basketRepo, IGenericRepository<Order> orderRepo,
            IGenericRepository<DeliveryMethod> deliveryRepo, IGenericRepository<Product> productRepo)
        {
            this.basketRepo = basketRepo;
            this.orderRepo = orderRepo;
            this.deliveryRepo = deliveryRepo;
            this.productRepo = productRepo;
        }

        public async Task<Order> CreateOrderAsync(string buyerEmail, int deliveryMethod, string basketId, Address shippingAddress)
        {
            var basket = await basketRepo.GetBasketAsync(basketId);
            var orderItems = new List<OrderItem>();
            foreach (var item in basket.Items)
            {
                var product = await productRepo.GetByIdAsync(item.Id);
                var orderItem = new OrderItem()
                {
                    ItemOrdered = new ProductItemOredered(product.Id, product.Name, product.PictureUrl),
                    Price = product.Price,
                    Quantity = item.Quantity,
                };
                orderItems.Add(orderItem);
            }

            var dMethod = await deliveryRepo.GetByIdAsync(deliveryMethod);
            var subtotal = orderItems.Sum(x => x.Price * x.Quantity);
            var order = new Order(orderItems, buyerEmail, shippingAddress, dMethod, subtotal);

            return order;
        }

        public async Task<IReadOnlyList<DeliveryMethod>> GetDeliveryMethodsAsync()
        {
            return await deliveryRepo.ListAllAsync();
        }

        public Task<Order> GetOrderByIdAsync(int id, string buyerEmail)
        {
            throw new NotImplementedException();
        }

        public Task<IReadOnlyList<Order>> GetOrdersForUserAsync(string buyerEmail)
        {
            throw new NotImplementedException();
        }
    }
}
