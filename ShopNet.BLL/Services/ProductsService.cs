using Microsoft.EntityFrameworkCore;
using ShopNet.BLL.Interfaces;
using ShopNet.BLL.Services.Abstract;
using ShopNet.DAL.Data;
using ShopNet.DAL.Entities;

namespace ShopNet.BLL.Services
{
    public class ProductsService : BaseService, IProductsService
    {
        public ProductsService(StoreContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Product>> GetAllProdudctsAsync()
        {
            return await _context.Products.ToListAsync();
        }

        public async Task<Product> GetProductByIdAsync(int id)
        {
            var product = await _context.Products.SingleOrDefaultAsync(p => p.Id == id);
            return product ?? throw new ArgumentNullException("Product Not Found", new Exception(nameof(id)));
        }
    }
}