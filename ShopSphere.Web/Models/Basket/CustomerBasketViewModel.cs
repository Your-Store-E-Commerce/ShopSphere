using System.ComponentModel.DataAnnotations;

namespace ShopSphere.Web.Models.Basket
{
    public class CustomerBasketViewModel
    {
        [Required]
        public string Id { get; set; } = null!;
        public List<BasketItemViewModel> Items { get; set; }
		public decimal Total { get; set; }
	}
}
