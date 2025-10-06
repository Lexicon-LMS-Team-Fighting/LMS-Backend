using LMS.Shared.DTOs.DocumentDtos;
using LMS.Shared.DTOs.LMSActivityFeedbackDtos;
using LMS.Shared.DTOs.UserDtos;

namespace LMS.Shared.DTOs.LMSActivityDtos
{
    /// <summary>
    /// Data Transfer Object (DTO) representing an extended view of a Learning Management System (LMS) activity.
    /// </summary>
    public class LMSActivityExtendedDto
    {
        /// <summary>
        /// Gets or sets the unique identifier of the activity.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Gets or sets the unique identifier of the parent course to which the activity belongs.
        /// </summary>
        public Guid CourseId { get; set; }

        /// <summary>
        /// Gets or sets the name of the parent course to which the activity belongs.
        /// </summary>
        public string CourseName { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the unique identifier of the parent module to which the activity belongs.
        /// </summary>
        public Guid ModuleId { get; set; }

        /// <summary>
        /// Gets or sets the name of the parent module to which the activity belongs.
        /// </summary>
        public string ModuleName { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the ID of the activity type.
        /// </summary>
        public Guid ActivityTypeId { get; set; }

        /// <summary>
        /// Gets or sets the name of the activity type.
        /// </summary>
        public string ActivityTypeName { get; set; } = string.Empty;

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

        /// <summary>
        /// Gets the collection of user previews representing participants (students) in the activity.
        /// </summary>
        public ICollection<UserPreviewDto> Participants { get; } = [];

        /// <summary>
        /// Gets the collection of feedback previews associated with the activity.
        /// </summary>
        public ICollection<LMSActivityFeedbackPreviewDto> Feedbacks { get; } = [];

        /// <summary>
        /// Gets the collection of document previews associated with the activity.
        /// </summary>
        public ICollection<DocumentPreviewDto> Documents { get; } = [];
    }
}
