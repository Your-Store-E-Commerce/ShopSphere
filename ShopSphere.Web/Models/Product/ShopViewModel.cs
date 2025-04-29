namespace ShopSphere.Web.Models.Product
{
    public class ShopViewModel
    {
       
            public IReadOnlyList<ProductViewModel> Products { get; set; } = new List<ProductViewModel>();
            public IReadOnlyList<ProductTypeViewModel> Types { get; set; } = new List<ProductTypeViewModel>();
      
    }
}
