using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models.Exceptions.BadRequest
{
    /// <summary>
    /// Exception thrown when there is a role mismatch in the application.
    /// </summary>
    public class RoleMismatchException : BadRequestException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RoleMismatchException"/> class with a default message.
        /// </summary>
        public RoleMismatchException()
            : base("The user's role does not match the expected role for this operation.") { }

        /// <summary>
        /// Initializes a new instance of the <see cref="RoleMismatchException"/> class with a specified error message.
        /// </summary>
        public RoleMismatchException(string message) : base(message) { }
    }
}