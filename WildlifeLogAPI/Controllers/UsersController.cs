using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WildlifeLogAPI.Models.DomainModels;
using WildlifeLogAPI.Models.DTO;
using WildlifeLogAPI.Repositories;

namespace WildlifeLogAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserRepository userRepository;
        private readonly IMapper mapper;

        public UsersController(IUserRepository userRepository, IMapper mapper)
        {
            this.userRepository = userRepository;
            this.mapper = mapper;
        }


        //Create User 
        [HttpPost]
        public async Task<IActionResult> CreateAsync([FromBody] CreateUserDto createUserDto)
        {
            if (ModelState.IsValid)
            {
                //convert from DTO to Domain Model(IdentityUser) using Automapper 
                var user = mapper.Map<IdentityUser>(createUserDto);

                //Use repository to create the user 
                var result = await userRepository.CreateAsync(user, createUserDto.Password, createUserDto.Roles);

                //Convert Dto back to IdentityUser 
                var userDto = mapper.Map<CreateUserDto>(user);

                //return the DTO to the client 
                //return CreatedAtAction(nameof(GetByIdAsync), new { id = user.Id }, userDto);
                return Ok(userDto);

            }
            else
            {
                return BadRequest(ModelState);
            }
        }

        //Get User By Id 
        [HttpGet]
        [Route("{id:Guid}")]
        public async Task<IActionResult> GetByIdAsync([FromRoute] Guid id)
        {
            //Get user by id using repository and store in a variable 
            var user = await userRepository.GetByIdAsync(id);

            //If null, return Not Found
            if (user == null)
            {
               return NotFound();
            }

            //If user IS found, convert the IdentityUser to a Dto 
            var userDto = mapper.Map<UserDto>(user);

            //send back to client
            return Ok(userDto); 
        }


        //Get all users 
        [HttpGet]
        public async Task<IActionResult> GetAllAsync()
        {
            //get the list of IdentityUsers using repositoey 
            var users = await userRepository.GetAllAsync();

            //convert to Dto
            var usersDto = mapper.Map<List<UserDto>>(users);

            //return to client
            return Ok(usersDto);
          
        }

        //UpdateUser 
        [HttpPut]
        [Route("{id:Guid}")]
        public async Task<IActionResult> UpdateAsync([FromRoute] Guid id, [FromBody] UpdateUserRequestDto updateUserRequestDto)
        {
            if (ModelState.IsValid)
            {
                //convert dto to domain model 
                var user = mapper.Map<IdentityUser>(updateUserRequestDto);

                //use repository to update 
                var updatedUser = await userRepository.UpdateAsync(id, user, updateUserRequestDto.Password, updateUserRequestDto.Roles);

                //if null, return notFOund
                if (updatedUser == null)
                {
                    return NotFound();
                }

                //if not null, convert back to dto 
                var updatedUserDto = mapper.Map<UserDto>(updatedUser);

                //return updated user
                return Ok(user);
            }
            else
            {
                 return BadRequest();
            }
        }

        //Delete User 
        [HttpDelete]
        [Route("{id:Guid}")]
        public async Task<IActionResult> DeleteAsync([FromRoute]Guid id)
        {
            //delete the user from databse 
            var user = await userRepository.DeleteAsync(id);

            //If null, return not found 
            if(user== null)
            {
                return NotFound(); 
            }

            //Convert the user that was deleted into a dto 
            var userDto= mapper.Map<IdentityUser>(user);

            return Ok(userDto);
        }
    }
}
