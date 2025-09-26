using System;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace LMS.Shared.DTOs.LMSActivityDtos
{
    /// <summary>
    /// Data Transfer Object (DTO) used for creating a new <see cref="LMSActivity"/>.
    /// This class contains the properties required to create a new activity in the system.
    /// </summary>
    public class CreateLMSActivityDto
    {
        /// <summary>
        /// Gets or sets the ID of the parent module to which the activity belongs.
        /// This property is required and must be provided when creating an activity.
        /// </summary>
        [Required]
        public Guid ModuleId { get; set; }

        /// <summary>
        /// Gets or sets the type of the activity.
        /// This property is required and must not be empty.
        /// </summary>
        [Required]
        [MinLength(1, ErrorMessage = "LMS Activity type must not be empty.")]
        public string ActivityType { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the name of the activity.
        /// This property is required and must not be empty.
        /// </summary>
        [Required]
        [MinLength(3, ErrorMessage = "LMS Activity name must be at least 3 characters long.")]
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the description of the activity.
        /// This property is optional and can be null or empty if no description is provided.
        /// </summary>
        [AllowNull]
        [MinLength(10, ErrorMessage = "LMS Activity description must be at least 10 characters long.")]
        public string? Description { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the start date of the activity.
        /// This property is required and must be provided when creating an activity.
        /// </summary>
        [Required]
        public DateTime StartDate { get; set; }

        /// <summary>
        /// Gets or sets the end date of the activity.
        /// This property is required and must be provided when creating an activity.
        /// </summary>
        [Required]
        public DateTime EndDate { get; set; }
    }
}
