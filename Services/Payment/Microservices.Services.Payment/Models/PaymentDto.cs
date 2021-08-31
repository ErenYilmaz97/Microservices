namespace Microservices.Services.Payment.Models
{
    public class PaymentDto
    {
        public string CardName { get; set; }
        public string CardNumber { get; set; }
        public string Expiration { get; set; }
        public string CVV { get; set; }
        public decimal PaymentAmount { get; set; }

        //ASENKRON İLETİŞİM İÇİN
        public OrderDto Order { get; set; }
    }
}