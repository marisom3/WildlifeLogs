using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using WildlifeLog.UI.Repositories;

namespace WildlifeLog.UI.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class ImageController : ControllerBase
	{

		private readonly IImageRepository imageRepository;

		public ImageController(IImageRepository imageRepository)
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
	}
}
