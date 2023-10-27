using Microsoft.AspNetCore.Mvc;
using ShopNet.API.Errors;

namespace ShopNet.API.Controllers
{
    [ApiExplorerSettings(IgnoreApi = true)]
    public class ErrorController : BaseApiController
    {
        public IActionResult Error(int code)
        {
            return new ObjectResult(new ApiResponse(code));
        }
    }
}