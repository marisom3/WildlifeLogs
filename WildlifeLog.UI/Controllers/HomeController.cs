using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
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


        public async Task<IActionResult> Index(Guid? parkId)
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

                var requestUri = "https://localhost:7075/api/log";

                // Check if a specific park is selected for filtering
                if (parkId.HasValue)
                {
                    requestUri += "?parkId=" + parkId;
                }


                // Make a GET request to the API to fetch all logs
                var httpResponseMessage = await client.GetAsync(requestUri);

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

            // Fetch the list of parks for the dropdown
            using (var client = httpClientFactory.CreateClient())
            {
                var parksResponse = await client.GetAsync("https://localhost:7075/api/parks");
                parksResponse.EnsureSuccessStatusCode();
                var parks = await parksResponse.Content.ReadFromJsonAsync<List<ParkDto>>();

                // Populate the ViewBag with the list of parks
                ViewBag.Parks = new SelectList(parks, "Id", "Name");
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