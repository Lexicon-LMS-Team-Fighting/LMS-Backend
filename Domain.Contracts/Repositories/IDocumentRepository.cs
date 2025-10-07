using Domain.Models.Entities;

namespace Domain.Contracts.Repositories
{
    /// <summary>
    /// Defines the contract for document-specific data access operations. <br/>
    /// </summary>
    public interface IDocumentRepository : IRepositoryBase<Document>
    {
        /// <summary>
        /// Retrieves a paginated list of all documents in the system.
        /// </summary>
        /// <returns>
        /// A collection of <see cref="Document"/> entities with pagination support.
        /// </returns>
        Task<IEnumerable<Document>> GetAllAsync();

        /// <summary>
        /// Retrieves a paginated list of documents related to a specific user. <br/>
        /// If the user is a student → returns only their own uploaded documents. <br/>
        /// If the user is a teacher → returns all documents across their courses/modules.
        /// </summary>
        /// <param name="userId">The unique identifier of the user.</param>
        /// <returns>
        /// A collection of <see cref="Document"/> entities associated with the given user.
        /// </returns>
        Task<IEnumerable<Document>> GetAllAsync(string userId);

        /// <summary>
        /// Retrieves a specific document by its unique identifier.
        /// </summary>
        /// <param name="documentId">The unique identifier of the document.</param>
        /// <returns>The corresponding <see cref="Document"/> entity, or null if not found.</returns>
        Task<Document?> GetByIdAsync(Guid documentId);

        /// <summary>
        /// Retrieves a specific document by ID, ensuring that it belongs to or is accessible by the given user.
        /// </summary>
        /// <param name="documentId">The unique identifier of the document.</param>
        /// <param name="userId">The unique identifier of the user requesting the document.</param>
        /// <returns>
        /// The <see cref="Document"/> entity if accessible by the user; otherwise, null.
        /// </returns>
        Task<Document?> GetByIdAsync(Guid documentId, string userId);

        /// <summary>
        /// Retrieves all documents attached to a specific course.
        /// </summary>
        /// <param name="courseId">The unique identifier of the course.</param>
        /// <returns>A collection of <see cref="Document"/> entities associated with the specified course.</returns>
        Task<IEnumerable<Document>> GetByCourseIdAsync(Guid courseId);

        /// <summary>
        /// Retrieves all documents attached to a specific module.
        /// </summary>
        /// <param name="moduleId">The unique identifier of the module.</param>
        /// <returns>A collection of <see cref="Document"/> entities associated with the specified module.</returns>
        Task<IEnumerable<Document>> GetByModuleIdAsync(Guid moduleId);

        /// <summary>
        /// Retrieves all documents attached to a specific LMS activity.
        /// </summary>
        /// <param name="activityId">The unique identifier of the activity.</param>
        /// <returns>A collection of <see cref="Document"/> entities associated with the specified activity.</returns>
        Task<IEnumerable<Document>> GetByActivityIdAsync(Guid activityId);

        /// <summary>
        /// Attaches a document to a specific course.
        /// </summary>
        /// <param name="courseId">The unique identifier of the course.</param>
        /// <param name="documentId">The unique identifier of the document to attach.</param>
        /// <returns>A task representing the asynchronous attach operation.</returns>
        Task AttachToCourseAsync(Guid courseId, Guid documentId);

        /// <summary>
        /// Removes a document from a specific course.
        /// </summary>
        /// <param name="courseId">The unique identifier of the course.</param>
        /// <param name="documentId">The unique identifier of the document to remove.</param>
        /// <returns>A task representing the asynchronous detach operation.</returns>
        Task DetachFromCourseAsync(Guid courseId, Guid documentId);

        /// <summary>
        /// Attaches a document to a specific module.
        /// </summary>
        /// <param name="moduleId">The unique identifier of the module.</param>
        /// <param name="documentId">The unique identifier of the document to attach.</param>
        /// <returns>A task representing the asynchronous attach operation.</returns>
        Task AttachToModuleAsync(Guid moduleId, Guid documentId);

        /// <summary>
        /// Removes a document from a specific module.
        /// </summary>
        /// <param name="moduleId">The unique identifier of the module.</param>
        /// <param name="documentId">The unique identifier of the document to remove.</param>
        /// <returns>A task representing the asynchronous detach operation.</returns>
        Task DetachFromModuleAsync(Guid moduleId, Guid documentId);

        /// <summary>
        /// Attaches a document to a specific LMS activity.
        /// </summary>
        /// <param name="activityId">The unique identifier of the activity.</param>
        /// <param name="documentId">The unique identifier of the document to attach.</param>
        /// <returns>A task representing the asynchronous attach operation.</returns>
        Task AttachToActivityAsync(Guid activityId, Guid documentId);

        /// <summary>
        /// Removes a document from a specific LMS activity.
        /// </summary>
        /// <param name="activityId">The unique identifier of the activity.</param>
        /// <param name="documentId">The unique identifier of the document to remove.</param>
        /// <returns>A task representing the asynchronous detach operation.</returns>
        Task DetachFromActivityAsync(Guid activityId, Guid documentId);
    }
}
