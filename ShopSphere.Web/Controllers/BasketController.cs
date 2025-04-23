using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using ShopSphere.Data.Entities.Basket;
using ShopSphere.Data.Repositories.Interfaces;
using ShopSphere.Services.Interfaces;
using ShopSphere.Web.Models.Basket;

namespace ShopSphere.Web.Controllers
{
	public class BasketController : Controller
	{

		private readonly IMapper _mapper;
		private readonly IBasketServices _basketService;

		public BasketController(IBasketServices basketService)
		{
			_basketService = basketService;
		}
		public async Task<IActionResult> Index(string basketId)
		{
			if (string.IsNullOrEmpty(basketId))
				return BadRequest("Basket ID is required");

			var basket = await _basketService.GetBasketAsync(basketId);

			if (basket == null)
				basket = new CustomerBasket(basketId);

			var basketVM = _mapper.Map< List<BasketItemViewModel>>(basket);
			return View(basketVM);
		}

		public async Task<IActionResult> GetBasket(string basketId)
		{
			var basket = await _basketService.GetBasketAsync(basketId);
			return View(basket ?? new CustomerBasket(basketId));
		}

		[HttpPost("add-item")]
		public async Task<IActionResult> AddItem(string basketId, BasketItemViewModel itemViewModel)
		{
			if (string.IsNullOrEmpty(basketId))
			{
				basketId = Guid.NewGuid().ToString(); 
				HttpContext.Session.SetString("BasketId", basketId);
			}

			var item = _mapper.Map<BasketItem>(itemViewModel);
			var updatedBasket = await _basketService.AddItemToBasketAsync(basketId, item);

			if (updatedBasket == null)
				return BadRequest("Error while adding item");

			return RedirectToAction("Index", new { basketId = updatedBasket.Id });
		}


		[HttpPost]
		public async Task<IActionResult> UpdateBasket(string basketId, List<BasketItemViewModel> updatedItems)
		{
			if (updatedItems == null || !updatedItems.Any())
				return BadRequest("No items to update.");

			var basket = await _basketService.GetBasketAsync(basketId);

			if (basket == null)
				return NotFound("Basket not found");

			foreach (var itemViewModel in updatedItems)
			{
				var item = _mapper.Map<BasketItem>(itemViewModel);
				await _basketService.AddItemToBasketAsync(basketId, item); // تأكد من أنه يتم تحديث العنصر فقط
			}

			return RedirectToAction("Index", new { basketId = basketId });
		}

		[HttpDelete]
		public async Task<IActionResult> DeleteBasket(string basketId)
		{
			var deleted = await _basketService.DeleteBasketAsync(basketId);
			if (!deleted) return NotFound();
			return Ok();
		}



	}
}
