using Microsoft.AspNetCore.Mvc;

namespace ShopSphere.Web.Controllers
{
    public class PaymentController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
