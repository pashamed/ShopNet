﻿using Microsoft.EntityFrameworkCore;
using ShopNet.BLL.Interfaces;
using ShopNet.BLL.Services.Abstract;
using ShopNet.DAL.Data;
using ShopNet.DAL.Entities;

namespace ShopNet.BLL.Services
{
    public class ProductsService : BaseService, IProductsRepository
    {
        public ProductsService(StoreContext context) : base(context)
        {
        }

        public async Task<IReadOnlyList<Product>> GetAllProductsAsync()
        {
            return await _context.Products.Include(p => p.ProductType)
                .Include(p => p.ProductBrand)
                .ToListAsync();
        }

        public async Task<Product> GetProductByIdAsync(int id)
        {
            return await _context.Products.Where(p => p.Id == id)
                .Include(p => p.ProductType)
                .Include(p => p.ProductBrand)
                .FirstOrDefaultAsync();
        }

        public async Task<IReadOnlyList<ProductType>> GetProductTypesAsync()
        {
            return await _context.ProductTypes.ToListAsync();
        }

        public async Task<IReadOnlyList<ProductBrand>> GetProductBrandsAsync()
        {
            return await _context.ProductBrands.ToListAsync();
        }
    }
}