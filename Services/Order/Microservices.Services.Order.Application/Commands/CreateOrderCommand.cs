using System.Collections.Generic;
using MediatR;
using Microservices.Services.Order.Application.Dtos;
using Microservices.Shared;

namespace Microservices.Services.Order.Application.Commands
{
    public class CreateOrderCommand : IRequest<ResponseObject<CreatedOrderDto>>
    {
        public string BuyerId { get; set; }
        public List<OrderItemDto> OrderItems { get; set; }
        public AddressDto Address { get; set; }
    }
}