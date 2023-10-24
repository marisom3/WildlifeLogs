using WildlifeLogAPI.Models.DomainModels;

namespace WildlifeLogAPI.Repositories
{
    public interface IImageRepository
    {
        //upload Image 
        Task<Image> Upload(Image image);
    }
}
