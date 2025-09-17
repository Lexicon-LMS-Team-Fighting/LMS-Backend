using System.Net;

namespace Domain.Models.Exceptions;

/// <summary>
/// Base exception class for application-specific exceptions that are handled in a standardized way.
/// Inherit from this class to create exceptions that cover more specific cases.
/// </summary>
public class AppException : Exception
{
    public HttpStatusCode StatusCode { get; }
    public string? Title { get; }

    /// <summary>
    /// Initializes a new instance of the AppException class with a specified error message, status code, and optional title.
    /// </summary>
    /// <param name="message">The error message that explains the reason for the exception.</param>
    /// <param name="statusCode">The HTTP status code associated with the exception. Defaults to 500 (Internal Server Error).</param>
    /// <param name="title">An optional title for the error.</param>
    public AppException(string message, HttpStatusCode statusCode = HttpStatusCode.InternalServerError, string? title = null)
        : base(message)
    {
        StatusCode = statusCode;
        Title = title;
    }
}