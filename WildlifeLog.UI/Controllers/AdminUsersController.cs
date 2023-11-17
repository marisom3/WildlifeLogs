using Azure;
using Microsoft.AspNetCore.Mvc;
using System.Text;
using System.Text.Json;
using WildlifeLog.UI.Models.DTO;
using WildlifeLog.UI.Models.ViewModels;


namespace WildlifeLog.UI.Controllers
{
    public class AdminUsersController : Controller
    {

        private readonly IHttpClientFactory httpClientFactory;

        public AdminUsersController(IHttpClientFactory httpClientFactory)
        {
            this.httpClientFactory = httpClientFactory;
        }


        [HttpGet]
        public async Task<IActionResult> Index()
        {
            //Create a list of type UserDto 
            List<UsersDto> users = new List<UsersDto>();

            try
            {
                //create a http client which we use to send http requests to talk to teh web api 
                var client = httpClientFactory.CreateClient();

                //use the client to get the http response message from the api from this ("address") 
                var httpResponseMessage = await client.GetAsync("https://localhost:7075/api/users");

                //Use this built in method EnsureSuccessStatusCode to ensure that its a sucess
                //or it will throw an exception which if it does, the try catch gets it  
                httpResponseMessage.EnsureSuccessStatusCode();

                //takes that success message and extract the body of the http response message 
                users.AddRange(await httpResponseMessage.Content.ReadFromJsonAsync<IEnumerable<UsersDto>>());


            }
            catch (Exception ex)
            {
                //log exception 
            }

            return View(users);
        }

        [HttpPost]
        public async Task<IActionResult> Add(CreateUserDto createUserDto)
        {

            // Create a HttpClient
            var client = httpClientFactory.CreateClient();

            // Initialize Roles to an empty list if it's null
            if (createUserDto.Roles == null)
            {
                createUserDto.Roles = new List<string>();
            }

            // Set the default role to "User" if the "Admin" checkbox is not checked
            if (!createUserDto.Roles.Contains("Admin"))
            {
                createUserDto.Roles = new List<string> { "User" };
            }

            //Create httprequestmessage 
            var httpRequestMessage = new HttpRequestMessage()
            {
                Method = HttpMethod.Post,
                RequestUri = new Uri("https://localhost:7075/api/users"),
                Content = new StringContent(JsonSerializer.Serialize(createUserDto), Encoding.UTF8, "application/json")
            };

            //Use the client to sent the httt request message to the api 
            var httpResponseMessage = await client.SendAsync(httpRequestMessage);

            //Make sure it's a success 
            httpResponseMessage.EnsureSuccessStatusCode();
           
            //convert json response to  Dto object 
            var response = await httpResponseMessage.Content.ReadFromJsonAsync<UsersDto>();

            if (response is not null)
            {
                return RedirectToAction("Index", "AdminUsers");
            }
            
            //otherwise return view 
            return View();
        }
    }

}
