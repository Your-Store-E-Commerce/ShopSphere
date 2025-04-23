using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;
using ShopSphere.Data.Entities.Data;
using ShopSphere.Data.Interfaces;
using ShopSphere.Services.Interfaces;

namespace ShopSphere.Web.Controllers
{
    public class ProductsController : Controller
    {
        private readonly IProductsServices _productServices;

        public ProductsController(IProductsServices productServices )
        {
            _productServices = productServices;
        }
        //public async Task<IActionResult> IndexAsync()
        //{
        //  var items= await _productServices.GetProductsAsync();
           
        //    return View(items);
        //}   
        
        public async Task<IActionResult> IndexAsync(int id)
        {
          var items= await _productServices.GetProductByIdAsync(id);
           
            return View(items);
        }


    }
}
