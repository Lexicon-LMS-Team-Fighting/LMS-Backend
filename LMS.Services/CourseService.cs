using AutoMapper;
using Domain.Contracts.Repositories;
using Domain.Models.Entities;
using Domain.Models.Exceptions;
using Domain.Models.Exceptions.BadRequest;
using Domain.Models.Exceptions.Conflict;
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

	/// <summary>
	/// Initializes a new instance of the <see cref="CourseService"/> class.
	/// </summary>
	/// <param name="unitOfWork">The unit of work for repository access.</param>
	/// <param name="mapper">The AutoMapper instance for mapping domain entities to DTOs.</param>
	public CourseService(IUnitOfWork unitOfWork, IMapper mapper)
	{
		_unitOfWork = unitOfWork;
		_mapper = mapper;
	}

	/// <inheritdoc/>
	public async Task<CourseDetailedDto> GetCourseAsync(Guid courseId)
	{
		var course = await _unitOfWork.Course.GetCourseAsync(courseId);
		
		if (course is null) 
			throw new NotFoundException($"Course with id: {courseId} was not found.");

		return _mapper.Map<CourseDetailedDto>(course);
	}

	/// <inheritdoc/>
	public async Task<IEnumerable<CourseDto>> GetCoursesAsync()
	{
		var courses = await _unitOfWork.Course.GetCoursesAsync();

		return _mapper.Map<IEnumerable<CourseDto>>(courses);
	}

	/// <inheritdoc/>
	public async Task<CourseDto> CreateCourseAsync(CreateCourseDto createCourseDto)
	{
		var course = _mapper.Map<Course>(createCourseDto);
		
		var nameExists = await IsNotUniqueCourseNameAsync(course.Name);

		if (nameExists) throw new CourseNameAlreadyExistsException(course.Name);
		
		if (course.StartDate >= course.EndDate)
			throw new InvalidDateRangeException(course.StartDate, course.EndDate);

		_unitOfWork.Course.Create(course);
		await _unitOfWork.CompleteAsync();

		return _mapper.Map<CourseDto>(course);
	}

	/// <inheritdoc/>
	public async Task<bool> IsNotUniqueCourseNameAsync(string name) =>	
		await _unitOfWork.Course.AnyAsync(name);
	
}
