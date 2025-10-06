using System.Net;
using Domain.Models.Exceptions;

/// <summary>
/// Base exception type for authorization-related errors
/// Inherit from this class to create specific authorization exceptions.
/// </summary>
public class AuthorizationException : AppException
{
	/// <summary>
	/// Initializes a new instance of the class resulting in a <see cref="HttpStatusCode.Unauthorized"/> with a custom message.
	/// </summary>
	public AuthorizationException(string message, HttpStatusCode statusCode = HttpStatusCode.Unauthorized)
        : base(message, statusCode) { }

	/// <summary>
	/// Initializes a new instance of the class resulting in a <see cref="HttpStatusCode.Unauthorized"/> with a default message.
	/// </summary>
	public AuthorizationException(HttpStatusCode statusCode = HttpStatusCode.Unauthorized)
        : base("You do not have permission to perform this action.", statusCode) { }
}