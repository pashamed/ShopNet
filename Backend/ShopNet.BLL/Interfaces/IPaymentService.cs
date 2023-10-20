using ShopNet.DAL.Entities;

namespace ShopNet.BLL.Interfaces;

public interface IPaymentService
{
    Task<Basket> CreateOrUpdatePaymentIntent(string basketId);
}
