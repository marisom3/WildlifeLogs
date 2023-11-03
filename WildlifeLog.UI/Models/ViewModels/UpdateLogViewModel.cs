using System.ComponentModel.DataAnnotations;
using WildlifeLog.UI.Models.DTO;


namespace WildlifeLog.UI.Models.ViewModels
{
    public class UpdateLogViewModel
    {

        [Required]
        [MaxLength(100)]
        public string ObserverName { get; set; }

        [Required]

        public DateTime Date { get; set; }

        [Required]
        public bool Confidence { get; set; }

        public string? Species { get; set; }

        [Required]
        public int Count { get; set; }

        [Required]
        [MaxLength(200)]
        public string Description { get; set; }

        [Required]
        public string Location { get; set; }

        public string? LogImageUrl { get; set; }

        [Required]
        [MaxLength(500)]
        public string Comments { get; set; }

        public Guid CategoryId { get; set; }

        public Guid ParkId { get; set; }

        public LogDto Log { get; set; }  // Log data that you want to edit
        public List<ParkDto> Parks { get; set; }  // List of parks for populating the park dropdown
        public List<CategoryDto> Categories { get; set; }  // List of categories for populating the category dropdown
    }

}
