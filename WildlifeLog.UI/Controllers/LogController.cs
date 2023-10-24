using Microsoft.AspNetCore.Mvc;

namespace WildlifeLog.UI.Controllers
{
	public class LogController : Controller
	{
		public IActionResult Index()
		{
			return View();
		} 


	}
}
