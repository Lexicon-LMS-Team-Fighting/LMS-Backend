using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace LMS.Shared.DTOs.LMSActivityFeedbackDtos
{
    /// <summary>
    /// Data Transfer Object (DTO) for creating feedback for a  activity.
    /// </summary>
    public class CreateLMSActivityFeedbackDto
    {
        /// <summary>
        /// Gets or sets the feedback text provided by the user.
        /// </summary>
        public string? Feedback { get; set; }

        /// <summary>
        /// Gets or sets the status of the feedback (e.g., "Pending", "Reviewed", "Resolved").
        /// </summary>
        [Required]
        [RegularExpression("^(Godkänd|Genomförd|Försenad)$", ErrorMessage = "Status must be 'Godkänd', 'Genomförd' or 'Försenad'.")]
        public string Status { get; set; } = string.Empty;
    }
}
