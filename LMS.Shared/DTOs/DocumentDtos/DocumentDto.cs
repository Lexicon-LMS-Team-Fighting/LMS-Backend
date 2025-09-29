using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMS.Shared.DTOs.DocumentDtos
{
    /// <summary>
    /// Data Transfer Object (DTO) for <see cref="Document"/>.
    /// Used to transfer document data without exposing the full entity.
    /// </summary>
    public class DocumentDto
    {
        /// <summary>
        /// Gets or sets the unique identifier of the document.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Gets or sets the ID of the user who owns the document.
        /// </summary>
        public string UserId { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the associated course ID, if applicable.
        /// </summary>
        public Guid? CourseId { get; set; }

        /// <summary>
        /// Gets or sets the associated module ID, if applicable.
        /// </summary>
        public Guid? ModuleId { get; set; }

        /// <summary>
        /// Gets or sets the associated activity ID, if applicable.
        /// </summary>
        public Guid? ActivityId { get; set; }

        /// <summary>
        /// Gets or sets the storage path or URL of the document.
        /// </summary>
        public string Path { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the name of the document.
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets an optional description of the document.
        /// </summary>
        public string? Description { get; set; }

        /// <summary>
        /// Gets or sets the timestamp when the document was created or last modified.
        /// </summary>
        public DateTime TimeStamp { get; set; }
    }

}
