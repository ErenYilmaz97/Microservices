using System.Collections.Generic;
using MediatR;
using Microservices.Services.Order.Application.Dtos;
using Microservices.Shared;

namespace Microservices.Services.Order.Application.Queries
{
    public class GetOrdersByUserIdQuery : IRequest<ResponseObject<ICollection<OrderDto>>>
    {
        public string UserId { get; set; }
    }
}