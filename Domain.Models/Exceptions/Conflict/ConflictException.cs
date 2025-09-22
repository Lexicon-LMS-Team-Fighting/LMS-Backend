using System;
using System.Net;

namespace Domain.Models.Exceptions
{
    /// <summary>
    /// Base exception type for conflict-related errors <see cref="HttpStatusCode.Conflict"/>.
    /// Inherit from this class to create specific conflict exceptions.
    /// </summary>
    public class ConflictException : AppException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ConflictException"/> class with a custom message.
        /// </summary>
        public ConflictException(string message)
            : base(message, HttpStatusCode.Conflict) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="ConflictException"/> class with a default message.
        /// </summary>
        public ConflictException()
            : base("A conflict occurred.", HttpStatusCode.Conflict) { }
    }
}