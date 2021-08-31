using System.Threading.Tasks;
using Microservices.Services.Basket.Dtos;
using Microservices.Shared;

namespace Microservices.Services.Basket.Services
{
    public interface IBasketService
    {
        Task<ResponseObject<BasketDto>> GetBasket(string userId);
        Task<ResponseObject<bool>> SaveOrUpdate(BasketDto basketDto);
        Task<ResponseObject<bool>> DeleteBasket(string userId);
    }
}