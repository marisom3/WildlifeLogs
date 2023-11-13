using Microsoft.AspNetCore.Identity;
using WildlifeLogAPI.Models.DomainModels;

namespace WildlifeLogAPI.Repositories
{
    public interface IUserRepository
    {
        //Get all users
        Task<IEnumerable<IdentityUser>> GetAllAsync();

        //get users by id 
        Task<IdentityUser?> GetByIdAsync(Guid id);

        //create users
        Task<IdentityResult> CreateAsync(IdentityUser user, string password);

        //update user 
        Task<IdentityUser?> UpdateAsync(Guid id, IdentityUser user, string password, IEnumerable<string> newRoles);

        //delete user 
        Task<IdentityUser?> DeleteAsync(Guid id);
    }
}
