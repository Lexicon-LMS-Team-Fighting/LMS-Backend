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
        /// Initializes a new instance of the <see cref="UserCreationException"/> class with the specified errors.
        /// </summary>
        /// <param name="errors">The errors that caused the exception.</param>
        public UserOperationException(IEnumerable<string> errors)
            : base($"An error occured while creating or updating the user: {string.Join(", ", errors)}", HttpStatusCode.BadRequest) { }
    }
}
