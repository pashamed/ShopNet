using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ShopNet.API.Controllers
{
    public class FallBackController : Controller
    {
        public IActionResult Index()
        {
            return PhysicalFile(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "index.html"), "text/html");
        }
    }
}
