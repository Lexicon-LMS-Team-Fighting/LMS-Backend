using AutoMapper;
using Domain.Contracts.Repositories;
using Domain.Models.Exceptions;
using LMS.Shared.DTOs.CourseDtos;
using Service.Contracts;

namespace LMS.Services;

/// <summary>
/// Provides operations related to courses, including retrieval of single or multiple courses.
/// Implements the <see cref="ICourseService"/> interface.
/// </summary>
public class CourseService : ICourseService
{
	private readonly IUnitOfWork _unitOfWork;
	private readonly IMapper _mapper;

	public CourseService(IUnitOfWork unitOfWork, IMapper mapper)
	{
		_unitOfWork = unitOfWork;
		_mapper = mapper;
	}

	public async Task<CourseDto> GetCourseAsync(Guid courseId)
	{
		var course = await _unitOfWork.Course.GetCourseAsync(courseId);
		
		if (course is null) 
			throw new NotFoundException($"Course with id: {courseId} was not found.");

		return _mapper.Map<CourseDto>(course);
	}

	public async Task<IEnumerable<CourseDto>> GetCoursesAsync()
	{
		var courses = await _unitOfWork.Course.GetCoursesAsync();

		return _mapper.Map<IEnumerable<CourseDto>>(courses);
	}
}
