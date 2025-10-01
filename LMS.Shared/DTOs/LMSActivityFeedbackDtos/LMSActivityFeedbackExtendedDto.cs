namespace LMS.Shared.DTOs.LMSActivityFeedbackDtos
{
    /// <summary>
    /// Data Transfer Object (DTO) representing extended feedback information for a Learning Management System (LMS) activity.
    /// </summary>
    public class LMSActivityFeedbackExtendedDto
    {
        /// <summary>
        /// Gets or sets the unique identifier of the user who provided the feedback.
        /// </summary>
        public string UserId { get; set; } = null!;

        /// <summary>
        /// Gets or sets the unique identifier of the LMS activity associated with the feedback.
        /// </summary>
        public Guid LMSActivityId { get; set; }

        /// <summary>
        /// Gets or sets the feedback text provided by the user.
        /// </summary>
        public string? Feedback { get; set; }

        /// <summary>
        /// Gets or sets the status of the feedback (e.g., "Pending", "Reviewed", "Resolved").
        /// </summary>
        public string Status { get; set; } = string.Empty;
    }
}
