using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ShopNet.DAL.Entities.Identity;

namespace ShopNet.DAL.Data.Identity
{
    public class AppIdentityDbContext : IdentityDbContext<AppUser>
    {
        public AppIdentityDbContext(DbContextOptions<AppIdentityDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<AppUser>().HasIndex(e => e.Id).IsUnique();

            builder.Entity<AppUser>()
            .HasOne(e => e.Address)
            .WithOne(e => e.User)
            .HasForeignKey<Address>("AddressId");
            base.OnModelCreating(builder);
        }
    }
}
