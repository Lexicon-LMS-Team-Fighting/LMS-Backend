using AutoMapper;
using Domain.Contracts.Repositories;
using Domain.Models.Entities;
using Domain.Models.Exceptions;
using Domain.Models.Exceptions.Authorization;
using Domain.Models.Exceptions.BadRequest;
using Domain.Models.Exceptions.Conflict;
using LMS.Shared.DTOs.CourseDtos;
using LMS.Shared.DTOs.ModuleDtos;
using LMS.Shared.DTOs.PaginationDtos;
using LMS.Shared.DTOs.UserDtos;
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

    /// <summary>
    /// Retrieves a single course by its unique identifier. <br/>
    /// </summary>
    /// <param name="courseId"> The unique identifier of the course.</param>
    /// <param name="include">Related entities to include (e.g., "participants", "modules", "documents").</param>
    /// <returns></returns>
    /// <exception cref="UserRoleNotSupportedException">Thrown when the current user's role is neither Teacher nor Student.</exception>
    /// <exception cref="CourseNotFoundException">Thrown when no course is found with the given <paramref name="courseId"/>.</exception>
    public async Task<CourseExtendedDto> GetCourseAsync(Guid courseId, string? include)
	{
        Course? course = null;

        if (_currentUserService.IsTeacher)
            course = await _unitOfWork.Course.GetCourseAsync(courseId, include);
        else if (_currentUserService.IsStudent)
            course = await _unitOfWork.Course.GetCourseAsync(courseId, _currentUserService.Id, include);
        else
            throw new UserRoleNotSupportedException();
		
		if (course is null) 
			throw new CourseNotFoundException(courseId);

        return _mapper.Map<CourseExtendedDto>(course);
	}

    /// <summary>
    /// Retrieves all courses from the data source. <br/>
    /// </summary>
    /// <param name="pageNumber">The page number to retrieve.</param>
    /// <param name="pageSize">The number of items per page.</param>
    /// <returns>A collection of <see cref="CoursePreviewDto"/> objects representing all users.</returns>
    /// <exception cref="UserRoleNotSupportedException">Thrown when the current user's role is neither Teacher nor Student.</exception>
    public async Task<PaginatedResultDto<CoursePreviewDto>> GetCoursesAsync(int pageNumber, int pageSize)
	{
		IEnumerable<Course>? courses = null;

        if (_currentUserService.IsTeacher)
            courses = await _unitOfWork.Course.GetCoursesAsync();
        else if (_currentUserService.IsStudent)
            courses = await _unitOfWork.Course.GetCoursesAsync(_currentUserService.Id);
        else
            throw new UserRoleNotSupportedException();

        var paginatedCourses = courses.ToPaginatedResult(new PagingParameters
        {
            PageNumber = pageNumber,
            PageSize = pageSize
        });

        return _mapper.Map<PaginatedResultDto<CoursePreviewDto>>(paginatedCourses);
	}

    /// <summary>
    /// Creates a new course based on the provided data. <br/>
    /// </summary>
    /// <param name="createDto">The data for the course to create.</param>
    /// <returns></returns>
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

    /// <summary>
    /// Retrieves participants of a specific course.
    /// </summary>
    /// <param name="courseId">The unique identifier of the course.</param>
    /// <param name="pageNumber">The page number to retrieve (1-based).</param>
    /// <param name="pageSize">The number of items per page.</param>
    /// <returns>A paginated list of participants enrolled in the specified course.</returns>
    public async Task<PaginatedResultDto<CourseParticipantDto>> GetParticipantsAsync(Guid courseId, int pageNumber, int pageSize)
    {
        var course = await _unitOfWork.Course.GetCourseAsync(courseId, nameof(CourseExtendedDto.Participants));

        if (course is null)
            throw new CourseNotFoundException(courseId);

        if (_currentUserService.IsTeacher)
        {
            // teachers can view participants of their courses 
        }
        else if (_currentUserService.IsStudent)
        {
            var isEnrolled = course.UserCourses.Any(uc => uc.UserId == _currentUserService.Id);

            if (!isEnrolled)
                throw new UserRoleNotSupportedException("You are not enrolled in this course.");
        }
        else throw new UserRoleNotSupportedException();

        var participants = await _unitOfWork.User.GetCourseParticipantsAsync(courseId);

        var paginatedParticipants = participants.ToPaginatedResult(new PagingParameters
        {
            PageNumber = pageNumber,
            PageSize = pageSize
        });

        return _mapper.Map<PaginatedResultDto<CourseParticipantDto>>(paginatedParticipants);
    }

    /// <param name="id">The unique identifier of the course to update.</param>
    /// <param name="updateDto">The updated data for the course.</param>
    /// <exception cref="CourseNotFoundException">Thrown if the course is not found.</exception>
    /// <exception cref="ModuleNameAlreadyExistsException">Thrown if the updated course name is not unique within the course.</exception>
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
  /// <summary>
  /// Enrolls a student into the specified course.
  /// </summary>
  /// <param name="courseId">The unique identifier of the course.</param>
  /// <param name="studentId">The unique identifier of the student to enroll.</param>
  /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
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

  /// <summary>
  /// Unenrolls a student from the specified course and removes all their related feedback.
  /// </summary>
  /// <param name="courseId">The unique identifier of the course.</param>
  /// <param name="studentId">The unique identifier of the student to unenroll.</param>
  /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
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

  /// <summary>
  /// Updates an existing course.
  /// </summary>
  /// <param name="id">The unique identifier of the course to update.</param>
  /// <param name="updateDto">The updated data for the course.</param>
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
