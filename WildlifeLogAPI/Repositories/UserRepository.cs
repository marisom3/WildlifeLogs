using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WildlifeLogAPI.Models.DomainModels;



namespace WildlifeLogAPI.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly UserManager<IdentityUser> userManager;

        //Inject userManager 
        public UserRepository(UserManager<IdentityUser> userManager)
        {
            this.userManager = userManager;
        }


        //Create a user 
        public async Task<IdentityResult> CreateAsync(IdentityUser user, string password, IEnumerable<string> roles)
        {
            //Create the user with IdentityUser and the password 
          var result = await userManager.CreateAsync(user, password);


            //If it worked, then add the roles if any 
            if (result.Succeeded && roles != null)
            {
                // Assign roles to the user
                await userManager.AddToRolesAsync(user, roles);
            }

            return result;

        }

        //Delete User by Id
        public async Task<IdentityUser?> DeleteAsync(Guid id)
        {
            var userToDelete = await userManager.Users.FirstOrDefaultAsync(x => x.Id == id.ToString());

            if (userToDelete != null)
            {
                var result = await userManager.DeleteAsync(userToDelete);

                if(result.Succeeded)
                {
                    return userToDelete;
                }
                else
                {
                    return null;
                }
            }
           
            return null; 
        }

        //Get all Users
        public async Task<IEnumerable<IdentityUser>> GetAllAsync()
        {
           return await userManager.Users.Include(u => u.Roles).ToListAsync();
        }

        //Get User By Id
        public async Task<IdentityUser?> GetByIdAsync(Guid id)
        {
            return await userManager.Users.Include(u => u.Roles).FirstOrDefaultAsync(x => x.Id == id.ToString());
        }


        //Update User 
        public async Task<IdentityUser?> UpdateAsync(Guid id, IdentityUser user, string newPassword, IEnumerable<string> newRoles)
        {
            //Get the user we want to update
            var existingUser = await userManager.Users.FirstOrDefaultAsync(x => x.Id == id.ToString());

            //If the user is found, 
            if (existingUser != null)
            {
                // Update properties of the existing user
                existingUser.UserName = user.UserName;
                existingUser.Email = user.Email;

                //update password 
                if(!string.IsNullOrEmpty(newPassword))
                {
                    var token = await userManager.GeneratePasswordResetTokenAsync(existingUser);
                    var results = await userManager.ResetPasswordAsync(existingUser, token, newPassword);

                    if(!results.Succeeded)
                    {
                        return null; 
                    }
                }

                //Update roles if new roles are provided 
                if(newRoles != null)
                {
                    var existingRoles = await userManager.GetRolesAsync(existingUser);

                    var rolesToAdd = newRoles.Except(existingRoles);
                    await userManager.AddToRolesAsync(existingUser, rolesToAdd);

                    var rolesToRemove = existingRoles.Except(newRoles);
                    await userManager.RemoveFromRolesAsync(existingUser, rolesToRemove);
                }

                // Call UpdateAsync to update the user in the database
                var result = await userManager.UpdateAsync(existingUser);

                if (!result.Succeeded)
                {
                    // Handle user update failure (throw an exception or handle it as needed)
                    return null;
                }

                return existingUser;

            }

            return null;
        }

      
    }
    
}
