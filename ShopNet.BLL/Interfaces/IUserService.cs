using ShopNet.Common.DTO;
using ShopNet.Common.DTO.User;
using ShopNet.DAL.Entities.Identity;

namespace ShopNet.BLL.Interfaces
{
    public interface IUserService
    {
        public Task<UserDto> RegisterAsync(RegisterDto registerDto);
        public Task<UserDto> UserLoginAsync(LoginDto loginDto);
        public Task<UserDto> GetCurrentUserAsync(string email);
        public Task<AppUser> GetCurrentUserAddressAsync(string email);
        public Task<Address> UpdateCurrentUserAddressAsync(AddressDto address, string email);

    }
}
