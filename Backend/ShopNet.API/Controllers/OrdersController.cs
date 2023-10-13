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

        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<OrderWithItemsDto>>> GetOrdersForUser()
        {
            var orders = await orderService.GetOrdersForUserAsync(User.FindFirstValue(ClaimTypes.Email));
            return Ok(mapper.Map<IReadOnlyList<OrderWithItemsDto>>(orders));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<OrderWithItemsDto>> GetOrderByIdForUser(int id)
        {
            var order = await orderService.GetOrderByIdAsync(id, User.FindFirstValue(ClaimTypes.Email));
            return order is null ? NotFound(new ApiResponse(404)) : mapper.Map<OrderWithItemsDto>(order);
        }

        [HttpGet("deliveryMethods")]
        public async Task<ActionResult<IReadOnlyList<Order>>> GetDeliveryMethods()
        {
            return Ok(await orderService.GetDeliveryMethodsAsync());
        }
    }
}
