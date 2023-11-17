namespace WildlifeLog.UI.Models.DTO
{
	public class UpdateUserDto
	{
		public Guid Id { get; set; }
		public string UserName { get; set; }
		public string Email { get; set; }
		public IEnumerable<string>? Roles { get; set; }
	}
}
