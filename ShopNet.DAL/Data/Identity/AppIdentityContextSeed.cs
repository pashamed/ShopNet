using Microsoft.AspNetCore.Identity;
using ShopNet.DAL.Entities.Identity;

namespace ShopNet.DAL.Data.Identity
{
    public class AppIdentityContextSeed
    {
        public static async Task SeedUserAsync(UserManager<AppUser> userManager)
        {
            if (!userManager.Users.Any())
            {
                var user = new AppUser
                {
                    DisplayName = "Pavlo",
                    Email = "test@test.com",
                    UserName = "pashamed",
                    Address = new Address
                    {
                        FirstName = "Pavlo",
                        LastName = "Med",
                        Street = "Kyiv street",
                        City = "Kyiv",
                        PostalCode = "02754"
                    }
                };

                await userManager.CreateAsync(user, "Pa$$w0rd");
            }
        }
    }
}