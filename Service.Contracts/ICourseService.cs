using LMS.Shared.DTOs.CourseDtos;

namespace Service.Contracts;

/// <summary>
/// Defines the contract for course-related business logic operations. <br/>
/// Provides methods to retrieve user data as <see cref="CourseDto"/> objects.
/// </summary>
public interface ICourseService
{
	/// <summary>
	/// Retrieves a specific course by its unique identifier.
	/// </summary>
	/// <param name="courseId">The unique identifier of the course.</param>
	/// <returns>A <see cref="CourseDto"/> representing the requested course.</returns>
	/// <exception cref="NotFoundException">
	/// Thrown when no course is found with the given <paramref name="courseId"/>.
	/// </exception>
	Task<CourseDto> GetCourseAsync(Guid courseId);

	/// <summary>
	/// Retrieves all courses from the data source.
	/// </summary>
	/// <returns>
	/// A collection of <see cref="CourseDto"/> objects representing all users.
	/// </returns>
	Task<IEnumerable<CourseDto>> GetCoursesAsync();

	/// <summary>
	/// Creates a new course based on the provided data.
	/// </summary>
	/// <param name="createCourseDto">The data for the course to create.</param>
	/// <returns>A <see cref="CourseDto"/> representing the newly created course.</returns>
	Task<CourseDto> CreateCourseAsync(CreateCourseDto createCourseDto);


	/// <summary>
	/// Checks if a course name is unique (i.e., not already in use).
	/// </summary>
	/// <param name="name">Name to check.</param>
	/// <returns>Boolean indicating if the name is already in use.</returns>
	Task<bool> IsUniqueCourseNameAsync(string name);
}
