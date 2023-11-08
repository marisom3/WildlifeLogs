using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using WildlifeLogAPI.Models.DomainModels;
using WildlifeLogAPI.Models.DTO;
using WildlifeLogAPI.Repositories;

namespace WildlifeLogAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    


    public class ParksController : ControllerBase
    {
        private readonly IParkRepository parkRepository;
        private readonly IMapper mapper;
        private readonly ILogger<ParksController> logger;

        public ParksController(IParkRepository parkRepository, IMapper mapper, ILogger<ParksController> logger)
        {
            this.parkRepository = parkRepository;
            this.mapper = mapper;
            this.logger = logger;
        }

        //GET all parks: localhost/api/parks
        [HttpGet]
        [Authorize(Roles = "Admin")]



        public async Task<IActionResult> GetAll()
        {
            //Get list from database using repository, return domain model 
            var parksDomain = await parkRepository.GetAllAsync();


            //Convert Domian Model to Dto using automapper 
            var parksDto = mapper.Map<List<ParkDto>>(parksDomain);

            
            //return DTO back to the client
            return Ok(parksDto);
        }


        //GET park by id: localhost/api/parks/{id}
        [HttpGet]
        [Route("{id:Guid}")]


        public async Task<IActionResult> GetByIdAsync([FromRoute] Guid id)
        {
            //Get park by id - we have a domain model now 
            var parkDomain = await parkRepository.GetByIdAsync(id);

            //If nulll, return NotFound 
            if (parkDomain == null)
            {
                return NotFound();
            }

            //If not null, convert to dto using automapper
            var parkDto = mapper.Map<ParkDto>(parkDomain);

            //return dto to client
            return Ok(parkDto);

        }


        //POST - create park: localhost/api/parks
        [HttpPost]

        public async Task<IActionResult> CreateAsync([FromBody] AddParkRequestDto addParkRequestDto)
        {
            if (ModelState.IsValid)
            {
                //convert dto to domain model using auto mapper 
                var parkDomain = mapper.Map<Park>(addParkRequestDto);

                //Use repository to add the park to teh databse 
                await parkRepository.CreateAsync(parkDomain);

                //convert back to dto and retrun to client 
                var parkDto = mapper.Map<AddParkRequestDto>(parkDomain);

                //Idk why he did something different but this wors for me(See video 29)
                return Ok(parkDto);
            }
            else
            { 
                return BadRequest(ModelState); 
            }    

        }


        //PUT - UUpdate a park 
        [HttpPut]
        [Route("{id:guid}")]
   
        public async Task<IActionResult> UpdateAsync([FromRoute] Guid id, [FromBody] UpdateParkRequestDto updateParkRequestDto)
        {
            if (ModelState.IsValid)
            {
                //Convert the dto to domain model 
                var parkDomainModel = mapper.Map<Park>(updateParkRequestDto);

                //Check if park exists using the repository's UpdateAsync 
                parkDomainModel = await parkRepository.UpdateAsync(id, parkDomainModel);

                if (parkDomainModel == null)
                {
                    return NotFound();
                }

                //If something is found then convert it back to a dto to return to the client
                var parkDto = mapper.Map<UpdateParkRequestDto>(parkDomainModel);

                return Ok(parkDto);
            }
            else 
            { 
                return BadRequest(ModelState); 
            }
        }


        [HttpDelete]
        [Route("{id:Guid}")]
   
        public async Task<IActionResult> Delete([FromRoute] Guid id)
        {
            //use repository to get delete the park by id 
            var parkDomainModel = await parkRepository.DeleteAsync(id);

            //check if it was null or not 
            if (parkDomainModel == null)
            {
                return NotFound();
            }

            //if not null, convert the domain model to dto
            var parkDto = mapper.Map<ParkDto>(parkDomainModel);

            //reutrn dto 
            return Ok(parkDto);

        }
    }
}
