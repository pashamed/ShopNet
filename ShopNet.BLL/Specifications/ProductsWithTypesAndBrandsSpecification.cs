using ShopNet.BLL.Specifications.Abstract;
using ShopNet.DAL.Entities;

namespace ShopNet.BLL.Specifications
{
    public class ProductsWithTypesAndBrandsSpecification : BaseSpecification<Product>
    {
        public ProductsWithTypesAndBrandsSpecification(ProductSpecParams opt)
            : base(x =>
                (!opt.BrandId.HasValue || x.ProductBrand.Id == opt.BrandId) &&
                (!opt.TypeId.HasValue || x.ProductType.Id == opt.TypeId)
            )
        {
            AddInclude(x => x.ProductType);
            AddInclude(x => x.ProductBrand);
            AddOrderBy(x => x.Name);
            ApplyPaging(opt.PageSize * (opt.PageIndex - 1), opt.PageSize);
            if (string.IsNullOrEmpty(opt.Sort)) return;
            {
                switch (opt.Sort)
                {
                    case "priceAsc":
                        AddOrderBy(p => p.Price);
                        break;
                    case "priceDesc":
                        AddOrderByDescending(p => p.Price);
                        break;
                    default:
                        AddOrderBy(x => x.Name);
                        break;
                }
            }
        }

        public ProductsWithTypesAndBrandsSpecification(int id) : base(x => x.Id == id)
        {
            AddInclude(x => x.ProductType);
            AddInclude(x => x.ProductBrand);
        }
    }
}