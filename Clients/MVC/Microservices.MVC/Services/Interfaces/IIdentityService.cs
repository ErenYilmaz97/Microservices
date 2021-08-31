using System.Threading.Tasks;
using IdentityModel.Client;
using Microservices.MVC.Models;
using Microservices.Shared;

namespace Microservices.MVC.Services.Interfaces
{
    public interface IIdentityService
    {
        Task<ResponseObject<bool>> Login(LoginInput input);
        Task<TokenResponse> GetAccessTokenByRefreshToken();
        Task RevokeRefreshToken();

    }
}