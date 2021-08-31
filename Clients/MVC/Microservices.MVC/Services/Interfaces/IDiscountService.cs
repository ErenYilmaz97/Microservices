using System.Threading.Tasks;
using Microservices.MVC.Models.Discount;

namespace Microservices.MVC.Services.Interfaces
{
    public interface IDiscountService
    {
        Task<DiscountViewModel> GetDiscount(string discountCode);
    }
}