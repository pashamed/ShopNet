using ShopNet.BLL.Interfaces;
using ShopNet.DAL.Entities;
using StackExchange.Redis;
using System.Text.Json;

namespace ShopNet.BLL.Services
{
    public class BasketRepository : IBasketRepository
    {
        private readonly IProductsRepository _productRepo;
        private readonly IDatabase _database;

        public BasketRepository(IConnectionMultiplexer redis, IProductsRepository productRepo)
        {
            _productRepo = productRepo;
            _database = redis.GetDatabase();
        }

        public async Task<Basket> GetBasketAsync(string id)
        {
            var data = await _database.StringGetAsync(id);
            return data.IsNullOrEmpty ? null : JsonSerializer.Deserialize<Basket>(data);
        }

        public async Task<Basket> UpdateBasketAsync(Basket basket)
        {
            var productTypes = await _productRepo.GetProductTypesAsync();
            var productBrands = await _productRepo.GetProductBrandsAsync();
            foreach (BasketItem basketItem in basket.Items)
            {
                //Get ids from DB
                if (basketItem.Type.Id == 0)
                {
                    basketItem.Type.Id = productTypes.FirstOrDefault(p => p.Name == basketItem.Type.Name).Id;
                }

                if (basketItem.Brand.Id == 0)
                {
                    basketItem.Brand.Id = productBrands.FirstOrDefault(p => p.Name == basketItem.Brand.Name).Id;
                }
            }

            var created =
                await _database.StringSetAsync(basket.Id, JsonSerializer.Serialize(basket), TimeSpan.FromDays(30));
            return await GetBasketAsync(basket.Id) ?? null;
        }

        public async Task<bool> DeleteBasketAsync(string id)
        {
            return await _database.KeyDeleteAsync(id);
        }
    }
}