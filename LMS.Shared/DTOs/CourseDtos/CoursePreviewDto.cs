namespace LMS.Shared.DTOs.CourseDtos
{
    /// <summary>
    /// Data Transfer Object (DTO) representing a course preview to be used to show brief information about a course.
    /// </summary>
    public class CoursePreviewDto
    {
        /// <summary>
        /// Gets or sets the unique identifier of the course.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Gets or sets the name of the course.
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the start date of the course.
        /// </summary>
        public DateTime StartDate { get; set; }

        /// <summary>
        /// Gets or sets the end date of the course.
        /// </summary>
        public DateTime EndDate { get; set; }

        /// <summary>
        /// Gets or sets the normilized progress of the module (0 to 1).
        /// </summary>
        public decimal? Progress { get; set; }
    }
}
