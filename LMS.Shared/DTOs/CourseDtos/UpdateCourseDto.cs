using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMS.Shared.DTOs.CourseDtos
{
    /// <summary>
    /// Data Transfer Object (DTO) used for updating an existing course.
    /// </summary>
    public class UpdateCourseDto
    {
        /// <summary>
        /// Gets or sets the name of the activity.
        /// This property is optional and can be null or empty if the name is not being updated.
        /// </summary>
        [AllowNull]
        [MinLength(3, ErrorMessage = "Course name must be at least 3 characters long.")]
        public string? Name { get; set; }

        /// <summary>
        /// Gets or sets the description of the activity.
        /// This property is optional and can be null or empty if the description is not being updated.
        /// </summary>
        [AllowNull]
        [MinLength(10, ErrorMessage = "Course description must be at least 10 characters long.")]
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the start date of the activity.
        /// This property is optional and can be null if the start date is not being updated.
        /// </summary>
        [AllowNull]
        public DateTime? StartDate { get; set; }

        /// <summary>
        /// Gets or sets the end date of the activity.
        /// This property is optional and can be null if the end date is not being updated.
        /// </summary>
        [AllowNull]
        public DateTime? EndDate { get; set; }
    }
}
