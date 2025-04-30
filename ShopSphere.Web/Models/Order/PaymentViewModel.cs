namespace ShopSphere.Web.Models.Order
{
    public class PaymentViewModel
    {
        public string BasketId { get; set; } = null!;
        public string PaymentIntentId { get; set; } = null!;    
        public string ClientSecret { get; set; } = null!;   

    }
}
