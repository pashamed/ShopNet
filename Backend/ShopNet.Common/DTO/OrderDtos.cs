using ShopNet.DAL.Entities.Identity;

namespace ShopNet.Common.DTO
{
    public class OrderDto
    {
        public string BasketId { get; set; }
        public int DeliveryMethodId { get; set; }
        public AddressDto ShipToAddress { get; set; }
    }

    public record OrderWithItemsDto(int Id, string BuyerEmail, DateTime OrderDate, Address ShipToAddress, string DeliveryMethod, decimal ShippingPrice, IReadOnlyList<OrderItemDto> OrderItems, decimal Subtotal, decimal Total, string Status);
    public record OrderItemDto(int ProductId, string ProductName, string PictureUrl, decimal Price, int Quantity);
}
