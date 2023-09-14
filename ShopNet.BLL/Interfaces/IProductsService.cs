using ShopNet.DAL.Entities;

namespace ShopNet.BLL.Interfaces
{
    public interface IProductsService
    {
        Task<IEnumerable<Product>> GetAllProductsAsync();

        Task<Product> GetProductByIdAsync(int id);
    }
}