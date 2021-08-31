using System.Threading.Tasks;
using MassTransit;
using Microservices.Services.Order.İnfrastructure;
using Microservices.Shared.Messages;

namespace Microservices.Services.Order.Application.Consumer
{
    public class CreateOrderMessageCommentConsumer : IConsumer<CreateOrderMessageCommand>
    {
        private readonly OrderDbContext _context;

        public CreateOrderMessageCommentConsumer(OrderDbContext context)
        {
            _context = context;
        }


        public async Task Consume(ConsumeContext<CreateOrderMessageCommand> context)
        {
            var newAddress = new Domain.OrderAggregate.Address(context.Message.Province, context.Message.District,
                context.Message.Street, context.Message.ZipCode, context.Message.Line);
            Domain.OrderAggregate.Order order = new(context.Message.BuyerId, newAddress);

            context.Message.OrderItems.ForEach(x =>
            {
                order.AddOrderItem(x.ProductId, x.ProductName, x.Price, x.PictureUrl);
            });

            await _context.Orders.AddAsync(order);
            await _context.SaveChangesAsync();
        }
    }
}