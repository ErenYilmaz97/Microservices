using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microservices.Services.Order.Application.Commands;
using Microservices.Services.Order.Application.Dtos;
using Microservices.Services.Order.Domain.OrderAggregate;
using Microservices.Services.Order.İnfrastructure;
using Microservices.Shared;

namespace Microservices.Services.Order.Application.Handlers
{
    public class CreateOrderCommandHandler : IRequestHandler<CreateOrderCommand, ResponseObject<CreatedOrderDto>>
    {
        private readonly OrderDbContext _context;

        public CreateOrderCommandHandler(OrderDbContext context)
        {
            _context = context;
        }


        public async Task<ResponseObject<CreatedOrderDto>> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
        {
            Address newAddress = new(request.Address.Province, request.Address.District, request.Address.Street, request.Address.ZipCode, request.Address.Line);

            Domain.OrderAggregate.Order newOrder = new(request.BuyerId, newAddress);

            request.OrderItems.ForEach(item =>
            {
                newOrder.AddOrderItem(item.ProductId, item.ProductName, item.Price, item.PictureUrl);
            });


            await _context.Orders.AddAsync(newOrder);
            await _context.SaveChangesAsync();

            return ResponseObject<CreatedOrderDto>.CreateSuccessResponse(new CreatedOrderDto(){OrderId = newOrder.Id},200);

        }
    }
}