using ShopSphere.Data.Entities.Basket;
using ShopSphere.Data.Repositories.Interfaces;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace ShopSphere.Data.Repositories.Implementations
{
    public class BasketRepository :IBasketRepository
    {
        private readonly IDatabase _database;
        public BasketRepository(IConnectionMultiplexer connection)
        {
            _database = connection.GetDatabase();
        }


		public async Task<CustomerBasket?> AddItemBasketAsync(string basketId, BasketItem item)
		{
			// استرجاع السلة من Redis
			var jsonBasket = await _database.StringGetAsync(basketId);
			var basket = JsonSerializer.Deserialize<CustomerBasket>(jsonBasket) ?? new CustomerBasket(basketId);

			// إضافة العنصر إلى السلة
			var existingItem = basket.Items.FirstOrDefault(i => i.Id == item.Id);
			if (existingItem != null)

				// إذا كان العنصر موجودًا، قم بتحديث الكمية
				existingItem.Quantity += item.Quantity;
			
				// إذا لم يكن العنصر موجودًا، أضفه إلى السلة
				basket.Items.Add(item);
			

			// تحديث السلة في Redis
			var updatedBasketJson = JsonSerializer.Serialize(basket);
			var isSaved = await _database.StringSetAsync(basketId, updatedBasketJson, TimeSpan.FromDays(7));

			// إرجاع السلة المحدثة إذا تم الحفظ بنجاح، أو null إذا فشل الحفظ
			return isSaved ? basket : null;
		}


		public async Task<bool> DeleteBasketAsync(string id)
		{
			return await _database.KeyDeleteAsync(id);
		}

		public async Task<CustomerBasket?> GetBasketAsync(string id)
		{
			var jsonBasket = await _database.StringGetAsync(id);
			var basket = JsonSerializer.Deserialize<CustomerBasket>(jsonBasket);
			return jsonBasket.IsNullOrEmpty ? null : basket;
		}

		public async Task<CustomerBasket?> UpdateBasketAsync(string basketId, List<BasketItem> items)
		{
			var jsonBasket = JsonSerializer.Serialize(items);
			var creatOrUpdate = await _database.StringSetAsync(basketId, jsonBasket, TimeSpan.FromDays(7));
			if (creatOrUpdate is false) return null;
			return await GetBasketAsync(basketId);
		}
	}
}

