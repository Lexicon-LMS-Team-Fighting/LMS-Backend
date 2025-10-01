using LMS.Shared.DTOs.DocumentDtos;
using LMS.Shared.DTOs.LMSActivityDtos;
using LMS.Shared.DTOs.UserDtos;

namespace LMS.Shared.DTOs.ModuleDtos
{
    /// <summary>
    /// Data Transfer Object (DTO) representing an extended view of a module, including its details and associated entities.
    /// </summary>
    public class ModuleExtendedDto
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
        /// Gets or sets the name of the course to which the module belongs.
        /// </summary>
        public string CourseName { get; set; } = string.Empty;

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
        /// Gets or sets the collection of LMS activities associated with the module.
        /// </summary>
        public ICollection<LMSActivityPreviewDto> Activities { get; } = [];

        /// <summary>
        /// Gets or sets the collection of user previews representing the participants (students) of the module.
        /// </summary>
        public ICollection<UserPreviewDto> Participants { get; } = [];

        /// <summary>
        /// Gets or sets the collection of document previews associated with the module.
        /// </summary>
        public ICollection<DocumentPreviewDto> Documents { get; } = [];
    }
}
