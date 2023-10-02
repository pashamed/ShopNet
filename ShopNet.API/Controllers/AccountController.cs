using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ShopNet.DAL.Entities.Identity;
using ShopNet.Common.DTO.User;
using ShopNet.BLL.Interfaces;
using ShopNet.API.Errors;

namespace ShopNet.API.Controllers
{
    public class AccountController : BaseApiController
    {
        private readonly IUserService userService;

        public AccountController(IUserService userService)
        {
            this.userService = userService;
        }

        [HttpPost("login")]
        public async Task<ActionResult<UserDto>> Login(LoginDto loginDto)
        {
            if (loginDto == null) return BadRequest(new ApiResponse(400,loginDto.Email));

            var user = await userService.UserLoginAsync(loginDto);
            if (user == null) return Unauthorized(new ApiResponse(401));

            return Ok(user);
        }

        [HttpPost("register")]
        public async Task<ActionResult<UserDto>> Register(RegisterDto loginDto)
        {
            var newUser = await userService.RegisterAsync(loginDto);
            if (newUser is null) return BadRequest(new ApiResponse(400,"User is already registered"));
            return newUser;
        }
    }
}
