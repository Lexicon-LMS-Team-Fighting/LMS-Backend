using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models.Exceptions.BadRequest
{
    /// <summary>
    /// Exception type for cases where a provided date range for an activity is out of the date range of its parent course.
    /// </summary>
    public class InvalidLMSActivityDateRangeException : BadRequestException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="InvalidLMSActivityDateRangeException"/> class with a default message.
        /// </summary>
        public InvalidLMSActivityDateRangeException()
            : base("The provided date range is invalid. Please ensure that the activity date range falls within the course date range.")
        { }
    }

}
