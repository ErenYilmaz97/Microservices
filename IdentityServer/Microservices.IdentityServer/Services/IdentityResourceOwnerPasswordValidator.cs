using System.Collections.Generic;
using System.Threading.Tasks;
using IdentityModel;
using IdentityServer4.Validation;
using Microservices.IdentityServer.Models;
using Microsoft.AspNetCore.Identity;

namespace Microservices.IdentityServer.Services
{
    public class IdentityResourceOwnerPasswordValidator : IResourceOwnerPasswordValidator
    {
        private readonly UserManager<ApplicationUser> _userManager;
        

        //ASP CORE IDENTITY ILE KULLANICI DOGRULAMA ISLEMI YAPACAGIZ
        public IdentityResourceOwnerPasswordValidator(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }



        public async Task ValidateAsync(ResourceOwnerPasswordValidationContext context)
        {
            var currentUser = _userManager.FindByEmailAsync(context.UserName).Result;

            if (currentUser == null)
            {
                context.Result.CustomResponse = new Dictionary<string, object>()
                {
                    {"errors", new List<string>(){"Kullanıcı Bulunamadı."}}
                };

                return;
            }

            var passwordCheck = _userManager.CheckPasswordAsync(currentUser, context.Password).Result;


            if (!passwordCheck)
            {
                context.Result.CustomResponse = new Dictionary<string, object>()
                {
                    {"errors", new List<string>(){"Yanlış Şifre."}}
                };

                return;
            }

            
            //KULLANICI DOĞRULANDI. GERİYE OTOMATIK TOKEN DÖNER.
            context.Result = new GrantValidationResult(currentUser.Id.ToString(), OidcConstants.AuthenticationMethods.Password);

        }
    }
}