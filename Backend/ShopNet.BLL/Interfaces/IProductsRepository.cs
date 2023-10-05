using ShopNet.DAL.Entities;

namespace ShopNet.BLL.Interfaces
{
    public interface IProductsRepository
    {
        Task<IReadOnlyList<Product>> GetAllProductsAsync();

        Task<Product> GetProductByIdAsync(int id);

        Task<IReadOnlyList<ProductType>> GetProductTypesAsync();

        Task<IReadOnlyList<ProductBrand>> GetProductBrandsAsync();
    }
}