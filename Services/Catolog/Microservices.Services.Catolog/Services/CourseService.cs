 using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
 using MassTransit;
 using Microservices.Services.Catolog.Dtos;
using Microservices.Services.Catolog.Models;
using Microservices.Services.Catolog.Settings;
using Microservices.Shared;
 using Microservices.Shared.Messages;
 using MongoDB.Driver;

namespace Microservices.Services.Catolog.Services
{
    public class CourseService : ICourseService
    {
        private readonly IMongoCollection<Course> _courseCollection;
        private readonly IMongoCollection<Category> _categoryCollection;
        private readonly IMapper _mapper;
        private readonly IPublishEndpoint _publishEndpoint;

        //DI
        public CourseService(IMapper mapper, IDatabaseSettings databaseSettings, IPublishEndpoint publishEndpoint)
        {
            var client = new MongoClient(databaseSettings.ConnectionString);
            var database = client.GetDatabase(databaseSettings.DatabaseName);
            this._courseCollection = database.GetCollection<Course>(databaseSettings.CourseCollectionName);
            this._categoryCollection = database.GetCollection<Category>(databaseSettings.CategoryCollectionName);
            _mapper = mapper;
            _publishEndpoint = publishEndpoint;
        }



        public async Task<ResponseObject<ICollection<CourseDto>>> GetAllAsync()
        {
            var courses = await _courseCollection.Find(course => true).ToListAsync();

            if (courses.Any())
            {
                courses.ForEach(async (course) =>
                {
                    course.Category = await _categoryCollection.Find<Category>(x=>x.Id == course.CategoryId).FirstAsync();
                });
            }

            else
            {
                courses = new List<Course>();
            }

            return ResponseObject<ICollection<CourseDto>>.CreateSuccessResponse(_mapper.Map<List<CourseDto>>(courses), 200);
        }



        public async Task<ResponseObject<CourseDto>> GetByIdAsync(string id)
        {
            var course = await _courseCollection.Find<Course>(x => x.Id == id).FirstOrDefaultAsync();

            if (course == null)
            {
                return ResponseObject<CourseDto>.CreateErrorResponse(404,"Course Not Found.");
            }

            course.Category = await _categoryCollection.Find<Category>(x => x.Id == course.CategoryId).FirstAsync();
            return ResponseObject<CourseDto>.CreateSuccessResponse(_mapper.Map<CourseDto>(course), 200);

        }



        public async Task<ResponseObject<ICollection<CourseDto>>> GetAllByUserIdAsync(string userId)
        {
            var courses = await _courseCollection.Find<Course>(x => x.UserId == userId).ToListAsync();


            if (courses.Any())
            {
                courses.ForEach(async (course) =>
                {
                    course.Category = await _categoryCollection.Find<Category>(x => x.Id == course.CategoryId).FirstAsync();
                });
            }

            else
            {
                courses = new List<Course>();
            }

            return ResponseObject<ICollection<CourseDto>>.CreateSuccessResponse(_mapper.Map<List<CourseDto>>(courses), 200);
        }



        public async Task<ResponseObject<CourseDto>> CreateAsync(CreateCourseDto createCourseDto)
        {
            var newCourse = _mapper.Map<Course>(createCourseDto);
            await _courseCollection.InsertOneAsync(newCourse);

            return ResponseObject<CourseDto>.CreateSuccessResponse(_mapper.Map<CourseDto>(newCourse), 200); 
        }



        public async Task<ResponseObject<NoContentObject>> UpdateAsync(UpdateCourseDto updateCourseDto)
        {
            var updateCourse = _mapper.Map<Course>(updateCourseDto);

            var result = await _courseCollection.FindOneAndReplaceAsync(x => x.Id == updateCourseDto.Id, updateCourse);

            if (result == null)
            {
                return ResponseObject<NoContentObject>.CreateErrorResponse(404, "Course Not Found.");
            }

            //ORDER MICROSERVICE
            await _publishEndpoint.Publish<CourseNameChangedEvent>(new CourseNameChangedEvent()
            {
                CourseId = updateCourse.Id,
                UpdatedCourseName = updateCourse.Name
            });

            return ResponseObject<NoContentObject>.CreateSuccessResponse(204);
        }



        public async Task<ResponseObject<NoContentObject>> DeleteAsync(string id)
        {
            var result = await _courseCollection.DeleteOneAsync(x => x.Id == id);

            if (result.DeletedCount > 0)
            {
                return ResponseObject<NoContentObject>.CreateSuccessResponse(204);
            }

            return ResponseObject<NoContentObject>.CreateErrorResponse(404,"Course Not Found.");
        }
    }
}