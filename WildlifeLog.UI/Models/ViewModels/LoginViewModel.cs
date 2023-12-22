using System.ComponentModel.DataAnnotations;

namespace WildlifeLog.UI.Models.ViewModels
{
	public class LoginViewModel
	{
		[Required]
		[DataType(DataType.EmailAddress)]
		public string Email { get; set; }

		[Required]
		[DataType(DataType.Password)]
		[MinLength(6, ErrorMessage = "Password must be at least 6 characters long")]
		public string Password { get; set; }
	}
}
