namespace WildlifeLog.UI.Models.ViewModels
{
	public class AddUserViewModel
	{
		public string UserName { get; set; }
		public string Email { get; set; }
		public string Password { get; set; }
		public IEnumerable<string> Roles { get; set; }

	}
}
