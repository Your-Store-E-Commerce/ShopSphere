using Microsoft.AspNetCore.Mvc;

namespace ShopSphere.Web.Controllers
{
    public class BasketController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
