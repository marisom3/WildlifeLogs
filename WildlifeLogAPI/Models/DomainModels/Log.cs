namespace WildlifeLogAPI.Models.DomainModels
{
    public class Log
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
        public Guid CategoryId { get; set; }
        public Guid ParkId { get; set; }


        //Navigation Property 
        public Category Category { get; set; }
        public Park Park { get; set; }
    }
}
    




 