using System.Collections.Generic;
using System.Threading.Tasks;
using Microservices.MVC.Models.Order;

namespace Microservices.MVC.Services.Interfaces
{
    public interface IOrderService
    {
        //SENKRON İLETİŞİM
        Task<OrderCreatedViewModel> CreateOrder(CheckoutInfoRequest request);

        //ASENKRON İLETİŞİM (RABBITMQ)
        Task<OrderSuspendViewModel> SuspendOrder(CheckoutInfoRequest request);

        Task<List<OrderViewModel>> GetOrders();
    }
}