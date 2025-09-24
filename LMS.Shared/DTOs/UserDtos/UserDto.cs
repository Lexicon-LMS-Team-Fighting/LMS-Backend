using LMS.Shared.DTOs.CourseDtos;
using LMS.Shared.DTOs.UserCourseDtos;

namespace LMS.Shared.DTOs.UserDtos;

/// <summary>
/// Represents a data transfer object (DTO) for a user.
/// This DTO is used to transfer user-related data across application layers
/// without exposing the full domain entity.
/// </summary>
public class UserDto
{
	public string Id { get; set; } = null!;
	public string FirstName { get; set; } = string.Empty;
	public string LastName { get; set; } = string.Empty;
	public string UserName { get; set; } = string.Empty;
	public string Email { get; set; } = string.Empty;
	
	public ICollection<Guid> CourseIds { get; set; } = new List<Guid>();
}
