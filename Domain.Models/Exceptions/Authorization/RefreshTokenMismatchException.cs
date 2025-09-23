/// <summary>
/// Exception for cases where a provided refresh token does not match the expected value.
/// </summary>
public class RefreshTokenMismatchException : AuthorizationException
{
    /// <summary>
    /// Initializes a new instance of the <see cref="RefreshTokenMismatchException"/> class with a custom message.
    /// </summary>
    public RefreshTokenMismatchException(string message)
        : base(message) { }

    /// <summary>
    /// Initializes a new instance of the <see cref="RefreshTokenMismatchException"/> class with a default message.
    /// </summary>
    public RefreshTokenMismatchException()
        : base("The provided refresh token does not match.") { }
}