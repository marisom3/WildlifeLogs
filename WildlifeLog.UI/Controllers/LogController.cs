using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using System.Text;
using WildlifeLog.UI.Models.DTO;
using WildlifeLog.UI.Models.ViewModels;

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

        [HttpGet]
        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Add(AddLogViewModel addLogViewModel)
        {
            //Create client 
            var client = httpClientFactory.CreateClient();

            //create httpRequestMessage object
            var httpRequestMessage = new HttpRequestMessage()
            {
                Method = HttpMethod.Post,
                RequestUri = new Uri("https://localhost:7075/api/logs"),
                Content = new StringContent(JsonSerializer.Serialize(addLogViewModel), Encoding.UTF8, "application/json")
            };


            //Use the client to sent the httt request message to the api 
            var httpResponseMessage = await client.SendAsync(httpRequestMessage);

            //Make sure it's a success 
            httpResponseMessage.EnsureSuccessStatusCode();

            //convert json response to Region Dto object 
            var response = await httpResponseMessage.Content.ReadFromJsonAsync<LogDto>();

            //if sucessful redirect to 
            if (response is not null)
            {
                return RedirectToAction("Index", "Logs");
            }

            //otherwise return view 
            return View();
        }


    }
}
