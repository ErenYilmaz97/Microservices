using Microservices.MVC.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microservices.MVC.Exception;
using Microservices.MVC.Services.Interfaces;
using Microsoft.AspNetCore.Diagnostics;

namespace Microservices.MVC.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ICatalogService _catalogService;

        public HomeController(ILogger<HomeController> logger, ICatalogService catalogService)
        {
            _logger = logger;
            _catalogService = catalogService;
        }


        public async Task<IActionResult> Index()
        {
            var courses = await _catalogService.GetAllCoursesAsync();
            return View(courses);
        }


        public async Task<IActionResult> Detail(string courseId)
        {
            return View(await _catalogService.GetCourseById(courseId));
        }



        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            var errorFeature = HttpContext.Features.Get<IExceptionHandlerFeature>();

            if (errorFeature != null && errorFeature.Error is UnAuthorizedException)
            {
                return RedirectToAction(nameof(AuthController.Logout), "Auth");
            }
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
