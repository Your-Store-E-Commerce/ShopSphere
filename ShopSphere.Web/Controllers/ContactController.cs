using Microsoft.AspNetCore.Mvc;

namespace ShopSphere.Web.Controllers
{
	public class ContactController : Controller
	{
		public IActionResult Index()
		{
			return View();
		}
	}
}
