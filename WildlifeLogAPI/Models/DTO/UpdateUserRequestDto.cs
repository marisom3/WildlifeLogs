namespace WildlifeLogAPI.Models.DTO
{
    public class UpdateUserRequestDto
    {
        public string UserName { get; set; }
        public string EmailAddress { get; set; }
        public string Password { get; set; }
        public IEnumerable<string>? Roles { get; set; }
    }
}
