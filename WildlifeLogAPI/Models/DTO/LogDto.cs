namespace WildlifeLogAPI.Models.DTO
{
    public class LogDto
    {

        public Guid Id { get; set; }
        public string ObserverName { get; set; }
        public DateTime Date { get; set; }
        public bool Confidence { get; set; }
        public string? Species { get; set; }
        public int Count { get; set; }
        public string Description { get; set; }
        public string Location { get; set; }
        public string? LogImageUrl { get; set; }
        public string Comments { get; set; }
       

        public ParkDto Park { get; set; }
        public CategoryDto Category { get; set; }

	}
}
