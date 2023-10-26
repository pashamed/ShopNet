using ShopNet.DAL.Entities;
using ShopNet.DAL.Entities.OrderAggregate;

namespace ShopNet.BLL.Interfaces;

public interface IPaymentService
{
    Task<Basket> CreateOrUpdatePaymentIntent(string basketId);
    Task<Order> UpdateOrderPaymentSucceeded(string paymentIntentId);
    Task<Order> UpdateOrderPaymentFailed(string paymentIntentId);
}
