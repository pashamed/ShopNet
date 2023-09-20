using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using ShopNet.API.Errors;
using ShopNet.BLL.Interfaces;
using ShopNet.BLL.Services.Helpers;
using ShopNet.BLL.Specifications;
using ShopNet.Common.DTO;
using ShopNet.DAL.Entities;

namespace ShopNet.API.Controllers
{
    public class ProductsController : BaseApiController
    {
        private readonly IGenericRepository<Product> _productsRepo;
        private readonly IGenericRepository<ProductType> _typeRepo;
        private readonly IGenericRepository<ProductBrand> _brandRepo;
        private readonly IMapper _mapper;

        public ProductsController(IGenericRepository<Product> productsRepo, IGenericRepository<ProductType> typeRepo,
            IGenericRepository<ProductBrand> brandRepo, IMapper mapper)
        {
            _productsRepo = productsRepo;
            _typeRepo = typeRepo;
            _brandRepo = brandRepo;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<Pagination<ProductDto>>> GetProducts(
            [FromQuery] ProductSpecParams productParams)
        {
            var spec = new ProductsWithTypesAndBrandsSpecification(productParams);
            var countSpec = new ProductsWithFiltersForCountSpec(productParams);
            var totalItems = await _productsRepo.CountAsync(countSpec);
            var data =
                _mapper.Map<IReadOnlyList<Product>, IReadOnlyList<ProductDto>>(await _productsRepo.ListAsync(spec));

            return Ok(new Pagination<ProductDto>(productParams.PageIndex, productParams.PageSize, totalItems, data));
        }

        [HttpGet("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ProductDto>> GetProduct(int id)
        {
            var product = await _productsRepo.GetEntityWithSpec(new ProductsWithTypesAndBrandsSpecification(id));

            return product is not null
                ? _mapper.Map<Product, ProductDto>(product)
                : NotFound(new ApiResponse(404, "Product not found"));
        }

        [HttpGet("brands")]
        public async Task<ActionResult<ProductBrand>> GetProductBrands() =>
            Ok(await _brandRepo.ListAllAsync());

        [HttpGet("types")]
        public async Task<ActionResult<ProductType>> GetProductTypes() =>
            Ok(await _typeRepo.ListAllAsync());
    }
}