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
                basketId = HttpContext.Session.GetString("BasketId");
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

            // التحقق من تهيئة الخدمة
            if (_productServices == null)
                return StatusCode(500, "Service not available");


            // 1. الحصول على أو إنشاء BasketId
            var basketId = HttpContext.Session.GetString("BasketId") ?? Guid.NewGuid().ToString();
            HttpContext.Session.SetString("BasketId", basketId);

            // 2. جلب المنتج والتحقق من وجوده
            var product = await _productServices.GetProductByIdAsync(productId);
            if (product == null) return NotFound("Product not found");

            // 3. إنشاء BasketItem
            var basketItem = new BasketItem
            {
                Id = product.Id,
                ProductName = product.Name,
                Price = product.Price,
                PictureUrl = product.PictureUrl,
                Brand = product.Brand?.Name ?? string.Empty, // استخدام null-coalescing
                Type = product.Type?.Name ?? string.Empty,   // استخدام null-coalescing
                Quantity = quantity
            };

            // 4. إضافة العنصر للسلة
            var updatedBasket = await _basketService.AddItemToBasketAsync(basketId, basketItem);
            if (updatedBasket == null) return BadRequest("Failed to update basket");

            // 5. إرجاع النتيجة
            return Ok(new
            {
                success = true,
                itemCount = updatedBasket.Items.Sum(i => i.Quantity),
                basketId = updatedBasket.Id
            });


        }
        [HttpPost("UpdateBasketItem")]
        public async Task<IActionResult> UpdateBasket(string basketId, List<BasketItemViewModel> updatedItems)
        {
            if (string.IsNullOrEmpty(basketId) || updatedItems == null || !updatedItems.Any())
                return BadRequest("Invalid request");

            var items = _mapper.Map<List<BasketItem>>(updatedItems);
            var updatedBasket = await _basketService.UpdateBasketAsync(basketId, items);

            if (updatedBasket == null)
                return BadRequest("Failed to update basket");

            return RedirectToAction("Index", new { basketId });
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteBasket(string basketId)
        {
            if (string.IsNullOrEmpty(basketId))
                return BadRequest("Basket ID is required");

            var deleted = await _basketService.DeleteBasketAsync(basketId);
            if (!deleted) return NotFound();

            HttpContext.Session.Remove("BasketId");
            return Ok();
        }

        private string GetOrCreateBasketId()
        {

            var basketId = HttpContext.Session.GetString("BasketId");

            if (string.IsNullOrEmpty(basketId))
            {
                basketId = Guid.NewGuid().ToString();
                HttpContext.Session.SetString("BasketId", basketId);
            }

            return basketId;


        }
    }
}





