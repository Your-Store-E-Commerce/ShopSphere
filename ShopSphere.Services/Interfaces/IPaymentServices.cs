using ShopSphere.Data.Entities.Basket;
using ShopSphere.Data.Entities.Order;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopSphere.Services.Interfaces
{
    public interface IPaymentServices
    {
        Task<CustomerBasket> CrateOrUpdatePaymentIntent(string basketId);
        Task<Order?> UpdateOrderStatus(string paymentIntentId, bool isPaid);
    }
}
