using System.Net;

namespace Domain.Models.Exceptions;

/// <summary>
/// Base exception type for not found errors.
/// Inherit from this class to create specific not found exceptions.
/// </summary>
public class NotFoundException : AppException
{
    /// <summary>
    /// Initializes a new instance of the <see cref="NotFoundException"/> class with a custom message.
    /// </summary>
    public NotFoundException(string message)
        : base(message, HttpStatusCode.NotFound) { }

    /// <summary>
    /// Initializes a new instance of the <see cref="NotFoundException"/> class with a default message.
    /// </summary>
    public NotFoundException()
        : base("The requested resource was not found.", HttpStatusCode.NotFound) { }
}
