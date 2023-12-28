using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Net.Http.Headers;
using WildlifeLog.UI.Models;
using WildlifeLog.UI.Models.DTO;

namespace WildlifeLog.UI.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IHttpClientFactory httpClientFactory;

        public HomeController(ILogger<HomeController> logger, IHttpClientFactory httpClientFactory)
        {
            _logger = logger;
            this.httpClientFactory = httpClientFactory;
        }


        public async Task<IActionResult> Index()
        {
            //create list of type LogDto 
            List<LogDto> logs = new List<LogDto>();

            try
            {
                //create client 
                var client = httpClientFactory.CreateClient();

				// Send the JWT token in the Authorization header if the user is logged in
				var jwtToken = HttpContext.Session.GetString("JwtToken");
				if (!string.IsNullOrEmpty(jwtToken))
				{
					client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwtToken);
				}

				// Make a GET request to the API to fetch all logs
				var httpResponseMessage = await client.GetAsync("https://localhost:7075/api/log");

				// Ensure that the request was successful (status code 2xx)
				httpResponseMessage.EnsureSuccessStatusCode();

				// Convert the JSON data to a list of LogDto
				logs.AddRange(await httpResponseMessage.Content.ReadFromJsonAsync<IEnumerable<LogDto>>());

				// Sort logs in descending order based on the Date property
				logs = logs.OrderByDescending(log => log.Date).ToList();
			}
            catch (Exception ex)
            {
                //log exception 
            }

            //retrun the list of parkDto to teh View 
            return View(logs);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}