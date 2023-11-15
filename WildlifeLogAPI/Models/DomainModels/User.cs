namespace WildlifeLogAPI.Models.DomainModels
{
    public class User
    {
        public Guid Id { get; set; }
        public string UserName { get; set; }
        public string EmailAddress { get; set; }
        public string[]? Roles { get; set; }
    }
}
