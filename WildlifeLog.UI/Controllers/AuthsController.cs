
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using System.Text.Json;
using WildlifeLog.UI.Models.DTO;
using WildlifeLog.UI.Models.ViewModels;
using WildlifeLogAPI.Models.DTO;

namespace WildlifeLog.UI.Controllers
{
	public class AuthsController : Controller
	{
		private readonly IHttpClientFactory httpClientFactory;
		private readonly IHttpContextAccessor httpContextAccessor;
		private readonly SignInManager<IdentityUser> signInManager;

		public AuthsController(IHttpClientFactory httpClientFactory, IHttpContextAccessor httpContextAccessor, SignInManager<IdentityUser> signInManager)
		{
			this.httpClientFactory = httpClientFactory;
			this.httpContextAccessor = httpContextAccessor;
			this.signInManager = signInManager;
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

			//create the client 
			var client = httpClientFactory.CreateClient();


			//create httpRequestMessage
			var httpRequestMessage = new HttpRequestMessage()
			{
				Method = HttpMethod.Post,
				RequestUri = new Uri("https://localhost:7075/api/auth/login"),
				Content = new StringContent(JsonSerializer.Serialize(loginViewModel), Encoding.UTF8, "application/json")
			};

			try
			{
				// Send the JWT token in the Authorization header
				var jwtToken = HttpContext.Session.GetString("JwtToken");

				if (!string.IsNullOrEmpty(jwtToken))
				{
					client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwtToken);
				}

				//use cleint to send httpRequestMessage to api and we get a json response abck 
				var httpResponseMessage = await client.SendAsync(httpRequestMessage);

				//Ensure success 
				httpResponseMessage.EnsureSuccessStatusCode();

				//"Read the body" as a string
				var response = await httpResponseMessage.Content.ReadAsStringAsync();

				// Deserialize the JSON response to a DTO (assuming LoginResponseDto is your DTO class)
				var loginResponseDto = JsonSerializer.Deserialize<Models.DTO.LoginResponseDto>(response);

				// If successful, store the JWT token (consider using a secure storage method)
				jwtToken = loginResponseDto.jwtToken;

				// You may want to store the token for subsequent requests (e.g., in a secure cookie or session)
				HttpContext.Session.SetString("JwtToken", jwtToken);


				// Sign in the user using SignInManager
				var user = new ClaimsPrincipal(new ClaimsIdentity(new[]
				{
					new Claim(ClaimTypes.Email, loginViewModel.Email),

				}, "custom"));


				// Use SignInAsync to sign in the user

				await HttpContext.SignInAsync(user, new AuthenticationProperties
				{
					IsPersistent = false // You can change this based on your requirements
				});
				return RedirectToAction("Index", "Home");


			}
			catch (HttpRequestException)
			{
				// Handle request exception (e.g., log, display error message)
				return View();
			}

		}



	}
}
