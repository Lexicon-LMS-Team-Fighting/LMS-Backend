using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models.Exceptions.BadRequest
{
    /// <summary>
    /// Exception type for cases where a provided date range for a module is out of date range for a course
    /// </summary>
    public class InvalidModuleDateRangeException : BadRequestException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="InvalidModuleDateRangeException"/> class with a default message.
        /// </summary>
        public InvalidModuleDateRangeException()
            : base("The provided date range is invalid. Please ensure that module date range falls within the course date range.") { }
    }
}
