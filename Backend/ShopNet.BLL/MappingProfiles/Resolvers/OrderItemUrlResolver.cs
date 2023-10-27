using AutoMapper;
using Microsoft.AspNetCore.Http;
using ShopNet.Common.DTO;
using ShopNet.DAL.Entities.OrderAggregate;

namespace ShopNet.BLL.MappingProfiles.Resolvers
{
    public class OrderItemUrlResolver : IValueResolver<OrderItem, OrderItemDto, string>
    {
        private IHttpContextAccessor HttpContextAccessor { get; }

        public OrderItemUrlResolver(IHttpContextAccessor httpContextAccessor)
        {
            HttpContextAccessor = httpContextAccessor;
        }

        public string Resolve(OrderItem source, OrderItemDto destination, string destMember, ResolutionContext context)
        {
            var host =
               $"{HttpContextAccessor.HttpContext?.Request.Scheme}://{HttpContextAccessor.HttpContext?.Request.Host}";
            if (!string.IsNullOrEmpty(source.ItemOrdered.PictureUrl))
            {
                return host + "/Content/" + source.ItemOrdered.PictureUrl;
            }

            return null!;
        }
    }
}