namespace Domain.Models.Exceptions.NotFound
{
    /// <summary>
    /// Exception type for cases where an <see cref="ActivityType"/> is not found.
    /// </summary>
    public class ActivityTypeNotFoundException : NotFoundException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ActivityTypeNotFoundException"/> class with a specified course ID.
        /// </summary>
        public ActivityTypeNotFoundException(Guid activityId)
            : base($"Activity type with Id '{activityId}' was not found.") { }

        /// <summary>
        /// Initializes a new instance of the <see cref="LMSActivityNotFoundException"/> class with a default message.
        /// </summary>
        public ActivityTypeNotFoundException()
            : base("The requested activity was not found.") { }
    }
}
