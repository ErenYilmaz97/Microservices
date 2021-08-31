using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Microservices.MVC.Models.FakePayment;
using Microservices.MVC.Models.Order;
using Microservices.MVC.Services.Interfaces;
using Microservices.Shared;
using Microservices.Shared.Services;

namespace Microservices.MVC.Services.Implementations
{
    public class OrderService : IOrderService
    {

        private readonly IPaymentService _paymentService;
        private readonly IBasketService _basketService;
        private readonly HttpClient _httpClient;
        private readonly ISharedIdentityService _sharedIdentityService;



        public OrderService(IPaymentService paymentService, IBasketService basketService, HttpClient httpClient, ISharedIdentityService sharedIdentityService)
        {
            _paymentService = paymentService;
            _basketService = basketService;
            _httpClient = httpClient;
            _sharedIdentityService = sharedIdentityService;
        }


        public async Task<OrderCreatedViewModel> CreateOrder(CheckoutInfoRequest request)
        {
            var basket = await _basketService.GetBasket();

            var paymentRequest = new PaymentInfoRequest()
            {
                CardName = request.CardName,
                CardNumber = request.CardNumber,
                Expiration = request.Expiration,
                CVV = request.CVV,
                PaymentAmount = basket.TotalPrice //SEPETTEKİ TOPLAM FİYAT
            };

            var paymentResponse = await _paymentService.ReceivePayment(paymentRequest);

            if (!paymentResponse)
            {
                return new OrderCreatedViewModel() {Error = "Ödeme Alınamadı.", IsSuccess = false};
            }

            //ÖDEME ALINDI
            var createOrderRequest = new CreateOrderRequest()
            {
                BuyerId = _sharedIdentityService.GetCurrentUserIdFromClient(),
                Address = new AddressInput()
                {
                    Province = request.Province,
                    District = request.District,
                    Street = request.Street,
                    ZipCode = request.ZipCode,
                    Line = request.Line
                }
            };

            basket.BasketItems.ForEach(x =>
            {
                createOrderRequest.OrderItems.Add(new OrderItemCreateInput()
                {
                    ProductId = x.CourseId,
                    ProductName = x.CourseName,
                    Price = x.GetCurrentPrice
                });
            });

            var createOrderResponse = await _httpClient.PostAsJsonAsync<CreateOrderRequest>("orders", createOrderRequest);


            if (!createOrderResponse.IsSuccessStatusCode)
            {
                return new OrderCreatedViewModel() { Error = "Sipariş Oluşturulamadı.", IsSuccess = false };
            }

            var responseContent = await createOrderResponse.Content.ReadFromJsonAsync<ResponseObject<OrderCreatedViewModel>>();
            responseContent.Data.IsSuccess = true;
            return responseContent.Data;

        }


        public async Task<List<OrderViewModel>> GetOrders()
        {
            var response = await _httpClient.GetFromJsonAsync<ResponseObject<List<OrderViewModel>>>("orders");
            return response.Data;
        }



        public async Task<OrderSuspendViewModel> SuspendOrder(CheckoutInfoRequest request)
        {
            var basket = await _basketService.GetBasket();


            var createOrderRequest = new CreateOrderRequest()
            {
                BuyerId = _sharedIdentityService.GetCurrentUserIdFromClient(),
                Address = new AddressInput()
                {
                    Province = request.Province,
                    District = request.District,
                    Street = request.Street,
                    ZipCode = request.ZipCode,
                    Line = request.Line
                }
            };

            basket.BasketItems.ForEach(x =>
            {
                createOrderRequest.OrderItems.Add(new OrderItemCreateInput()
                {
                    ProductId = x.CourseId,
                    ProductName = x.CourseName,
                    Price = x.GetCurrentPrice
                });
            });


            var paymentRequest = new PaymentInfoRequest()
            {
                CardName = request.CardName,
                CardNumber = request.CardNumber,
                Expiration = request.Expiration,
                CVV = request.CVV,
                PaymentAmount = basket.TotalPrice, //SEPETTEKİ TOPLAM FİYAT
                Order = createOrderRequest
            };


            var paymentResponse = await _paymentService.ReceivePayment(paymentRequest);

            if (!paymentResponse)
            {
                return new OrderSuspendViewModel() { Error = "Ödeme Alınamadı.", IsSuccess = false };
            }

            //ÖDEME ALINDI, ASENKRON OLARAK SİPARİŞ OLUŞTURULACAK.
            return new OrderSuspendViewModel() {IsSuccess = true};

        }
    }
}