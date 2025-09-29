namespace LMS.Shared.DTOs.CourseDtos;

/// <summary>
/// Represents a data transfer object (DTO) for a <see cref="Course"/>. <br />
/// This DTO is used to transfer course-related data across application layers
/// without exposing the full domain entity.
/// </summary>
public class CourseDetailedDto
{
    /// <summary>
    /// Gets or sets the unique identifier of the course.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Gets or sets the name of the course.
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets a detailed description of the course.
    /// </summary>
    public string Description { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the start date of the course.
    /// </summary>
    public DateTime StartDate { get; set; }

    /// <summary>
    /// Gets or sets the end date of the course.
    /// </summary>
    public DateTime EndDate { get; set; }

    /// <summary>
    /// Gets or sets the collection of documents associated with the course,
    /// provided in a preview format.
    /// </summary>
    public IEnumerable<DocumentPreviewDto> Documents { get; set; } = [];
}