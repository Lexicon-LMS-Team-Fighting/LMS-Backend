using Domain.Models.Exceptions;

/// <summary>
/// Exception type for cases where a user already exists.
public class UserAlreadyExistsException : ConflictException
{
    /// <summary>
    /// Initializes a new instance of the <see cref="UserAlreadyExistsException"/> class with a specified user ID.
    /// </summary>
    public UserAlreadyExistsException(Guid userId)
        : base($"A user with ID '{userId}' already exists.") { }

    /// <summary>
    /// Initializes a new instance of the <see cref="UserAlreadyExistsException"/> class with a specified username.
    /// </summary>
    public UserAlreadyExistsException(string username)
        : base($"User with username '{username}' already exists.") { }
}