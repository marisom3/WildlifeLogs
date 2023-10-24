using Microsoft.AspNetCore.Mvc;
using WildlifeLogAPI.Models.DomainModels;

namespace WildlifeLogAPI.Repositories
{
    public interface IParkRepository
    {
        //Get all park
        Task<List<Park>> GetAllAsync();

        //get park by id 
        Task<Park?> GetByIdAsync(Guid id);

        //Create park 
        Task<Park> CreateAsync(Park park);

        //Update park 
        Task<Park?> UpdateAsync(Guid id, Park park);

        //Delete park 
        Task<Park?> DeleteAsync(Guid id);



    }
}
