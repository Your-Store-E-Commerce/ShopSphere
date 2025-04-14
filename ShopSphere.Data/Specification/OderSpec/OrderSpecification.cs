using ShopSphere.Data.Entities.Order;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopSphere.Data.Specification.OderSpec
{
    public class OrderSpecification :BaseSpecification<Order>
    {
        public OrderSpecification(string email) : base(o => o.BuyerEmail == email)
        {
            Includes.Add(o => o.DeliveryMethod);
            Includes.Add(o => o.Items);
             
            OrderByDesc = o => o.OrderDate;
        }

        public OrderSpecification(int orderId, string email) : base(
            o => o.Id == orderId && o.BuyerEmail == email)
        {
            Includes.Add(o => o.DeliveryMethod);
            Includes.Add(o => o.Items);


        }
    }
}
