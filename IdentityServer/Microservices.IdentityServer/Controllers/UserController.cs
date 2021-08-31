using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IdentityServer4;
using Microservices.IdentityServer.Dtos;
using Microservices.IdentityServer.Models;
using Microservices.Shared;
using Microservices.Shared.Microservices.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.JsonWebTokens;

namespace Microservices.IdentityServer.Controllers
{
    [Authorize(IdentityServerConstants.LocalApi.PolicyName)]
    [Route("api/[controller]/[Action]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;


        public UserController(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }



        [HttpPost]
        public async Task<IActionResult> Register(RegisterDto registerDto)
        {
            var user = new ApplicationUser()
            {
                UserName = registerDto.Email,
                Email = registerDto.Email,
                City = registerDto.City,
            };

            var result = await _userManager.CreateAsync(user, registerDto.Password);
             
            if (!result.Succeeded)
            {
                return BadRequest(ResponseObject<NoContentObject>.CreateErrorResponse(404, result.Errors.Select(x=>x.Description).ToArray()));
            }

            return Ok(ResponseObject<ApplicationUser>.CreateSuccessResponse(user, 200)); 
        }




        [HttpGet]
        public async Task<IActionResult> GetAllUserIds()
        {
            var userIds =  await _userManager.Users.Select(x=> x.Id).ToListAsync();
            return Ok(userIds);
        }



        [HttpGet]
        public async Task<IActionResult> GetUser()
        {
            var userIdClaim = User.Claims.FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Sub);


            if (userIdClaim == null)
            {
                return BadRequest(ResponseObject<string>.CreateErrorResponse(404, "Token Doğrulanamadı."));
            }

            var currentUser = await _userManager.FindByIdAsync(userIdClaim.Value);

            if (currentUser == null)
            {
                return BadRequest(ResponseObject<string>.CreateErrorResponse(404, "Kullanıcı Bulunamadı."));
            }

            var userRoles = await _userManager.GetRolesAsync(currentUser);

            return Ok(new UserViewModel
            {
                Id = currentUser.Id,
                UserName = currentUser.UserName, 
                Email = currentUser.Email, 
                City = currentUser.City,
                Roles = userRoles.ToList()
            });

        }





    }
}
