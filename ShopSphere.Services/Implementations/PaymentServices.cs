using Microsoft.Extensions.Configuration;
using ShopSphere.Data.Entities.Basket;
using ShopSphere.Data.Entities.Data;
using ShopSphere.Data.Entities.Order;
using ShopSphere.Data.Interfaces;
using ShopSphere.Data.Repositories.Interfaces;
using ShopSphere.Data.Specification.OderSpec;
using ShopSphere.Services.Interfaces;
using Stripe;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopSphere.Services.Implementations
{
    public class PaymentServices : IPaymentServices
    {

        private readonly IConfiguration _configuration;
        private readonly IBasketRepository _basketRepo;
        private readonly IUnitOfWork _unitOfWork;

        public PaymentServices(IConfiguration configuration,
                               IBasketRepository basketRepo,
                               IUnitOfWork unitOfWork)
        {
            _configuration = configuration;
            _basketRepo = basketRepo;
            _unitOfWork = unitOfWork;
        }
        public async Task<CustomerBasket> CrateOrUpdatePaymentIntent(string basketId)
        {
            
            var productRepo = _unitOfWork.Repository<Data.Entities.Data.Product>();
            var basket = await _basketRepo.GetBasketAsync(basketId);
            if (basket == null) return null;

            var shippingprice = 0m;

            if (basket.DeliveryMethodId.HasValue)
            {
                var deliveryMethod = await _unitOfWork.Repository<DeliveryMethod>().GetByIdAsync(basket.DeliveryMethodId.Value);
                shippingprice = deliveryMethod.Price;
            }

            if (basket.Items?.Count > 0)
            {
                foreach (var item in basket.Items)
                {
                    var product = await productRepo.GetByIdAsync(item.Id);

                    if (item.Price != product.Price)
                        item.Price = product.Price;
                }
            }

            PaymentIntent paymentIntent;
            PaymentIntentService paymentIntentService = new PaymentIntentService();

            if (string.IsNullOrEmpty(basket.PaymentIntentId))  // Create New Payment Intent 
            {
                var option = new PaymentIntentCreateOptions()
                {
                    Amount = (long)basket.Items.Sum(item => item.Price * item.Quantity * 100) + (long)shippingprice * 100,
                    Currency = "usd",
                    PaymentMethodTypes = new List<string> { "card" }

                };

                paymentIntent = await paymentIntentService.CreateAsync(option); // Integration with Stripe
                basket.PaymentIntentId = paymentIntent.Id;
                basket.ClientSecret = paymentIntent.ClientSecret;

            }

            else  // Update Existing Payment Intent
            {
                var option = new PaymentIntentUpdateOptions()
                {
                    Amount = (long)basket.Items.Sum(item => item.Price * item.Quantity * 100) + (long)shippingprice * 100,

                };
                await paymentIntentService.UpdateAsync(basket.PaymentIntentId, option);

            }

            await _basketRepo.UpdateBasketAsync(basket.Id, basket.Items);

            return basket;
        }
   

    public async Task<Order?> UpdateOrderStatus(string paymentIntentId, bool isPaid)
    {
        var orderRepo = _unitOfWork.Repository<Order>();
        var spec = new OrderWithPaymentSpecification(paymentIntentId);

        var order = await orderRepo.GetByIdWihSpecAsync(spec);

        if (order is null) return null;

        if (isPaid)
            order.orderStatus = OrderStatus.Received;
        else
            order.orderStatus = OrderStatus.Failed;

        await orderRepo.Update(order.Id, order);
        await _unitOfWork.CompleteAsync();
        return order;
    }
}
}
