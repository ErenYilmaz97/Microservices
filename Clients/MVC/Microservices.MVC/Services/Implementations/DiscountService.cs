using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Microservices.MVC.Models.Discount;
using Microservices.MVC.Services.Interfaces;
using Microservices.Shared;

namespace Microservices.MVC.Services.Implementations
{
    public class DiscountService : IDiscountService
    {
        private readonly HttpClient _httpClient;

        public DiscountService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }


        public async Task<DiscountViewModel> GetDiscount(string discountCode)
        {
            var response = await _httpClient.GetAsync($"Discounts/GetByCodeAndUserId/{discountCode}");

            if (!response.IsSuccessStatusCode)
            {
                return default(DiscountViewModel);
            }

            var discount = await response.Content.ReadFromJsonAsync<ResponseObject<DiscountViewModel>>();
            return discount.Data;
        }
    }
}