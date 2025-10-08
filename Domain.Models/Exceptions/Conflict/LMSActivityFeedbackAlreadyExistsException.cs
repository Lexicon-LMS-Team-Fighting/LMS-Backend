using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models.Exceptions.Conflict
{
    /// <summary>
    /// Exception type for cases where feedback for a specific LMS activity by a user already exists.
    /// </summary>
    public class LMSActivityFeedbackAlreadyExistsException : ConflictException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LMSActivityFeedbackAlreadyExistsException"/> class with the specified activity ID and user ID.
        /// </summary>
        public LMSActivityFeedbackAlreadyExistsException(Guid activityId, string userId)
            : base($"Feedback for activity with ID '{activityId}' by user '{userId}' already exists.") { }
    }
}
