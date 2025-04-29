using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using ShopSphere.Services.Implementations;
using ShopSphere.Services.Interfaces;
using ShopSphere.Web.Models;
using ShopSphere.Web.Models.Product;
using System.Diagnostics;

namespace ShopSphere.Web.Controllers
{
    public class HomeController : Controller



    {


		private readonly IProductsServices _productServices;
		private readonly IMapper _mapper;
		private readonly ILogger<HomeController> _logger;


		public HomeController(IProductsServices productServices, IMapper mapper,ILogger<HomeController> logger)
		{
			_productServices = productServices;
			_mapper = mapper;
			_logger = logger;
		}  
		
		public async Task<IActionResult> Index()
		{
			var products = await _productServices.GetAllProductsAsync();
			var productsVM = _mapper.Map<IReadOnlyList<ProductViewModel>>(products);
			return View(productsVM);
		}

		public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
