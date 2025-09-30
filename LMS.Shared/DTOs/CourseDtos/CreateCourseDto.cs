using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace LMS.Shared.DTOs.CourseDtos;

/// <summary>
/// Represents a creation data transfer object (DTO) for a <see cref="Course"/>. <br />
/// This DTO is used to transfer course-related data across application layers
/// without exposing the full domain entity.
/// </summary>
public class CreateCourseDto
{
	[Required(ErrorMessage = "Name is required.")]
	[MaxLength(100)]
	public string Name { get; set; } = string.Empty;
	
	[AllowNull]
	[MaxLength(500, ErrorMessage = "Description cant be longer than 500 characters.")]
	public string Description { get; set; } = string.Empty;
	
	[Required(ErrorMessage = "Start date is required.")]
	public DateTime StartDate { get; set; }
	
	[Required(ErrorMessage = "End date is required.")]
	public DateTime EndDate { get; set; }
}
