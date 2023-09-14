using ShopNet.DAL.Entities;

namespace ShopNet.BLL.Interfaces
{
    public interface IProductsService
    {
        Task<IEnumerable<Product>> GetAllProdudctsAsync();

        Task<Product> GetProductByIdAsync(int id);
    }
}