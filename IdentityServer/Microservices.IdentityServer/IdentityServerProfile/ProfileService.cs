using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using IdentityServer4.Models;
using IdentityServer4.Services;
using Microservices.IdentityServer.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.JsonWebTokens;
using JwtRegisteredClaimNames = System.IdentityModel.Tokens.Jwt.JwtRegisteredClaimNames;

namespace Microservices.IdentityServer.IdentityServerProfile
{
    public class ProfileService : IProfileService
    {
        protected UserManager<ApplicationUser> _userManager;


        public ProfileService(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }


        public async Task GetProfileDataAsync(ProfileDataRequestContext context)
        {
            var user = await _userManager.GetUserAsync(context.Subject);
            var userRoles = await _userManager.GetRolesAsync(user) as List<string>;


            var claims = new List<Claim>
            {
                new Claim("Email", $"{user.Email}"),
                new Claim("City", user.City),
                new Claim("userName", user.UserName)
            };


            //TÜM ROLLERI CLAIM OLARAK EKLE
            userRoles.ForEach((role) => { claims.Add(new Claim(ClaimTypes.Role, role)); });
            context.IssuedClaims.AddRange(claims);
        }




        public async Task IsActiveAsync(IsActiveContext context)
        {
            var user = await _userManager.GetUserAsync(context.Subject);

            context.IsActive = user != null;
        }
    }
}