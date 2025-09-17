using Domain.Models.Exceptions;

/// <summary>
/// Exception type for cases where a user is not found.
/// </summary>
public class UserNotFoundException : NotFoundException
{
    /// <summary>
    /// Initializes a new instance of the <see cref="UserNotFoundException"/> class with a specified user ID.
    /// </summary>
    public UserNotFoundException(Guid userId)
        : base($"User with Id '{userId}' was not found.") { }

    /// <summary>
    /// Initializes a new instance of the <see cref="UserNotFoundException"/> class with a specified username.
    /// </summary>
    public UserNotFoundException(string username)
        : base($"User with username '{username}' was not found.") { }

    /// <summary>
    /// Initializes a new instance of the <see cref="UserNotFoundException"/> class with a default message.
    /// </summary>
    public UserNotFoundException()
        : base("The requested user was not found.") { }
}