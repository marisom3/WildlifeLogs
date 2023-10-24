using System.ComponentModel.DataAnnotations;

namespace WildlifeLogAPI.Models.DTO
{
    public class UpdateLogRequestDto
    {
        [Required]
        public string ObserverName { get; set; }
        
        [Required]
        public DateTime Date { get; set; }

        [Required]
        public bool Confidence { get; set; }

       
        public string? Species { get; set; }

        [Required]
        public int Count { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        public string Location { get; set; }

        public string? LogImageUrl { get; set; }

        [Required]
        public string Comments { get; set; }

        [Required]
        public Guid CategoryId { get; set; }

        [Required]
        public Guid ParkId { get; set; }
    }
}
