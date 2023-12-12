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
		private readonly ILogger<AuthsController> logger;

		public AuthsController(IHttpClientFactory httpClientFactory, IHttpContextAccessor httpContextAccessor,
			SignInManager<IdentityUser> signInManager, ILogger<AuthsController> logger)
		{
			this.httpClientFactory = httpClientFactory;
			this.httpContextAccessor = httpContextAccessor;
			this.signInManager = signInManager;
			this.logger = logger;
		}

		[HttpGet]
		public IActionResult Register()
		{
			return View();
		}

		[HttpPost]
		public async Task<IActionResult> Register(RegisterViewModel registerViewModel)
		{
			// Created client 
			var client = httpClientFactory.CreateClient();

			// create httpRequestMessage
			var httpRequestMessage = new HttpRequestMessage()
			{
				Method = HttpMethod.Post,
				RequestUri = new Uri("https://localhost:7075/api/auth/register"),
				Content = new StringContent(JsonSerializer.Serialize(registerViewModel), Encoding.UTF8, "application/json")
			};

			// use client to send httpRequestMessage to api and we get a json response abck 
			var httpResponseMessage = await client.SendAsync(httpRequestMessage);

			// Ensure success 
			httpResponseMessage.EnsureSuccessStatusCode();

			// "Read the body" as a string DONT convert form JSON to Dto 
			var response = await httpResponseMessage.Content.ReadAsStringAsync();

			// If successful, redirect to ? 
			if (response != null)
			{
				return RedirectToAction("Index", "Home");
			}

			// else just return to view 
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
			try
			{
				// create the client 
				var client = httpClientFactory.CreateClient();

				// create httpRequestMessage
				var httpRequestMessage = new HttpRequestMessage()
				{
					Method = HttpMethod.Post,
					RequestUri = new Uri("https://localhost:7075/api/auth/login"),
					Content = new StringContent(JsonSerializer.Serialize(loginViewModel), Encoding.UTF8, "application/json")
				};

				// use client to send httpRequestMessage to api and we get a json response back 
				var httpResponseMessage = await client.SendAsync(httpRequestMessage);

				// Ensure success 
				httpResponseMessage.EnsureSuccessStatusCode();

				// "Read the body" as a string
				var response = await httpResponseMessage.Content.ReadAsStringAsync();

				// Deserialize the JSON response to a DTO (assuming LoginResponseDto is your DTO class)
				var loginResponseDto = JsonSerializer.Deserialize<Models.DTO.LoginResponseDto>(response);

				// Extract the JWT token from the response
				var jwtToken = loginResponseDto.jwtToken;

				// Store the token for subsequent requests (consider more secure storage options)
				HttpContext.Session.SetString("JwtToken", jwtToken);

				// Include the token in the Authorization header for subsequent requests
				client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwtToken);

				// Specify the authentication type when creating ClaimsIdentity
				var userIdentity = new ClaimsIdentity(new[]
				{
					new Claim(ClaimTypes.Email, loginViewModel.Email),
                    new Claim(ClaimTypes.Name, loginResponseDto.userName),
                    new Claim(ClaimTypes.AuthenticationMethod, "AuthScheme"),
				}, "AuthScheme");
                
				// Add roles to claims if the user has roles
                if (loginResponseDto.roles != null && loginResponseDto.roles.Any())
                {
                    foreach (var role in loginResponseDto.roles)
                    {
                        userIdentity.AddClaim(new Claim(ClaimTypes.Role, role));
                    }
                }
                // Use ClaimsPrincipal with the specified ClaimsIdentity
                var user = new ClaimsPrincipal(userIdentity);

				// Use SignInAsync to sign in the user
				await HttpContext.SignInAsync("AuthScheme", user, new AuthenticationProperties
				{
					ExpiresUtc = DateTime.UtcNow.AddMinutes(20),
					IsPersistent = false, // You can change this based on your requirements
					AllowRefresh = false
				});

				// Log successful login
				logger.LogInformation("User successfully logged in.");

				return RedirectToAction("Index", "Logs");
			}
			catch (HttpRequestException)
			{
				// Handle request exception (e.g., log, display error message)
				logger.LogError("Login failed due to HttpRequestException.");
				return View();
			}
			catch (Exception ex)
			{
				// Handle other exceptions
				logger.LogError(ex, "An unexpected error occurred during login.");
				return View();
			}
		}


        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            try
            {
                // Use HttpContext.SignOutAsync to sign out the user
                await HttpContext.SignOutAsync();

                // Optionally, clear any session data or perform additional cleanup
                // Clear session data (optional)
                HttpContext.Session.Clear();

                // Redirect to the home page or another appropriate page after logout
                return RedirectToAction("Index", "Home");
            }
            catch (Exception ex)
            {
                // Handle exceptions, log errors, or display an error message
                logger.LogError(ex, "An unexpected error occurred during logout.");
                return View("Error");
            }
        }

        public IActionResult AccessDenied()
        {
            return View();
        }

    }
}


