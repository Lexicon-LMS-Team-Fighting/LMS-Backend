using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models.Exceptions.Conflict
{
    /// <summary>
    /// Exception type for cases where a module already exists.
    public class ModuleNameAlreadyExistsException : ConflictException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ModuleNameAlreadyExistsException"/> class with a specified name and course ID.
        /// </summary>
        public ModuleNameAlreadyExistsException(string name, Guid courseId)
            : base($"A module with the name '{name}' already exists in the course with ID '{courseId}'.") { }

        /// <summary>
        /// Initializes a new instance of the <see cref="ModuleNameAlreadyExistsException"/> class with a specified name.
        /// </summary>
        public ModuleNameAlreadyExistsException(string name)
            : base($"A module with the name '{name}' already exists in a course.") { }

        /// <summary>
        /// Initializes a new instance of the <see cref="ModuleNameAlreadyExistsException"/> class with a default message.
        /// </summary>
        public ModuleNameAlreadyExistsException()
            : base($"A module with the specified name already exists in a course") { }
    }
}
