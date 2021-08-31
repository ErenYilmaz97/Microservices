using System.Threading.Tasks;

namespace Microservices.MVC.Services.Implementations
{
    public interface IClientCredentialsTokenService
    {
        Task<string> GetToken();
    }
}