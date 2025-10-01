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
    /// <summary>
    /// Gets or sets the ID of the course 
    /// This property is required and must be provided when creating a module.
    /// </summary>
    [Required(ErrorMessage = "Name is required.")]
	[MaxLength(100)]
	public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the description of the course. <br />
	/// This property is optional and can be null or empty if no description is provided.
    /// </summary>
    [AllowNull]
    [MinLength(10, ErrorMessage = "Course description must be at least 10 characters long.")]
    [MaxLength(500, ErrorMessage = "Course description cant be longer than 500 characters.")]
	public string Description { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the start date of the course. <br />
    /// This property is required and must be provided when creating a module.
    [Required(ErrorMessage = "Start date is required.")]
	public DateTime StartDate { get; set; }

    /// <summary>
    /// Gets or sets the end date of the course. <br />
    /// This property is required and must be provided when creating a module.
    /// </summary>
    [Required(ErrorMessage = "End date is required.")]
	public DateTime EndDate { get; set; }
}
