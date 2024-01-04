using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using System.Text;
using WildlifeLog.UI.Models.DTO;
using WildlifeLog.UI.Models.ViewModels;
using System.Net.Http.Headers;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using WildlifeLogAPI.Models.DTO;
using LogDto = WildlifeLog.UI.Models.DTO.LogDto;
using ParkDto = WildlifeLog.UI.Models.DTO.ParkDto;
using CategoryDto = WildlifeLog.UI.Models.DTO.CategoryDto;
using Microsoft.AspNetCore.Mvc.Rendering;
using WildlifeLogAPI.Repositories;

namespace WildlifeLog.UI.Controllers
{
    public class LogsController : Controller
    {
        private readonly IHttpClientFactory httpClientFactory;

        public LogsController(IHttpClientFactory httpClientFactory)
        {
            this.httpClientFactory = httpClientFactory;

        }
		private async Task<int> GetTotalLogCount(string observerName, Guid? parkId, string? filterOn = null, string? filterQuery = null)
		{
			var client = httpClientFactory.CreateClient();
			var parkFilter = parkId.HasValue ? $"&parkId={parkId}" : "";
			var filterParameters = !string.IsNullOrEmpty(filterOn) && !string.IsNullOrEmpty(filterQuery) ? $"&filterOn={filterOn}&filterQuery={filterQuery}" : "";

			var countResponse = await client.GetAsync($"https://localhost:7075/api/log/totalcount?observerName={observerName}{parkFilter}{filterParameters}");
			countResponse.EnsureSuccessStatusCode();
			return await countResponse.Content.ReadFromJsonAsync<int>();
		}


		[HttpGet]
        public async Task<IActionResult> Index(Guid? parkId, int page = 1, int pageSize = 12)
        {
            //create list of type LogDto
            List<LogDto> logs = new List<LogDto>();

            try
            {
                //create client 
                var client = httpClientFactory.CreateClient();

                // Send the JWT token in the Authorization header
                var jwtToken = HttpContext.Session.GetString("JwtToken");

                if (!string.IsNullOrEmpty(jwtToken))
                {
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwtToken);
                }

                // Get the current user's ObserverName from session
                var observerName = User.Identity.Name;
                
               
                // Append the username as a query parameter when making the request
                var requestUri = $"https://localhost:7075/api/log?pageNumber={page}&pageSize={pageSize}&filterOn=ObserverName&filterQuery={observerName}";


				// Check if a specific park is selected for filtering
				if (parkId.HasValue)
				{
					requestUri += $"&ParkId={parkId}";
				}

				//use client to talk to the API + get back all the park info 
				var httpResponseMessage = await client.GetAsync(requestUri);

                //EnsureSuccess 
                httpResponseMessage.EnsureSuccessStatusCode();

                //Convert the JSON data to a list of LogDto and add to the log list 
                logs.AddRange(await httpResponseMessage.Content.ReadFromJsonAsync<IEnumerable<LogDto>>());


				// Calculate total pages based on observer's log
				int totalCount = await GetTotalLogCount(observerName, parkId);
				int totalPages = (int)Math.Ceiling((double)totalCount / pageSize);

				// Set ViewBag properties
				ViewBag.CurrentPage = page;
				ViewBag.TotalPages = totalPages;
				ViewBag.ParkId = parkId;

				// Sort logs in descending order based on the Date property
				logs = logs.OrderByDescending(log => log.Date).ToList();


				// Fetch the list of parks for the dropdown
				var parksResponse = await client.GetAsync("https://localhost:7075/api/parks");
				parksResponse.EnsureSuccessStatusCode();
				var parks = await parksResponse.Content.ReadFromJsonAsync<List<ParkDto>>();

                // Populate the ViewBag with the list of parks
                ViewBag.Parks = new SelectList(parks, "Id", "Name");

				return View(logs);

			}
            catch (Exception ex)
            {
                //log exception
            }

            //return the list of parkDto to the View 
            return View(logs);

        }

        [HttpGet]
        [Authorize(Roles = "User, Admin")]
        public async Task<IActionResult> Add()
        {

            //Create client 
            var client = httpClientFactory.CreateClient();

            // Send the JWT token in the Authorization header
            var jwtToken = HttpContext.Session.GetString("JwtToken");
            if (!string.IsNullOrEmpty(jwtToken))
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwtToken);
            }
            //Fetch the list of parks and categories 
            var parksResponse = await client.GetAsync("https://localhost:7075/api/parks");
            var categoriesResponse = await client.GetAsync("https://localhost:7075/api/categories");

            // Get the current user's username
            var observerName = User.Identity.Name;

            // Ensure success for both requests
            parksResponse.EnsureSuccessStatusCode();
            categoriesResponse.EnsureSuccessStatusCode();

            // Convert JSON responses to lists of ParkDto and CategoryDto
            var parks = await parksResponse.Content.ReadFromJsonAsync<List<ParkDto>>();
            var categories = await categoriesResponse.Content.ReadFromJsonAsync<List<Models.DTO.CategoryDto>>();

            // Create a view model that includes the list of parks and categories AND observer name 
            var viewModel = new AddLogViewModel
            {
                Parks = parks,
                Categories = categories,
                ObserverName = observerName
            };

            return View(viewModel);


        }

        [HttpPost]
        [Authorize(Roles = "User, Admin")]

        public async Task<IActionResult> Add(AddLogViewModel addLogViewModel)
        {

            //Create client 
            var client = httpClientFactory.CreateClient();

            // Send the JWT token in the Authorization header
            var jwtToken = HttpContext.Session.GetString("JwtToken");
            if (!string.IsNullOrEmpty(jwtToken))
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwtToken);
            }

            //create httpRequestMessage object
            var httpRequestMessage = new HttpRequestMessage()
            {
                Method = HttpMethod.Post,
                RequestUri = new Uri("https://localhost:7075/api/log"),
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

        [HttpGet]
        [Authorize(Roles = "User, Admin")]
        public async Task<IActionResult> Edit(Guid id)
        {

            //create client 
            var client = httpClientFactory.CreateClient();

            // Send the JWT token in the Authorization header
            var jwtToken = HttpContext.Session.GetString("JwtToken");
            if (!string.IsNullOrEmpty(jwtToken))
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwtToken);
            }

            //use client to get the log data from the api 
            var log = await client.GetFromJsonAsync<LogDto>($"https://localhost:7075/api/log/{id.ToString()}");


            //if you get something by the id, then return it to the view
            if (log is null)
            {
                return View();

            }

            // Fetch the lists of parks and categories just like in your Add method
            var parksResponse = await client.GetAsync("https://localhost:7075/api/parks");
            var categoriesResponse = await client.GetAsync("https://localhost:7075/api/categories");

            //Make sure its a success?
            parksResponse.EnsureSuccessStatusCode();
            categoriesResponse.EnsureSuccessStatusCode();

            //use the client to convert the Json response form the API to a list of the DTOs
            var parks = await parksResponse.Content.ReadFromJsonAsync<List<ParkDto>>();
            var categories = await categoriesResponse.Content.ReadFromJsonAsync<List<CategoryDto>>();

            //
            var viewModel = new UpdateLogViewModel
            {
                ObserverName = log.ObserverName,
                Date = log.Date,
                Confidence = log.Confidence,
                Species = log.Species,
                Count = log.Count,
                Description = log.Description,
                Location = log.Location,
                LogImageUrl = log.LogImageUrl,
                Comments = log.Comments,
                CategoryId = log.CategoryId,
                ParkId = log.ParkId,
                Parks = parks,
                Categories = categories
            };

            return View(viewModel);

        }

        [HttpPost]
        [Authorize(Roles = "User, Admin")]
        public async Task<IActionResult> Edit(LogDto request)
        {
            //create client 
            var client = httpClientFactory.CreateClient();

            // Send the JWT token in the Authorization header
            var jwtToken = HttpContext.Session.GetString("JwtToken");
            if (!string.IsNullOrEmpty(jwtToken))
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwtToken);
            }

            //create httpRequestMessage 
            var httpRequestMessage = new HttpRequestMessage()
            {
                Method = HttpMethod.Put,
                RequestUri = new Uri($"https://localhost:7075/api/log/{request.Id}"),
                Content = new StringContent(JsonSerializer.Serialize(request), Encoding.UTF8, "application/json")
            };

            //Send request through client 
            var httpResponseMessage = await client.SendAsync(httpRequestMessage);

            //Ensure Sucess
            httpResponseMessage.EnsureSuccessStatusCode();

            //convert the response coming back form the api from json to Region Dto 
            var response = await httpResponseMessage.Content.ReadFromJsonAsync<LogDto>();

            //if succesful then redirect to the dit page 
            if (response != null)
            {
                return RedirectToAction("Index", "Logs");

            }

            return View();

        }


        [HttpPost]
        [Authorize(Roles = "User, Admin")]
        public async Task<IActionResult> Delete(Guid id)
        {
            try
            {
                var client = httpClientFactory.CreateClient();

                // Send the JWT token in the Authorization header
                var jwtToken = HttpContext.Session.GetString("JwtToken");
                if (!string.IsNullOrEmpty(jwtToken))
                {
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwtToken);
                }

                var httpResponseMessage = await client.DeleteAsync($"https://localhost:7075/api/log/{id}");

                httpResponseMessage.EnsureSuccessStatusCode();

                return RedirectToAction("Index", "Logs");
            }
            catch (Exception ex)
            {
                //


            }
            return View();
        }

    }
}
