using LMS.Shared.DTOs.DocumentDtos;

namespace LMS.Shared.DTOs.ModuleDtos
{
    /// <summary>
    /// Data Transfer Object (DTO) representing a module.
    /// This class is used to transfer module data
    /// </summary>
    public class ModuleDetailedDto
    {
        /// <summary>
        /// Gets or sets the unique identifier of the module.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Gets or sets the unique identifier of the course to which the module belongs.
        /// </summary>
        public Guid CourseId { get; set; }

        /// <summary>
        /// Gets or sets the name of the module.
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the description of the module.
        /// </summary>
        public string Description { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the start date of the module.
        /// </summary>
        public DateTime StartDate { get; set; }

        /// <summary>
        /// Gets or sets the end date of the module.
        /// </summary>
        public DateTime EndDate { get; set; }

        /// <summary>
        /// Gets or sets the collection of documents associated with the module in a preview format.
        /// </summary>
        public IEnumerable<DocumentPreviewDto> Documents { get; set; } = [];
    }
}
