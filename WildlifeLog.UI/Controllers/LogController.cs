using Microsoft.AspNetCore.Mvc;
using WildlifeLog.UI.Models.DTO;

namespace WildlifeLog.UI.Controllers
{
	public class LogController : Controller
	{
		private readonly IHttpClientFactory httpClientFactory;

		public LogController(IHttpClientFactory httpClientFactory)
		{
			this.httpClientFactory = httpClientFactory;
		}
		

		[HttpGet]
		public async Task<IActionResult> Index()
		{
			//create list of type ParkDto 
			List<LogDto> logs = new List<LogDto>();

			try
			{
				//create client 
				var client = httpClientFactory.CreateClient();

				//use client to talk to the API + get back all the park info 
				var httpResponseMessage = await client.GetAsync("https://localhost:7075/api/log");

				//EnsureSuccess 
				httpResponseMessage.EnsureSuccessStatusCode();

				//Convert the JSON data to a list of ParkDto and add to the parks list 
				logs.AddRange(await httpResponseMessage.Content.ReadFromJsonAsync<IEnumerable<LogDto>>());
			}
			catch (Exception ex)
			{
				//log exception 
			}

			//retrun the list of parkDto to teh View 
			return View(logs);
		}

	}
}
