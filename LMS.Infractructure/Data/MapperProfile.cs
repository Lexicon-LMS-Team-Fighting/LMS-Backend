using AutoMapper;
using Domain.Models.Entities;
using LMS.Shared.DTOs.AuthDtos;
using LMS.Shared.DTOs.CourseDtos;
using LMS.Shared.DTOs.LMSActivityDtos;
using LMS.Shared.DTOs.ModuleDtos;
using LMS.Shared.DTOs.PaginationDtos;
using LMS.Shared.DTOs.UserDtos;
using LMS.Shared.Pagination;

namespace LMS.Infractructure.Data;

public class MapperProfile : Profile
{
    public MapperProfile()
    {
        // User mappings
        CreateMap<UserRegistrationDto, ApplicationUser>();

        CreateMap<ApplicationUser, UserDto>()
            .ForMember(dest => dest.CourseIds,
                opt => opt.MapFrom(src => src.UserCourses.Select(uc => uc.CourseId)));

        // Course mappings
        CreateMap<Course, CourseDto>();

        // Module mappings
        CreateMap<Module, ModuleDto>();
        
        CreateMap<CreateModuleDto, Module>()
            .ForMember(d => d.Id, o => o.MapFrom(_ => Guid.NewGuid())); 

        // LMSActivity mappings
        CreateMap<LMSActivity, LMSActivityDto>()
            .ForMember(d => d.ActivityType, o => o.MapFrom(s => s.ActivityType != null ? s.ActivityType.Name : string.Empty));
            
        CreateMap<CreateLMSActivityDto, LMSActivity>()
            .ForMember(d => d.Id, o => o.MapFrom(_ => Guid.NewGuid()))
            .ForMember(d => d.ActivityType, opt => opt.Ignore());

        // Pagination mappings
        CreateMap<PaginationMetadata, PaginationMetadataDto>();
        CreateMap(typeof(PaginatedResult<>), typeof(PaginatedResultDto<>));
	}
}
