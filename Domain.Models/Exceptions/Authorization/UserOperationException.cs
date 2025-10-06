using System;
using System.Collections.Generic;
using System.Net;

namespace Domain.Exceptions
{
    /// <summary>
    /// Exception thrown when user creation fails.
    /// </summary>
    public class UserOperationException : AuthorizationException
    {
        /// <summary>
        /// Gets the list of errors that caused the exception.
        /// </summary>
        public IEnumerable<string> Errors { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="UserOperationException"/> class with the specified errors.
        /// </summary>
        /// <param name="errors">The errors that caused the exception.</param>
        public UserOperationException(IEnumerable<string> errors)
            : base("User creation failed due to one or more errors.", HttpStatusCode.InternalServerError)
        {
            Errors = errors;
        }
    }
}
