using ShopSphere.Data.Entities.Order;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopSphere.Services.Interfaces
{
    public interface IOrderServices
    {
     
        Task<Order?> CreateOrderAsync(string basketId, int deliveryMethodId, string buyerEmail, string shippingAddress);
        Task<Order?> GetOrderByIdAsync(int orderId);
        Task<IReadOnlyList<DeliveryMethod>> GetDeliveryMethod();
        Task<IReadOnlyList<Order>> GetOrderForUser(string buyerEmail);
    }
}
