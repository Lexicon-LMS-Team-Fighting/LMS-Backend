namespace Domain.Models.Exceptions.Conflict
{
    /// <summary>
    /// Exception representing a conflict when a document is already attached to an entity
    /// (e.g., a Course, Module, or Activity).
    /// </summary>
    public class DocumentAlreadyAttachedException : ConflictException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DocumentAlreadyAttachedException"/> class
        /// for a general entity type when the specific type is known at runtime.
        /// </summary>
        /// <param name="documentId">The unique identifier of the document.</param>
        /// <param name="entityId">The unique identifier of the related entity.</param>
        /// <param name="entityType">The type of the entity (e.g., "Course", "Module", "Activity").</param>
        public DocumentAlreadyAttachedException(Guid documentId, Guid entityId, string entityType)
            : base($"Document with Id '{documentId}' is already attached to {entityType} with Id '{entityId}'.")
        {
        }
    }

}
