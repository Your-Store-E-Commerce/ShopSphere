using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using ShopSphere.Data.Entities.Order;
using ShopSphere.Services.Interfaces;
using ShopSphere.Web.Models.Order;
using System.Net;
using System.Security.Claims;

namespace ShopSphere.Web.Controllers
{
    public class OrdersController : Controller
    {

        private readonly IOrderServices _order;
        private readonly IMapper _mapper;


        public OrdersController(IOrderServices order, IMapper mapper)
        {
            _order = order;
            _mapper = mapper;
        }

        [HttpPost]
        public async Task<IActionResult> CreateOrder(OrderViewModel orderVM)
        {
            var email = User.FindFirstValue(ClaimTypes.Email);

            var order = await _order.CreateOrderAsync(orderVM.BasketId, orderVM.DeliveryMethodId, email, orderVM.ShippingAddress);

            if (order is null)
                return BadRequest();

            return View(_mapper.Map<Order, OrderToReturnViewModel>(order));
        }


        [HttpGet]
        public async Task<IActionResult> GetOrderForUser()
        {
            var email = User.FindFirstValue(ClaimTypes.Email);
            var orders = await _order.GetOrderForUser(email);
            return View(_mapper.Map<IReadOnlyList<Order>, OrderToReturnViewModel>(orders));
        }





        ////[HttpGet("{id}")]
        //public async Task<ActionResult<OrderToReturnViewModel>> GetOrderForUserById(int orderId)
        //{
        //    var email = User.FindFirstValue(ClaimTypes.Email);
        //    var order = await _order.GetOrderForUserById(orderId, email);
        //    return View(_mapper.Map<Order, OrderToReturnViewModel>(order));
        //}

        [HttpGet("deliveryMethods")]
        public async Task<IActionResult> GetDeliveryMethods()
        {

            var result = await _order.GetDeliveryMethod();

            return View(result);
        }
    }
}
