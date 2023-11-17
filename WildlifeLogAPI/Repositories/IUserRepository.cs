using Microsoft.AspNetCore.Identity;
using WildlifeLogAPI.Models.DomainModels;
using WildlifeLogAPI.Models.DTO;

namespace WildlifeLogAPI.Repositories
{
    public interface IUserRepository
    {
        //Get all users
        Task<IEnumerable<UserDto>> GetAllAsync();

        //get users by id 
        Task<UserDto?> GetByIdAsync(Guid id);

        //create users
        Task<IdentityResult> CreateAsync(IdentityUser user, string password, IEnumerable<string> roles);

        //update user 
        Task<IdentityUser?> UpdateAsync(Guid id, IdentityUser user, string password, IEnumerable<string> newRoles);

        //delete user 
        Task<IdentityUser?> DeleteAsync(Guid id);
    }
}
