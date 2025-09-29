using System;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace LMS.Shared.DTOs.LMSActivityDtos
{
    /// <summary>
    /// Data Transfer Object (DTO) used for updating an <see cref="LMSActivity"/>.
    /// Contains the properties that can be modified when updating an existing activity.
    /// </summary>
    public class UpdateLMSActivityDto
    {
        /// <summary>
        /// Gets or sets the ID of the parent module to which the activity belongs.
        /// This property is optional and can be null if the module association is not being updated.
        /// </summary>
        [AllowNull]
        public Guid? ModuleId { get; set; }

        /// <summary>
        /// Gets or sets the ID of the activity type.
        /// This property is required and must be provided when creating an activity.
        /// </summary>
        [AllowNull]
        public Guid? ActivityTypeId { get; set; }

        /// <summary>
        /// Gets or sets the name of the activity.
        /// This property is optional and can be null or empty if the name is not being updated.
        /// </summary>
        [AllowNull]
        [MinLength(3, ErrorMessage = "Activity name must be at least 3 characters long.")]
        public string? Name { get; set; }

        /// <summary>
        /// Gets or sets the description of the activity.
        /// This property is optional and can be null or empty if the description is not being updated.
        /// </summary>
        [AllowNull]
        [MinLength(10, ErrorMessage = "Activity description must be at least 10 characters long.")]
        public string? Description { get; set; }

        /// <summary>
        /// Gets or sets the start date of the activity.
        /// This property is optional and can be null if the start date is not being updated.
        /// </summary>
        public DateTime? StartDate { get; set; }

        /// <summary>
        /// Gets or sets the end date of the activity.
        /// This property is optional and can be null if the end date is not being updated.
        /// </summary>
        public DateTime? EndDate { get; set; }
    }
}
