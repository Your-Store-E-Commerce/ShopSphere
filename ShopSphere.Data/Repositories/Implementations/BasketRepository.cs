using Microsoft.Extensions.Logging;
using ShopSphere.Data.Entities.Basket;
using ShopSphere.Data.Repositories.Interfaces;
using StackExchange.Redis;
using System.Text.Json;

namespace ShopSphere.Data.Repositories.Implementations
{
	public class BasketRepository : IBasketRepository
	{
		private readonly IDatabase _database;
		private readonly JsonSerializerOptions _serializerOptions;
	

		public BasketRepository(IConnectionMultiplexer connection)
		{
			_database = connection.GetDatabase();
			_serializerOptions = new JsonSerializerOptions
			{
				PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
				WriteIndented = true
			};
		}


		//public async Task<CustomerBasket?> AddItemBasketAsync(string basketId, BasketItem item)
		//{
		//	// استرجاع السلة من Redis
		//	var jsonBasket = await _database.StringGetAsync(basketId);
		//	var basket = JsonSerializer.Deserialize<CustomerBasket>(jsonBasket) ?? new CustomerBasket(basketId);

		//	// إضافة العنصر إلى السلة
		//	var existingItem = basket.Items.FirstOrDefault(i => i.Id == item.Id);
		//	if (existingItem != null)

		//		// إذا كان العنصر موجودًا، قم بتحديث الكمية
		//		existingItem.Quantity += item.Quantity;

		//		// إذا لم يكن العنصر موجودًا، أضفه إلى السلة
		//		basket.Items.Add(item);


		//	// تحديث السلة في Redis
		//	var updatedBasketJson = JsonSerializer.Serialize(basket);
		//	var isSaved = await _database.StringSetAsync(basketId, updatedBasketJson, TimeSpan.FromDays(7));

		//	// إرجاع السلة المحدثة إذا تم الحفظ بنجاح، أو null إذا فشل الحفظ
		//	return isSaved ? basket : null;
		//}

		//      public async Task<CustomerBasket?> GetBasketAsync(string id)
		//{
		//	var jsonBasket = await _database.StringGetAsync(id);
		//	var basket = JsonSerializer.Deserialize<CustomerBasket>(jsonBasket);
		//	return jsonBasket.IsNullOrEmpty ? null : basket;
		//}

		//      public async Task<CustomerBasket?> UpdateBasketAsync(string basketId, List<BasketItem> items)
		//      {
		//          var basket = new CustomerBasket(basketId) { Items = items };
		//          var jsonBasket = JsonSerializer.Serialize(basket, _serializerOptions);

		//          var isSaved = await _database.StringSetAsync(
		//              basketId,
		//              jsonBasket,
		//              TimeSpan.FromDays(7));

		//          return isSaved ? basket : null;
		//      }

		public async Task<CustomerBasket?> GetBasketAsync(string id)
		{
			if (string.IsNullOrWhiteSpace(id))
				throw new ArgumentNullException(nameof(id));

			var jsonBasket = await _database.StringGetAsync(id);

			if (jsonBasket.IsNullOrEmpty)
				return null;

				return JsonSerializer.Deserialize<CustomerBasket>(jsonBasket, _serializerOptions);
			
			
		}

		public async Task<CustomerBasket?> UpdateBasketAsync(string basketId, List<BasketItem> items)
		{
			if (string.IsNullOrWhiteSpace(basketId))
				throw new ArgumentNullException(nameof(basketId));

			if (items == null)
				throw new ArgumentNullException(nameof(items));

			var basket = new CustomerBasket(basketId) { Items = items };

			var jsonBasket = JsonSerializer.Serialize(basket, _serializerOptions);
			var isSaved = await _database.StringSetAsync(
				basketId,
				jsonBasket,
				TimeSpan.FromDays(7));

			return isSaved ? basket : null;

		}

		public async Task<bool> DeleteBasketAsync(string id)
		{
			return await _database.KeyDeleteAsync(id);
		}
	}
}

