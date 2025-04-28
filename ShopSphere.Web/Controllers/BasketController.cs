using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using ShopSphere.Data.Entities.Basket;
using ShopSphere.Services.Interfaces;
using ShopSphere.Web.Models.Basket;

namespace ShopSphere.Web.Controllers
{
    public class BasketController : Controller
    {

        private readonly IMapper _mapper;
        private readonly IBasketServices _basketService;
        private readonly IProductsServices _productServices;


        public BasketController(IBasketServices basketService, IMapper mapper, IProductsServices productsServices)
        {
            _basketService = basketService;
            _mapper = mapper;
            _productServices = productsServices;

        }
        public async Task<IActionResult> Index(string basketId)
        {
            if (string.IsNullOrEmpty(basketId))
            {
                basketId = HttpContext.Request.Cookies["BasketId"];
                if (string.IsNullOrEmpty(basketId))
                    return RedirectToAction("Index", "Home");
            }

            var basket = await _basketService.GetBasketAsync(basketId) ?? new CustomerBasket(basketId);
            var basketVM = _mapper.Map<CustomerBasketViewModel>(basket);

            // حساب المجموع الكلي
            basketVM.Total = basket.Items.Sum(item => item.Price * item.Quantity);

            return View(basketVM);
        }

        [HttpPost("add-item")]
        public async Task<IActionResult> AddItemToBasket(int productId, int quantity)
        {
            if (quantity <= 0)
                return BadRequest("Quantity must be greater than zero");

            if (_productServices == null)
                return StatusCode(500, "Service not available");

            var basketId = Request.Cookies["BasketId"] ?? Guid.NewGuid().ToString();

            var product = await _productServices.GetProductByIdAsync(productId);
            if (product == null) return NotFound("Product not found");

            var basketItem = new BasketItem
            {
                Id = product.Id,
                ProductName = product.Name,
                Price = product.Price,
                PictureUrl = product.PictureUrl,
                Brand = product.Brand?.Name ?? string.Empty,
                Type = product.Type?.Name ?? string.Empty,
                Quantity = quantity
            };

            var updatedBasket = await _basketService.AddItemToBasketAsync(basketId, basketItem);
            if (updatedBasket == null) return BadRequest("Failed to update basket");

            if (!Request.Cookies.ContainsKey("BasketId"))
            {
                Response.Cookies.Append("BasketId", basketId, new CookieOptions
                {
                    Expires = DateTime.Now.AddDays(30),
                    HttpOnly = true,
                    IsEssential = true,
                    SameSite = SameSiteMode.Lax
                });
            }

            // ✨ تحديث كوكي العداد
            Response.Cookies.Append("BasketCount", updatedBasket.Items.Sum(i => i.Quantity).ToString(), new CookieOptions
            {
                Expires = DateTimeOffset.Now.AddDays(30),
                HttpOnly = false,
                IsEssential = true,
                SameSite = SameSiteMode.Lax
            });

            return Ok(new
            {
                success = true,
                itemCount = updatedBasket.Items.Sum(i => i.Quantity),
                basketId = updatedBasket.Id
            });
        }



        [HttpPost]
        public async Task<IActionResult> DeleteBasket(string basketId)
        {
            if (string.IsNullOrEmpty(basketId))
                return BadRequest("Invalid Basket ID");


            var result = await _basketService.DeleteBasketAsync(basketId);

            if (!result)
                return BadRequest("Failed to delete basket");


            return RedirectToAction(nameof(Index));
        }




        [HttpGet]
        public async Task<IActionResult> GetBasket()
        {
            var basketId = Request.Cookies["BasketId"];
            if (string.IsNullOrEmpty(basketId))
            {
                return Ok(new CustomerBasket("empty"));
            }

            var basket = await _basketService.GetBasketAsync(basketId);
            return Ok(basket ?? new CustomerBasket(basketId));
        }


        [HttpPost]
        public async Task<IActionResult> RemoveFromBasket(string productId)
        {
            var basketId = Request.Cookies["BasketId"] ?? Guid.NewGuid().ToString();
            Response.Cookies.Append("BasketId", basketId, new CookieOptions
            {
                Expires = DateTime.Now.AddDays(30),
                HttpOnly = true,
                IsEssential = true,
                SameSite = SameSiteMode.Lax
            });

            if (string.IsNullOrEmpty(basketId))
                return BadRequest("Basket ID is missing");

            var removed = await _basketService.RemoveItemFromBasketAsync(basketId, productId);

            if (!removed)
                return NotFound("Product not found in basket");

            return RedirectToAction(nameof(Index));
        }



        [HttpPost("UpdateBasketItem")]
        public async Task<IActionResult> UpdateBasket(string basketId, int productId, int quantity)
        {
            if (string.IsNullOrEmpty(basketId) || productId <= 0 || quantity <= 0)
                return BadRequest("Invalid request");

            // تحديث الكمية باستخدام الميثود في BasketServices
            var updatedBasket = await _basketService.UpdateItemQuantityAsync(basketId, productId, quantity);

            if (updatedBasket == null)
                return BadRequest("Failed to update basket");

            return RedirectToAction("Index", new { basketId });
        }

    }
}





