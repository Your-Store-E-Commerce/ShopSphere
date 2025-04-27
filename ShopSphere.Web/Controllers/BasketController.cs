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


        [HttpPost]
        public async Task<IActionResult> RemoveFromBasket(string productId)
        {
            var basketId = HttpContext.Session.GetString("BasketId");

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





