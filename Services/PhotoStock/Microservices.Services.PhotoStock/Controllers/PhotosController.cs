using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microservices.Services.PhotoStock.Dtos;
using Microservices.Shared;
using Microservices.Shared.ControllerBase;
using Microsoft.AspNetCore.Authorization;

namespace Microservices.Services.PhotoStock.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class PhotosController : CustomControllerBase
    {

        [HttpPost]
        public async Task<IActionResult> PhotoSave(IFormFile photo, CancellationToken cancellationToken)
        {
            if (photo != null && photo.Length > 0)
            {
                var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/photos", photo.FileName);

                using var stream = new FileStream(path, FileMode.Create);
                
                await photo.CopyToAsync(stream, cancellationToken);

                var returnPath = photo.FileName;

                var photoDto = new PhotoDto(returnPath);

                return CreateActionResultInstance(ResponseObject<PhotoDto>.CreateSuccessResponse(photoDto, 200));

            }

            return CreateActionResultInstance(ResponseObject<NoContentObject>.CreateErrorResponse(400, "Photo is Empty."));

        }




          
        [HttpDelete]
        public IActionResult PhotoDelete([FromQuery] string photoName)
        {
            var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/photos", photoName);

            if (!System.IO.File.Exists(path))
            {
                return CreateActionResultInstance(ResponseObject<NoContentObject>.CreateErrorResponse(404,"Photo Not Found."));
            }


            System.IO.File.Delete(path);
            return CreateActionResultInstance(ResponseObject<string>.CreateSuccessResponse("Photo Deleted.", 200));
        }
    }
}
