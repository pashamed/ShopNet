using Microsoft.AspNetCore.Mvc;
using ShopNet.BLL.Interfaces;
using ShopNet.DAL.Entities;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly IProductsService _productsService;

        public ProductsController(IProductsService productsService)
        {
            _productsService = productsService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Product>>> GetProducts()
        {
            var products = await _productsService.GetAllProdudctsAsync();
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
                return NotFound(string.Format("{0} : {1}", ex.Message, ex.InnerException.Message));
            }
        }
    }
}