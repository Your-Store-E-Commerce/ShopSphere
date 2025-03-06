using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;
using ShopSphere.Data.Entities.Data;
using ShopSphere.Data.Interfaces;

namespace ShopSphere.Web.Controllers
{
    public class ProductsController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public ProductsController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<IActionResult> IndexAsync()
        {
          var items= await _unitOfWork.Repository<Product>().GetAllAsync();
           
            return View(items);
        }
    }
}
