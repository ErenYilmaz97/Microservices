using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Text.Json;
using System.Threading.Tasks;
using IdentityModel.Client;
using Microservices.MVC.Models;
using Microservices.MVC.Services.Interfaces;
using Microservices.Shared;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;

namespace Microservices.MVC.Services.Implementations
{
    public class IdentityService :IIdentityService
    {
        private readonly HttpClient _client;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ClientSettings _clientSettings;
        private readonly ServiceApiSettings _serviceApiSettings;


        public  IdentityService(HttpClient client, IHttpContextAccessor httpContextAccessor,
                IOptions<ClientSettings> clientSettings, IOptions<ServiceApiSettings> serviceApiSettings)
        {
            _client = client;
            _httpContextAccessor = httpContextAccessor;
            _clientSettings = clientSettings.Value;
            _serviceApiSettings = serviceApiSettings.Value;
        }



        public async Task<ResponseObject<bool>> Login(LoginInput input)
        {

            var passwordTokenRequest = new PasswordTokenRequest()
            {
                ClientId = _clientSettings.WebClientForUser.ClientId,
                ClientSecret = _clientSettings.WebClientForUser.ClientSecret,
                UserName = input.Email,
                Password = input.Password,
                Address = "http://localhost:5001/connect/token"
                
            };

            var token = await _client.RequestPasswordTokenAsync(passwordTokenRequest);

            //KULLANICI ADI VEYA ŞİFRE YANLIŞ İSE
            if (token.IsError)
            {
                var responseContent = await token.HttpResponse.Content.ReadAsStringAsync();
                var errorDto = JsonSerializer.Deserialize<ErrorDto>(responseContent, new JsonSerializerOptions()
                {
                    //LOWER-UPPER CASE SENSITIVITY
                    PropertyNameCaseInsensitive = true
                });

                return ResponseObject<bool>.CreateErrorResponse(404, errorDto.Errors.ToArray());
            }

            var userInfoRequest = new UserInfoRequest()
            {
                Token = token.AccessToken,
                Address = "http://localhost:5001/api/user/getuser"
            };


            var userInfo = await _client.GetUserInfoAsync(userInfoRequest);

            if (userInfo.IsError)
            {
                throw userInfo.Exception;
            }


            ClaimsIdentity claimsIdentity = new ClaimsIdentity(userInfo.Claims, CookieAuthenticationDefaults.AuthenticationScheme, "userName", "roles");
            ClaimsPrincipal ClaimsPrincipal = new ClaimsPrincipal(claimsIdentity);

            var authenticationProperties = new AuthenticationProperties();
            authenticationProperties.StoreTokens(new List<AuthenticationToken>()
            {
               new AuthenticationToken(){Name = OpenIdConnectParameterNames.AccessToken, Value = token.AccessToken},
               new AuthenticationToken(){Name = OpenIdConnectParameterNames.RefreshToken, Value = token.RefreshToken},
               new AuthenticationToken(){Name = OpenIdConnectParameterNames.ExpiresIn, Value = DateTime.Now.AddSeconds(token.ExpiresIn).ToString("O", CultureInfo.InvariantCulture)},

            });

            authenticationProperties.IsPersistent = input.RememberMe;
            await _httpContextAccessor.HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                  ClaimsPrincipal, authenticationProperties);

            //LOGİN BAŞARILI
            return ResponseObject<bool>.CreateSuccessResponse();

        }



        public async Task<TokenResponse> GetAccessTokenByRefreshToken()
        {
            var refreshToken = await _httpContextAccessor.HttpContext.GetTokenAsync(OpenIdConnectParameterNames.RefreshToken);

            RefreshTokenRequest request = new()
            {
                ClientId = _clientSettings.WebClientForUser.ClientId,
                ClientSecret = _clientSettings.WebClientForUser.ClientSecret,
                RefreshToken = refreshToken,
                Address = "http://localhost:5001/connect/token"
            };

            var token = await _client.RequestRefreshTokenAsync(request);

            if (token.IsError)
            {
                return null;
            }

            
            var authenticationTokens = new List<AuthenticationToken>()
            {
                new AuthenticationToken(){Name = OpenIdConnectParameterNames.AccessToken, Value = token.AccessToken},
                new AuthenticationToken(){Name = OpenIdConnectParameterNames.RefreshToken, Value = token.RefreshToken},
                new AuthenticationToken(){Name = OpenIdConnectParameterNames.ExpiresIn, Value = DateTime.Now.AddSeconds(token.ExpiresIn).ToString("O", CultureInfo.InvariantCulture)},

            };


            var authenticationResult = await _httpContextAccessor.HttpContext.AuthenticateAsync();
            var properties = authenticationResult.Properties;

            properties.StoreTokens(authenticationTokens);

            await _httpContextAccessor.HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, authenticationResult.Principal, properties);

            return token;




        }



        public async Task RevokeRefreshToken()
        {
            var refreshToken = await _httpContextAccessor.HttpContext.GetTokenAsync(OpenIdConnectParameterNames.RefreshToken);

            TokenRevocationRequest tokenRevocationRequest = new()
            {
                ClientId = _clientSettings.WebClientForUser.ClientId,
                ClientSecret = _clientSettings.WebClientForUser.ClientSecret,
                Token = refreshToken,
                TokenTypeHint = "refresh_token",
                Address = "http://localhost:5001/connect/revocation"
            };

            await _client.RevokeTokenAsync(tokenRevocationRequest);

        }
    }
}