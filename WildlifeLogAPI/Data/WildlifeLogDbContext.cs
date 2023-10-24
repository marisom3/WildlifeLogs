using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using WildlifeLogAPI.Models.DomainModels;

namespace WildlifeLogAPI.Data
{
    public class WildlifeLogDbContext : DbContext
    {
        public WildlifeLogDbContext(DbContextOptions<WildlifeLogDbContext> dbContextOptions) : base(dbContextOptions)
        {


        }
        public DbSet<Log> Logs { get; set; }
        public DbSet<Park> Parks { get; set; }
        public DbSet<Category> AnimalCategories { get; set; }
        public DbSet<Image> Images { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            //Seed Data for AnimalCategories 
            //Mammal, Insect, Fish, Reptile, Bird

            var categories = new List<Category>()
            {
                new Category()
                {
                    Id= Guid.Parse("1357f76e-ce9f-4ee8-a78e-b69c221d1eb9"),
                    Name="Mammal"
                },

                new Category()
                {
                    Id= Guid.Parse("faf4a089-1d82-4b56-aff2-a1509d7a4d10"),
                    Name="Fish"
                },

                new Category()
                {
                    Id= Guid.Parse("f5dc2264-a69c-4ecc-a93d-f2738a050b6f"),
                    Name="Reptile"
                },

                new Category()
                {
                    Id= Guid.Parse("c160cad4-bf5e-443b-9764-9f7bfb121601"),
                    Name="Bird"
                },

                new Category()
                {
                    Id= Guid.Parse("a40d0d0b-7e91-48ac-a13d-49aa15cfaded"),
                    Name="Amphibian"
                },

                new Category()
                {
                    Id= Guid.Parse("b39bb696-2cbc-4542-9ca6-7fd41c38c21b"),
                    Name="Insect"
                }
            };

            modelBuilder.Entity<Category>().HasData(categories);


            //Seed data for the Parks 
            var parks = new List<Park>()
            {
                new Park()
                {
                    Id= Guid.Parse("33e41267-7a3b-45d3-bd4f-3abd52c79ffb"),
                    Name="Crystal Cove State Park",
                    Address="8504 Pacific Coast Hwy, Newport Beach, CA 92657",
                    Ecosystem = "Coastal Sage Scrub",
                    ParkImageUrl = "https://upload.wikimedia.org/wikipedia/commons/a/a0/Crystal_Cove_State_Park_photo_d_ramey_logan.jpg"
                },
                new Park()
                {
                    Id= Guid.Parse("e48b30c8-737d-47fd-8541-a5bf59c7fa92"),
                    Name="Chino Hills State Park",
                    Address="4721 Sapphire Rd, Chino Hills, CA 91709",
                    Ecosystem = "Chapparal",
                    ParkImageUrl = ""
                },
                new Park()
                {
                    Id= Guid.Parse("ecd24f9a-10b7-4335-9e8b-cf65cf17077b"),
                    Name="Newport Back Bay Nature Preserve",
                    Address="2301 University Drive Newport Beach, CA 92660",
                    Ecosystem = "Wetland",
                    ParkImageUrl = "The_Back_Bay_of_Newport_Beach_CA_by_D_Ramey_Logan"
                },
                new Park()
                {
                    Id= Guid.Parse("fc6b4c42-6d34-470c-be32-850efd9c1796"),
                    Name="O Neill Regional Park",
                    Address="30892 Trabuco Canyon Road, Trabuco Canyon, CA 92678",
                    Ecosystem = "Oak Woodland",
                    ParkImageUrl = "https://images.squarespace-cdn.com/content/v1/6037075ce7791c1277a04ce9/1648348253378-E5WB5H5XXKS413HOYEHO/PXL_20220312_164435073.MP_2.jpg?format=750w"
                },
                new Park()
                {
                    Id= Guid.Parse("6efbc351-e98b-48e9-9dac-4a2357bc244f"),
                    Name="Anza-Borrego Desert State Park",
                    Address="200 Palm Canyon Dr, Borrego Springs, CA 92004",
                    Ecosystem = "Desert",
                    ParkImageUrl = ""
                }
            };
                
            modelBuilder.Entity<Park>().HasData(parks);
        }
    }
}

