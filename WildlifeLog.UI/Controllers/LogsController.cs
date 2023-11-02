﻿using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using System.Text;
using WildlifeLog.UI.Models.DTO;
using WildlifeLog.UI.Models.ViewModels;

namespace WildlifeLog.UI.Controllers
{
	public class LogsController : Controller
	{
		private readonly IHttpClientFactory httpClientFactory;

		public LogsController(IHttpClientFactory httpClientFactory)
		{
			this.httpClientFactory = httpClientFactory;
		}
		

		[HttpGet]
		public async Task<IActionResult> Index()
		{
			//create list of type LogDto
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
        public async Task<IActionResult> Add()
        {
			//Create client 
			var client = httpClientFactory.CreateClient();

			//Fetch the list of parks and categories 
			var parksResponse = await client.GetAsync("https://localhost:7075/api/parks");
			var categoriesResponse = await client.GetAsync("https://localhost:7075/api/categories");

			// Ensure success for both requests
			parksResponse.EnsureSuccessStatusCode();
			categoriesResponse.EnsureSuccessStatusCode();

			// Convert JSON responses to lists of ParkDto and CategoryDto
			var parks = await parksResponse.Content.ReadFromJsonAsync<List<ParkDto>>();
			var categories = await categoriesResponse.Content.ReadFromJsonAsync<List<CategoryDto>>();

			// Create a view model that includes the list of parks and categories
			var viewModel = new AddLogViewModel
			{
				Parks = parks,
				Categories = categories
			};

			return View(viewModel);
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
        public async Task<IActionResult> Edit(Guid id)
        {
			//create client 
            var client = httpClientFactory.CreateClient();

			//use client to get the log data from the api 
            var repsonse= await client.GetFromJsonAsync<LogDto>($"https://localhost:7075/api/log/{id.ToString()}");


			//if you get something by the id, then return it to the view
            if (repsonse is not null)
            {
                return View(repsonse);

            }

            return View();
		}

		[HttpPost]
		public async Task<IActionResult> Edit(LogDto request)
		{
			//create client 
			var client = httpClientFactory.CreateClient();

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
		public async Task<IActionResult> Delete(LogDto request)
		{
			try
			{
				var client = httpClientFactory.CreateClient();

				var httpResponseMessage = await client.DeleteAsync($"https://localhost:7075/api/log/{request.Id}");

				httpResponseMessage.EnsureSuccessStatusCode();

				return RedirectToAction("Index", "Logs");
			}
			catch (Exception ex)
			{
				// Console

			}

			return View("Edit");
		}

		

	}
}
