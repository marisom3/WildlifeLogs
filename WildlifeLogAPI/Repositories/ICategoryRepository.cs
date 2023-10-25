using WildlifeLogAPI.Models.DomainModels;

namespace WildlifeLogAPI.Repositories
{
	public interface ICategoryRepository
	{

		//Get all categories
		Task<List<Category>> GetAllAsync();
	}
}
