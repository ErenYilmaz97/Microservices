using System;
using System.Collections.Generic;
using System.Linq;
using Microservices.Services.Order.Domain.Core;

namespace Microservices.Services.Order.Domain.OrderAggregate
{
    public class Order : EntityBase, IAggregateRoot
    {
        public DateTime CreatedTime { get; private set; }
        public Address Address { get; private set; }
        public string BuyerId { get; private set; }

        //HELPER PROP
        public decimal TotalPrice
        {
            get => _orderItems.Sum(x => x.Price);
        }


        //NAV PROP ÜZERİNDN DOĞRUDAN ORDERITEM EKLENEMESİN
        private readonly List<OrderItem> _orderItems;

        public IReadOnlyCollection<OrderItem> OrderItems => _orderItems;



        public Order(string buyerId, Address address)
        {
            _orderItems = new List<OrderItem>();
            CreatedTime = DateTime.Now;
            BuyerId = buyerId;
            Address = address;
        }


        public Order()
        {
            CreatedTime = DateTime.Now;
        }


        public void AddOrderItem(string productId, string productName, decimal price, string pictureUrl)
        {
            //BUSINESS RULE
            var existProduct = _orderItems.Any(x => x.ProductId == productId);

            if (!existProduct)
            {
                OrderItem newOrderItem = new(productId, productName, pictureUrl, price);
                _orderItems.Add(newOrderItem);
            }
        }
    }
}