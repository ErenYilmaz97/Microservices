using System.Threading.Tasks;
using MassTransit;
using Microservices.Services.Basket.Services;
using Microservices.Shared.Messages;

namespace Microservices.Services.Basket.Consumer
{
    public class CourseNameChangedEventConsumer : IConsumer<CourseNameChangedEvent>
    {
        private readonly IBasketService _basketService;

        public CourseNameChangedEventConsumer(IBasketService basketService)
        {
            _basketService = basketService;
        }

        public async Task Consume(ConsumeContext<CourseNameChangedEvent> context)
        {
            
        }
    }
}