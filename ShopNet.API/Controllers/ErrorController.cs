using Microsoft.AspNetCore.Mvc;
using ShopNet.API.Errors;

namespace ShopNet.API.Controllers
{
    [Route("errors/{code:int}")]
    [ApiExplorerSettings(IgnoreApi = true)]
    [ApiController]
    public class ErrorController : BaseApiController
    {
        public IActionResult Error(int code)
        {
            return new ObjectResult(new ApiResponse(code));
        }
    }
}