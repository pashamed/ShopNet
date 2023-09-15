﻿using AutoMapper;
using ShopNet.Common.DTO;
using ShopNet.DAL.Entities;

namespace ShopNet.BLL.MappingProfiles
{
    public class ProductProfile : Profile
    {
        public ProductProfile()
        {
            CreateMap<Product, ProductDto>()
                .ForMember(dest => dest.ProductBrand, opt => opt.MapFrom(src => src.ProductBrand.Name))
                .ForMember(dest => dest.ProductType, opt => opt.MapFrom(src => src.ProductType.Name));
        }
    }
}