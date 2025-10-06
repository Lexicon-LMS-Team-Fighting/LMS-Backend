namespace Domain.Models.Exceptions.Conflict
{
    /// <summary>
    /// Exception class representing a conflict error when a user is not enrolled in a specific activity.
    /// </summary>
    public class UserNotEnrolledInActivityException : ConflictException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UserNotEnrolledInActivityException"/> class.
        /// </summary>
        /// <param name="userId">The unique identifier of the user.</param>
        /// <param name="activityId">The unique identifier of the activity.</param>
        public UserNotEnrolledInActivityException(string userId, Guid activityId)
            : base($"User with ID '{userId}' is not enrolled in activity with ID '{activityId}'.")
        {
        }
    }
}
