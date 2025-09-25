namespace Domain.Models.Exceptions;

/// <summary>
/// Exception type for cases where a course is not found.
/// </summary>
public class CourseNotFoundException : NotFoundException
{
    /// <summary>
    /// Initializes a new instance of the <see cref="CourseNotFoundException"/> class with a specified course ID.
    /// </summary>
    public CourseNotFoundException(Guid moduleId)
        : base($"Course with Id '{moduleId}' was not found.") { }

    /// <summary>
    /// Initializes a new instance of the <see cref="CourseNotFoundException"/> class with a default message.
    /// </summary>
    public CourseNotFoundException()
        : base("The requested course was not found.") { }
}