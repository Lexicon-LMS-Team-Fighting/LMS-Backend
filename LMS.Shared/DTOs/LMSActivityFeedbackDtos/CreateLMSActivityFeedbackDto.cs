using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMS.Shared.DTOs.LMSActivityFeedbackDtos
{
    /// <summary>
    /// Data Transfer Object (DTO) for creating feedback for a  activity.
    /// </summary>
    public class CreateLMSActivityFeedbackDto
    {
        /// <summary>
        /// Gets or sets the unique identifier of the user who provided the feedback.
        /// </summary>
        public string UserId { get; set; } = null!;

        /// <summary>
        /// Gets or sets the unique identifier of the LMS activity associated with the feedback.
        /// </summary>
        public Guid ActivityId { get; set; }

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
