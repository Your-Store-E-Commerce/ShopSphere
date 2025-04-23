using ShopSphere.Data.Entities.Basket;
using ShopSphere.Data.Repositories.Interfaces;
using ShopSphere.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopSphere.Services.Implementations
{
	public class BasketServices : IBasketServices
	{

		private readonly IBasketRepository _basketRepo;

		public BasketServices(IBasketRepository basketRepo)
		{
			_basketRepo = basketRepo;
		}

		public async Task<CustomerBasket?> GetBasketAsync(string basketId)
		{
			return await _basketRepo.GetBasketAsync(basketId);
		}

		public async Task<CustomerBasket?> UpdateBasketAsync(string basketId, List<BasketItem> items)
		{
			return await _basketRepo.UpdateBasketAsync(basketId ,items);
		}

		public async Task<bool> DeleteBasketAsync(string basketId)
		{
			return await _basketRepo.DeleteBasketAsync(basketId);
		}

		public async Task<CustomerBasket> AddItemToBasketAsync(string basketId, BasketItem item)
		{
			var basket = await _basketRepo.GetBasketAsync(basketId) ?? new CustomerBasket(basketId);

			var existingItem = basket.Items.FirstOrDefault(i => i.Id == item.Id);
			if (existingItem != null)
			
				existingItem.Quantity += item.Quantity;
			
		
				basket.Items.Add(item);
			

			return await _basketRepo.AddItemBasketAsync(basketId ,item);
		}


	}
}
