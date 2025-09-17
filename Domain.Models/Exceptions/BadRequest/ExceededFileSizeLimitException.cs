/// <summary>
/// Exception type for cases where an uploaded file exceeds the allowed size limit.
/// </summary>
public class ExceededFileSizeLimitException : BadRequestException
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ExceededFileSizeLimitException"/> class with a custom message.
    /// </summary>
    public ExceededFileSizeLimitException(string message)
        : base(message) { }

    /// <summary>
    /// Initializes a new instance of the <see cref="ExceededFileSizeLimitException"/> class with a default message.
    /// </summary>
    public ExceededFileSizeLimitException()
        : base("The uploaded file exceeds the allowed size limit.") { }
}