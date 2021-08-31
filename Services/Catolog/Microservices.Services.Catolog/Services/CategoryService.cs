using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microservices.Services.Catolog.Dtos;
using Microservices.Services.Catolog.Models;
using Microservices.Services.Catolog.Settings;
using Microservices.Shared;
using MongoDB.Driver;

namespace Microservices.Services.Catolog.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly IMongoCollection<Category> _categorCollection;
        private readonly IMapper _mapper;

        //DI
        public CategoryService(IMapper mapper, IDatabaseSettings databaseSettings)
        {
            var client = new MongoClient(databaseSettings.ConnectionString);
            var database = client.GetDatabase(databaseSettings.DatabaseName);
            this._categorCollection = database.GetCollection<Category>(databaseSettings.CategoryCollectionName);    
            _mapper = mapper;
        }



        public async Task<ResponseObject<ICollection<CategoryDto>>> GetAllAsync()
        {
            var categories = await _categorCollection.Find(category => true).ToListAsync();
            return ResponseObject<ICollection<CategoryDto>>.CreateSuccessResponse(_mapper.Map<List<CategoryDto>>(categories), 200);
        }




        public async Task<ResponseObject<CategoryDto>> CreateAsync(CategoryDto categoryDto)
        {
            var newCategory = _mapper.Map<Category>(categoryDto);
            await _categorCollection.InsertOneAsync(newCategory);
            return ResponseObject<CategoryDto>.CreateSuccessResponse(_mapper.Map<CategoryDto>(newCategory), 200);
        }




        public async Task<ResponseObject<CategoryDto>> GetByIdAsync(string id)
        {
            var category = await _categorCollection.Find<Category>(x => x.Id == id).FirstOrDefaultAsync();

            if (category == null)
            {
                return ResponseObject<CategoryDto>.CreateErrorResponse(404, "Category Not Found.");
            }

            return ResponseObject<CategoryDto>.CreateSuccessResponse(_mapper.Map<CategoryDto>(category), 200);
        }

    }
}