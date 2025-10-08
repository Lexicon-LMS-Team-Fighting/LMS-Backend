using LMS.Shared.DTOs.DocumentDtos;
using LMS.Shared.DTOs.ModuleDtos;
using LMS.Shared.DTOs.UserDtos;

namespace LMS.Shared.DTOs.CourseDtos
{
    /// <summary>
    /// Data Transfer Object (DTO) representing a course preview to be used to show brief information about a course.
    /// </summary>
    public class CourseExtendedDto
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
        /// Gets or sets the description of the course.
        /// </summary>
        public string Description { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the start date of the course.
        /// </summary>
        public DateTime StartDate { get; set; }

        /// <summary>
        /// Gets or sets the end date of the course.
        /// </summary>
        public DateTime EndDate { get; set; }

        /// <summary>
        /// Gets or sets the normalized progress of the module, represented as a decimal value between 0 and 1.
        /// </summary>
        public decimal? Progress { get; set; }

        /// <summary>
        /// Gets the collection of modules associated with the course.
        /// </summary>
        public ICollection<ModulePreviewDto> Modules { get; } = [];

        /// <summary>
        /// Gets the collection of participants (students) enrolled in the course.
        /// </summary>
        public ICollection<UserPreviewDto> Participants { get; } = [];

        /// <summary>
        /// Gets the collection of documents associated with the course.
        /// </summary>
        public ICollection<DocumentPreviewDto> Documents { get; } = [];
    }
}
