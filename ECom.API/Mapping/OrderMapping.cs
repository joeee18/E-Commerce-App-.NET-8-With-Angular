using AutoMapper;
using ECom.Core.DTO;
using ECom.Core.Entites;
using ECom.Core.Entites.Order;
using Microsoft.OpenApi.Extensions;

namespace ECom.API.Mapping
{
    public class OrderMapping : Profile
    {
        public OrderMapping()
        {
            CreateMap<Orders,OrderToReturnDTO>()
                .ForMember(d=>d.deliveryMethod,o=>o
                .MapFrom(s=>s.deliveryMethod.Name))
                .ReverseMap();
            CreateMap<OrderItem, OrderItemDTO>().ReverseMap();

            CreateMap<ShippingAddress, ShipAddressDTO>().ReverseMap();
            CreateMap<Address, ShipAddressDTO>().ReverseMap();

        }


    }
}
