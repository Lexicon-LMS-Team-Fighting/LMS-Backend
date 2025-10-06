using AutoMapper;
using Domain.Models.Entities;
using LMS.Shared.DTOs.ActivityTypeDto;
using LMS.Shared.DTOs.AuthDtos;
using LMS.Shared.DTOs.CourseDtos;
using LMS.Shared.DTOs.DocumentDtos;
using LMS.Shared.DTOs.LMSActivityDtos;
using LMS.Shared.DTOs.LMSActivityFeedbackDtos;
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
        CreateMap<ApplicationUser, UserPreviewDto>();
        CreateMap<ApplicationUser, CourseParticipantDto>();
        CreateMap<ApplicationUser, UserExtendedDto>()
            .ForMember(d => d.Courses, o => o.MapFrom(s => s.UserCourses.Select(uc => uc.Course)));

        // Course mappings
        CreateMap<Course, CoursePreviewDto>();
        CreateMap<Course, CourseExtendedDto>()
            .ForMember(d => d.Documents, o => o.MapFrom(s => s.Documents))
            .ForMember(d => d.Participants, o => o.MapFrom(s => s.UserCourses.Select(uc => uc.User)))
            .ForMember(d => d.Modules, o => o.MapFrom(s => s.Modules));

        CreateMap<CreateCourseDto, Course>();

        // Module mappings
        CreateMap<Module, ModulePreviewDto>();
        CreateMap<Module, ModuleExtendedDto>()
            .ForMember(d => d.Documents, o => o.MapFrom(s => s.Documents))
            .ForMember(d => d.Activities, o => o.MapFrom(s => s.LMSActivities))
            .ForMember(d => d.Participants, o => o.MapFrom(s => s.Course.UserCourses.Select(uc => uc.User)));

        CreateMap<CreateModuleDto, Module>()
            .ForMember(d => d.Id, o => o.MapFrom(_ => Guid.NewGuid()));

        // Document mappings
        CreateMap<Document, DocumentPreviewDto>();
        CreateMap<Document, DocumentExtendedDto>();

        // LMSActivity mappings
        CreateMap<LMSActivity, LMSActivityPreviewDto>()
            .ForMember(d => d.ActivityTypeName, o => o.MapFrom(s => s.ActivityType.Name))
            .ForMember(d => d.CourseId, o => o.MapFrom(s => s.Module.Course.Id))
            .ForMember(d => d.CourseName, o => o.MapFrom(s => s.Module.Course.Name));

        CreateMap<LMSActivity, LMSActivityExtendedDto>()
            .ForMember(d => d.CourseId, o => o.MapFrom(s => s.Module.Course.Id))
            .ForMember(d => d.CourseName, o => o.MapFrom(s => s.Module.Course.Name))
            .ForMember(d => d.ModuleName, o => o.MapFrom(s => s.Module.Name))
            .ForMember(d => d.ActivityTypeName, o => o.MapFrom(s => s.ActivityType.Name))
            .ForMember(d => d.Feedbacks, opt => opt.MapFrom(s => s.LMSActivityFeedbacks))
            .ForMember(d => d.Documents, o => o.MapFrom(s => s.Documents))
            .ForMember(d => d.Participants, o => o.MapFrom(s => s.Module.Course.UserCourses.Select(uc => uc.User)));

        CreateMap<CreateLMSActivityDto, LMSActivity>()
            .ForMember(d => d.Id, o => o.MapFrom(_ => Guid.NewGuid()));

        // ActivityType mappings
        CreateMap<ActivityType, ActivityTypeDto>();

        // Feedback mappings
        CreateMap<LMSActivityFeedback, LMSActivityFeedbackPreviewDto>();
        CreateMap<LMSActivityFeedback, LMSActivityFeedbackExtendedDto>();
        CreateMap<CreateLMSActivityFeedbackDto, LMSActivityFeedback>();

        // Pagination mappings
        CreateMap<PaginationMetadata, PaginationMetadataDto>();
        CreateMap(typeof(PaginatedResult<>), typeof(PaginatedResultDto<>));
	}
}
