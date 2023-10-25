using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using WildlifeLogAPI.Models.DTO;
using WildlifeLogAPI.Repositories;

namespace WildlifeLogAPI.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class CategoriesController : Controller
	{
		private readonly ICategoryRepository categoryRepository;
		private readonly IMapper mapper;

		public CategoriesController(ICategoryRepository categoryRepository, IMapper mapper)
		{
			this.categoryRepository = categoryRepository;
			this.mapper = mapper;
		}

		//GET all parks: localhost/api/categories
		[HttpGet]
		public async Task<IActionResult> GetAll()
		{
			//Get list from database using repository, return domain model 
			var categoryDomain = await categoryRepository.GetAllAsync();


			//Convert Domian Model to Dto using automapper 
			var categoryDto = mapper.Map<List<CategoryDto>>(categoryDomain);


			//return DTO back to the client
			return Ok(categoryDto);
		}
	}
}

