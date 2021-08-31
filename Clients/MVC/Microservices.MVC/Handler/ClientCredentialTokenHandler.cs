using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using Microservices.MVC.Exception;
using Microservices.MVC.Services.Implementations;

namespace Microservices.MVC.Handler
{
    public class ClientCredentialTokenHandler : DelegatingHandler
    {
        private readonly IClientCredentialsTokenService _clientCredentialsTokenService;

        public ClientCredentialTokenHandler(IClientCredentialsTokenService clientCredentialsTokenService)
        {
            _clientCredentialsTokenService = clientCredentialsTokenService;
        }


        //REQUEST ATILACAĞI ZAMAN ARAYA GİR
        protected async override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", await _clientCredentialsTokenService.GetToken());

            var response = await base.SendAsync(request, cancellationToken);

            if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                throw new UnAuthorizedException();
            }

            return response;
        }
    }
}