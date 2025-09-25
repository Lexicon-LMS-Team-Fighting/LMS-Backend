using AutoMapper;
using Domain.Models.Entities;
using Domain.Models.Pagination;
using LMS.Shared.DTOs.AuthDtos;
using LMS.Shared.DTOs.PaginationDtos;
using LMS.Shared.DTOs.CourseDtos;
using LMS.Shared.DTOs.UserDtos;

namespace LMS.Infractructure.Data;

public class MapperProfile : Profile
{
    public MapperProfile()
    {
        CreateMap<UserRegistrationDto, ApplicationUser>();

		CreateMap<ApplicationUser, UserDto>()
		  .ForMember(dest => dest.CourseIds,
			  opt => opt.MapFrom(src => src.UserCourses.Select(uc => uc.CourseId)));

		// Course mappings
		CreateMap<Course, CourseDto>();

        // Pagination mappings
        CreateMap<PaginationMetadata, PaginationMetadataDto>();
        CreateMap(typeof(PaginatedResult<>), typeof(PaginatedResultDto<>));
	}
}

   

