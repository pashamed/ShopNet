using ShopNet.BLL.Specifications.Abstract;
using ShopNet.DAL.Entities;

namespace ShopNet.BLL.Specifications
{
    public class ProductsWithFiltersForCountSpec : BaseSpecification<Product>
    {
        public ProductsWithFiltersForCountSpec(ProductSpecParams opt)
            : base(x =>
                (!opt.BrandId.HasValue || x.ProductBrand.Id == opt.BrandId) &&
                (!opt.TypeId.HasValue || x.ProductType.Id == opt.TypeId)
            )
        {
        }
    }
}