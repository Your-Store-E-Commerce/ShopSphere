using Microsoft.AspNetCore.Mvc;

namespace ShopSphere.Web.Controllers
{
    public class OrdersController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
