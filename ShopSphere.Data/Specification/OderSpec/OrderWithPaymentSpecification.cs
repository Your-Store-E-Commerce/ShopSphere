using ShopSphere.Data.Entities.Order;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopSphere.Data.Specification.OderSpec
{
    public class OrderWithPaymentSpecification :BaseSpecification<Order>
    {
        public OrderWithPaymentSpecification(string paymentIntentId) :
        base(O => O.PaymentIntentId == paymentIntentId)
        {

        }
    }
}
