using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;
using ShopSphere.Data.Entities.Order;
using ShopSphere.Services.Implementations;
using ShopSphere.Services.Interfaces;
using ShopSphere.Web.Models.Order;
using System.Net;
using System.Security.Claims;

namespace ShopSphere.Web.Controllers
{
    [Route("Orders/[action]")]
    public class OrdersController : Controller
    {
        private readonly IOrderServices _orderServices;
        private readonly IMapper _mapper;
        private readonly IBasketServices _basketServices;

        [BindProperty]
        public OrderViewModel OrderVM { get; set; }

        public OrdersController(IOrderServices orderServices, IMapper mapper,IBasketServices basketServices)
        {
            _orderServices = orderServices;
            _mapper = mapper;
           _basketServices = basketServices;
        }

		public IActionResult Index()
		{
			return View();
		}




        [HttpGet]
        public async Task<IActionResult> Checkout()
        {
            var basketId = Request.Cookies["BasketId"];
            var basket = await _basketServices.GetBasketAsync(basketId);

            if (basket == null)
                return RedirectToAction("Index", "Basket");

            var deliveryMethods = await _orderServices.GetDeliveryMethod();
            ViewBag.DeliveryMethods = deliveryMethods;

            var items = basket.Items.Select(i => new OrderItemViewModel
            {
                Id = i.Id,
                ProductId = i.Id,
                ProductName = i.ProductName,
                ProductPictureUrl = i.PictureUrl,
                Price = i.Price,
                Quantity = i.Quantity
            }).ToList();

            var subtotal = items.Sum(x => x.Price * x.Quantity);

            var orderVM = new OrderViewModel
            {
                BuyerEmail = "testuser@example.com",
                BasketId = basketId,
                Items = items,
                Subtotal = subtotal,
                Total = subtotal 
            };

            return View(orderVM);
        }





        // POST: /Orders/CreateOrder
        [HttpPost]
        
        public async Task<IActionResult> CreateOrder()
        {
            //var email = User.FindFirstValue(ClaimTypes.Email);

            if (!ModelState.IsValid)
            {
                var deliveryMethods = await _orderServices.GetDeliveryMethod();
                ViewBag.DeliveryMethods = deliveryMethods;
                return View("Checkout", OrderVM);
            }

            var createdOrder = await _orderServices.CreateOrderAsync(OrderVM.BasketId, OrderVM.DeliveryMethodId,  OrderVM.BuyerEmail, OrderVM.ShippingAddress);

            if (createdOrder is null)
                return BadRequest("Problem creating order");

            var mappedOrder = _mapper.Map<Order, OrderToReturnViewModel>(createdOrder);

            // Redirect to Payment page after creating the order
            return RedirectToAction("Payment", "Payments", new { paymentIntentId = mappedOrder.PaymentIntentId });
        }

        // GET: /Orders/Details/{id}
        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var email = User.FindFirstValue(ClaimTypes.Email);
            var orders = await _orderServices.GetOrderForUser(email);

            var orderToShow = orders.FirstOrDefault(o => o.Id == id);

            if (orderToShow == null)
                return NotFound();

            var mappedOrder = _mapper.Map<Order, OrderToReturnViewModel>(orderToShow);

            return View(mappedOrder);
        }

        // GET: /Orders/UserOrders
        [HttpGet]
        public async Task<IActionResult> UserOrders()
        {
            var email = User.FindFirstValue(ClaimTypes.Email);
            var orders = await _orderServices.GetOrderForUser(email);

            var mappedOrders = _mapper.Map<IReadOnlyList<Order>, List<OrderToReturnViewModel>>(orders);

            return View(mappedOrders);
        }

        // GET: /Orders/GetDeliveryMethods
        [HttpGet]
        public async Task<IActionResult> GetDeliveryMethods()
        {
            var result = await _orderServices.GetDeliveryMethod();
            return Json(result); // Useful if you load delivery methods dynamically in checkout
        }
    }


}
