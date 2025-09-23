using LMS.Shared.DTOs.CourseDtos;

namespace Service.Contracts;

/// <summary>
/// Defines the contract for course-related business logic operations. <br/>
/// Provides methods to retrieve user data as <see cref="CourseDto"/> objects.
/// </summary>
public interface ICourseService
{
	Task<CourseDto> GetCourseAsync(Guid courseId);
	Task<IEnumerable<CourseDto>> GetCoursesAsync();
}
