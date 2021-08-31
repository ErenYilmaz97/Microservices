using System.Threading.Tasks;
using Microservices.MVC.Models.FakePayment;

namespace Microservices.MVC.Services.Interfaces
{
    public interface IPaymentService
    {
        Task<bool> ReceivePayment(PaymentInfoRequest request);
    }
}