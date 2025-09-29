using AutoMapper;
using Domain.Models.Entities;
using LMS.Shared.DTOs.ActivityTypeDto;
using LMS.Shared.DTOs.AuthDtos;
using LMS.Shared.DTOs.CourseDtos;
using LMS.Shared.DTOs.DocumentDtos;
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
        CreateMap<Course, CourseDetailedDto>();
        CreateMap<CreateCourseDto, Course>();

        // Module mappings
        CreateMap<Module, ModuleDto>();
        CreateMap<Module, ModuleDetailedDto>();
        CreateMap<CreateModuleDto, Module>()
            .ForMember(d => d.Id, o => o.MapFrom(_ => Guid.NewGuid()));

        // Document mappings
        CreateMap<Document, DocumentPreviewDto>();
        
        CreateMap<CreateModuleDto, Module>()
            .ForMember(d => d.Id, o => o.MapFrom(_ => Guid.NewGuid()));

        // LMSActivity mappings
        CreateMap<LMSActivity, LMSActivityDto>();
        CreateMap<LMSActivity, LMSActivityDetailedDto>();
        CreateMap<CreateLMSActivityDto, LMSActivity>()
            .ForMember(d => d.Id, o => o.MapFrom(_ => Guid.NewGuid()))
            .ForMember(d => d.ActivityType, opt => opt.Ignore());

        // ActivityType mappings
        CreateMap<ActivityType, ActivityTypeDto>();

        // Pagination mappings
        CreateMap<PaginationMetadata, PaginationMetadataDto>();
        CreateMap(typeof(PaginatedResult<>), typeof(PaginatedResultDto<>));
	}
}
