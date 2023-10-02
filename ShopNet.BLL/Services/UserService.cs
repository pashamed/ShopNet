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
        public UserService(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
        }
        public async Task<UserDto> UserLogin(LoginDto loginDto)
        {
            var user = await userManager.FindByEmailAsync(loginDto.Email);
            if (user == null) return null;
            var signInResult = await signInManager.CheckPasswordSignInAsync(user, loginDto.Password,false);
            if(!signInResult.Succeeded) return null;

            return new UserDto
            {
                Email = user.Email,
                Token = "JWT token here",
                DiplayName = user.DisplayName
            };
        }
    }
}
