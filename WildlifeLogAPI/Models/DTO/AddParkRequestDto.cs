using System.ComponentModel.DataAnnotations;

namespace WildlifeLogAPI.Models.DTO
{
    public class AddParkRequestDto
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
