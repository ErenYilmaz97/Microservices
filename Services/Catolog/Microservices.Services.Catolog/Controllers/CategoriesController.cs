using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microservices.Services.Catolog.Dtos;
using Microservices.Services.Catolog.Services;
using Microservices.Shared.ControllerBase;
using Microsoft.AspNetCore.Authorization;

namespace Microservices.Services.Catolog.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : CustomControllerBase
    {
        private readonly ICategoryService _categoryService;


        //DI
        public CategoriesController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }



        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var response = await _categoryService.GetAllAsync();
            return CreateActionResultInstance(response);
        }



        [HttpGet]
        [Route("{categoryId}")]
        public async Task<IActionResult> GetById(string categoryId)
        {
            var response = await _categoryService.GetByIdAsync(categoryId);
            return CreateActionResultInstance(response);
        }



        [HttpPost]
        public async Task<IActionResult> Create(CategoryDto categoryDto)
        {
            var response = await _categoryService.CreateAsync(categoryDto);
            return CreateActionResultInstance(response);
        }



        //ROLE BASE - CLAIM BASE AUTHENTICATION
        //[HttpGet]
        //[Authorize(Roles = "Role 11")]
        //[Route("deneme")]
        //public IActionResult deneme()
        //{
        //    return Ok("Token Doğrulandı.");
        //}
    }
}
