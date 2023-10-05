using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ShopNet.BLL.Interfaces;
using ShopNet.Common.DTO;
using ShopNet.Common.DTO.User;
using ShopNet.DAL.Entities.Identity;

namespace ShopNet.BLL.Services
{
    public class UserService : IUserService
    {
        private readonly UserManager<AppUser> userManager;
        private readonly SignInManager<AppUser> signInManager;
        private readonly ITokenService tokenService;
        private readonly IMapper mapper;

        public UserService(UserManager<AppUser> userManager,
            SignInManager<AppUser> signInManager,
            ITokenService tokenService,
            IMapper mapper)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.tokenService = tokenService;
            this.mapper = mapper;
        }

        public async Task<AppUser> GetCurrentUserAddressAsync(string email)
        {
            var user = await userManager.Users.Include(x => x.Address)
                .SingleOrDefaultAsync(x => x.Email == email);
            return user ?? null;
        }

        public async Task<UserDto> GetCurrentUserAsync(string email)
        {
            var user = await userManager.FindByEmailAsync(email);
            if (user == null) { return null; }
            return new UserDto
            {
                Email = user.Email,
                Token = tokenService.CreateToken(user),
                DisplayName = user.DisplayName
            };
        }

        public async Task<UserDto> RegisterAsync(RegisterDto registerDto)
        {
            var user = new AppUser
            {
                DisplayName = registerDto.DisplayName,
                Email = registerDto.Email,
                UserName = registerDto.Email
            };

            var result = await userManager.CreateAsync(user, registerDto.Password);

            if (!result.Succeeded) return null;

            return new UserDto
            {
                DisplayName = user.DisplayName,
                Email = user.Email,
                Token = tokenService.CreateToken(user)
            };
        }

        public async Task<Address> UpdateCurrentUserAddressAsync(AddressDto address, string email)
        {
            var user = await GetCurrentUserAddressAsync(email);
            if (user == null) { return null; };
            user.Address = mapper.Map<AddressDto, Address>(address);
            return (await userManager.UpdateAsync(user)).Succeeded ? user.Address : null;
        }

        public async Task<UserDto> UserLoginAsync(LoginDto loginDto)
        {
            var newUser = await userManager.FindByEmailAsync(loginDto.Email);
            if (newUser == null) return null;
            var signInResult = await signInManager.CheckPasswordSignInAsync(newUser, loginDto.Password, false);
            if (!signInResult.Succeeded) return null;

            return new UserDto
            {
                Email = newUser.Email,
                Token = tokenService.CreateToken(newUser),
                DisplayName = newUser.DisplayName
            };
        }
    }
}