namespace Domain.Models.Exceptions.NotFound
{
    /// <summary>
    /// Exception type for cases where an <see cref="Document"/> is not found.
    /// </summary>
    public class DocumentNotFoundException : NotFoundException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DocumentNotFoundException"/> class with a specified document ID.
        /// </summary>
        public DocumentNotFoundException(Guid documentId)
            : base($"Document type with Id '{documentId}' was not found.") { }

        /// <summary>
        /// Initializes a new instance of the <see cref="DocumentNotFoundException"/> class with a default message.
        /// </summary>
        public DocumentNotFoundException()
            : base("The requested document was not found.") { }
    }
}
