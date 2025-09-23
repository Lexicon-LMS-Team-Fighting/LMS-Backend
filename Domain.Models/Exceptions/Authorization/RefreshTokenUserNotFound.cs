/// <summary>
/// Exception type for cases where a user associated with a refresh token is not found.
/// </summary>
public class RefreshTokenUserMissingException : AuthorizationException
{
    /// <summary>
    /// Initializes a new instance of the <see cref="RefreshTokenUserMissingException"/> class with a custom message.
    /// </summary>
    public RefreshTokenUserMissingException(string message)
        : base(message) { }

    /// <summary>
    /// Initializes a new instance of the <see cref="RefreshTokenUserMissingException"/> class with a default message.
    /// </summary>
    public RefreshTokenUserMissingException()
        : base("The user associated with the provided refresh token was not found.") { }
}