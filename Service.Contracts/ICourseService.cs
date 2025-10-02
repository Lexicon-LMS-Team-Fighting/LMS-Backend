using LMS.Shared.DTOs.CourseDtos;
using LMS.Shared.DTOs.PaginationDtos;

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
    /// <returns>
    /// A collection of <see cref="PaginatedResultDto{CoursePreviewDto}"/> objects representing all users.
    /// </returns>
    Task<PaginatedResultDto<CoursePreviewDto>> GetCoursesAsync(int pageNumber, int pageSize);

    /// <summary>
    /// Creates a new course based on the provided data.
    /// </summary>
    /// <param name="createDto">The data for the course to create.</param>
    /// <returns>A <see cref="CourseExtendedDto"/> representing the newly created course.</returns>
    Task<CourseExtendedDto> CreateAsync(CreateCourseDto createDto);

    /// <summary>
    /// Updates an existing course.
    /// </summary>
    /// <param name="id">The unique identifier of the course to update.</param>
    /// <param name="updateDto">The updated data for the course.</param>
    Task UpdateAsync(Guid id, UpdateCourseDto updateDto);
}
