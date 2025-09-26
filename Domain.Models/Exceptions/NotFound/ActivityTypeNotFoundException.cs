namespace Domain.Models.Exceptions.NotFound
{
    /// <summary>
    /// Exception type for cases where an <see cref="ActivityType"/> is not found.
    /// </summary>
    public class ActivityTypeNotFoundException : NotFoundException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LMSActivityNotFoundException"/> class with a specified activity type name.
        /// </summary>
        /// <param name="activityId">The name of the activity type that was not found.</param>
        public ActivityTypeNotFoundException(string activityName)
            : base($"Activity with name '{activityName}' was not found.") { }

        /// <summary>
        /// Initializes a new instance of the <see cref="LMSActivityNotFoundException"/> class with a default message.
        /// </summary>
        public ActivityTypeNotFoundException()
            : base("The requested activity was not found.") { }
    }
}
