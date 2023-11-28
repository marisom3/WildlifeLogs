using WildlifeLogAPI.Models.DomainModels;

namespace WildlifeLogAPI.Repositories
{
    public interface IImageRepository
    {
        //upload Image 
        Task<string> Upload(IFormFile file);
    }
}
