namespace ShopSphere.Web.Models.Product
{
	public class ProductViewModel
	{
		
			public int Id { get; set; }
			public string Name { get; set; } = null!;
			public string Description { get; set; } = null!;
			public string PictureUrl { get; set; } = null!;
			public decimal Price { get; set; }
			public int TypeId { get; set; }
			public int BrandId { get; set; }

			public string TypeName { get; set; } = null!;
			public string BrandName { get; set; } = null!;

		}
	}

