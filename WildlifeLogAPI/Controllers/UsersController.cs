using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WildlifeLogAPI.Models.DTO;
using WildlifeLogAPI.Repositories;

namespace WildlifeLogAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserRepository userRepository;

        public UsersController(IUserRepository userRepository)
        {
            this.userRepository = userRepository;
        }


        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            //Get list from the database using repository 
            var userDomain = await userRepository.GetAllAsync(); 

            //covert domain model to dto using auto mapper 
           

            //retrun dto to client 
        }


    }
}
