using AutoMapper;
using Domain.Models.Entities;
using LMS.Shared.DTOs.AuthDtos;
using LMS.Shared.DTOs.CourseDtos;
using LMS.Shared.DTOs.DocumentDtos;
using LMS.Shared.DTOs.ModuleDtos;
using LMS.Shared.DTOs.PaginationDtos;
using LMS.Shared.DTOs.UserDtos;
using LMS.Shared.Pagination;

namespace LMS.Infractructure.Data;

public class MapperProfile : Profile
{
    public MapperProfile()
    {
        CreateMap<UserRegistrationDto, ApplicationUser>();

        // User mappings
		CreateMap<ApplicationUser, UserDto>()
		  .ForMember(dest => dest.CourseIds,
			  opt => opt.MapFrom(src => src.UserCourses.Select(uc => uc.CourseId)));

		// Course mappings
		CreateMap<Course, CourseDto>();
        CreateMap<CreateCourseDto, Course>();

        // Module mappings
        CreateMap<Module, ModuleDto>();
        CreateMap<Module, ModuleDetailedDto>();
        CreateMap<CreateModuleDto, Module>()
            .ForMember(d => d.Id, o => o.MapFrom(_ => Guid.NewGuid()));

        // Document mappings
        CreateMap<Document, DocumentPreviewDto>();

        // Pagination mappings
        CreateMap<PaginationMetadata, PaginationMetadataDto>();
        CreateMap(typeof(PaginatedResult<>), typeof(PaginatedResultDto<>));
	}
}

   

        
