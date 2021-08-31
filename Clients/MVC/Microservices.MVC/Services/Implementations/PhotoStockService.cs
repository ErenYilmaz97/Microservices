using System;
using System.IO;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Microservices.MVC.Models.PhotoStock;
using Microservices.MVC.Services.Interfaces;
using Microservices.Shared;
using Microsoft.AspNetCore.Http;

namespace Microservices.MVC.Services.Implementations
{
    public class PhotoStockService : IPhotoStockService
    {

        private readonly HttpClient _httpClient;


        public PhotoStockService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }


        public async Task<PhotoViewModel> UploadPhoto(IFormFile photo)
        {
            if (photo == null || photo.Length <= 0)
            {
                return null;
            }

            var randomFileName = $"{Guid.NewGuid().ToString()}{Path.GetExtension(photo.FileName)}";

            using var ms = new MemoryStream();

            await photo.CopyToAsync(ms);

            var multipartContent = new MultipartFormDataContent();
            multipartContent.Add(new ByteArrayContent(ms.ToArray()),"photo",randomFileName);

            var response = await _httpClient.PostAsync("photos", multipartContent);

            if (!response.IsSuccessStatusCode)
            {
                return null;
            }

            var responseContent = await response.Content.ReadFromJsonAsync<ResponseObject<PhotoViewModel>>();
            return responseContent.Data;
        }



        public async Task<bool> DeletePhoto(string photoUrl)
        {
            var response = await _httpClient.DeleteAsync($"photos?photoName={photoUrl}");
            return response.IsSuccessStatusCode;
        }
    }
}