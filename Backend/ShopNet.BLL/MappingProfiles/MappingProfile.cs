using AutoMapper;
using ShopNet.BLL.MappingProfiles.Resolvers;
using ShopNet.Common.DTO;
using ShopNet.DAL.Entities;
using ShopNet.DAL.Entities.OrderAggregate;

namespace ShopNet.BLL.MappingProfiles
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Product, ProductDto>()
                //.ForMember(dest => dest.ProductBrand, opt => opt.MapFrom(src => src.ProductBrand.Name))
                //.ForMember(dest => dest.ProductType, opt => opt.MapFrom(src => src.ProductType.Name))
                .ForMember(dest => dest.PictureUrl, opt => opt.MapFrom<ProductUrlResolver>());

            CreateMap<DAL.Entities.Identity.Address, AddressDto>().ReverseMap();
            CreateMap<Basket, BasketDto>().ReverseMap();
            CreateMap<BasketItem, BasketItemDto>().ReverseMap();
            CreateMap<AddressDto, Address>();
            CreateMap<Order, OrderWithItemsDto>();
            CreateMap<OrderItem, OrderItemDto>();
            //CreateMap<AppUser, UserDto>();2`
        }
    }
}