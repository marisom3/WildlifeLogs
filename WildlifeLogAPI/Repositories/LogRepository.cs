using Microsoft.EntityFrameworkCore;
using WildlifeLogAPI.Data;
using WildlifeLogAPI.Models.DomainModels;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;


namespace WildlifeLogAPI.Repositories
{
	public class LogRepository : ILogRepository
	{
		private readonly WildlifeLogDbContext dbContext;

		//Inject dbContext + automapper


		public LogRepository(WildlifeLogDbContext dbContext)
		{
			this.dbContext = dbContext;
		}


		public async Task<Log> CreateAsync(Log log)
		{
			await dbContext.Logs.AddAsync(log);
			await dbContext.SaveChangesAsync();
			return log;
		}

		public async Task<Log?> DeleteAsync(Guid id)
		{
			//Check if id exists 
			var existingLog = await dbContext.Logs.FirstOrDefaultAsync(x => x.Id == id);

			//if null return null 
			if (existingLog == null)
			{
				return null;
			}

			//if not null then delete rhat hoe 
			dbContext.Logs.Remove(existingLog);
			await dbContext.SaveChangesAsync();

			//return
			return existingLog;
		}

		public async Task<List<Log>> GetAllAsync(string? filterOn = null, string? filterQuery = null,
			string? sortBy = null, bool isAscending = true, int pageNumber = 1, int pageSize = 20, Guid? parkId = null)
		{
			//get all logs 
			var logs = dbContext.Logs.Include("Park").Include("Category").AsQueryable();

			//Filtering 
			//check if parameters are not null 
			if (string.IsNullOrWhiteSpace(filterOn) == false && string.IsNullOrEmpty(filterQuery) == false)
			{
				//check that the column is "ObserverName"
				if (filterOn.Equals("ObserverName", StringComparison.OrdinalIgnoreCase))
				{
					//check for anything in the Park's Name column that contains the second parameter filterQuery

					logs = logs.Where(log => log.ObserverName == filterQuery);

				}
			}

			// Additional filter for ParkId
			if (parkId.HasValue)
			{
				logs = logs.Where(log => log.ParkId == parkId.Value);
			}


			//Sorting 
			//check if the sortBy column has a value, if so run this code  
			if (string.IsNullOrWhiteSpace(sortBy) == false)
			{
				if (sortBy.Equals("Date", StringComparison.OrdinalIgnoreCase))
				{
					logs = isAscending ? logs.OrderBy(x => x.Date) : logs.OrderByDescending(x => x.Date);
				}
			}
			//Pagination 

			//# of results we skip
			var skipResults = (pageNumber - 1) * pageSize;

			return await logs.Skip(skipResults).Take(pageSize).ToListAsync();
		}
		public async Task<int> GetTotalCountAsync(string? observerName, Guid? parkId, string? filterOn = null, string? filterQuery = null)
		{
			// set up base query
			var query = dbContext.Logs.AsQueryable();

            // Filter by ObserverName if it is not null or empty
            if (!string.IsNullOrWhiteSpace(observerName))
            {
                query = query.Where(log => log.ObserverName == observerName);
            }


            // Filter by ParkId if provided
            if (parkId.HasValue)
			{
				query = query.Where(log => log.ParkId == parkId.Value);
			}

			// Apply additional filters if provided
			if (!string.IsNullOrWhiteSpace(filterOn) && !string.IsNullOrWhiteSpace(filterQuery))
			{
				if (filterOn.Equals("ObserverName", StringComparison.OrdinalIgnoreCase))
				{
					query = query.Where(log => log.ObserverName == filterQuery);
				}
				
			}

			var totalCount = await query.CountAsync();

			return totalCount;
		}


		public async Task<Log?> GetByIdAsync(Guid id)
		{
			return await dbContext.Logs
				.Include("Park")
				.Include("Category")
				.FirstOrDefaultAsync(x => x.Id == id);
		}

		public async Task<Log?> UpdateAsync(Guid id, Log log)
		{
			//Check if the id exists
			var existingLog = dbContext.Logs.FirstOrDefault(x => x.Id == id);

			//if null, then return null 
			if (existingLog == null)
			{
				return null;
			}

			//otherwise convert it from the eisting ot the domian model that was passeed in 

			existingLog.ObserverName = log.ObserverName;
			existingLog.Date = log.Date;
			existingLog.Confidence = log.Confidence;
			existingLog.Species = log.Species;
			existingLog.Count = log.Count;
			existingLog.Description = log.Description;
			existingLog.Location = log.Location;
			existingLog.LogImageUrl = log.LogImageUrl;
			existingLog.Comments = log.Comments;
			existingLog.CategoryId = log.CategoryId;
			existingLog.ParkId = log.ParkId;

			//Save changes to databse 
			await dbContext.SaveChangesAsync();

			//retur existing log that now holds the new data 
			return existingLog;

		}

    }
}
