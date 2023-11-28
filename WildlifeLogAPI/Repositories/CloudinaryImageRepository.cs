using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Http;
using WildlifeLogAPI.Data;
using WildlifeLogAPI.Models.DomainModels;


namespace WildlifeLogAPI.Repositories
{
	public class CloudinaryImageRepository : IImageRepository
	{
		private readonly IConfiguration configuration;
		private readonly Account account; 

		public CloudinaryImageRepository(IConfiguration configuration)
        {
			this.configuration = configuration;

			//create a CLoudinary account 
			account = new Account(
				configuration.GetSection("Cloudinary")["CloudName"],
				configuration.GetSection("Cloudinary")["ApiKey"],
				configuration.GetSection("Cloudinary")["ApiSecret"]);
		}

        public async Task<string> Upload(IFormFile file)
		{
			//create a new client 
			var client = new Cloudinary(account);

			//upload params
			var uploadParams = new ImageUploadParams()
			{
				File = new FileDescription(file.FileName, file.OpenReadStream()),
				DisplayName = file.FileName
			};

			//call the api to upload the file
			var uploadResult = await client.UploadAsync(uploadParams);

			//check if result was successful or not ( aka if the file was uploaded or not)
			if (uploadResult != null && uploadResult.StatusCode == System.Net.HttpStatusCode.OK)
			{
				//return the url 
				return uploadResult.SecureUrl.ToString();
			}
			return null;
		}

	}
}
