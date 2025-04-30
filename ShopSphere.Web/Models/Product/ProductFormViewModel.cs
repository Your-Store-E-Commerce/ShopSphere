using Microsoft.AspNetCore.Mvc.Rendering;

namespace ShopSphere.Web.Models.Product
{
	public class ProductFormViewModel
	{
		public int Id { get; set; }
		public string Name { get; set; } = null!;
		public string Description { get; set; } = null!;
		public decimal Price { get; set; }
		public IFormFile? ImageFile { get; set; }

		public int BrandId { get; set; }
		public int CategoryId { get; set; }

		// SelectList for dropdowns
		public SelectList Brands { get; set; } = null!;
		public SelectList Types { get; set; } = null!;
	}
}
