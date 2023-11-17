namespace WildlifeLogAPI.Models.DTO
{
    public class UpdateUserRequestDto
    {
        public string UserName { get; set; }
        public string Email { get; set; }
        public IEnumerable<string> Roles { get; set; }
    }
}
