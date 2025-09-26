namespace Domain.Models.Exceptions.NotFound
{
    /// <summary>
    /// Exception type for cases where an <see cref="LMSActivity"/> is not found.
    /// </summary>
    public class LMSActivityNotFoundException : NotFoundException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LMSActivityNotFoundException"/> class with a specified activity ID.
        /// </summary>
        /// <param name="activityId">The ID of the activity that was not found.</param>
        public LMSActivityNotFoundException(Guid activityId)
            : base($"Activity with Id '{activityId}' was not found.") { }

        /// <summary>
        /// Initializes a new instance of the <see cref="LMSActivityNotFoundException"/> class with a default message.
        /// </summary>
        public LMSActivityNotFoundException()
            : base("The requested activity was not found.") { }
    }
}