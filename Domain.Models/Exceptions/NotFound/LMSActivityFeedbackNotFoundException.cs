namespace Domain.Models.Exceptions.NotFound
{
    /// <summary>
    /// Exception type for cases where a LMS activity feedback is not found.
    /// </summary>
    public class LMSActivityFeedbackNotFoundException : NotFoundException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LMSActivityFeedbackNotFoundException"/> class with specified activity feedback ID.
        /// </summary>
        public LMSActivityFeedbackNotFoundException(Guid activityId, string userId)
            : base($"LMS activity feedback for activity with id: '{activityId}' and user with id: '{userId}' does not exist.") { }
    }
}