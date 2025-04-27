using ShopSphere.Data.Entities.Basket;
using ShopSphere.Data.Repositories.Interfaces;
using ShopSphere.Services.Interfaces;

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
            var basket = await _basketRepo.GetBasketAsync(basketId) ?? new CustomerBasket(basketId);
            basket.Items = items;
            return await _basketRepo.UpdateBasketAsync(basketId, basket.Items);
        }

        public async Task<bool> DeleteBasketAsync(string basketId)
        {
            return await _basketRepo.DeleteBasketAsync(basketId);
        }

		//public async Task<CustomerBasket> AddItemToBasketAsync(string basketId, BasketItem item)
		//{
		//    var basket = await _basketRepo.GetBasketAsync(basketId) ?? new CustomerBasket(basketId);

		//    var existingItem = basket.Items.FirstOrDefault(i => i.Id == item.Id);
		//    if (existingItem != null)

		//        existingItem.Quantity += item.Quantity;

		//    else

		//        basket.Items.Add(item);


		//    return await _basketRepo.UpdateBasketAsync(basketId, basket.Items) ?? basket;
		//}

		public async Task<CustomerBasket> AddItemToBasketAsync(string basketId, BasketItem item)
		{
			if (item == null || item.Quantity <= 0)
				throw new ArgumentException("Invalid basket item");

			// الحصول على السلة أو إنشاء جديدة
			var basket = await _basketRepo.GetBasketAsync(basketId) ?? new CustomerBasket(basketId);

			// البحث عن العنصر الموجود
			var existingItem = basket.Items.FirstOrDefault(i => i.Id == item.Id);

			if (existingItem != null)
			{
				existingItem.Quantity += item.Quantity;
			}
			else
			{
				basket.Items.Add(item);
			}

			// تحديث السلة في Redis
			var updatedBasket = await _basketRepo.UpdateBasketAsync(basketId, basket.Items);

			return updatedBasket ?? throw new Exception("Failed to update basket in repository");
		}
	}


}

