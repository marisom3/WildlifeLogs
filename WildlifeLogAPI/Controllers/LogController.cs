using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using WildlifeLogAPI.Models.DomainModels;
using WildlifeLogAPI.Models.DTO;
using WildlifeLogAPI.Repositories;

namespace WildlifeLogAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    
    public class LogController : ControllerBase
    {
        private readonly ILogRepository logRepository;
        private readonly IMapper mapper;
		private readonly IHttpContextAccessor httpAcc;

		public LogController(ILogRepository logRepository, IMapper mapper, IHttpContextAccessor httpAcc)
        {
            this.logRepository = logRepository;
            this.mapper = mapper;
			this.httpAcc = httpAcc;
		}

        //localhost/api/logs/?filterOn=Name&filterQuery= *what they are searchign to filter on*/&SortBy=Date&isAscenting=true
        [HttpGet]
        public async Task<IActionResult> GetAllAsync([FromQuery] string? filterOn, [FromQuery] string? filterQuery,
            [FromQuery] string? sortBy, [FromQuery] bool? isAscending,
            [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 20, [FromQuery] Guid? parkId = null)
        {
			
			//Get logs domain model using repository 
			var logsDomainModel = await logRepository.GetAllAsync(filterOn, filterQuery, sortBy, isAscending ?? true, pageNumber, pageSize, parkId);

            //Convert domain model to dto 
            var logsDto = mapper.Map<List<LogDto>>(logsDomainModel);

            //return to client
            return Ok(logsDto);
        }

        [HttpGet]
        [Route("{id:Guid}")]
        [Authorize(Roles = "User, Admin")]
        public async Task<IActionResult> GetByIdAsync([FromRoute] Guid id)
        {
            //Get log by id using repository 
            var log = await logRepository.GetByIdAsync(id);


            //If null then return Not Found
            if (log == null)
            {
                return NotFound();
            }

            //convert to dto if not null 
            var logDto = mapper.Map<LogDto>(log);

            //send to client
            return Ok(logDto);
        }

        [HttpPost]
        [Authorize(Roles = "User, Admin")]
        public async Task<IActionResult> CreateAsync(AddLogRequestDto addLogRequestDto)
        {
            if (ModelState.IsValid)
            {
                //convert the dto to domain model 
                var logDomain = mapper.Map<Log>(addLogRequestDto);

                //add domain model to the databse usign repository 
                await logRepository.CreateAsync(logDomain);

                //convert the domain model back to dto 
                var logDto = mapper.Map<LogDto>(logDomain);

                //return dto to client 
                return Ok(logDto);
            }
            else
            {
                return BadRequest(ModelState);
            }
        }

        [HttpPut]
        [Route("{id:Guid}")]
        [Authorize(Roles = "User, Admin")]
        public async Task<IActionResult> UpdateAsync([FromRoute] Guid id, [FromBody] UpdateLogRequestDto updateLogRequestDto)
        {
            if (ModelState.IsValid)
            {
                //Convert the dto to domain model 
                var logDomainModel = mapper.Map<Log>(updateLogRequestDto);

                //use repository to UpdateAsync 
                logDomainModel = await logRepository.UpdateAsync(id, logDomainModel);

                //iff null
                if (logDomainModel == null)
                {
                    return NotFound();
                }

                //if not then convert back to dto 
                var logDto = mapper.Map<UpdateLogRequestDto>(logDomainModel);

                //return dto back to client 
                return Ok(logDto);

            }
            else
            { 
                return BadRequest(ModelState); 
            }

        }

        [HttpDelete]
        [Route("{id:Guid}")]
        [Authorize(Roles = "User, Admin")]
        public async Task<IActionResult> DeleteAsync([FromRoute] Guid id)
        {
            //check databse 
            var log = await logRepository.DeleteAsync(id);

            //if null return not found 
            if (log == null)
            {
                return NotFound();
            }

            //convert from dm to dto 
            var logDto = mapper.Map<LogDto>(log);


            //return to client
            return Ok(logDto);
        }
    }
}
