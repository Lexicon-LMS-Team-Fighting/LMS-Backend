using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models.Exceptions.Conflict
{
    /// <summary>
    /// Exception type for cases where a course already exists in the system.
    /// </summary>
    public class CourseAlreadyExistsException : ConflictException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CourseAlreadyExistsException"/> with a specified course ID.
        /// </summary>
        public CourseAlreadyExistsException(Guid courseId)
            : base($"Course with Id '{courseId}' already exists.") { }

        /// <summary>
        /// Initializes a new instance of the <see cref="CourseAlreadyExistsException"/> class with a default message.
        /// </summary>
        public CourseAlreadyExistsException()
            : base("The requested course already exists") { }
    }
}
