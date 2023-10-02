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
            if (loginDto == null) return BadRequest(loginDto.Email);

            var user = await userService.UserLogin(loginDto);
            if (user == null) Unauthorized(new ApiResponse(401));

            return Ok(user);
        }
    }
}
