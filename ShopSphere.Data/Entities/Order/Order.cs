using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ShopSphere.Data.Entities.Order
{
    public class Order:BaseEntity
    {
        public Order()
        {

        }
        public Order(string buyerEmail, string shippingAddress, DeliveryMethod? deliveryMethod, ICollection<OrderItem> items, decimal subtotal, string paymentIntentId)
        {
            BuyerEmail = buyerEmail;
            ShippingAddress = shippingAddress;
            DeliveryMethod = deliveryMethod;
            Items = items;
            Subtotal = subtotal;
            PaymentIntentId = paymentIntentId;
        }


        public string BuyerEmail { get; set; } = null!;
        public string ShippingAddress { get; set; } = null!;

        public DeliveryMethod? DeliveryMethod { get; set; } = null!;

        public OrderStatus orderStatus { get; set; } = OrderStatus.Pending;

        public DateTimeOffset OrderDate { get; set; } = DateTimeOffset.UtcNow;
        public ICollection<OrderItem> Items { get; set; } = new HashSet<OrderItem>();

        public decimal Subtotal { get; set; }
        public decimal GetTotal() => Subtotal + DeliveryMethod.Price;
        public string PaymentIntentId { get; set; }
    }
}
