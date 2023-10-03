using Microsoft.AspNetCore.Identity;
using ShopNet.BLL.Interfaces;
using ShopNet.Common.DTO.User;
using ShopNet.DAL.Entities.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopNet.BLL.Services
{
    public class UserService : IUserService
    {
        private readonly UserManager<AppUser> userManager;
        private readonly SignInManager<AppUser> signInManager;
        private readonly ITokenService tokenService;

        public UserService(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, ITokenService tokenService)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.tokenService = tokenService;
        }

        public async Task<UserDto> RegisterAsync(RegisterDto registerDto)
        {
            var user = new AppUser
            {
                DisplayName = registerDto.DisplayName,
                Email = registerDto.Email,
                UserName = registerDto.Email
            };

            var result = await userManager.CreateAsync(user,registerDto.Password);

            if (!result.Succeeded) return null;

            return new UserDto
            {
                DiplayName = user.DisplayName,
                Email = user.Email,
                Token = tokenService.CreateToken(user)
            };
        }

        public async Task<UserDto> UserLoginAsync(LoginDto loginDto)
        {
            var newUser = await userManager.FindByEmailAsync(loginDto.Email);
            if (newUser == null) return null;
            var signInResult = await signInManager.CheckPasswordSignInAsync(newUser, loginDto.Password,false);
            if(!signInResult.Succeeded) return null;

            return new UserDto
            {
                Email = newUser.Email,
                Token = tokenService.CreateToken(newUser),
                DiplayName = newUser.DisplayName
            };
        }
    }
}
