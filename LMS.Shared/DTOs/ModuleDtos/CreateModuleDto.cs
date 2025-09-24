using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMS.Shared.DTOs.ModuleDtos
{
    /// <summary>
    /// Data Transfer Object (DTO) used for creating a new module.
    /// This class contains the properties required to create a new module in the system.
    /// </summary>
    public class CreateModuleDto
    {
        /// <summary>
        /// Gets or sets the ID of the course to which the module belongs.
        /// This property is required and must be provided when creating a module.
        /// </summary>
        [Required]
        public Guid CourseId { get; set; }

        /// <summary>
        /// Gets or sets the name of the module.
        /// This property is required and must not be empty.
        /// </summary>
        [Required]
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the description of the module.
        /// This property is optional and can be null or empty if no description is provided.
        /// </summary>
        public string? Description { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the start date of the module.
        /// This property is required and must be provided when creating a module.
        /// </summary>
        [Required]
        public DateTime StartDate { get; set; }

        /// <summary>
        /// Gets or sets the end date of the module.
        /// This property is required and must be provided when creating a module.
        /// </summary>
        [Required]
        public DateTime EndDate { get; set; }
    }
}
