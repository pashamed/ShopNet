﻿using System.ComponentModel.DataAnnotations;

namespace ShopNet.DAL.Entities.OrderAggregate
{
    public class Order : BaseEntity
    {
        public Order()
        {
        }

        public Order(IReadOnlyList<OrderItem> orderItems, string buyerEmail, Address shipToAddress, DeliveryMethod deliveryMethod, decimal subtotal, string paymentIntentId)
        {
            BuyerEmail = buyerEmail;
            ShipToAddress = shipToAddress;
            DeliveryMethod = deliveryMethod;
            OrderItems = orderItems;
            Subtotal = subtotal;
            this.PaymentIntentId = paymentIntentId;
        }

        public string BuyerEmail { get; set; } = string.Empty;
        public DateTime OrderDate { get; set; } = DateTime.UtcNow;

        [Required]
        public Address ShipToAddress { get; set; }

        public DeliveryMethod DeliveryMethod { get; set; }
        public IReadOnlyList<OrderItem> OrderItems { get; set; }
        public decimal Subtotal { get; set; }
        public OrderStatus Status { get; set; } = OrderStatus.Pending;

        //payment sys id
        public string PaymentIntentId { get; set; } = string.Empty;

        public decimal GetTotal()
        {
            return Subtotal + DeliveryMethod.Price;
        }
    }
}