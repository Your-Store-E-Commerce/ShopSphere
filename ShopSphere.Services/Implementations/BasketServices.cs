using ShopSphere.Data.Entities.Basket;
using ShopSphere.Data.Entities.Order;
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


        public async Task<bool> RemoveItemFromBasketAsync(string basketId, string productId)
        {
            if (string.IsNullOrWhiteSpace(basketId))
                throw new ArgumentNullException(nameof(basketId));

            if (string.IsNullOrWhiteSpace(productId))
                throw new ArgumentNullException(nameof(productId));

            var basket = await _basketRepo.GetBasketAsync(basketId);

            if (basket == null)
                return false;

            var itemId = int.Parse(productId);
            var itemToRemove = basket.Items.FirstOrDefault(i => i.Id == itemId);

            if (itemToRemove == null)
                return false;

            basket.Items.Remove(itemToRemove);

            var updatedBasket = await _basketRepo.UpdateBasketAsync(basketId, basket.Items);
            return updatedBasket != null;
        }


        public async Task<CustomerBasket?> UpdateItemQuantityAsync(string basketId, int productId, int quantity)
        {
            if (string.IsNullOrWhiteSpace(basketId) || productId <= 0 || quantity <= 0)
                throw new ArgumentException("Invalid input parameters");

            // جلب السلة بناءً على basketId
            var basket = await _basketRepo.GetBasketAsync(basketId);
            if (basket == null)
                return null;

            // البحث عن العنصر الذي يحتاج لتحديث
            var itemToUpdate = basket.Items.FirstOrDefault(i => i.Id == productId);
            if (itemToUpdate == null)
                return null;

            // تحديث الكمية للعنصر المطلوب
            itemToUpdate.Quantity = quantity;

            // تحديث السلة في المستودع
            return await _basketRepo.UpdateBasketAsync(basketId, basket.Items);
        }






    }


}




