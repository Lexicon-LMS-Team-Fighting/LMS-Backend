using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models.Exceptions.Conflict
{
    /// <summary>
    /// Exception type for cases where an activity with the specified name already exists in a course.
    /// </summary>
    public class LMSActivityNameAlreadyExistsException : ConflictException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LMSActivityNameAlreadyExistsException"/> class with a specified name and course ID.
        /// </summary>
        public LMSActivityNameAlreadyExistsException(string name, Guid courseId)
            : base($"An activity with the name '{name}' already exists in the course with ID '{courseId}'.") { }

        /// <summary>
        /// Initializes a new instance of the <see cref="LMSActivityNameAlreadyExistsException"/> class with a specified name.
        /// </summary>
        public LMSActivityNameAlreadyExistsException(string name)
            : base($"An activity with the name '{name}' already exists in a course.") { }

        /// <summary>
        /// Initializes a new instance of the <see cref="LMSActivityNameAlreadyExistsException"/> class with a default message.
        /// </summary>
        public LMSActivityNameAlreadyExistsException()
            : base("An activity with the specified name already exists in a course.") { }
    }
}
