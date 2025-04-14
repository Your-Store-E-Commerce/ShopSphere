using System.ComponentModel.DataAnnotations;

namespace ShopSphere.Web.Models.Basket
{
    public class BasketItemViewModel
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public string ProductName { get; set; }
        [Required]
        public string PictureUrl { get; set; }
        [Required]
        public string Brand { get; set; }
        [Required]
        public string Type { get; set; }
        [Required]
        [Range(0.1, int.MaxValue, ErrorMessage = "Price must be greater than Zero !!")]
        public decimal Price { get; set; }
        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Quantity must be at least one !!")]
        public int Quantity { get; set; }
    }
}
