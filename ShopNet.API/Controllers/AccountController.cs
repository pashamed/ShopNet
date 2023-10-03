using Microsoft.AspNetCore.Mvc;
using ShopNet.Common.DTO.User;
using ShopNet.BLL.Interfaces;
using ShopNet.API.Errors;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using ShopNet.DAL.Entities.Identity;
using AutoMapper;
using ShopNet.Common.DTO;

namespace ShopNet.API.Controllers
{
    public class AccountController : BaseApiController
    {
        private readonly IUserService userService;
        private readonly IMapper mapper;

        public AccountController(IUserService userService, IMapper mapper)
        {
            this.userService = userService;
            this.mapper = mapper;
        }

        [HttpGet("emailexists")]
        public async Task<ActionResult<bool>> CheckEmailExists([FromQuery] string email)
        {
            return await userService.GetCurrentUserAsync(email) != null;
        }

        [Authorize]
        [HttpGet("address")]
        public async Task<ActionResult<AddressDto>> GetUserAddress()
        {
            var user = await userService.GetCurrentUserAddressAsync(User.FindFirstValue(ClaimTypes.Email));
            return mapper.Map<Address,AddressDto>(user.Address);
        }

        [Authorize]
        [HttpPut("address")]
        public async Task<ActionResult<AddressDto>> UpdateUserAddress(AddressDto address)
        {
            var add = await userService.UpdateCurrentUserAddressAsync(address, User.FindFirstValue(ClaimTypes.Email));
            return add is not null ? Ok(mapper.Map<Address, AddressDto>(add)) :
                BadRequest("Problem with updating Address");
        }

        [Authorize]
        [HttpGet]
        public async Task<ActionResult<UserDto>> GetCurrentUser()
        {
            return await userService.GetCurrentUserAsync(User.FindFirstValue(ClaimTypes.Email));
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
