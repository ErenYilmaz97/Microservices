using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microservices.Services.Discount.Services;
using Microservices.Shared.ControllerBase;
using Microservices.Shared.Services;

namespace Microservices.Services.Discount.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DiscountsController : CustomControllerBase
    {
        private readonly IDiscountService _discountService;
        private readonly ISharedIdentityService _sharedIdentityService;

        public DiscountsController(IDiscountService discountService, ISharedIdentityService sharedIdentityService)
        {
            _sharedIdentityService = sharedIdentityService;
            _discountService = discountService;
        }


        [HttpGet]
        public async Task<IActionResult> Getall()
        {
            return CreateActionResultInstance(await _discountService.GetAll());
        }


        [HttpGet]
        [Route("{discountId:int}")]
        public async Task<IActionResult> GetbyId(int discountId)
        {
            var discount = await _discountService.GetById(x => x.Id.Equals(discountId));
            return CreateActionResultInstance(discount);
        }



        [HttpGet]
        [Route("[Action]/{code}")]
        public async Task<IActionResult> GetByCodeAndUserId(string code)
        {
            var currentUserId =  _sharedIdentityService.GetCurrentUserId();
            var discount = await _discountService.GetByCodeAndUserId(code, currentUserId);
            return CreateActionResultInstance(discount);
        }



        [HttpPost]
        public async Task<IActionResult> InsertDiscount(Models.Discount discount)
        {
            return CreateActionResultInstance(await _discountService.Insert(discount));
        }



        [HttpPut]
        public async Task<IActionResult> UpdateDiscount(Models.Discount discount)
        {
            return CreateActionResultInstance(await _discountService.Update(discount));
        }



        [HttpDelete]
        [Route("{discountId:int}")]
        public async Task<IActionResult> RemoveDiscount(int discountId)
        {
            return CreateActionResultInstance(await _discountService.Delete(discountId));
        }


    }
}
