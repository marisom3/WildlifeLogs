using Microsoft.AspNetCore.Mvc;
using System.Text;
using System.Text.Json;
using WildlifeLog.UI.Models.DTO;
using WildlifeLog.UI.Models.ViewModels;
using System.Net.Http;
using System.Net.Http.Headers;
using Microsoft.AspNetCore.Authorization;

namespace WildlifeLog.UI.Controllers
{

	public class ParksController : Controller
	{

		private readonly IHttpClientFactory httpClientFactory;

		public ParksController(IHttpClientFactory httpClientFactory)
		{
			this.httpClientFactory = httpClientFactory;
		}

		[HttpGet]
		public async Task<IActionResult> Index()
		{
			//Create list so we can put out parks into later
			List<ParkDto> response = new List<ParkDto>();

			//create a http client which we use to send http requests to talk to teh web api 
			var client = httpClientFactory.CreateClient();

			//send toeken to Authoirzation header
			var jwtToken = HttpContext.Session.GetString("JwtToken");

			if (!string.IsNullOrEmpty(jwtToken))
			{
				client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwtToken);
			}

			//use the client to get the http response message from the api from this ("address") 
			var httpResponseMessage = await client.GetAsync("https://localhost:7075/api/parks");

			//Use this built in method EnsureSuccessStatusCode to ensure that its a sucess
			//or it will throw an exception which if it does, the try catch gets it  
			httpResponseMessage.EnsureSuccessStatusCode();

			//takes that success message and extract the body of the http response message (aka all the parks data)
			response.AddRange(await httpResponseMessage.Content.ReadFromJsonAsync<IEnumerable<ParkDto>>());

			//return the parks data to the view so we can display it 
			return View(response);

		}

		[HttpGet]
		[Authorize(Roles = "Admin")]
		public IActionResult Add()
		{
			return View();
		}


		[HttpPost]
		[Authorize(Roles = "Admin")]
		public async Task<IActionResult> Add(AddParkViewModel addParkViewModel)
		{

			//Create client 
			var client = httpClientFactory.CreateClient();

			// Retrieve the JWT token from wherever it's stored (e.g., session, cookie)
			var jwtToken = HttpContext.Session.GetString("JwtToken");

			if (!string.IsNullOrEmpty(jwtToken))
			{
				// Set the Authorization header with the JWT token
				client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwtToken);
			}

			//create httpRequestMessage object
			var httpRequestMessage = new HttpRequestMessage()
			{
				Method = HttpMethod.Post,
				RequestUri = new Uri("https://localhost:7075/api/parks"),
				Content = new StringContent(JsonSerializer.Serialize(addParkViewModel), Encoding.UTF8, "application/json")
			};


			//Use the client to sent the httt request message to the api 
			var httpResponseMessage = await client.SendAsync(httpRequestMessage);

			//Make sure it's a success 
			httpResponseMessage.EnsureSuccessStatusCode();

			//convert json response to Region Dto object 
			var response = await httpResponseMessage.Content.ReadFromJsonAsync<ParkDto>();

			//if sucessful redirect to 
			if (response is not null)
			{
				return RedirectToAction("Index", "Parks");
			}

			//otherwise return view 
			return View();


		}

		[HttpGet]
		[Authorize(Roles = "Admin")]
		public async Task<IActionResult> Edit(Guid id)
		{

			//create client 
			var client = httpClientFactory.CreateClient();

			// Retrieve the JWT token from wherever it's stored (e.g., session, cookie)
			var jwtToken = HttpContext.Session.GetString("JwtToken");

			if (!string.IsNullOrEmpty(jwtToken))
			{
				// Set the Authorization header with the JWT token
				client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwtToken);
			}

			//use client to get the data from teh api
			//we send it to that url and include the id 
			//also we convert the response from JSON to Park Dto 
			var response = await client.GetFromJsonAsync<ParkDto>($"https://localhost:7075/api/Parks/{id.ToString()}");

			//if the response is not null aka we were able to grab the park by its id,
			//return it to the view 
			if (response != null)
			{
				return View(response);
			}

			//otherwise just retun to the view 
			return View();


		}

		[HttpPost]
		[Authorize(Roles = "Admin")]
		public async Task<IActionResult> Edit(ParkDto request)
		{
			//create client 
			var client = httpClientFactory.CreateClient();

			// Retrieve the JWT token from wherever it's stored (e.g., session, cookie)
			var jwtToken = HttpContext.Session.GetString("JwtToken");

			if (!string.IsNullOrEmpty(jwtToken))
			{
				// Set the Authorization header with the JWT token
				client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwtToken);
			}

			//create httpRequestMessage 
			var httpRequestMessage = new HttpRequestMessage()
			{
				Method = HttpMethod.Put,
				RequestUri = new Uri($"https://localhost:7075/api/parks/{request.Id}"),
				Content = new StringContent(JsonSerializer.Serialize(request), Encoding.UTF8, "application/json")
			};

			//Send request through client 
			var httpResponseMessage = await client.SendAsync(httpRequestMessage);

			//Ensure Sucess
			httpResponseMessage.EnsureSuccessStatusCode();

			//convert the response coming back form the api from json to Region Dto 
			var response = await httpResponseMessage.Content.ReadFromJsonAsync<ParkDto>();

			//if succesful then redirect to the dit page 
			if (response != null)
			{
				return RedirectToAction("Index", "Parks");

			}

			return View();
		}


		[HttpPost]
		[Authorize(Roles = "Admin")]
		public async Task<IActionResult> Delete(ParkDto request)
		{
			try
			{
				var client = httpClientFactory.CreateClient();

				// Retrieve the JWT token from wherever it's stored (e.g., session, cookie)
				var jwtToken = HttpContext.Session.GetString("JwtToken");

				if (!string.IsNullOrEmpty(jwtToken))
				{
					// Set the Authorization header with the JWT token
					client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwtToken);
				}

				var httpResponseMessage = await client.DeleteAsync($"https://localhost:7075/api/parks/{request.Id}");

				httpResponseMessage.EnsureSuccessStatusCode();

				return RedirectToAction("Index", "Parks");
			}
			catch (HttpRequestException ex)
			{
				//console
			}

			return View("Edit");
		}



	}

}

