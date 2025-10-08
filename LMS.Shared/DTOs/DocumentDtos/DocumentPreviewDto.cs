namespace LMS.Shared.DTOs.DocumentDtos
{
    /// <summary>
    /// Data Transfer Object (DTO) representing a document preview to be used to show brief information about a document.
    /// </summary>
    public class DocumentPreviewDto
    {
        /// <summary>
        /// Gets or sets the unique identifier of the document.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Gets or sets the name of the document.
        /// </summary>
        public string Name { get; set; } = string.Empty;
    }
}
