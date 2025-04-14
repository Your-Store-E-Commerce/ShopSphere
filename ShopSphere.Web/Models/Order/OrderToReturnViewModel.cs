using System.Net;

namespace ShopSphere.Web.Models.Order
{
    public class OrderToReturnViewModel
    {
        public int Id { get; set; }
        public string BuyerEmail { get; set; }
        public string ShippingAddress { get; set; }

        public string DeliveryMethod { get; set; }
        public string DeliveryMethodCost { get; set; }

        public string orderStatus { get; set; }

        public DateTimeOffset OrderDate { get; set; }
        public ICollection<OrderItemViewModel> Items { get; set; }

        public decimal Subtotal { get; set; }
        public decimal Total { get; set; }
        public string PaymentIntentId { get; set; } = string.Empty;
    }
}
