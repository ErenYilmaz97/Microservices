using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Microservices.MVC.Models.Basket;
using Microservices.MVC.Services.Interfaces;
using Microservices.Shared;

namespace Microservices.MVC.Services.Implementations
{
    public class BasketService : IBasketService
    {
        private readonly HttpClient _httpClient;
        private readonly IDiscountService _discountService;


        public BasketService(HttpClient httpClient, IDiscountService discountService)
        {
            _httpClient = httpClient;
            _discountService = discountService;
        }



        public async Task<bool> SaveorUpdate(BasketViewModel basket)
        {
            var response = await _httpClient.PostAsJsonAsync<BasketViewModel>("baskets", basket);
            return response.IsSuccessStatusCode;
        }



        public async Task<BasketViewModel> GetBasket()
        {
            BasketViewModel basket = null;
            var response = await _httpClient.GetAsync("baskets");

            if (response.IsSuccessStatusCode)
            {
                var responseContent = await response.Content.ReadFromJsonAsync<ResponseObject<BasketViewModel>>();
                basket = responseContent.Data;
            }

            return basket;
        }



        public async Task<bool> DeleteBasket()
        {
            var result = await _httpClient.DeleteAsync("baskets");
            return result.IsSuccessStatusCode;
        }



        public async Task AddBasketItem(BasketItemViewModel basketItemViewModel)
        {
            var basket = await GetBasket();

            //ELSE İSE DE, QUANTİTY +1 YAPILABİLİR.
            if (!basket.BasketItems.Any(x => x.CourseId == basketItemViewModel.CourseId))
            {
                basket.BasketItems.Add(basketItemViewModel);
            }

            await SaveorUpdate(basket);
        }



        public async Task<bool> RemoveBasketItem(string courseId)
        {
            var basket = await GetBasket();
            bool result = true;

            var targetBasketItem = basket.BasketItems.FirstOrDefault(x => x.CourseId == courseId);

            if (targetBasketItem != null)
            {
                result = basket.BasketItems.Remove(targetBasketItem);
            }

            //ÜRÜN SORUNSUZ SİLİNDİYSE
            if (result == true && !basket.BasketItems.Any())
            {
                //SON ÜRÜN SİLİNDİYSE, İNDİRİMİ SIFIRLA
                basket.DiscountCode = null;
                basket.DiscountRate = 0;
            }

            //SEPETİ GÜNCELLE
            var saveResult = await SaveorUpdate(basket);
            return result && saveResult;


        }



        public async Task<bool> ApplyDiscount(string discountCode)
        {
            var discount = await _discountService.GetDiscount(discountCode);

            if (discount == null)
            {
                return false;
            }

            //SEPETTEKİ İNDİRİM KODUNU SİL
            await CancelApplyDiscount();
            var basket = await GetBasket();

            if (basket.DiscountCode != null)
            {
                return false;
            }

            basket.DiscountCode = discount.Code;
            basket.DiscountRate = discount.Rate;

            return await SaveorUpdate(basket);
        }



        public async Task<bool> CancelApplyDiscount()
        {
            var basket = await GetBasket();

            if (basket.DiscountCode == null)
            {
                return false;
            }

            basket.DiscountCode = null;
            basket.DiscountRate = 0;
            return await SaveorUpdate(basket);
        }
    }
}