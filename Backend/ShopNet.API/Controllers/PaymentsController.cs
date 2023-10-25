using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShopNet.API.Errors;
using ShopNet.BLL.Interfaces;
using ShopNet.DAL.Entities;

namespace ShopNet.API.Controllers;

public class PaymentsController : BaseApiController
{
    private readonly IPaymentService _paymentService;

    public PaymentsController(IPaymentService paymentService)
    {
        _paymentService = paymentService;
    }

    [Authorize]
    [HttpPost("{basketId}")]
    public async Task<ActionResult<Basket>> CreateOrUpdatePaymentIntent(string basketId)
    {
        var basket = await _paymentService.CreateOrUpdatePaymentIntent(basketId);
        return basket is null ? BadRequest(new ApiResponse(400, "Problem with basket")) : basket;
    }
}