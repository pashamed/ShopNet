using ShopNet.BLL.Specifications.Abstract;
using ShopNet.DAL.Entities.OrderAggregate;

namespace ShopNet.BLL.Specifications
{
    public class OrderByPaymentIntentIdSpecification : BaseSpecification<Order>
    {
        public OrderByPaymentIntentIdSpecification(string paymentIntentId) : base(o => o.PaymentIntentId == paymentIntentId)
        {
        }
    }
}