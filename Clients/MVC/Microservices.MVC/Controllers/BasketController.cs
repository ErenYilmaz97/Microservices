using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microservices.MVC.Models.Basket;
using Microservices.MVC.Models.Discount;
using Microservices.MVC.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;

namespace Microservices.MVC.Controllers
{
    [Authorize]
    public class BasketController : Controller
    {

        private readonly ICatalogService _catalogService;
        private readonly IBasketService _basketService;


        public BasketController(ICatalogService catalogService, IBasketService basketService)
        {
            _catalogService = catalogService;
            _basketService = basketService;
        }



        public async Task<IActionResult> Index()
        {
            return View(await _basketService.GetBasket());
        }



       
        public async Task<IActionResult> AddBasketItem(string courseId)
        {
            var course = await _catalogService.GetCourseById(courseId);

            if (course != null)
            {
                var basketItem = new BasketItemViewModel()
                {
                    CourseId = courseId,
                    CourseName = course.Name,
                    Price = course.Price,
                    Quantity = 1,

                };

                await _basketService.AddBasketItem(basketItem);
            }

            return RedirectToAction("Index", "Basket");
        }



      
        public async Task<IActionResult> RemoveBasketItem(string courseId)
        {
            await _basketService.RemoveBasketItem(courseId);
            return RedirectToAction("Index", "Basket");
        }



        public async Task<IActionResult> ApplyDiscount(string discountCode)
        {
            var discountStatus = await _basketService.ApplyDiscount(discountCode);

            TempData["discountStatus"] = discountStatus;
            return RedirectToAction("Index", "Basket");
        }



        public async Task<IActionResult> CancelApplyDiscount()
        {
            await _basketService.CancelApplyDiscount();
            return RedirectToAction("Index", "Basket");
        }
    }
}
