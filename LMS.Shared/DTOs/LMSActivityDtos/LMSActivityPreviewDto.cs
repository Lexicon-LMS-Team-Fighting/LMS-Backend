namespace LMS.Shared.DTOs.LMSActivityDtos
{
    /// <summary>
    /// Data Transfer Object (DTO) representing an extended view of a Learning Management System (LMS) activity.
    /// </summary>
    public class LMSActivityPreviewDto
    {
        /// <summary>
        /// Gets or sets the unique identifier of the activity.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Gets or sets the unique identifier of the parent module to which the activity belongs.
        /// </summary>
        public Guid ModuleId { get; set; }

        /// <summary>
        /// Gets or sets the name of the parent module.
        /// </summary>
        public Guid CourseId { get; set; }

        /// <summary>
        /// Gets or sets the name of the course.
        /// </summary>
        public string CourseName { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the name of the activity type.
        /// </summary>
        public string ActivityTypeName { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the name of the activity.
        /// </summary>
        public string Name { get; set; } = string.Empty;


        /// <summary>
        /// Gets or sets the start date of the activity.
        /// </summary>
        public DateTime StartDate { get; set; }

        /// <summary>
        /// Gets or sets the end date of the activity.
        /// </summary>
        public DateTime EndDate { get; set; }

    }
}
