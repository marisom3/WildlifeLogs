using Microsoft.AspNetCore.Http;
using WildlifeLogAPI.Data;
using WildlifeLogAPI.Models.DomainModels;


namespace WildlifeLogAPI.Repositories
{
    public class ImageRepository : IImageRepository
    {
        private readonly IWebHostEnvironment webHostEnvironment;
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly WildlifeLogDbContext dbContext;

        public ImageRepository(IWebHostEnvironment webHostEnvironment, IHttpContextAccessor httpContextAccessor, WildlifeLogDbContext dbContext)
        {
            this.webHostEnvironment = webHostEnvironment;
            this.httpContextAccessor = httpContextAccessor;
            this.dbContext = dbContext;
        }

        public async Task<Image> Upload(Image image)
        {
            //Create local file path 
            var localFilePath = Path.Combine(webHostEnvironment.ContentRootPath, 
                "Images", $"{image.FileName}{image.FileExtension}");

            //upload the image to the local path 
            using var stream = new FileStream(localFilePath, FileMode.Create);
            await image.File.CopyToAsync(stream);

            var urlFilePath = $"{httpContextAccessor.HttpContext.Request.Scheme}://" +
                $"{httpContextAccessor.HttpContext.Request.Host}{httpContextAccessor.HttpContext.Request.PathBase}" +
                $"/Images/{image.FileName}{image.FileExtension}";

            image.FilePath = urlFilePath;

            //Add Image to the Images table in the database 
            await dbContext.Images.AddAsync(image);
            await dbContext.SaveChangesAsync();
            return image;
        
        }
    }
}
