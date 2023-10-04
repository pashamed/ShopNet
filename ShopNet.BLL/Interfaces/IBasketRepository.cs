using ShopNet.DAL.Entities;

namespace ShopNet.BLL.Interfaces
{
    public interface IBasketRepository
    {
        Task<Basket> GetBasketAsync(string id);

        Task<Basket> UpdateBasketAsync(Basket basket);

        Task<bool> DeleteBasketAsync(string id);
    }
}