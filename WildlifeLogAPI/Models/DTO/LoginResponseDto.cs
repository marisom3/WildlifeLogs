namespace WildlifeLogAPI.Models.DTO
{
    public class LoginResponseDto
    {
        public string jwtToken { get; set; }
        public string userName { get; set; }
        public List<string> roles {get; set;}
	}
}
 