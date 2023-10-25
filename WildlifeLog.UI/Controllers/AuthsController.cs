
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc; 
using System.Text;
using System.Text.Json;
using WildlifeLog.UI.Models.DTO;
using WildlifeLog.UI.Models.ViewModels;

namespace WildlifeLog.UI.Controllers
{
	public class AuthsController : Controller
	{
        private readonly IHttpClientFactory httpClientFactory;

        public AuthsController(IHttpClientFactory httpClientFactory)
        {
            this.httpClientFactory = httpClientFactory;
        }

        [HttpGet]
		public IActionResult Register()
		{
			return View();
		}

		[HttpPost]
		public async Task<IActionResult> Register(RegisterViewModel registerViewModel)
		{
			//Created client 
			var client = httpClientFactory.CreateClient();

            //create httpRequestMessage
            var httpRequestMessage = new HttpRequestMessage()
            {
                Method = HttpMethod.Post,
                RequestUri = new Uri("https://localhost:7075/api/auth/register"),
                Content = new StringContent(JsonSerializer.Serialize(registerViewModel), Encoding.UTF8, "application/json")
            }; 
	

			//use cleint to send httpRequestMessage to api and we get a json response abck 
			var httpResponseMessage = await client.SendAsync(httpRequestMessage);

			//Ensure success 
			httpResponseMessage.EnsureSuccessStatusCode();

			//"Read the body" as as tring DONT Convert form JSON to Dto 
			var response = await httpResponseMessage.Content.ReadAsStringAsync();

            //If successful, redirect to ? 
            if (response != null)
			{
				return RedirectToAction("Index", "Home");
			}

			//else just return to view 
			return View();
	

		}

		 
		[HttpGet]
		public IActionResult Login()
		{
			return View();
		}

		[HttpPost]
		public async Task<IActionResult> Login(LoginViewModel loginViewModel)
		{
			var client = httpClientFactory.CreateClient();


			//create httpRequestMessage
			var httpRequestMessage = new HttpRequestMessage()
			{
				Method = HttpMethod.Post,
				RequestUri = new Uri("https://localhost:7075/api/auth/login"),
				Content = new StringContent(JsonSerializer.Serialize(loginViewModel), Encoding.UTF8, "application/json")
			};


			//use cleint to send httpRequestMessage to api and we get a json response abck 
			var httpResponseMessage = await client.SendAsync(httpRequestMessage);

			//Ensure success 
			httpResponseMessage.EnsureSuccessStatusCode();

			//"Read the body" as as tring DONT Convert form JSON to Dto 
			var response = await httpResponseMessage.Content.ReadAsStringAsync();

			//If successful, redirect to ? 
			if (response != null)
			{
				return RedirectToAction("Index", "Home");
			}

			//else just return to view 
			return View();
		}

		[HttpPost]
		public async Task<IActionResult> Logout()
		{
			var client = httpClientFactory.CreateClient();


			//create httpRequestMessage
			var httpRequestMessage = new HttpRequestMessage()
			{
				Method = HttpMethod.Post,
				RequestUri = new Uri("https://localhost:7075/api/auth/logout"),
				
			};


			//use cleint to send httpRequestMessage to api and we get a json response abck 
			var httpResponseMessage = await client.SendAsync(httpRequestMessage);

			//Ensure success 
			httpResponseMessage.EnsureSuccessStatusCode();

			//"Read the body" as as tring DONT Convert form JSON to Dto 
			var response = await httpResponseMessage.Content.ReadAsStringAsync();

			//If successful, redirect to ? 
			if (response != null)
			{
				return RedirectToAction("Index", "Home");
			}

			//else just return to view 
			return RedirectToAction("Login");
		}

		
	}
}
