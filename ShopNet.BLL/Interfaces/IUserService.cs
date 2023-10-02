using ShopNet.Common.DTO.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopNet.BLL.Interfaces
{
    public interface IUserService
    {
        public Task<UserDto> UserLogin(LoginDto loginDto);
    }
}
