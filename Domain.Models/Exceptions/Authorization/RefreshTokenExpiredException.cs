/// <summary>
/// Exception for cases where a provided refresh token has expired.
/// </summary>
public class RefreshTokenExpiredException : AuthorizationException
{
    /// <summary>
    /// Initializes a new instance of the <see cref="RefreshTokenExpiredException"/> class with a custom message.
    /// </summary>
    public RefreshTokenExpiredException(string message)
        : base(message) { }

    /// <summary>
    /// Initializes a new instance of the <see cref="RefreshTokenExpiredException"/> class with a default message.
    /// </summary>
    public RefreshTokenExpiredException()
        : base("The provided refresh token has expired.") { }
}