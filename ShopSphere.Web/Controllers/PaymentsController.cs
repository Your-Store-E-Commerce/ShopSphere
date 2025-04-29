using Microsoft.AspNetCore.Mvc;
using ShopSphere.Services.Implementations;
using ShopSphere.Services.Interfaces;
using ShopSphere.Web.Models.Order;


namespace ShopSphere.Controllers
{
    public class PaymentsController : Controller
    {                                                                                 
        private readonly IPaymentServices _paymentServices;



		private readonly IConfiguration _configuration;

		public PaymentsController(IPaymentServices paymentServices, IConfiguration configuration)
		{
			_paymentServices = paymentServices;
			_configuration = configuration;
		}




		[HttpGet]
		public async Task<IActionResult> Payment()
		{
			var basketId = Request.Cookies["BasketId"];

			if (string.IsNullOrEmpty(basketId))
				return BadRequest("Basket ID is required.");

			var basket = await _paymentServices.CrateOrUpdatePaymentIntent(basketId);
			if (basket == null)
				return NotFound("Basket not found.");

			var model = new OrderViewModel
			{
				BasketId = basket.Id,
				PaymentIntentId = basket.PaymentIntentId,
				ClientSecret = basket.ClientSecret,
				PublishKey = _configuration["StripeSetting:PublishKey"],

				// ممكن تضيفي باقي بيانات المستخدم من سلة الشراء أو من الـ session أو auth
				FullName = "Test Name",
				PhoneNumber = "1234567890",
				ShippingAddress = "Test Address",
				BuyerEmail = "test@example.com",
				Items = basket.Items.Select(i => new OrderItemViewModel
				{
					ProductName = i.ProductName,
					Quantity = i.Quantity,
					Price = i.Price
				}).ToList(),
				Subtotal = basket.Items.Sum(i => i.Quantity * i.Price),
				Total = basket.Items.Sum(i => i.Quantity * i.Price) // + Shipping لو فيه
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

            return RedirectToAction("OrderDetails", "Order", new { orderId = order.Id });
        }
    }
}
