using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microservices.Services.Basket.Dtos;
using Microservices.Services.Basket.Services;
using Microservices.Shared.ControllerBase;
using Microservices.Shared.Services;

namespace Microservices.Services.Basket.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BasketsController : CustomControllerBase
    {
        private readonly IBasketService _basketService;
        private readonly ISharedIdentityService _sharedIdentityService;

        public BasketsController(IBasketService basketService, ISharedIdentityService sharedIdentityService)
        {
            _basketService = basketService;
            _sharedIdentityService = sharedIdentityService;
        }




        [HttpGet]
        public async Task<IActionResult> GetBasket()
        {
            string currentUserId = _sharedIdentityService.GetCurrentUserId();
            return CreateActionResultInstance(await _basketService.GetBasket(currentUserId));
        }



        [HttpPost]
        public async Task<IActionResult> SaveOrUpdateBasket(BasketDto basketDto)
        {
            basketDto.UserId = _sharedIdentityService.GetCurrentUserId();
            var response = await _basketService.SaveOrUpdate(basketDto);
            return CreateActionResultInstance(response);
        }


        [HttpDelete]
        public async Task<IActionResult> DeleteBasket()
        {
            string currentUserId = _sharedIdentityService.GetCurrentUserId();
            return CreateActionResultInstance(await _basketService.DeleteBasket(currentUserId));
        }

    }
}
