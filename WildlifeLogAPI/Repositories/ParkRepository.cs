using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WildlifeLogAPI.Data;
using WildlifeLogAPI.Models.DomainModels;

namespace WildlifeLogAPI.Repositories
{
    public class ParkRepository : IParkRepository
    {

        //Inject dbContext
        private readonly WildlifeLogDbContext dbContext;

        public ParkRepository(WildlifeLogDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        //Create a park 
        public async Task<Park> CreateAsync(Park park)
        {
            await dbContext.Parks.AddAsync(park);
            await dbContext.SaveChangesAsync();
            return park;
        }


        public async Task<Park?> DeleteAsync(Guid id)
        {
            //get the park by id and sve int oa variable 
            var existingPark = await dbContext.Parks.FirstOrDefaultAsync(x => x.Id == id);
        
            //if null, return null 
            if (existingPark == null)
            {
                return null; 
            }

            //If found, then remove it from the database
            dbContext.Parks.Remove(existingPark);

            //save changes async
            await dbContext.SaveChangesAsync();

            //return the deleted thign in case 
            return(existingPark);



        }

        //Get all Parks 
        public async Task<List<Park>> GetAllAsync()
        {
            return await dbContext.Parks.ToListAsync();
        } 

        //Get park by id
        public async Task<Park?> GetByIdAsync(Guid id)
        {
           return await dbContext.Parks.FirstOrDefaultAsync(x => x.Id == id);

        }

        //update existng Park 
        public async Task<Park?> UpdateAsync(Guid id, Park park)
        {
          //check if park exists by id
            var existingPark = await dbContext.Parks.FirstOrDefaultAsync(x => x.Id == id);

            //if null then return null 
            if (existingPark == null)
            {
                return null; 
            }

            //Else we update the existing region with
            //the new values coming in from Domain Model 
            existingPark.Name = park.Name;
            existingPark.Address = park.Address;
            existingPark.Ecosystem = park.Ecosystem;
            existingPark.ParkImageUrl = park.ParkImageUrl;

            //save chanegs and return the domain model 
            await dbContext.SaveChangesAsync();

            return existingPark;

        }


    }
}
