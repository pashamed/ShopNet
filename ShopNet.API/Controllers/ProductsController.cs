using Microsoft.AspNetCore.Mvc;
using ShopNet.BLL.Interfaces;
using ShopNet.BLL.Specifications;
using ShopNet.DAL.Entities;

namespace ShopNet.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly IGenericRepository<Product> _productsRepo;
        private readonly IGenericRepository<ProductType> _typeRepo;
        private readonly IGenericRepository<ProductBrand> _brandRepo;

        public ProductsController(IGenericRepository<Product> productsRepo, IGenericRepository<ProductType> typeRepo,
            IGenericRepository<ProductBrand> brandRepo)
        {
            _productsRepo = productsRepo;
            _typeRepo = typeRepo;
            _brandRepo = brandRepo;
        }

        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<Product>>> GetProducts()
        {
            return Ok(await _productsRepo.ListAsync(new ProductsWithTypesAndBrandsSpecification()));
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<Product>> GetProduct(int id)
        {
            try
            {
                return await _productsRepo.GetEntityWithSpec(new ProductsWithTypesAndBrandsSpecification(id));
            }
            catch (Exception ex)
            {
                return NotFound($"{ex.Message} : {ex.InnerException!.Message}");
            }
        }

        [HttpGet("brands")]
        public async Task<ActionResult<ProductBrand>> GetProductBrands() =>
            Ok(await _brandRepo.ListAllAsync());

        [HttpGet("types")]
        public async Task<ActionResult<ProductType>> GetProductTypes() =>
            Ok(await _typeRepo.ListAllAsync());
    }
}