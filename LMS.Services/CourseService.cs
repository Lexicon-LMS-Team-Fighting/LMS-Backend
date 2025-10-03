using AutoMapper;
using Domain.Contracts.Repositories;
using Domain.Models.Entities;
using Domain.Models.Exceptions;
using Domain.Models.Exceptions.Authorization;
using Domain.Models.Exceptions.BadRequest;
using Domain.Models.Exceptions.Conflict;
using LMS.Shared.DTOs.CourseDtos;
using LMS.Shared.DTOs.PaginationDtos;
using LMS.Shared.Pagination;
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
	/// <param name="currentUserService">The service to get information about the current user.</param>
    public CourseService(IUnitOfWork unitOfWork, IMapper mapper, ICurrentUserService currentUserService)
	{
		_unitOfWork = unitOfWork;
		_mapper = mapper;
		_currentUserService = currentUserService;
    }

    /// <inheritdoc />
    /// <exception cref="UserRoleNotSupportedException">Thrown when the current user's role is neither Teacher nor Student.</exception>
    /// <exception cref="CourseNotFoundException">Thrown when no course is found with the given <paramref name="courseId"/>.</exception>
    public async Task<CourseExtendedDto> GetCourseAsync(Guid courseId, string? include)
	{
        Course? course = null;
        bool includeProgress = !string.IsNullOrEmpty(include) && include.Contains(nameof(CourseExtendedDto.Progress), StringComparison.OrdinalIgnoreCase);
        decimal progress = 0;

        if (_currentUserService.IsTeacher)
        {
            course = await _unitOfWork.Course.GetCourseAsync(courseId, include);

            if (includeProgress)
                progress = await _unitOfWork.Course.CalculateProgress(courseId);
        }
        else if (_currentUserService.IsStudent)
        {
            course = await _unitOfWork.Course.GetCourseAsync(courseId, _currentUserService.Id, include);

            if (includeProgress)
                progress = await _unitOfWork.Course.CalculateProgress(courseId, _currentUserService.Id);
        }
        else throw new UserRoleNotSupportedException();
		
		if (course is null) 
			throw new CourseNotFoundException(courseId);

        var courseDto = _mapper.Map<CourseExtendedDto>(course);

        if (includeProgress)
            courseDto.Progress = progress;

        return courseDto;
	}

    /// <inheritdoc />
    /// <exception cref="UserRoleNotSupportedException">Thrown when the current user's role is neither Teacher nor Student.</exception>
    public async Task<PaginatedResultDto<CoursePreviewDto>> GetCoursesAsync(int pageNumber, int pageSize, string? include = null)
	{
		IEnumerable<Course>? courses = null;

        if (_currentUserService.IsTeacher)
        {
            courses = await _unitOfWork.Course.GetCoursesAsync();
        }
        else if (_currentUserService.IsStudent)
        {
            courses = await _unitOfWork.Course.GetCoursesAsync(_currentUserService.Id);
        }
        else throw new UserRoleNotSupportedException();

        var paginatedCourses = courses.ToPaginatedResult(new PagingParameters
        {
            PageNumber = pageNumber,
            PageSize = pageSize
        });

        var coursesDto = _mapper.Map<PaginatedResultDto<CoursePreviewDto>>(paginatedCourses);

        if (!string.IsNullOrEmpty(include) && include.Contains(nameof(CoursePreviewDto.Progress), StringComparison.OrdinalIgnoreCase))
        {
            Func<Guid, Task<decimal>> calculateProgressFunc = _currentUserService.IsTeacher
                ? (async (courseId) => await _unitOfWork.Course.CalculateProgress(courseId))
                : _currentUserService.IsStudent
                    ? (async (courseId) => await _unitOfWork.Course.CalculateProgress(courseId, _currentUserService.Id))
                    : throw new UserRoleNotSupportedException();

            foreach (var c in coursesDto.Items)
            {
                c.Progress = await calculateProgressFunc(c.Id);
            }
        }

        return coursesDto;
    }

    /// <inheritdoc />
    /// <exception cref="CourseNameAlreadyExistsException">Thrown when a course with the same name already exists.</exception>
    /// <exception cref="InvalidDateRangeException">Thrown when the start date is not earlier than the end date.</exception>
    public async Task<CourseExtendedDto> CreateAsync(CreateCourseDto createDto)
	{
		var course = _mapper.Map<Course>(createDto);
		
        if (!await _unitOfWork.Course.IsUniqueNameAsync(createDto.Name))
            throw new CourseNameAlreadyExistsException(createDto.Name);

        if (course.StartDate >= course.EndDate)
			throw new InvalidDateRangeException(course.StartDate, course.EndDate);

		_unitOfWork.Course.Create(course);
		await _unitOfWork.CompleteAsync();

		return _mapper.Map<CourseExtendedDto>(course);
	}

    /// <inheritdoc />
    /// <exception cref="CourseNotFoundException">Thrown if the course is not found.</exception>
    /// <exception cref="UserNotFoundException">Thrown if the user is not found.</exception>
    /// <exception cref="RoleMismatchException">Thrown if the user is not a student.</exception>
    public async Task EnrollStudentAsync(Guid courseId, string studentId)
    {
        var course = await _unitOfWork.Course.GetCourseAsync(courseId, include: nameof(CourseExtendedDto.Participants));

        if (course is null)
            throw new CourseNotFoundException(courseId);

        if (await _unitOfWork.User.GetUserAsync(studentId) is null)
            throw new UserNotFoundException(studentId);

        if (!await _unitOfWork.User.IsUserStudentAsync(studentId))
            throw new RoleMismatchException("User with provided ID is not a Student.");

        if (course.UserCourses.Any(uc => uc.UserId == studentId))
            return;

        _unitOfWork.UserCourse.Create(new UserCourse { UserId = studentId, CourseId = courseId });
        await _unitOfWork.CompleteAsync();
    }

    /// <inheritdoc />
    /// <exception cref="CourseNotFoundException">Thrown if the course is not found.</exception>
    /// <exception cref="UserNotFoundException">Thrown if the user is not found.</exception>
    /// <exception cref="RoleMismatchException">Thrown if the user is not a student.</exception>
    public async Task UnenrollStudentAsync(Guid courseId, string studentId)
    {
        var course = await _unitOfWork.Course.GetCourseAsync(courseId, null);

        if (course is null)
            throw new CourseNotFoundException(courseId);

        if (await _unitOfWork.User.GetUserAsync(studentId) is null)
            throw new UserNotFoundException(studentId);

        if (!await _unitOfWork.User.IsUserStudentAsync(studentId))
            throw new RoleMismatchException("User with provided ID is not a Student.");

        await _unitOfWork.UserCourse.DeleteAllByUserId(studentId);
        await _unitOfWork.LMSActivityFeedback.DeleteAllInCourseByUserId(studentId, courseId);

        await _unitOfWork.CompleteAsync();
    }

    /// <inheritdoc />
    /// <exception cref="CourseNotFoundException">Thrown if the course is not found.</exception>
    /// <exception cref="CourseNameAlreadyExistsException">Thrown if the updated course name is not unique.</exception>
    /// <exception cref="InvalidDateRangeException">Thrown if the updated start date is greater than or equal to the end date.</exception>
    public async Task UpdateAsync(Guid id, UpdateCourseDto updateDto)
    {
        var course = await _unitOfWork.Course.GetCourseAsync(id, null, true);

        if (course is null)
            throw new CourseNotFoundException(id);

        if (updateDto.Name is not null)
        {
            if (!await _unitOfWork.Course.IsUniqueNameAsync(updateDto.Name, id))
                throw new CourseNameAlreadyExistsException(updateDto.Name);

            course.Name = updateDto.Name;
        }

        if (updateDto.Description is not null)
            course.Description = updateDto.Description;

        if (updateDto.StartDate.HasValue)
            course.StartDate = updateDto.StartDate.Value;

        if (updateDto.EndDate.HasValue)
            course.EndDate = updateDto.EndDate.Value;

        if (course.StartDate >= course.EndDate)
            throw new InvalidDateRangeException(course.StartDate, course.EndDate);

        _unitOfWork.Course.Update(course);
        await _unitOfWork.CompleteAsync();
    }
}
