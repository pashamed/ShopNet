using ShopNet.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopNet.BLL.Interfaces
{
    public interface IProductsService
    {
        Task<IEnumerable<Product>> GetAllProdudctsAsync();
        Task<Product> GetProductByIdAsync(int id);
    }
}
