using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microservices.Shared;

namespace Microservices.Services.Discount.Services
{
    public interface IDiscountService
    {
        Task<ResponseObject<ICollection<Models.Discount>>> GetAll();
        Task<ResponseObject<Models.Discount>> GetById(Func<Models.Discount, bool> predicate);
        Task<ResponseObject<Models.Discount>> Insert(Models.Discount discount);
        Task<ResponseObject<Models.Discount>> Update(Models.Discount discount);
        Task<ResponseObject<NoContentObject>> Delete(int discountId);
        Task<ResponseObject<Models.Discount>> GetByCodeAndUserId(string code, string userId);
    }
}