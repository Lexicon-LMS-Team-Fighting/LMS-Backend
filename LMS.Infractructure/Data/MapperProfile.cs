using AutoMapper;
using Domain.Models.Entities;
using LMS.Shared.DTOs.AuthDtos;
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
	}
}
