using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

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
        [AllowNull]
        public Guid? CourseId { get; set; }

        /// <summary>
        /// Gets or sets the name of the module.
        /// This property is optional and can be null or empty if the name is not being updated.
        /// </summary>
        [AllowNull]
        [MinLength(3, ErrorMessage = "Module name must be at least 3 characters long.")]
        public string? Name { get; set; }
        /// <summary>
        /// Gets or sets the description of the module.
        /// This property is optional and can be null or empty if the description is not being updated.
        /// </summary>
        [AllowNull]
        [MinLength(10, ErrorMessage = "Module description must be at least 10 characters long.")]
        public string? Description { get; set; }

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
