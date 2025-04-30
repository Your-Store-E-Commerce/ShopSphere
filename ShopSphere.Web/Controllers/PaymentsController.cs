using Microsoft.AspNetCore.Mvc;
using ShopSphere.Data.Entities.Order;
using ShopSphere.Services.Implementations;
using ShopSphere.Services.Interfaces;
using ShopSphere.Web.Models.Order;
using Stripe.Climate;


namespace ShopSphere.Controllers
{
    [Route("Payments/[action]")]
    public class PaymentsController : Controller
    {
        private readonly IPaymentServices _paymentServices;



        private readonly IConfiguration _configuration;
        private readonly IOrderServices _orderServices;
        private readonly IBasketServices _basketServices;

        public PaymentsController(IPaymentServices paymentServices, IConfiguration configuration, IOrderServices orderServices ,IBasketServices basketServices)
        {
            _paymentServices = paymentServices;
            _configuration = configuration;
            _orderServices = orderServices;
            _basketServices = basketServices;
        }



        [HttpGet]
        public async Task<IActionResult> Payment(string shippingAddress)
        {

            var basketId = Request.Cookies["BasketId"];

            if (string.IsNullOrEmpty(basketId))
                return BadRequest("Basket ID is required.");

            var basket = await _paymentServices.CrateOrUpdatePaymentIntent(basketId);
            if (basket == null)
                return NotFound("Basket not found.");

            var deliveryMethods = await _orderServices.GetDeliveryMethod();
            ViewBag.DeliveryMethods = deliveryMethods;

            var selectedDeliveryMethod = deliveryMethods.FirstOrDefault();
            var shippingPrice = selectedDeliveryMethod?.Price ?? 0;

            var model = new PaymentViewModel
            {
                BasketId = basket.Id,
              
                PaymentIntentId = basket.PaymentIntentId,
                ClientSecret = basket.ClientSecret,
                PublishKey = "",
                CardNumber = string.Empty,
                ExpiryDate = string.Empty,
                CVC = string.Empty,
                CardholderName = string.Empty,
                BuyerEmail = string.Empty,
                ShippingAddress = shippingAddress


            };

            return View(model);
        } 
  
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

            return RedirectToAction("OrderDetails", "Orders", new { orderId = order.Id });
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ConfirmPayment(PaymentViewModel paymentModel)
        {
            if (string.IsNullOrEmpty(paymentModel.BasketId))
            {
                TempData["Error"] = "رقم السلة غير موجود.";
                return RedirectToAction("Checkout");
            }

            var paymentIntent = await _paymentServices.CrateOrUpdatePaymentIntent(paymentModel.BasketId);

            if (paymentIntent == null)
            {
                TempData["Error"] = "فشل في إنشاء عملية الدفع.";
                return RedirectToAction("Checkout");
            }
            var deliveryMethods = await _orderServices.GetDeliveryMethod();
            var order = await _orderServices.CreateOrderAsync(paymentModel.BasketId  , paymentModel.DeliveryMethodId, paymentModel.BuyerEmail, paymentModel.ShippingAddress);

            if (order == null)
            {
                TempData["Error"] = "حدث خطأ أثناء إنشاء الطلب.";
                return RedirectToAction("Checkout");
            }

            await _basketServices.DeleteBasketAsync(paymentModel.BasketId);
            Response.Cookies.Append("BasketCount", "0", new CookieOptions
            {
                Expires = DateTimeOffset.Now.AddDays(30),
                HttpOnly = false,
                IsEssential = true,
                SameSite = SameSiteMode.Lax
            });

            return RedirectToAction("Success", new { orderId = order.Id });
        }




        [HttpGet]
        public async Task<IActionResult> Success(int orderId)
        {
            return View();
        }



    }
}