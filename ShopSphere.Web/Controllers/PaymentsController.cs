using Microsoft.AspNetCore.Mvc;
using ShopSphere.Services.Implementations;
using ShopSphere.Services.Interfaces;
using ShopSphere.Web.Models.Order;


namespace ShopSphere.Controllers
{
    public class PaymentsController : Controller
    {
        private readonly IPaymentServices _paymentServices;

        public PaymentsController(IPaymentServices paymentServices)
        {
            _paymentServices = paymentServices;
        }

        // عرض صفحة الدفع
        [HttpGet]
        public async Task<IActionResult> Payment()
        {
            var basketId = Request.Cookies["BasketId"];

            if (string.IsNullOrEmpty(basketId))
            {
                return BadRequest("Basket ID is required.");
            }

            var basket = await _paymentServices.CrateOrUpdatePaymentIntent(basketId);
            if (basket == null)
            {
                return NotFound("Basket not found.");
            }

            var model = new PaymentViewModel
            {
                BasketId = basket.Id,
                PaymentIntentId = basket.PaymentIntentId,
                ClientSecret = basket.ClientSecret
            };

            return View(model);
        }

        // تحديث حالة الدفع بعد التأكد من الدفع
        [HttpPost]
        public async Task<IActionResult> UpdateOrderStatus(string paymentIntentId, bool isPaid)
        {
            if (string.IsNullOrEmpty(paymentIntentId))
            {
                return BadRequest("PaymentIntent ID is required.");
            }

            var order = await _paymentServices.UpdateOrderStatus(paymentIntentId, isPaid);
            if (order == null)
            {
                return NotFound("Order not found.");
            }

            return RedirectToAction("OrderDetails", "Order", new { orderId = order.Id });
        }
    }
}
