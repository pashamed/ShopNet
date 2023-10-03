﻿using AutoMapper;
using Microsoft.Data.SqlClient;
using ShopNet.BLL.Interfaces;
using ShopNet.BLL.MappingProfiles.Resolvers;
using ShopNet.Common.DTO;
using ShopNet.Common.DTO.User;
using ShopNet.DAL.Entities;
using ShopNet.DAL.Entities.Identity;

namespace ShopNet.BLL.MappingProfiles
{
    public class MappingProfile : Profile
    {
        public MappingProfile(ITokenService tokenService)
        {
            CreateMap<Product, ProductDto>()
                //.ForMember(dest => dest.ProductBrand, opt => opt.MapFrom(src => src.ProductBrand.Name))
                //.ForMember(dest => dest.ProductType, opt => opt.MapFrom(src => src.ProductType.Name))
                .ForMember(dest => dest.PictureUrl, opt => opt.MapFrom<ProductUrlResolver>());

            CreateMap<Address, AddressDto>().ReverseMap();
            //CreateMap<AppUser, UserDto>();
        }
    }
}