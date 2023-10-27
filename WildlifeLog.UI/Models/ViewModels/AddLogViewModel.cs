using System.ComponentModel.DataAnnotations;
using WildlifeLog.UI.Models.DTO;

namespace WildlifeLog.UI.Models.ViewModels
{
    public class AddLogViewModel
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

   

		public List<ParkDto> Parks { get; set; }
		public List<CategoryDto> Categories { get; set; }
	}
}
