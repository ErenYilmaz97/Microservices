using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Microservices.MVC.Models.FakePayment;
using Microservices.MVC.Services.Interfaces;

namespace Microservices.MVC.Services.Implementations
{
    public class PaymentService : IPaymentService
    {
        private readonly HttpClient _httpClient;

        public PaymentService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }


        public async Task<bool> ReceivePayment(PaymentInfoRequest request)
        {
            var response = await _httpClient.PostAsJsonAsync<PaymentInfoRequest>("Payments", request);
            return response.IsSuccessStatusCode;
        } 
    }
}