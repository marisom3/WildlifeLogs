using System.ComponentModel.DataAnnotations;

namespace WildlifeLog.UI.Models.ViewModels
{
	public class AddParkViewModel
	{
		public string Name { get; set; }

		public string Address { get; set; }

		public string Ecosystem { get; set; }

		public string? ParkImageUrl { get; set; }
	}
}
