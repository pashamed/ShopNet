using ShopNet.DAL.Entities.Identity;

namespace ShopNet.BLL.Interfaces
{
    public interface IToketService
    {
        string CreateToken(AppUser user);
    }
}
