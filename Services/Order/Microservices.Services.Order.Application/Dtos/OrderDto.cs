﻿using System;
using System.Collections.Generic;
using Microservices.Services.Order.Domain.OrderAggregate;

namespace Microservices.Services.Order.Application.Dtos
{
    public class OrderDto
    {
        public int Id { get; set; }
        public DateTime CreatedTime { get; private set; }
        public AddressDto Address { get; private set; }
        public string BuyerId { get; private set; }

        public List<OrderItemDto> OrderItems { get; set; }
    }
}