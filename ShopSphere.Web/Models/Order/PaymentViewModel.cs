using Microsoft.AspNetCore.Mvc;

namespace ShopSphere.Web.Models.Order
{
    public class PaymentViewModel
    {
        public string BasketId { get; set; }
        [HiddenInput]
        public int DeliveryMethodId { get; set; }
    
        public string ShippingAddress { get; set; }
        public string ClientSecret { get; set; }
        public string PublishKey { get; set; }
        public string PaymentIntentId { get; set; }
        public string BuyerEmail { get; set; }
        // معلومات البطاقة
        public string CardNumber { get; set; } = null!;
        public string ExpiryDate { get; set; } = null!;
        public string CVC { get; set; } = null!;
        public string CardholderName { get; set; } = null!;
        public string Country { get; set; } = "Egypt";

    }
}
