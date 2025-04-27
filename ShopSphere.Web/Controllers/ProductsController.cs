using AutoMapper;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;
using ShopSphere.Data.Entities.Data;
using ShopSphere.Data.Interfaces;
using ShopSphere.Data.Specification.ProductSpec;
using ShopSphere.Data.UnitOfWork;
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

            ViewBag.CurrentTypeId = productSpec.TypeId;

            return View(productsVM);
        }

		public async Task<IActionResult> Details(int? id)
		{

			if (id == null)
				return BadRequest();//status code 400

			var product = await _productServices.GetProductByIdAsync(id.Value);

			var productVM = _mapper.Map<ProductViewModel>(product);

			if (productVM == null)
				return NotFound();
			return View(productVM);

		}





	}
}
