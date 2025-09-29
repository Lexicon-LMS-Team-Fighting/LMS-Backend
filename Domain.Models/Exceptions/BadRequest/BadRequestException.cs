using System.Net;
using Domain.Models.Exceptions;

/// <summary>
/// Base exception type for bad request errors <see cref="HttpStatusCode.BadRequest"/>.
/// Inherit from this class to create specific bad request exceptions.
/// </summary>
public class BadRequestException : AppException
{
	/// <summary>
	/// Initializes a new instance of the class resulting in a <see cref="HttpStatusCode.BadRequest"/> with a custom message.
	/// </summary>
	public BadRequestException(string message)
        : base(message, HttpStatusCode.BadRequest) { }

	/// <summary>
	/// Initializes a new instance of the class resulting in a <see cref="HttpStatusCode.BadRequest"/> with a default message.
	/// </summary>
	public BadRequestException()
        : base("The request was invalid.", HttpStatusCode.BadRequest) { }
}