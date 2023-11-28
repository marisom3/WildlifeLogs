namespace WildlifeLog.UI.Repositories
{
	public interface IImageRepository
	{
		//upload Image 
		Task<string> Upload(IFormFile file);
	}
}
