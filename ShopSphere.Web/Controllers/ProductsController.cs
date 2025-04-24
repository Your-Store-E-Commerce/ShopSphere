using AutoMapper;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;
using ShopSphere.Data.Entities.Data;
using ShopSphere.Data.Interfaces;
using ShopSphere.Data.Specification.ProductSpec;
using ShopSphere.Services.Interfaces;
using ShopSphere.Web.Models.Product;

namespace ShopSphere.Web.Controllers
{
    public class ProductsController : Controller
    {
        private readonly IProductsServices _productServices;
		private readonly IMapper _mapper;

		public ProductsController(IProductsServices productServices ,IMapper mapper)
        {
            _productServices = productServices;
			_mapper = mapper;
		}
        public async Task<IActionResult> Index([FromQuery] ProductSpecParams productSpec)
        {
			
			var products = await _productServices.GetProductsAsync(productSpec);

			var productsVM = _mapper.Map<IReadOnlyList<ProductViewModel>>(products);



			return View(productsVM);
        }

        public async Task<IActionResult> Details(int id)
        {
			var product = await _productServices.GetProductByIdAsync(id);

			var productVM = _mapper.Map<ProductViewModel>(product);

			return View(productVM);
		}


    }
}
