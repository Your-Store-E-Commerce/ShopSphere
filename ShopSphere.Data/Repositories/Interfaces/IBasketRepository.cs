using ShopSphere.Data.Entities.Basket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopSphere.Data.Repositories.Interfaces
{
    public interface IBasketRepository
    {
        Task<CustomerBasket?> GetBasketAsync(string id);
        Task<CustomerBasket?> UpdateBasketAsync(string basketId, List<BasketItem> items);
        //Task<CustomerBasket?> AddItemBasketAsync(string basketId, BasketItem item);
        Task<bool> DeleteBasketAsync(string id);
    }
}
