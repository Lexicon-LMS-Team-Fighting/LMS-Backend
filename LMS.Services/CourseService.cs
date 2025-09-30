using AutoMapper;
using Domain.Contracts.Repositories;
using Domain.Models.Entities;
using Domain.Models.Exceptions;
using Domain.Models.Exceptions.Authorization;
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
	private readonly ICurrentUserService _currentUserService;

    /// <summary>
    /// Initializes a new instance of the <see cref="CourseService"/> class.
    /// </summary>
    /// <param name="unitOfWork">The unit of work for repository access.</param>
    /// <param name="mapper">The AutoMapper instance for mapping domain entities to DTOs.</param>
    public CourseService(IUnitOfWork unitOfWork, IMapper mapper, ICurrentUserService currentUserService)
	{
		_unitOfWork = unitOfWork;
		_mapper = mapper;
		_currentUserService = currentUserService;
    }

	/// <inheritdoc/>
	public async Task<CourseDetailedDto> GetCourseAsync(Guid courseId)
	{
        Course? course = null;

        if (_currentUserService.IsTeacher)
            course = await _unitOfWork.Course.GetCourseAsync(courseId);
        else if (_currentUserService.IsStudent)
            course = await _unitOfWork.Course.GetCourseAsync(courseId, _currentUserService.Id);
        else
            throw new UserRoleNotSupportedException();
		
		if (course is null) 
			throw new CourseNotFoundException(courseId);

        return _mapper.Map<CourseDetailedDto>(course);
	}

	/// <inheritdoc/>
	public async Task<IEnumerable<CourseDto>> GetCoursesAsync()
	{
		IEnumerable<Course>? courses = null;

        if (_currentUserService.IsTeacher)
            courses = await _unitOfWork.Course.GetCoursesAsync();
        else if (_currentUserService.IsStudent)
            courses = await _unitOfWork.Course.GetCoursesAsync(_currentUserService.Id);
        else
            throw new UserRoleNotSupportedException();

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
