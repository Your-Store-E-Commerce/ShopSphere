using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace ShopSphere.Web.Models.Order
{
   



    public class OrderViewModel
    {
        public string FullName { get; set; }
        public string PhoneNumber { get; set; }
        public string ShippingAddress { get; set; }

        public int DeliveryMethodId { get; set; }

        [HiddenInput]
        public string BasketId { get; set; }

        [HiddenInput]
        public string BuyerEmail { get; set; }

        public List<OrderItemViewModel> Items { get; set; } = new();
        public decimal Subtotal { get; set; }
        public decimal Total { get; set; }




		[HiddenInput]
		public string PaymentIntentId { get; set; }

		[HiddenInput]
		public string ClientSecret { get; set; }

		[HiddenInput]
		public string PublishKey { get; set; }

	}

}
