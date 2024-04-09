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

        //private controller to get the total log count after filtering for parkId 
        private async Task<int> GetTotalLogCount(Guid? parkId, string? filterOn = null, string? filterQuery = null)
        {
            //create client 
            var client = httpClientFactory.CreateClient();

            //if parkid is passed in, add it to the URL
            var parkFilter = parkId.HasValue ? $"&parkId={parkId}" : "";

            var filterParameters = !string.IsNullOrEmpty(filterOn) && !string.IsNullOrEmpty(filterQuery) ? $"&filterOn={filterOn}&filterQuery={filterQuery}" : "";

            //use the client to send the rewuest to the API to get the total # of logs that fit the parameters
            var countResponse = await client.GetAsync($"https://localhost:7075/api/log/totalcount?{parkFilter}{filterParameters}");
            //Ensure success
            countResponse.EnsureSuccessStatusCode();

            //Convert to int
            return await countResponse.Content.ReadFromJsonAsync<int>();
        }


        public async Task<IActionResult> Index(Guid? parkId, int page = 1, int pageSize = 12)
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

				//Append the pagesize and page number to the requestUri
				var requestUri = $"https://localhost:7075/api/log?pageNumber={page}&pageSize={pageSize}";


				// Check if a specific park is selected for filtering
				if (parkId.HasValue)
                {
                    requestUri += $"&ParkId={parkId}";
                }

                // Add the page number to the requestUri
                requestUri += $"&page={page}";

                // Make a GET request to the API to fetch all logs
                var httpResponseMessage = await client.GetAsync(requestUri);

                // Ensure that the request was successful (status code 2xx)
                httpResponseMessage.EnsureSuccessStatusCode();

                // Convert the JSON data to a list of LogDto
                logs.AddRange(await httpResponseMessage.Content.ReadFromJsonAsync<IEnumerable<LogDto>>());

                // Calculate total log count based on how many logs match the parkId (from dropdown)  
                int totalCount = await GetTotalLogCount(parkId);

                //Calculate total pages
                int totalPages = (int)Math.Ceiling((double)totalCount / pageSize);

                // Set ViewBag properties to access in View
                ViewBag.CurrentPage = page;
                ViewBag.TotalPages = totalPages;
                ViewBag.ParkId = parkId;

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


            //retrun the list of logs to the View 
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