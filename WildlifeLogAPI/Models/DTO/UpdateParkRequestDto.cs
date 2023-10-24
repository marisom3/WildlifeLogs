using System.ComponentModel.DataAnnotations;

namespace WildlifeLogAPI.Models.DTO
{
    public class UpdateParkRequestDto
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public string Address { get; set; }

        [Required]
        public string Ecosystem { get; set; }

     
        public string? ParkImageUrl { get; set; }
    }
}

