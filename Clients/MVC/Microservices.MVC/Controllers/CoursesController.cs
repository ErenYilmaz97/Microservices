using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microservices.MVC.Models.Catalog;
using Microservices.MVC.Services.Interfaces;
using Microservices.Shared.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Microservices.MVC.Controllers
{
    [Authorize]
    public class CoursesController : Controller
    {
        private readonly ICatalogService _catalogService;
        private readonly ISharedIdentityService _sharedIdentityService;

        public CoursesController(ICatalogService catalogService, ISharedIdentityService sharedIdentityService)
        {
            _catalogService = catalogService;
            _sharedIdentityService = sharedIdentityService;
        }



        public async Task<IActionResult> Index()
        {
            var model = await _catalogService.GetAllCoursesByUserIdAsync(_sharedIdentityService.GetCurrentUserIdFromClient());
            return View(model);
        }



        public async Task<IActionResult> Create()
        {
            var categories = await _catalogService.GetAllCategoriesAsync();
            ViewBag.categoryList = new SelectList(categories, "Id", "Name");
            return View();
        }




        [HttpPost]
        public async Task<IActionResult> Create(CreateCourseRequest request)
        {
            if (!ModelState.IsValid)
            {
                var categories = await _catalogService.GetAllCategoriesAsync();
                ViewBag.categoryList = new SelectList(categories, "Id", "Name");
                return View();
            }

            request.UserId = _sharedIdentityService.GetCurrentUserIdFromClient();
            var result = await _catalogService.CreateCourseAsync(request);

            if (!result)
            {
                ModelState.AddModelError(string.Empty, "Kurs Eklenirken bir hat oluştu.");
            }

            return RedirectToAction("Index", "Courses");
        }




        public async Task<IActionResult> Update(string id)
        {
            var course = await _catalogService.GetCourseById(id);

            if (course == null)
            {
                return RedirectToAction("Index", "Courses");
            }


            var categories = await _catalogService.GetAllCategoriesAsync();
            ViewBag.categoryList = new SelectList(categories, "Id", "Name", course.CategoryId);
            UpdateCourseRequest updateCourseModel = new()
            {
                Id = id,
                Name = course.Name,
                Description = course.Description,
                Price = course.Price,
                Feature = course.Feature,
                CategoryId = course.CategoryId,
                Picture = course.Picture,
                UserId = course.UserId
            };

            return View(updateCourseModel);
        }



        [HttpPost]
        public async Task<IActionResult> Update(UpdateCourseRequest request)
        {
            if (!ModelState.IsValid)
            {
                var categories = await _catalogService.GetAllCategoriesAsync();
                ViewBag.categoryList = new SelectList(categories, "Id", "Name");
                return View();
            }

            bool result = await _catalogService.UpdateCourseAsync(request);

            if (!result)
            {
                ModelState.AddModelError(string.Empty, "Kurs Güncellenirken Bir Hata Oluştu.");
                return View();
            }

            return RedirectToAction("Index", "Courses");
        }



        public async Task<IActionResult> Delete(string id)
        {
            bool result = await _catalogService.DeleteCourseAsync(id);
            return RedirectToAction("Index", "Courses");
        }
    }
}
