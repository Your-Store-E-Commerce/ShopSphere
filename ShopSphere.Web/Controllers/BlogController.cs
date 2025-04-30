using Microsoft.AspNetCore.Mvc;

namespace ShopSphere.Web.Controllers
{
    public class BlogController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
