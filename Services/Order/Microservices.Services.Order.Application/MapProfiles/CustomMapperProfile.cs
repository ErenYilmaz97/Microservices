using System.Linq;
using AutoMapper;
using Microservices.Services.Order.Application.Dtos;
using Microservices.Services.Order.Domain.OrderAggregate;

namespace Microservices.Services.Order.Application.MapProfiles
{
    public class CustomMapperProfile : Profile
    {
        public CustomMapperProfile()
        {
            CreateMap<Domain.OrderAggregate.Order, OrderDto>().ReverseMap();
            CreateMap<OrderItem, OrderItemDto>().ReverseMap();
            CreateMap<Address, AddressDto>().ReverseMap();
        }
    }
}