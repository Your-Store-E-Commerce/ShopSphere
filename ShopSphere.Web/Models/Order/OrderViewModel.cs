using System.ComponentModel.DataAnnotations;

namespace ShopSphere.Web.Models.Order
{
    public class OrderViewModel
    {
        [Required]
        public string BuyerEmail { get; set; }
        [Required]
        public string BasketId { get; set; }
        public string ShippingAddress { get; set; }
        [Required]
        public int DeliveryMethodId { get; set; }

    }
}
