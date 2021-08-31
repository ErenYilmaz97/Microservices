using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microservices.MVC.Models.Order;
using Microservices.MVC.Services.Interfaces;

namespace Microservices.MVC.Controllers
{
    public class OrderController : Controller
    {

        private readonly IBasketService _basketService;
        private readonly IOrderService _orderService;

        public OrderController(IBasketService basketService, IOrderService orderService)
        {
            _basketService = basketService;
            _orderService = orderService;
        }


        
        public async Task<IActionResult> Checkout()
        {
            var basket = await _basketService.GetBasket();
            ViewBag.basket = basket;

            return View(new CheckoutInfoRequest());
        }



        [HttpPost]
        public async Task<IActionResult> Checkout(CheckoutInfoRequest request)
        {
            var createOrder = await _orderService.SuspendOrder(request);

            if (!createOrder.IsSuccess)
            {
                var basket = await _basketService.GetBasket();
                ViewBag.basket = basket;

                ViewBag.createOrderError = createOrder.Error;
                return View();
            }

            //return RedirectToAction("SuccessfulCheckout", new {orderId = createOrder.OrderId});
            return RedirectToAction("SuccessfulCheckout", new { orderId = new Random().Next(1,1000) });
        }



        public IActionResult SuccessfulCheckout(int orderId)
        {
            ViewBag.orderId = orderId;
            return View();
        }


        public async Task<IActionResult> CheckoutHistory()
        {
            return View(await _orderService.GetOrders());
        }
    }
}
