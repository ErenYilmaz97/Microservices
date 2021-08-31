using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Microservices.MVC.Models;
using Microservices.MVC.Services.Interfaces;

namespace Microservices.MVC.Services.Implementations
{
    public class UserService : IUserService
    {
        private readonly HttpClient _httpClient;

        public UserService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }


        public async Task<UserViewModel> GetUser()
        {
            return await _httpClient.GetFromJsonAsync<UserViewModel>("/api/User/GetUser");
        }
    }
}