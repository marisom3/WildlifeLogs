using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
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
            //create list of type ParkDto 
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