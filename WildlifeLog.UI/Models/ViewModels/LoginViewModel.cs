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
		public string Password { get; set; }
	}
}
