using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using Microservices.MVC.Exception;
using Microservices.MVC.Services.Interfaces;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;

namespace Microservices.MVC.Handler
{
    public class ResourceOwnerPasswordTokenHandler : DelegatingHandler
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IIdentityService _identityService;


        public ResourceOwnerPasswordTokenHandler(IHttpContextAccessor httpContextAccessor, IIdentityService identityService)
        {
            _httpContextAccessor = httpContextAccessor;
            _identityService = identityService;
        }


        //REQUEST ATILACAĞI ZAMAN ARAYA GİR
        protected async override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var accessToken = await _httpContextAccessor.HttpContext.GetTokenAsync(OpenIdConnectParameterNames.AccessToken);

            //REQUESTE TOKENI EKLE
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            //İSTEĞİ YOLLA
            var response = await base.SendAsync(request, cancellationToken);


            //401 DÖNERSE, TOKEN EXPIRE OLMUŞ OLABİLİR.
            if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                //REFRESH TOKEN ILE YENİ TOKEN AL.
                var tokenResponse = await _identityService.GetAccessTokenByRefreshToken();

                //YENİ TOKEN İLE İSTEK AT. 
                if (tokenResponse != null)
                {
                    request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", tokenResponse.AccessToken);
                    response = await base.SendAsync(request, cancellationToken);
                }
            }


            //RESPONSE HALA 401 İSE, LOGİN OLMASI GEREKİYOR.
            if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                throw new UnAuthorizedException();
            }


            return response;

        }
    }
}