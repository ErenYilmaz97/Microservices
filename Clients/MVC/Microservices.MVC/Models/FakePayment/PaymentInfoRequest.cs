using Microservices.MVC.Models.Order;

namespace Microservices.MVC.Models.FakePayment
{
    public class PaymentInfoRequest
    {
        public string CardName { get; set; }
        public string CardNumber { get; set; }
        public string Expiration { get; set; }
        public string CVV { get; set; }
        public decimal PaymentAmount { get; set; }
        public CreateOrderRequest Order { get; set; }
    }
}