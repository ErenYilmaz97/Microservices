﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace Microservices.MVC.Models.Basket
{
    public class BasketViewModel
    {
        public string UserId { get; set; }
        public string DiscountCode { get; set; }
        public int? DiscountRate { get; set; }

        private List<BasketItemViewModel> _basketItems { get; set; }


        public List<BasketItemViewModel> BasketItems
        {
            get
            {
                if (HasDiscount)
                {
                    _basketItems.ForEach(x =>
                    {
                        var discountPrice = x.Price * ((decimal) DiscountRate / 100);
                        x.AppliedDiscount(Math.Round(x.Price - discountPrice,2));
                    });
                }

                return _basketItems;
            }

            set
            {
                _basketItems = value;
            }
        }

        //YARDIMCI PROPSLAR
        public decimal TotalPrice
        {
            get => _basketItems.Sum(x => x.GetCurrentPrice * x.Quantity);
        }


        public bool HasDiscount
        {
            get => !string.IsNullOrEmpty(DiscountCode) && DiscountRate.HasValue;
        }
    }
}