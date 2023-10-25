using ShopNet.BLL.Interfaces;
using ShopNet.BLL.Services.Abstract;
using ShopNet.BLL.Specifications;
using ShopNet.DAL.Data;
using ShopNet.DAL.Entities;
using ShopNet.DAL.Entities.OrderAggregate;

namespace ShopNet.BLL.Services
{
    public class OrderService : IOrderService
    {
        private readonly IBasketRepository basketRepo;
        private readonly IUnitOfWork unitOfWork;

        public OrderService(IBasketRepository basketRepo, IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
            this.basketRepo = basketRepo;
        }

        public async Task<Order> CreateOrderAsync(string buyerEmail, int deliveryMethod, string basketId, Address shippingAddress)
        {
            var basket = await basketRepo.GetBasketAsync(basketId);
            var orderItems = new List<OrderItem>();
            foreach (var item in basket.Items)
            {
                var product = await unitOfWork.Repository<Product>().GetByIdAsync(item.Id);
                var orderItem = new OrderItem()
                {
                    ItemOrdered = new ProductItemOrdered(product.Id, product.Name, product.PictureUrl),
                    Price = product.Price,
                    Quantity = item.Quantity,
                };
                orderItems.Add(orderItem);
            }

            var dMethod = await unitOfWork.Repository<DeliveryMethod>().GetByIdAsync(deliveryMethod);
            var subtotal = orderItems.Sum(x => x.Price * x.Quantity);

            var order = await unitOfWork.Repository<Order>().GetEntityWithSpec(new OrderByPaymentIntentIdSpecification(basket.PaymentIntentId));
            if(order is not null)
            {
                order.ShipToAddress = shippingAddress;
                order.DeliveryMethod = dMethod;
                order.Subtotal = subtotal;
                unitOfWork.Repository<Order>().Update(order);
            }
            else
            {
                order = new Order(orderItems, buyerEmail, shippingAddress, dMethod, subtotal,basket.PaymentIntentId);
                unitOfWork.Repository<Order>().Add(order);
            }
                          
            var result = await unitOfWork.Complete();
            return result <= 0 ? null : order;
        }

        public async Task<IReadOnlyList<DeliveryMethod>> GetDeliveryMethodsAsync()
        {
            return await unitOfWork.Repository<DeliveryMethod>().ListAllAsync();
        }

        public async Task<Order> GetOrderByIdAsync(int id, string buyerEmail)
        {
            return await unitOfWork.Repository<Order>().GetEntityWithSpec(new OrdersWithItemsAndOrderingSpecification(id, buyerEmail));
        }

        public async Task<IReadOnlyList<Order>> GetOrdersForUserAsync(string buyerEmail)
        {
            return await unitOfWork.Repository<Order>().ListAsync(new OrdersWithItemsAndOrderingSpecification(buyerEmail));
        }
    }
}
