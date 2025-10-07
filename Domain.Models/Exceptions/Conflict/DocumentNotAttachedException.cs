namespace Domain.Models.Exceptions.Conflict
{
    /// <summary>
    /// Exception representing a conflict when a document is not attached to an entity
    /// </summary>
    public class DocumentNotAttachedException : ConflictException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DocumentAlreadyAttachedException"/> class
        /// </summary>
        /// <param name="documentId">The unique identifier of the document.</param>
        /// <param name="entityType">The type of the entity (e.g., "Course", "Module", "Activity").</param>
        public DocumentNotAttachedException(Guid documentId, string entityType)
            : base($"Document with Id '{documentId}' is not attached to any {entityType}.") { }

        public DocumentNotAttachedException(Guid documentId, string entityType, Guid entityId)
            : base($"Document with Id '{documentId}' is not attached to the {entityType} with Id '{entityId}'.") { }
    }

}
