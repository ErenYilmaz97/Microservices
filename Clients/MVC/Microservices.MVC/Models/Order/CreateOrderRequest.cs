using System.Collections.Generic;

namespace Microservices.MVC.Models.Order
{
    public class CreateOrderRequest
    {
        public string BuyerId { get; set; }
        public List<OrderItemCreateInput> OrderItems { get; set; }
        public AddressInput Address { get; set; }


        public CreateOrderRequest()
        {
            OrderItems = new List<OrderItemCreateInput>();
        }
    }
}