using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WildlifeLogAPI.Models.DomainModels;
using WildlifeLogAPI.Models.DTO;
using WildlifeLogAPI.Repositories;

namespace WildlifeLogAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImagesController : ControllerBase
    {
        private readonly IImageRepository imageRepository;

        public ImagesController(IImageRepository imageRepository)
        {
            this.imageRepository = imageRepository;
        }

        //POST: /api/Images/Upload 
        [HttpPost]
        [Route("Upload")]
        public async Task<IActionResult> Upload([FromForm] ImageUploadRequestDto request)
        {
            //use the private method to make sure thatt he file extentions is supported 
            ValidateFileUpload(request);

            //convert the dto to domain model 
            var imageDomainModel = new Image
            {
                File = request.File,
                FileExtension = Path.GetExtension(request.File.FileName),
                FileSizeInBytes = request.File.Length,
                FileName = request.File.FileName,
                FileDescription = request.FileDescription
            };

            //use repository to upload ikmage
            await imageRepository.Upload(imageDomainModel);

            //return domain model to user 
            return Ok(imageDomainModel);
        }

        private void ValidateFileUpload(ImageUploadRequestDto request)
        {
            //check if its a valid extention type 

            //make an array of the extentions we do support
            var allowedExtensions = new string[] { ".jpeg", ".jpg", ".png" };

            //if the file name of the image DOES NOT include the supported extention
            //then Moel error 
            if (allowedExtensions.Contains(Path.GetExtension(request.File.FileName)) == false)
            {
                ModelState.AddModelError("file", "Unsupported file extension");
            }


            //Validate file size
            if (request.File.Length > 10485760)
            {
                ModelState.AddModelError("file", "File size more than 10MB, please upload smalled image");
            }
        }
    }
}
