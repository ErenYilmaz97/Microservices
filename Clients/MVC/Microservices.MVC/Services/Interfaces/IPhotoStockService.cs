using System.Threading.Tasks;
using Microservices.MVC.Models.PhotoStock;
using Microservices.Shared;
using Microsoft.AspNetCore.Http;

namespace Microservices.MVC.Services.Interfaces
{
    public interface IPhotoStockService
    {
        Task<PhotoViewModel> UploadPhoto(IFormFile photo);
        Task<bool> DeletePhoto(string photoUrl);
    }
}