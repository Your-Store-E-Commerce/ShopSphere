using ShopSphere.Data.Entities.Data;
using ShopSphere.Data.Entities.Order;
using ShopSphere.Data.Interfaces;
using ShopSphere.Data.Repositories.Interfaces;
using ShopSphere.Data.Specification.OderSpec;
using ShopSphere.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ShopSphere.Services.Implementations
{
    public class OrderServices :IOrderServices
    {
        private readonly IBasketRepository _basket;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IPaymentServices _paymentServices;


        public OrderServices(IBasketRepository basket,
                            IUnitOfWork unitOfWork,
                            IPaymentServices paymentServices)

        {
            _basket = basket;
            _unitOfWork = unitOfWork;
            _paymentServices = paymentServices;
        }
        public IPaymentServices PaymentServices { get; }

        public async Task<Order?> CreateOrderAsync(string basketId, int deliveryMethodId, string BuyerEmail, string ShippingAddress)
        {
            //1.Get BasketId from Basket Services

            var basket = await _basket.GetBasketAsync(basketId);

            //2.Get Selected Item at basket from ProductRepo

            var OrderItems = new List<OrderItem>();
            if (basket?.Items.Count > 0)
            {
                foreach (var item in basket.Items)
                {
                    var product = await _unitOfWork.Repository<Product>().GetByIdAsync(item.Id);
                    var ProductItemOrder = new ProductOrderItem(product.Id, product.Name, product.PictureUrl);
                    var orderItem = new OrderItem(ProductItemOrder, product.Price, item.Quantity);

                    OrderItems.Add(orderItem);
                }
            }


            //3.Calculate SubTotal

            var subTotal = OrderItems.Sum(item => item.Price * item.Quantity);

            //4.Get DeliveryMethod from DeliveryMethodRepo
            var DeliveryMethod = await _unitOfWork.Repository<DeliveryMethod>().GetByIdAsync(deliveryMethodId);

            ////Check if Order Create With same Payment Intent Id

            var orderRepo = _unitOfWork.Repository<Order>();

            var spec = new OrderWithPaymentSpecification(basketId);
            var existingOrder = await orderRepo.GetByIdWihSpecAsync(spec);

            if (existingOrder is not null)
            {
               await orderRepo.Delete(existingOrder.Id);

                //Check Payment =>Amount => new Order
                await _paymentServices.CrateOrUpdatePaymentIntent(basketId);
            }


                //5.Create Oder 

                var order = new Order(
                buyerEmail: BuyerEmail,
                shippingAddress: ShippingAddress,
                deliveryMethod: DeliveryMethod,
                items: OrderItems,
                subtotal: subTotal,
                paymentIntentId: basket?.PaymentIntentId ?? ""

                );

           await orderRepo.CreateAsync(order);

            //6.Save Database 
            var result = await _unitOfWork.CompleteAsync();
            if (result <= 0)
                return null;

            return order;

        }

        public async Task<IReadOnlyList<DeliveryMethod>> GetDeliveryMethod()
        {
            var result = await _unitOfWork.Repository<DeliveryMethod>().GetAllAsync();
            return result;
        }

        public async Task<IReadOnlyList<Order>> GetOrderForUser(string BuyerEmail)
        {
            var orderRepo = _unitOfWork.Repository<Order>();

            var spec = new OrderSpecification(BuyerEmail);

            var orders = await orderRepo.GetAllWihSpecAsync(spec);

            return orders;

        }

        //public Task<Order> GetOrderForUserById(int orderId, string BuyerEmail)
        //{
        //    var orderRepo = _unitOfWork.Repository<Order>();

        //    var spec = new OrderSpecification(orderId, BuyerEmail);

        //    var order = orderRepo.GetByIdWihSpecAsync(spec);

        //    return order;
        //}
    }
}
