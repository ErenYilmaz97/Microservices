using System.Threading.Tasks;
using Microservices.MVC.Models.Basket;

namespace Microservices.MVC.Services.Interfaces
{
    public interface IBasketService
    {
        Task<bool> SaveorUpdate(BasketViewModel basket);
        Task<BasketViewModel> GetBasket();
        Task<bool> DeleteBasket();
        Task AddBasketItem(BasketItemViewModel basketItemViewModel);
        Task<bool> RemoveBasketItem(string courseId);
        Task<bool> ApplyDiscount(string discountCode);
        Task<bool> CancelApplyDiscount();
    }
}