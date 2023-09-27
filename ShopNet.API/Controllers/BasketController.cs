using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ShopNet.BLL.Interfaces;
using ShopNet.DAL.Entities;

namespace ShopNet.API.Controllers
{
    public class BasketController : BaseApiController
    {
        private readonly IBasketRepository _basketRepository;

        public BasketController(IBasketRepository basketRepository)
        {
            _basketRepository = basketRepository;
        }

        [HttpGet]
        public async Task<ActionResult<CustomerBasket>> GetBasketId(string id)
        {
            return Ok(await _basketRepository.GetBasketAsync(id) ?? new CustomerBasket(id));
        }

        [HttpPost]
        public async Task<ActionResult<CustomerBasket>> UpdateBasket(CustomerBasket basket)
        {
            return Ok(await _basketRepository.UpdateBasketAsync(basket));
        }

        [HttpDelete]
        public async Task DeleteBasket(string id)
        {
            await _basketRepository.DeleteBasketAsync(id);
        }
    }
}