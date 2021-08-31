using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microservices.Services.Catolog.Dtos;
using Microservices.Services.Catolog.Models;
using Microservices.Shared;

namespace Microservices.Services.Catolog.Services
{
    public interface ICategoryService
    {
        Task<ResponseObject<ICollection<CategoryDto>>> GetAllAsync();
        Task<ResponseObject<CategoryDto>> CreateAsync(CategoryDto categoryDto);
        Task<ResponseObject<CategoryDto>> GetByIdAsync(string id);
    }
}
