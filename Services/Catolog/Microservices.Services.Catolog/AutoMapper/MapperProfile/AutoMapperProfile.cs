using AutoMapper;
using Microservices.Services.Catolog.Dtos;
using Microservices.Services.Catolog.Models;

namespace Microservices.Services.Catolog.AutoMapper.MapperProfile
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<Course, CourseDto>().ReverseMap();
            CreateMap<Category, CategoryDto>().ReverseMap();
            CreateMap<Feature, FeatureDto>().ReverseMap();

            CreateMap<CreateCourseDto, Course>();
            CreateMap<UpdateCourseDto, Course>();
        }

        
    }
}