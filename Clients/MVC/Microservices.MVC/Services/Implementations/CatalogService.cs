using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Microservices.MVC.Helpers;
using Microservices.MVC.Models.Catalog;
using Microservices.MVC.Services.Interfaces;
using Microservices.Shared;

namespace Microservices.MVC.Services.Implementations
{
    public class CatalogService : ICatalogService
    {

        private readonly HttpClient _httpClient;
        private readonly IPhotoStockService _photoStockService;
        private readonly PhotoHelper _photoHelper;


        public CatalogService(HttpClient httpClient, IPhotoStockService photoStockService, PhotoHelper photoHelper)
        {
            _httpClient = httpClient;
            _photoStockService = photoStockService;
            _photoHelper = photoHelper;
        }


        public async Task<List<CourseViewModel>> GetAllCoursesAsync()
        {
            //http:localhost:5000/services/catalog/courses (GET)
            var response = await _httpClient.GetAsync("Courses");

            if (!response.IsSuccessStatusCode)
            {
                return null;
            }

            var responseContent = await response.Content.ReadFromJsonAsync<ResponseObject<ICollection<CourseViewModel>>>();
            return SetCoursesPictures(responseContent.Data.ToList());
        }



        public async Task<ICollection<CategoryViewModel>> GetAllCategoriesAsync()
        {
            //http:localhost:5000/services/catalog/categories (GET)
            var response = await _httpClient.GetAsync("Categories");

            if (!response.IsSuccessStatusCode)
            {
                return null;
            }

            var responseContent = await response.Content.ReadFromJsonAsync<ResponseObject<ICollection<CategoryViewModel>>>();
            return responseContent.Data;
        }



        public async Task<ICollection<CourseViewModel>> GetAllCoursesByUserIdAsync(string userId)
        {
            //http:localhost:5000/services/catalog/courses/getallbyuserid/{userId} (GET)
            var response = await _httpClient.GetAsync($"Courses/GetAllByUserId/{userId}");

            if (!response.IsSuccessStatusCode)
            {
                return null;
            }

            var responseContent = await response.Content.ReadFromJsonAsync<ResponseObject<List<CourseViewModel>>>();
            return SetCoursesPictures(responseContent.Data);

        }



        public async Task<CourseViewModel> GetCourseById(string courseId)
        {
            //http:localhost:5000/services/catalog/courses/{courseId} (GET)
            var response = await _httpClient.GetAsync($"Courses/{courseId}");

            if (!response.IsSuccessStatusCode)
            {
                return null;
            }

            var responseContent = await response.Content.ReadFromJsonAsync<ResponseObject<CourseViewModel>>();
            responseContent.Data.PictureUrl = _photoHelper.GetPhotoStockUrl(responseContent.Data.Picture);
            return responseContent.Data;
        }



        public async Task<bool> CreateCourseAsync(CreateCourseRequest request)
        {

            var resultPhotoService = await _photoStockService.UploadPhoto(request.Photo);

            if (resultPhotoService != null)
            {
                request.Picture = resultPhotoService.URL;
            }

            //http:localhost:5000/services/catalog/courses (POST)
            var response = await _httpClient.PostAsJsonAsync<CreateCourseRequest>("Courses", request);
            return response.IsSuccessStatusCode;
        }



        public async Task<bool> UpdateCourseAsync(UpdateCourseRequest request)
        {
            var resultPhotoService = await _photoStockService.UploadPhoto(request.Photo);

            if (resultPhotoService != null)
            {
                //ESKİSİNİ SİL
                await _photoStockService.DeletePhoto(request.Picture);
                request.Picture = resultPhotoService.URL;
            }

            //http:localhost:5000/services/catalog/courses (PUT)
            var response = await _httpClient.PutAsJsonAsync<UpdateCourseRequest>("Courses", request);
            return response.IsSuccessStatusCode;
        }

         

        public async Task<bool> DeleteCourseAsync(string courseId)
        {
            //http:localhost:5000/services/catalog/courses/{courseId} (DELETE)
            var response = await _httpClient.DeleteAsync($"Courses/{courseId}");
            return response.IsSuccessStatusCode;
        }



        private List<CourseViewModel> SetCoursesPictures(List<CourseViewModel> courses)
        {
            courses.ForEach(x =>
            {
                x.PictureUrl = _photoHelper.GetPhotoStockUrl(x.Picture);
            });

            return courses;
        }
    }
}