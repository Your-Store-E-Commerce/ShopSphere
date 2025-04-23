using AutoMapper;
using ShopSphere.Data.Entities.Basket;
using ShopSphere.Data.Entities.Order;
using ShopSphere.Web.Models.Basket;
using ShopSphere.Web.Models.Order;

namespace ShopSphere.Web.Mapper
{
    public class MappingProfile :Profile 
    {
        public MappingProfile()
        {
            CreateMap<CustomerBasketViewModel, CustomerBasket>().ReverseMap();
            CreateMap<Order, OrderToReturnViewModel>().ReverseMap();
            CreateMap<BasketItem, BasketItemViewModel>().ReverseMap();
        }
    }
}
