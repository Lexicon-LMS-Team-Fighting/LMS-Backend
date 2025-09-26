namespace LMS.Shared.DTOs.LMSActivityDtos
{
    /// <summary>
    /// Data Transfer Object (DTO) representing an <see cref="LMSActivity"/>.
    /// This class is used to transfer activity data.
    /// </summary>
    public class LMSActivityDto
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
        /// Gets or sets the type of the activity.
        /// </summary>
        public string ActivityType { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the name of the activity.
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the description of the activity.
        /// </summary>
        public string Description { get; set; } = string.Empty;

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
