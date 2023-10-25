using Microsoft.EntityFrameworkCore;
using WildlifeLogAPI.Data;
using WildlifeLogAPI.Models.DomainModels;

namespace WildlifeLogAPI.Repositories
{
	public class CategoryRepository : ICategoryRepository
	{
		//Inject dbContext
		private readonly WildlifeLogDbContext dbContext;

		public CategoryRepository(WildlifeLogDbContext dbContext)
		{
			this.dbContext = dbContext;
		}


		public async Task<List<Category>> GetAllAsync()
		{
			return await dbContext.AnimalCategories.ToListAsync();
		}
	}
}

