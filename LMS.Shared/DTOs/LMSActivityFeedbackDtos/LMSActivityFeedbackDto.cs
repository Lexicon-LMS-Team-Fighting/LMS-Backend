namespace LMS.Shared.DTOs.LMSActivityFeedbackDtos
{
    /// <summary>
    /// Data Transfer Object (DTO) for representing feedback on an LMS activity.
    /// </summary>
    public class LMSActivityFeedbackDto
    {
        /// <summary>
        /// The unique identifier of the user associated with this feedback.
        /// </summary>
        public string UserId { get; set; } = null!;

        /// <summary>
        /// The status of the feedback
        /// </summary>
        public string Status { get; set; } = string.Empty;

        /// <summary>
        /// Optional feedback provided by the user for the activity.
        /// </summary>
        public string? Feedback { get; set; }
    }
}
