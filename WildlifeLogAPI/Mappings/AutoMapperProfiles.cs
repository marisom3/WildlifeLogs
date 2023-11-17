using AutoMapper;
using Microsoft.AspNetCore.Identity;
using WildlifeLogAPI.Models.DomainModels;
using WildlifeLogAPI.Models.DTO;

namespace WildlifeLogAPI.Mappings
{
    public class AutoMapperProfiles : Profile
    {
        
        public AutoMapperProfiles()
        {
            CreateMap<Park, ParkDto>().ReverseMap();
            CreateMap<Park, AddParkRequestDto>().ReverseMap();
            CreateMap<Park, UpdateParkRequestDto>().ReverseMap();
            CreateMap<Log, LogDto>().ReverseMap();
            CreateMap<Log, AddLogRequestDto>().ReverseMap();
            CreateMap<CategoryDto, Category>().ReverseMap();
            CreateMap<UpdateLogRequestDto, Log>().ReverseMap();
            CreateMap<IdentityUser, CreateUserDto>().ReverseMap();
            CreateMap<IdentityUser, UpdateUserRequestDto>().ReverseMap();  
            CreateMap<IdentityUser, UserDto>().ReverseMap();

        }
    }
}
