using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microservices.Services.Order.Application.Dtos;
using Microservices.Services.Order.Application.MapProfiles;
using Microservices.Services.Order.Application.Queries;
using Microservices.Services.Order.İnfrastructure;
using Microservices.Shared;
using Microsoft.EntityFrameworkCore;

namespace Microservices.Services.Order.Application.Handlers
{
    public class GetOrdersByUserIdQueryHandler : IRequestHandler<GetOrdersByUserIdQuery, ResponseObject<ICollection<OrderDto>>>
    {
        private readonly OrderDbContext _context;

        public GetOrdersByUserIdQueryHandler(OrderDbContext context)
        {
            _context = context;
        }

        public async Task<ResponseObject<ICollection<OrderDto>>> Handle(GetOrdersByUserIdQuery request, CancellationToken cancellationToken)
        {
            var orders = await _context.Orders.Include(x => x.OrderItems).Where(x => x.BuyerId.Equals(request.UserId)).ToListAsync();

            if (!orders.Any())
            {
                return ResponseObject<ICollection<OrderDto>>.CreateSuccessResponse();
            }


            var mappedOrders = ObjectMapper.Mapper.Map<List<OrderDto>>(orders);
            return ResponseObject<ICollection<OrderDto>>.CreateSuccessResponse(mappedOrders);
        }
    }
}