namespace WildlifeLogAPI.Models.DTO
{
    public class UserDto
    {
        public Guid Id { get; set; }
        public string UserName { get; set; }
        public string EmailAddress { get; set; }
        public string[]? Roles { get; set; }
    }
}
