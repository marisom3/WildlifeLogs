using System.ComponentModel.DataAnnotations;

namespace WildlifeLogAPI.Models.DTO
{
    public class RegisterRequestDto
    {

		[Required]
		public string Username { get; set;}

		[Required]
		[DataType(DataType.EmailAddress)]
		public string Email { get; set;}

		[Required]
		[DataType(DataType.Password)]
		public string Password { get; set;}

		public string[]? Roles { get; set;}

    }
}
