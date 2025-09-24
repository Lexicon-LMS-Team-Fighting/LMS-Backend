using LMS.Shared.DTOs.UserCourseDtos;
using LMS.Shared.DTOs.UserDtos;
using System.ComponentModel.DataAnnotations;

namespace LMS.Shared.DTOs.CourseDtos;

/// <summary>
/// Represents a data tranfer object (DTO) for a <see cref="Course"/>. <br />
/// This DTO is used to transfer course-related data across application layers
/// without exposing the full domain entity.
/// </summary>
public class CourseDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }

	//Relations
	//TODO: Un-comment this one UserDto is implemented
	public Guid CourseId { get; set; }
	public ICollection<string> UserIds { get; set; } = new List<string>();
    //public ICollection<DocumentDto> Documents { get; set; } = new();
    //public ICollection<ModuleDto> Modules { get; set; } = new();
}
