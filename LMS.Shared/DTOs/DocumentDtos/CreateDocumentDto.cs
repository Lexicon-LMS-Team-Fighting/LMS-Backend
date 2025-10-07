using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace LMS.Shared.DTOs.DocumentDtos
{
    /// <summary>
    /// Data Transfer Object (DTO) representing detailed information about a document in the Learning Management System (LMS).
    /// </summary>
    public class CreateDocumentDto
    {
        /// <summary>
        /// Gets or sets the name of the document.
        /// </summary>
        [Required]
        [MinLength(3, ErrorMessage = "Document name must be at least 3 characters long.")]
        [MaxLength(100, ErrorMessage = "Document name cannot exceed 100 characters.")]
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets a brief description of the document.
        /// </summary>
        [AllowNull]
        [MinLength(10, ErrorMessage = "Description must be at least 10 characters long.")]
        [MaxLength(500, ErrorMessage = "Description cannot exceed 500 characters.")]
        public string? Description { get; set; }

        /// <summary>
        /// Gets or sets the file associated with the document.
        /// </summary>
        [Required(ErrorMessage = "A document file is required.")]
        public IFormFile File { get; set; } = default!;
    }
}
