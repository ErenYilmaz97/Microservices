using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microservices.Services.Catolog.Dtos;
using Microservices.Services.Catolog.Services;
using Microservices.Shared.ControllerBase;

namespace Microservices.Services.Catolog.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CoursesController : CustomControllerBase
    {
        private readonly ICourseService _courseService;


        public CoursesController(ICourseService courseService)
        {
            _courseService = courseService;
        }



        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var response = await _courseService.GetAllAsync();
            return CreateActionResultInstance(response);
        }



        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> GetById(string id)
        {
            var response = await _courseService.GetByIdAsync(id);
           return CreateActionResultInstance(response);
        }



        [HttpGet]
        [Route("GetAllByUserId/{userId}")]
        public async Task<IActionResult> GetAllByUserId(string userId)
        {
            var response = await _courseService.GetAllByUserIdAsync(userId);
            return CreateActionResultInstance(response);
        }



        [HttpPost]
        public async Task<IActionResult> Create(CreateCourseDto createCourseDto)
        {
            var response = await _courseService.CreateAsync(createCourseDto);
            return CreateActionResultInstance(response);
        }



        [HttpPut]
        public async Task<IActionResult> Update(UpdateCourseDto updateCourseDto)
        {
            var response = await _courseService.UpdateAsync(updateCourseDto);
            return CreateActionResultInstance(response);
        }



        [HttpDelete]
        [Route("{courseId}")]
        public async Task<IActionResult> Delete(string courseId)
        {
            var response = await _courseService.DeleteAsync(courseId);
            return CreateActionResultInstance(response);
        }

    }
}
