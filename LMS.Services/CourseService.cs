using AutoMapper;
using Domain.Contracts.Repositories;
using Domain.Models.Entities;
using Domain.Models.Exceptions;
using Domain.Models.Exceptions.Authorization;
using Domain.Models.Exceptions.BadRequest;
using Domain.Models.Exceptions.Conflict;
using LMS.Shared.DTOs.CourseDtos;
using LMS.Shared.DTOs.PaginationDtos;
using LMS.Shared.DTOs.UserDtos;
using LMS.Shared.Pagination;
using Service.Contracts;

namespace LMS.Services;

/// <summary>
/// Provides operations related to courses, including retrieval, enrollment, and updates.
/// </summary>
public class CourseService : ICourseService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ICurrentUserService _currentUserService;

    public CourseService(IUnitOfWork unitOfWork, IMapper mapper, ICurrentUserService currentUserService)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _currentUserService = currentUserService;
    }

    /// <inheritdoc />
    public async Task<CourseExtendedDto> GetCourseAsync(Guid courseId, string? include)
    {
        Course? course = null;
        bool includeProgress = !string.IsNullOrEmpty(include) && include.Contains(nameof(CourseExtendedDto.Progress), StringComparison.OrdinalIgnoreCase);
        decimal progress = 0;

        if (_currentUserService.IsTeacher)
        {
            course = await _unitOfWork.Course.GetCourseAsync(courseId, include);
            if (includeProgress)
                progress = await _unitOfWork.Course.CalculateProgressAsync(courseId);
        }
        else if (_currentUserService.IsStudent)
        {
            course = await _unitOfWork.Course.GetCourseAsync(courseId, _currentUserService.Id, include);
            if (includeProgress)
                progress = await _unitOfWork.Course.CalculateProgressAsync(courseId, _currentUserService.Id);
        }
        else throw new UserRoleNotSupportedException();

        if (course is null)
            throw new CourseNotFoundException(courseId);

        var courseDto = _mapper.Map<CourseExtendedDto>(course);
        if (includeProgress)
        {
            courseDto.Progress = progress;
            foreach (var module in courseDto.Modules)
            {
                module.Progress = await _unitOfWork.Module.CalculateProgressAsync(module.Id, _currentUserService.IsStudent ? _currentUserService.Id : null);
            }
        }

        return courseDto;
    }

    /// <inheritdoc />
    public async Task<PaginatedResultDto<CoursePreviewDto>> GetCoursesAsync(PaginatedQueryDto queryDto)
    {
        PaginatedResult<Course>? paginatedCourses = null;

        if (_currentUserService.IsTeacher)
            paginatedCourses = await _unitOfWork.Course.GetCoursesAsync(queryDto);
        else if (_currentUserService.IsStudent)
            paginatedCourses = await _unitOfWork.Course.GetCoursesAsync(_currentUserService.Id, queryDto);
        else
            throw new UserRoleNotSupportedException();

        var coursesDto = _mapper.Map<PaginatedResultDto<CoursePreviewDto>>(paginatedCourses);

        if (!string.IsNullOrEmpty(queryDto.Include) && queryDto.Include.Contains(nameof(CoursePreviewDto.Progress), StringComparison.OrdinalIgnoreCase))
        {
            Func<Guid, Task<decimal>> calculateProgressFunc = _currentUserService.IsTeacher
                ? (async (courseId) => await _unitOfWork.Course.CalculateProgressAsync(courseId))
                : _currentUserService.IsStudent
                    ? (async (courseId) => await _unitOfWork.Course.CalculateProgressAsync(courseId, _currentUserService.Id))
                    : throw new UserRoleNotSupportedException();

            foreach (var c in coursesDto.Items)
                c.Progress = await calculateProgressFunc(c.Id);
        }

        return coursesDto;
    }

    /// <inheritdoc />
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
    public async Task<PaginatedResultDto<CourseParticipantDto>> GetParticipantsAsync(Guid courseId, int pageNumber, int pageSize)
    {
        var course = await _unitOfWork.Course.GetCourseAsync(courseId, nameof(CourseExtendedDto.Participants));
        if (course is null)
            throw new CourseNotFoundException(courseId);

        if (_currentUserService.IsStudent && !course.UserCourses.Any(uc => uc.UserId == _currentUserService.Id))
            throw new UserRoleNotSupportedException("You are not enrolled in this course.");

        var participants = await _unitOfWork.User.GetCourseParticipantsAsync(courseId);

        var paginatedParticipants = participants.ToPaginatedResult(new PagingParameters
        {
            PageNumber = pageNumber,
            PageSize = pageSize
        });

        return _mapper.Map<PaginatedResultDto<CourseParticipantDto>>(paginatedParticipants);
    }

    /// <inheritdoc />
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

    /// <inheritdoc />
    /// <exception cref="CourseNotFoundException">Thrown when the course with the specified ID does not exist.</exception>
    public async Task DeleteAsync(Guid courseId)
    {
        var course = await _unitOfWork.Course.GetCourseAsync(courseId, null);

        if (course is null)
            throw new CourseNotFoundException(courseId);

        await _unitOfWork.Course.ClearDocumentRelationsAsync(courseId);
        await _unitOfWork.CompleteAsync();
        _unitOfWork.DetachAllEntities();

        _unitOfWork.Course.Delete(course);
        await _unitOfWork.CompleteAsync();
    }
}