namespace WildlifeLog.UI.Repositories
{
	public interface IImageRepository
	{
		//upload Image 
		Task<string> UploadAsync(IFormFile file);
	}
}
