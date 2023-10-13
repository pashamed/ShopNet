using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ShopNet.API.Errors;
using ShopNet.BLL.Interfaces;
using ShopNet.Common.DTO;
using ShopNet.DAL.Entities.OrderAggregate;
using System.Security.Claims;

namespace ShopNet.API.Controllers
{
    [Authorize]
    public class OrdersController : BaseApiController
    {
        private readonly IOrderService orderService;
        private readonly IMapper mapper;

        public OrdersController(IOrderService orderService, IMapper mapper)
        {
            this.orderService = orderService;
            this.mapper = mapper;
        }

        [HttpPost]
        public async Task<ActionResult<Order>> CreateOrder(OrderDto orderDto)
        {
            var order = await orderService.CreateOrderAsync(HttpContext.User.FindFirstValue(ClaimTypes.Email),
                orderDto.DeliveryMethodId,
                orderDto.BasketId,
                mapper.Map<AddressDto, Address>(orderDto.ShipToAddress));
            return order is null ? BadRequest(new ApiResponse(400, "Problem creating order")) : Ok(order);
        }
    }
}
