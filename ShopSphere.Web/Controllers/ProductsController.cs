using AutoMapper;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;
using ShopSphere.Data.Entities.Data;
using ShopSphere.Data.Interfaces;
using ShopSphere.Data.Specification.ProductSpec;
using ShopSphere.Data.UnitOfWork;
using ShopSphere.Services.Implementations;
using ShopSphere.Services.Interfaces;
using ShopSphere.Web.Models.Product;

namespace ShopSphere.Web.Controllers
{
    public class ProductsController : Controller
    {
        private readonly IProductsServices _productServices;
		private readonly IMapper _mapper;
		private readonly IBasketServices _basketServices;

		public ProductsController(IProductsServices productServices ,IMapper mapper,IBasketServices basketServices)
        {
            _productServices = productServices;
			_mapper = mapper;
			_basketServices = basketServices;
		}



		public async Task<IActionResult> Index([FromQuery] ProductSpecParams productSpec)
        {

            var types = await _productServices.GetTypesAsync();

            var products = await _productServices.GetProductsAsync(productSpec);

            foreach (var type in types)
            {
                Console.WriteLine($"Type Id: {type.Id}, Type Name: {type.Name}");
            }


            var productsVM = _mapper.Map<IReadOnlyList<ProductViewModel>>(products);
            var typesVM = _mapper.Map<IReadOnlyList<ProductTypeViewModel>>(types);


            var viewModel = new ShopViewModel
            {
                Products = productsVM,
                Types = typesVM
            };

            ViewBag.CurrentTypeId = productSpec.TypeId;

            return View(viewModel);
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
