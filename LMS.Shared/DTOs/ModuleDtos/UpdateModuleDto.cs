namespace LMS.Shared.DTOs.ModuleDtos
{
    /// <summary>
    /// Data Transfer Object (DTO) used for updating a module.
    /// This class contains the properties that can be modified when updating an existing module.
    /// </summary>
    public class UpdateModuleDto
    {
        /// <summary>
        /// Gets or sets the ID of the course to which the module belongs.
        /// This property is optional and can be null if the course association is not being updated.
        /// </summary>
        public Guid? CourseId { get; set; }

        /// <summary>
        /// Gets or sets the name of the module.
        /// This property is optional and can be null or empty if the name is not being updated.
        /// </summary>
        public string? Name { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the description of the module.
        /// This property is optional and can be null or empty if the description is not being updated.
        /// </summary>
        public string? Description { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the start date of the module.
        /// This property is optional and can be null if the start date is not being updated.
        /// </summary>
        public DateTime? StartDate { get; set; }

        /// <summary>
        /// Gets or sets the end date of the module.
        /// This property is optional and can be null if the end date is not being updated.
        /// </summary>
        public DateTime? EndDate { get; set; }
    }
}
