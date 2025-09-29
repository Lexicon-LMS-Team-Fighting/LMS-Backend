using System.Net;
using Domain.Models.Exceptions;

/// <summary>
/// Base exception type for authorization-related errors <see cref="HttpStatusCode.Unauthorized"/>.
/// Inherit from this class to create specific authorization exceptions.
/// </summary>
public class AuthorizationException : AppException
{
	/// <summary>
	/// Initializes a new instance of the class resulting in a <see cref="HttpStatusCode.Unauthorized"/> with a custom message.
	/// </summary>
	public AuthorizationException(string message)
        : base(message, HttpStatusCode.Unauthorized) { }

	/// <summary>
	/// Initializes a new instance of the class resulting in a <see cref="HttpStatusCode.Unauthorized"/> with a default message.
	/// </summary>
	public AuthorizationException()
        : base("You do not have permission to perform this action.", HttpStatusCode.Unauthorized) { }
}