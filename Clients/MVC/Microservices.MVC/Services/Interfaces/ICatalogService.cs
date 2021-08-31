using System.Collections.Generic;
using System.Threading.Tasks;
using Microservices.MVC.Models.Catalog;

namespace Microservices.MVC.Services.Interfaces
{
    public interface ICatalogService
    {
        Task<List<CourseViewModel>> GetAllCoursesAsync();
        Task<ICollection<CategoryViewModel>> GetAllCategoriesAsync();
        Task<ICollection<CourseViewModel>> GetAllCoursesByUserIdAsync(string userId);
        Task<CourseViewModel> GetCourseById(string courseId);
        Task<bool> CreateCourseAsync(CreateCourseRequest request);
        Task<bool> UpdateCourseAsync(UpdateCourseRequest request);
        Task<bool> DeleteCourseAsync(string courseId);
    }
}