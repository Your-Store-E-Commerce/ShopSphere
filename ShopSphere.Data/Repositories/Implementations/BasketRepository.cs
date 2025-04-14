using ShopSphere.Data.Entities.Basket;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace ShopSphere.Data.Repositories.Implementations
{
    internal class BasketRepository
    {
        private readonly IDatabase _database;
        public BasketRepository(IConnectionMultiplexer connection)
        {
            _database = connection.GetDatabase();
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

        public async Task<CustomerBasket?> UpdateBasketAsync(CustomerBasket basket)
        {
            var jsonBasket = JsonSerializer.Serialize(basket);
            var creatOrUpdate = await _database.StringSetAsync(basket.Id, jsonBasket, TimeSpan.FromDays(7));
            if (creatOrUpdate is false) return null;
            return await GetBasketAsync(basket.Id);
        }
    }
}
