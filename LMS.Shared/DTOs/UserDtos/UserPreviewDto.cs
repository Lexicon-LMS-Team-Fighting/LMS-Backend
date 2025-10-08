namespace LMS.Shared.DTOs.UserDtos
{
    /// <summary>
    /// Data Transfer Object (DTO) representing a user preview to be used to show brief information about a user.
    /// </summary>
    public class UserPreviewDto
    {
        /// <summary>
        /// Gets or sets the unique identifier of the user.
        /// </summary>
        public string Id { get; set; } = null!;

        /// <summary>
        /// Gets or sets the first name of the user.
        /// </summary>
        public string FirstName { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the last name of the user.
        /// </summary>
        public string LastName { get; set; } = string.Empty;
    }
}
