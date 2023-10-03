using ShopNet.DAL.Entities.Identity;

namespace ShopNet.BLL.Interfaces
{
    public interface ITokenService
    {
        string CreateToken(AppUser user);
    }
}
