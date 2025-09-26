namespace Domain.Models.Exceptions.Conflict;

/// <summary>
/// Exception type for cases where a course name already exists.
public class CourseNameAlreadyExistsException : ConflictException
{
    /// <summary>
    /// Initializes a new instance of the <see cref="CourseNameAlreadyExistsException"/> class with specified name.
    /// </summary>
    public CourseNameAlreadyExistsException(string name)
        : base($"A course with the name '{name}' already exists.") { }

}
