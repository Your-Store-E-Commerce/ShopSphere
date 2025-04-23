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
        Task<Order?> CreateOrderAsync(
          string basketId, int deliveryMethod, string BuyerEmail, string ShippingAddress);
        Task<IReadOnlyList<Order>> GetOrderForUser(string BuyerEmail);
        //Task<Order> GetOrderForUserById(int orderId, string BuyerEmail);

        Task<IReadOnlyList<DeliveryMethod>> GetDeliveryMethod();
    }
}
