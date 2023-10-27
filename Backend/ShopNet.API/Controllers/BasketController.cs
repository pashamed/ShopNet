using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using ShopNet.BLL.Interfaces;
using ShopNet.Common.DTO;
using ShopNet.DAL.Entities;

namespace ShopNet.API.Controllers
{
    public class BasketController : BaseApiController
    {
        private readonly IBasketRepository _basketRepository;
        private readonly IMapper mapper;

        public BasketController(IBasketRepository basketRepository, IMapper mapper)
        {
            _basketRepository = basketRepository;
            this.mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<Basket>> GetBasketId(string id)
        {
            return Ok(await _basketRepository.GetBasketAsync(id) ?? new Basket(id));
        }

        [HttpPost]
        public async Task<ActionResult<Basket>> UpdateBasket(BasketDto basket)
        {
            return Ok(await _basketRepository.UpdateBasketAsync(mapper.Map<BasketDto, Basket>(basket)));
        }

        [HttpDelete]
        public async Task DeleteBasket(string id)
        {
            await _basketRepository.DeleteBasketAsync(id);
        }
    }
}