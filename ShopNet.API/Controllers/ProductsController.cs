using Microsoft.AspNetCore.Mvc;
using ShopNet.BLL.Interfaces;
using ShopNet.DAL.Entities;

namespace ShopNet.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly IProductsRepository _productsService;

        public ProductsController(IProductsRepository productsService)
        {
            _productsService = productsService;
        }

        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<Product>>> GetProducts()
        {
            var products = await _productsService.GetAllProductsAsync();
            return products == null ? NotFound("Error") : Ok(products);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Product>> GetProduct(int id)
        {
            try
            {
                return await _productsService.GetProductByIdAsync(id);
            }
            catch (Exception ex)
            {
                return NotFound($"{ex.Message} : {ex.InnerException!.Message}");
            }
        }
    }
}