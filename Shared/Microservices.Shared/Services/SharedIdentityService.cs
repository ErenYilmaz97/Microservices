using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Http;

namespace Microservices.Shared.Services
{
    public class SharedIdentityService : ISharedIdentityService
    {
        private readonly IHttpContextAccessor _httpContext;


        public SharedIdentityService(IHttpContextAccessor httpContext)
        {
            _httpContext = httpContext;
        }


        public string GetCurrentUserId() => _httpContext.HttpContext.User.FindFirst("sub").Value;
        public string GetCurrentUserIdFromClient() => _httpContext.HttpContext.User.FindFirst("id").Value;

        //public string GetUserId
        //{
        //    get => _httpContext.HttpContext.User.FindFirst("sub").Value; 
        //}

    }
}