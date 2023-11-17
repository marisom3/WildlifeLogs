using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace WildlifeLogAPI.Data
{
    public class WildlifeLogAuthDbContext : IdentityDbContext<IdentityUser>
    {
        public WildlifeLogAuthDbContext(DbContextOptions<WildlifeLogAuthDbContext> options) : base(options) 
        {
        
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            var userRoleId = "05c9e249-15a3-47ec-82e5-a656f6dbb9d2";
            var adminRoleId = "0ebfff4c-da7d-44da-9ef9-f5feca981ed3";


            var roles = new List<IdentityRole>
            {
                new IdentityRole
                {
                    Id = userRoleId,
                    ConcurrencyStamp = userRoleId,
                    Name = "User",
                    NormalizedName= "User".ToUpper()

                },

                new IdentityRole
                {
                    Id = adminRoleId,
                    ConcurrencyStamp = adminRoleId,
                    Name = "Admin",
                    NormalizedName= "Admin".ToUpper()

                },
            };

            builder.Entity<IdentityRole>().HasData(roles);
        }
    }
}
