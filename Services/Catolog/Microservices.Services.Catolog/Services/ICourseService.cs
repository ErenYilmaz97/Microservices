using System.Collections.Generic;
using System.Threading.Tasks;
using Microservices.Services.Catolog.Dtos;
using Microservices.Shared;

namespace Microservices.Services.Catolog.Services
{
    public interface ICourseService
    {
        Task<ResponseObject<ICollection<CourseDto>>> GetAllAsync();
        Task<ResponseObject<CourseDto>> GetByIdAsync(string id);
        Task<ResponseObject<ICollection<CourseDto>>> GetAllByUserIdAsync(string userId);
        Task<ResponseObject<CourseDto>> CreateAsync(CreateCourseDto createCourseDto);
        Task<ResponseObject<NoContentObject>> UpdateAsync(UpdateCourseDto updateCourseDto);
        Task<ResponseObject<NoContentObject>> DeleteAsync(string id);
    }
}