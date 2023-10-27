using AutoMapper;
using Microsoft.AspNetCore.Http;
using ShopNet.Common.DTO;
using ShopNet.DAL.Entities;

namespace ShopNet.BLL.MappingProfiles.Resolvers
{
    internal class ProductUrlResolver : IValueResolver<Product, ProductDto, string>
    {
        private IHttpContextAccessor HttpContextAccessor { get; }

        public ProductUrlResolver(IHttpContextAccessor httpContextAccessor)
        {
            HttpContextAccessor = httpContextAccessor;
        }

        public string Resolve(Product source, ProductDto destination, string destMember, ResolutionContext context)
        {
            var host =
                $"{HttpContextAccessor.HttpContext?.Request.Scheme}://{HttpContextAccessor.HttpContext?.Request.Host}";
            if (!string.IsNullOrEmpty(source.PictureUrl))
            {
                return host + "/Content/" + source.PictureUrl;
            }

            return null!;
        }
    }
}