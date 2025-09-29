namespace LMS.Shared.DTOs.ActivityTypeDto
{
    /// <summary>
    /// Data Transfer Object (DTO) representing an <see cref="ActivityType"/>.
    /// This class is used to transfer activity data.
    /// </summary>
    public class ActivityTypeDto
    {
        /// <summary>
        /// Gets or sets the unique identifier of the activity type.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Gets or sets the name of the activity type.
        /// </summary>
        public string Name { get; set; } = string.Empty;
    }
}
