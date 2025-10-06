using LMS.Shared.DTOs.CourseDtos;
using LMS.Shared.DTOs.PaginationDtos;
using LMS.Shared.DTOs.UserDtos;

namespace Service.Contracts;

/// <summary>
/// Defines the contract for course-related business logic operations.
/// </summary>
public interface ICourseService
{
    /// <summary>
    /// Retrieves a specific course by its unique identifier.
    /// </summary>
    /// <param name="courseId">The unique identifier of the course.</param>
    /// <param name="include">Related entities to include (e.g., "participants", "modules", "documents").</param>
    /// <returns>A <see cref="CourseExtendedDto"/> representing the requested course.</returns>
    Task<CourseExtendedDto> GetCourseAsync(Guid courseId, string? include);

    /// <summary>
    /// Retrieves all courses from the data source.
    /// </summary>
    /// <param name="pageNumber">The page number to retrieve (1-based).</param>
    /// <param name="pageSize">The number of items per page.</param>
    /// <param name="include">Optional fields to include (e.g., "progress").</param>
    /// <returns>
    /// A collection of <see cref="PaginatedResultDto{CoursePreviewDto}"/> objects representing all users.
    /// </returns>
    Task<PaginatedResultDto<CoursePreviewDto>> GetCoursesAsync(int pageNumber, int pageSize, string? include = null);

    /// <summary>
    /// Creates a new course based on the provided data.
    /// </summary>
    /// <param name="createDto">The data for the course to create.</param>
    /// <returns>A <see cref="CourseExtendedDto"/> representing the newly created course.</returns>
    Task<CourseExtendedDto> CreateAsync(CreateCourseDto createDto);

    /// <summary>
    /// Retrieves participants of a specific course.
    /// </summary>
    /// <param name="courseId">The unique identifier of the course.</param>
    /// <param name="pageNumber">The page number to retrieve (1-based).</param>
    /// <param name="pageSize">The number of items per page.</param>
    /// <returns>A paginated list of participants enrolled in the specified course.</returns>
    Task<PaginatedResultDto<CourseParticipantDto>> GetParticipantsAsync(Guid courseId, int pageNumber, int pageSize);
    /// Enrolls a student into the specified course.
    /// </summary>
    /// <param name="courseId">The unique identifier of the course.</param>
    /// <param name="studentId">The unique identifier of the student to enroll.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    Task EnrollStudentAsync(Guid courseId, string studentId);

    /// <summary>
    /// Unenrolls a student from the specified course and removes all their related feedback.
    /// </summary>
    /// <param name="courseId">The unique identifier of the course.</param>
    /// <param name="studentId">The unique identifier of the student to unenroll.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    Task UnenrollStudentAsync(Guid courseId, string studentId);

    /// <summary>
    /// Updates an existing course.
    /// </summary>
    /// <param name="id">The unique identifier of the course to update.</param>
    /// <param name="updateDto">The updated data for the course.</param>
    Task UpdateAsync(Guid id, UpdateCourseDto updateDto);
}
