﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShopNet.API.Errors;
using ShopNet.BLL.Interfaces;
using ShopNet.DAL.Entities;
using Stripe;
using Order = ShopNet.DAL.Entities.OrderAggregate.Order;

namespace ShopNet.API.Controllers;

public class PaymentsController : BaseApiController
{
    private readonly string endpointSecret;
    private readonly IPaymentService _paymentService;
    private readonly ILogger<PaymentsController> logger;

    public PaymentsController(IPaymentService paymentService, ILogger<PaymentsController> logger, IConfiguration config)
    {
        _paymentService = paymentService;
        this.logger = logger;
        endpointSecret = config.GetSection("StripeSettings:WhSecret").Value;
    }

    [Authorize]
    [HttpPost("{basketId}")]
    public async Task<ActionResult<Basket>> CreateOrUpdatePaymentIntent(string basketId)
    {
        var basket = await _paymentService.CreateOrUpdatePaymentIntent(basketId);
        return basket is null ? BadRequest(new ApiResponse(400, "Problem with basket")) : basket;
    }

    [HttpPost("webhook")]
    public async Task<IActionResult> StripeWebhook()
    {
        var json = await new StreamReader(HttpContext.Request.Body).ReadToEndAsync();
        try
        {
            var stripeEvent = EventUtility.ConstructEvent(json,
                Request.Headers["Stripe-Signature"], endpointSecret);
            PaymentIntent intent;
            Order order;
            // Handle the event
            if (stripeEvent.Type == Events.PaymentIntentPaymentFailed)
            {
                intent = stripeEvent.Data.Object as PaymentIntent;
                logger.LogInformation("Payment failed", intent.Id);
                order = await _paymentService.UpdateOrderPaymentFailed(intent.Id);
                logger.LogInformation($"Payment failed: {intent.Id}");
            }
            else if (stripeEvent.Type == Events.PaymentIntentSucceeded)
            {
                intent = stripeEvent.Data.Object as PaymentIntent;
                logger.LogInformation("Payment succeeded", intent.Id);
                order = await _paymentService.UpdateOrderPaymentSucceeded(intent.Id);
                logger.LogInformation($"Payment succeeded: {intent.Id}");
            }
            // ... handle other event types
            else
            {
                logger.LogError("Unhandled event type: {0}", stripeEvent.Type);
            }

            return Ok();
        }
        catch (StripeException e)
        {
            logger.LogError($"Error: {e.Message}");
            return BadRequest();
        }
    }
}