using System.Linq;
using System.Threading.Tasks;
using MassTransit;
using Microservices.Services.Order.İnfrastructure;
using Microservices.Shared.Messages;
using Microsoft.EntityFrameworkCore;

namespace Microservices.Services.Order.Application.Consumer
{
    public class CourseNameChangedEventConsumer : IConsumer<CourseNameChangedEvent>
    {
        private readonly OrderDbContext _context;

        public CourseNameChangedEventConsumer(OrderDbContext context)
        {
            _context = context;
        }

        public async Task Consume(ConsumeContext<CourseNameChangedEvent> context)
        {
            //İÇERİSİNDE BU ÜRÜN BULUNAN ORDRITEMSLARI GETİR.
            var orderItems = await _context.OrderItems.Where((x) => x.ProductId == context.Message.CourseId).ToListAsync();

            orderItems.ForEach(x =>
            {
                //HERBİRİNİN ADINI GÜNCELLE.    
                x.UpdateOrderItem(context.Message.UpdatedCourseName, x.PictureUrl, x.Price);
                _context.Update(x);
            });

            await _context.SaveChangesAsync();
        }
    }
}