using System.Threading.Tasks;
using Microservices.MVC.Models;

namespace Microservices.MVC.Services.Interfaces
{
    public interface IUserService
    {
        Task<UserViewModel> GetUser();
    }
}