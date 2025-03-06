using Microsoft.AspNetCore.Mvc;

namespace ShopSphere.Web.Controllers
{
    public class AccountController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
