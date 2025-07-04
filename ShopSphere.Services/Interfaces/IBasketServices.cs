﻿using ShopSphere.Data.Entities.Basket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopSphere.Services.Interfaces
{
	public interface IBasketServices
	{

        Task<bool> RemoveItemFromBasketAsync(string basketId, string productId);

        Task<CustomerBasket?> UpdateItemQuantityAsync(string basketId, int productId, int quantity);
        Task<CustomerBasket?> GetBasketAsync(string id);
		Task<CustomerBasket?> AddItemToBasketAsync(string basketId, BasketItem item);
		Task<CustomerBasket?> UpdateBasketAsync(string basketId, List<BasketItem> items);
		Task<bool> DeleteBasketAsync(string id);
	}
}
