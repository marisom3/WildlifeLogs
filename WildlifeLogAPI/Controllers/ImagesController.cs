using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;
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
		public async Task<IActionResult> Upload(IFormFile file)
		{

			//call repository 
			var imageURL = await imageRepository.Upload(file);

			//check if an image url string was returned, if not then show an error
			if (imageURL == null)
			{
				return Problem("Something went wrong!", null, (int)HttpStatusCode.InternalServerError);
			}

			//upload was succesful , return a json rsponse
			return new JsonResult(new { link = imageURL });
		}

		//private void ValidateFileUpload(ImageUploadRequestDto request)
		//{
		//	//check if its a valid extention type 

		//	//make an array of the extentions we do support
		//	var allowedExtensions = new string[] { ".jpeg", ".jpg", ".png" };

		//	//if the file name of the image DOES NOT include the supported extention
		//	//then Moel error 
		//	if (allowedExtensions.Contains(Path.GetExtension(request.File.FileName)) == false)
		//	{
		//		ModelState.AddModelError("file", "Unsupported file extension");
		//	}


		//	//Validate file size
		//	if (request.File.Length > 10485760)
		//	{
		//		ModelState.AddModelError("file", "File size more than 10MB, please upload smalled image");
		//	}
		//}
	}
}
