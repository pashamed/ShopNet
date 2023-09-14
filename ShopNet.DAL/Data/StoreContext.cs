using Microsoft.EntityFrameworkCore;
using ShopNet.DAL.Entities;

namespace ShopNet.DAL.Data
{
    public class StoreContext : DbContext
    {
        public StoreContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Product> Products { get; set; }
    }
}