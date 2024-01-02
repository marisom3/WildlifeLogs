using Microsoft.AspNetCore.Mvc;
using WildlifeLogAPI.Models.DomainModels;

namespace WildlifeLogAPI.Repositories
{
    public interface ILogRepository
    {
        //Get all
        Task<List<Log>> GetAllAsync(string? filterOn = null, string? filterQuery = null, 
            string? sortBy = null, bool isAscending = true, int pageNumber = 1, int pageSize = 20);

        //Get by id
        Task<Log?> GetByIdAsync(Guid id);

        //Create log 
        Task<Log> CreateAsync(Log log);

        //Update log 
        Task<Log?> UpdateAsync(Guid id, Log log);

        //Delete log 
        Task<Log?> DeleteAsync(Guid id);

    }
}
