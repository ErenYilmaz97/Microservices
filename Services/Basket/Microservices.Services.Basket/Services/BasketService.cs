using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;
using Microservices.Services.Basket.Dtos;
using Microservices.Shared;
using StackExchange.Redis;

namespace Microservices.Services.Basket.Services
{
    public class BasketService : IBasketService
    {
        private readonly RedisService _redisService;

        public BasketService(RedisService redisService)
        {
            _redisService = redisService;
        }



        public async Task<ResponseObject<BasketDto>> GetBasket(string userId)
        {
            var basket = await _redisService.GetRedisDatabase().StringGetAsync($"baskets/{userId}");

            if (basket.IsNullOrEmpty)
            {
                BasketDto newBasket = new()
                {
                    UserId = userId,
                    BasketItems = new List<BasketItemDto>()
                };
              
                await _redisService.GetRedisDatabase().StringSetAsync($"baskets/{userId}", JsonSerializer.Serialize(newBasket));
                return ResponseObject<BasketDto>.CreateSuccessResponse(newBasket, 200);
            }

            var baskett = JsonSerializer.Deserialize<BasketDto>(basket);
            return ResponseObject<BasketDto>.CreateSuccessResponse(baskett , 200);
        }




        public async Task<ResponseObject<bool>> SaveOrUpdate(BasketDto basketDto)
        {
           var cacheResult = await _redisService.GetRedisDatabase().StringSetAsync($"baskets/{basketDto.UserId}", JsonSerializer.Serialize(basketDto));

           return cacheResult
                  ? ResponseObject<bool>.CreateSuccessResponse(true, 200)
                  : ResponseObject<bool>.CreateErrorResponse(500, "Basket Could Not Update Or Save.");
        }
        



        public async Task<ResponseObject<bool>> DeleteBasket(string userId)
        {
            var cachedBasket = await _redisService.GetRedisDatabase().StringGetAsync($"baskets/{userId}");

            if (!cachedBasket.IsNullOrEmpty)
            {
                var basketDto = JsonSerializer.Deserialize<BasketDto>(cachedBasket);
                basketDto.BasketItems = new List<BasketItemDto>();
                basketDto.DiscountCode = string.Empty;
                var result = SaveOrUpdate(basketDto).Result.Data;

                //BULURSA AMA GÜNCELLEYEMEZSE
                if(!result)
                    return ResponseObject<bool>.CreateErrorResponse(400, "An error occurred while updating the basket.");
            }

            return ResponseObject<bool>.CreateSuccessResponse(true, 200);
        }

    }
}