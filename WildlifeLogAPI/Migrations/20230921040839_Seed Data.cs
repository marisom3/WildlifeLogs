using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace WildlifeLogAPI.Migrations
{
    /// <inheritdoc />
    public partial class SeedData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AnimalCategories",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { new Guid("1357f76e-ce9f-4ee8-a78e-b69c221d1eb9"), "Mammal" },
                    { new Guid("a40d0d0b-7e91-48ac-a13d-49aa15cfaded"), "Amphibian" },
                    { new Guid("b39bb696-2cbc-4542-9ca6-7fd41c38c21b"), "Insect" },
                    { new Guid("c160cad4-bf5e-443b-9764-9f7bfb121601"), "Bird" },
                    { new Guid("f5dc2264-a69c-4ecc-a93d-f2738a050b6f"), "Reptile" },
                    { new Guid("faf4a089-1d82-4b56-aff2-a1509d7a4d10"), "Fish" }
                });

            migrationBuilder.InsertData(
                table: "Parks",
                columns: new[] { "Id", "Address", "Ecosystem", "Name", "ParkImageUrl" },
                values: new object[,]
                {
                    { new Guid("33e41267-7a3b-45d3-bd4f-3abd52c79ffb"), "8504 Pacific Coast Hwy, Newport Beach, CA 92657", "Coastal Sage Scrub", "Crystal Cove State Park", "https://upload.wikimedia.org/wikipedia/commons/a/a0/Crystal_Cove_State_Park_photo_d_ramey_logan.jpg" },
                    { new Guid("6efbc351-e98b-48e9-9dac-4a2357bc244f"), "200 Palm Canyon Dr, Borrego Springs, CA 92004", "Desert", "Anza-Borrego Desert State Park", "" },
                    { new Guid("e48b30c8-737d-47fd-8541-a5bf59c7fa92"), "4721 Sapphire Rd, Chino Hills, CA 91709", "Chapparal", "Chino Hills State Park", "" },
                    { new Guid("ecd24f9a-10b7-4335-9e8b-cf65cf17077b"), "2301 University Drive Newport Beach, CA 92660", "Wetland", "Newport Back Bay Nature Preserve", "The_Back_Bay_of_Newport_Beach_CA_by_D_Ramey_Logan" },
                    { new Guid("fc6b4c42-6d34-470c-be32-850efd9c1796"), "30892 Trabuco Canyon Road, Trabuco Canyon, CA 92678", "Oak Woodland", "O Neill Regional Park", "https://images.squarespace-cdn.com/content/v1/6037075ce7791c1277a04ce9/1648348253378-E5WB5H5XXKS413HOYEHO/PXL_20220312_164435073.MP_2.jpg?format=750w" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AnimalCategories",
                keyColumn: "Id",
                keyValue: new Guid("1357f76e-ce9f-4ee8-a78e-b69c221d1eb9"));

            migrationBuilder.DeleteData(
                table: "AnimalCategories",
                keyColumn: "Id",
                keyValue: new Guid("a40d0d0b-7e91-48ac-a13d-49aa15cfaded"));

            migrationBuilder.DeleteData(
                table: "AnimalCategories",
                keyColumn: "Id",
                keyValue: new Guid("b39bb696-2cbc-4542-9ca6-7fd41c38c21b"));

            migrationBuilder.DeleteData(
                table: "AnimalCategories",
                keyColumn: "Id",
                keyValue: new Guid("c160cad4-bf5e-443b-9764-9f7bfb121601"));

            migrationBuilder.DeleteData(
                table: "AnimalCategories",
                keyColumn: "Id",
                keyValue: new Guid("f5dc2264-a69c-4ecc-a93d-f2738a050b6f"));

            migrationBuilder.DeleteData(
                table: "AnimalCategories",
                keyColumn: "Id",
                keyValue: new Guid("faf4a089-1d82-4b56-aff2-a1509d7a4d10"));

            migrationBuilder.DeleteData(
                table: "Parks",
                keyColumn: "Id",
                keyValue: new Guid("33e41267-7a3b-45d3-bd4f-3abd52c79ffb"));

            migrationBuilder.DeleteData(
                table: "Parks",
                keyColumn: "Id",
                keyValue: new Guid("6efbc351-e98b-48e9-9dac-4a2357bc244f"));

            migrationBuilder.DeleteData(
                table: "Parks",
                keyColumn: "Id",
                keyValue: new Guid("e48b30c8-737d-47fd-8541-a5bf59c7fa92"));

            migrationBuilder.DeleteData(
                table: "Parks",
                keyColumn: "Id",
                keyValue: new Guid("ecd24f9a-10b7-4335-9e8b-cf65cf17077b"));

            migrationBuilder.DeleteData(
                table: "Parks",
                keyColumn: "Id",
                keyValue: new Guid("fc6b4c42-6d34-470c-be32-850efd9c1796"));
        }
    }
}
