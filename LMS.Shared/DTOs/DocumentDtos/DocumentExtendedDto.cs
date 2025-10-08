namespace LMS.Shared.DTOs.DocumentDtos
{
    /// <summary>
    /// Data Transfer Object (DTO) representing detailed information about a document in the Learning Management System (LMS).
    /// </summary>
    public class DocumentExtendedDto
    {
        /// <summary>
        /// Gets or sets the unique identifier of the document.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Gets or sets the unique identifier of the user who uploaded or is associated with the document.
        /// </summary>
        public string UserId { get; set; } = null!;

        /// <summary>
        /// Gets or sets the full name of the user who uploaded or is associated with the document.
        /// </summary>
        public string UserFullName { get; set; } = null!;

        /// <summary>
        /// Gets or sets the unique identifier of the course the document is associated with, if any.
        /// </summary>
        public Guid? CourseId { get; set; }

        /// <summary>
        /// Gets or sets the unique identifier of the module the document is associated with, if any.
        /// </summary>
        public Guid? ModuleId { get; set; }

        /// <summary>
        /// Gets or sets the unique identifier of the activity the document is associated with, if any.
        /// </summary>
        public Guid? ActivityId { get; set; }

        /// <summary>
        /// Gets or sets the name of the document.
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets a brief description of the document.
        /// </summary>
        public string? Description { get; set; }

        /// <summary>
        /// Gets or sets the timestamp indicating when the document was uploaded or last modified.
        /// </summary>
        public DateTime TimeStamp { get; set; }
    }
}
