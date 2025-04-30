using AutoMapper;
using ShopSphere.Data.Entities.Basket;
using ShopSphere.Data.Entities.Data;
using ShopSphere.Data.Entities.Order;
using ShopSphere.Web.Models.Basket;
using ShopSphere.Web.Models.Order;
using ShopSphere.Web.Models.Product;

namespace ShopSphere.Web.Mapper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Product, ProductViewModel>()
                .ForMember(dest => dest.TypeName, opt => opt.MapFrom(src => src.Type.Name))
                .ForMember(dest => dest.BrandName, opt => opt.MapFrom(src => src.Brand.Name))
                .ReverseMap();

            CreateMap<CustomerBasketViewModel, CustomerBasket>().ReverseMap();
            CreateMap<BasketItem, BasketItemViewModel>().ReverseMap();
            CreateMap<Order, OrderToReturnViewModel>().ReverseMap();
            
            CreateMap<OrderItem, OrderItemViewModel>().ReverseMap();
            CreateMap<Order, OrderViewModel>()
                .ForMember(dest=>dest.ShippingPrice ,opt=>opt.MapFrom(src=>src.DeliveryMethod.Price)).ReverseMap();

            CreateMap<ProductType, ProductTypeViewModel>()
             .ForMember(dest => dest.TypeId, opt => opt.MapFrom(src => src.Id))
           .ForMember(dest => dest.TypeName, opt => opt.MapFrom(src => src.Name));

            CreateMap<ProductType, ProductTypeViewModel>()
             .ForMember(dest => dest.TypeId, opt => opt.MapFrom(src => src.Id))
           .ForMember(dest => dest.TypeName, opt => opt.MapFrom(src => src.Name));

        }
    }
}
