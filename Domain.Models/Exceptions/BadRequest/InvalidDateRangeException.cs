using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models.Exceptions.BadRequest
{
    /// <summary>
    /// Exception type for cases where a provided date range is invalid, such as when the start date is later than the end date.
    /// </summary>
    public class InvalidDateRangeException : BadRequestException
    {
		/// <summary>
		/// Initializes a new instance of the <see cref="InvalidDateRangeException"/> class with a custom message.
		/// </summary>
		public InvalidDateRangeException(DateTime start, DateTime end)
            : base($"The provided date range is invalid: Start Date ({start}) must be earlier than End Date ({end}).") { }

        /// <summary>
        /// Initializes a new instance of the <see cref="InvalidDateRangeException"/> class with a default message.
        /// </summary>
        public InvalidDateRangeException()
            : base("The provided date range is invalid. Please ensure that the start date is earlier than the end date.") { }
    }
}
