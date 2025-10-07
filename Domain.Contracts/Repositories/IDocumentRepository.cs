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
        /// <param name="changeTracking">If <c>true</c>, Entity Framework change tracking will be enabled. <br/></param>
        /// <returns>
        /// A collection of <see cref="Document"/> entities with pagination support.
        /// </returns>
        Task<IEnumerable<Document>> GetAllAsync(bool changeTracking = false);

        /// <summary>
        /// Retrieves a paginated list of documents related to a specific user. <br/>
        /// If the user is a student → returns only their own uploaded documents. <br/>
        /// If the user is a teacher → returns all documents across their courses/modules.
        /// </summary>
        /// <param name="userId">The unique identifier of the user.</param>
        /// <param name="changeTracking">If <c>true</c>, Entity Framework change tracking will be enabled. <br/></param>
        /// <returns>
        /// A collection of <see cref="Document"/> entities associated with the given user.
        /// </returns>
        Task<IEnumerable<Document>> GetAllAsync(string userId, bool changeTracking = false);

        /// <summary>
        /// Retrieves a specific document by its unique identifier.
        /// </summary>
        /// <param name="documentId">The unique identifier of the document.</param>
        /// <param name="changeTracking">If <c>true</c>, Entity Framework change tracking will be enabled. <br/></param>
        /// <returns>The corresponding <see cref="Document"/> entity, or null if not found.</returns>
        Task<Document?> GetByIdAsync(Guid documentId, bool changeTracking = false);

        /// <summary>
        /// Retrieves a specific document by ID, ensuring that it belongs to or is accessible by the given user.
        /// </summary>
        /// <param name="documentId">The unique identifier of the document.</param>
        /// <param name="userId">The unique identifier of the user requesting the document.</param>
        /// <param name="changeTracking">If <c>true</c>, Entity Framework change tracking will be enabled. <br/></param>
        /// <returns>
        /// The <see cref="Document"/> entity if accessible by the user; otherwise, null.
        /// </returns>
        Task<Document?> GetByIdAsync(Guid documentId, string userId, bool changeTracking = false);

        /// <summary>
        /// Retrieves all documents attached to a specific course.
        /// </summary>
        /// <param name="courseId">The unique identifier of the course.</param>
        /// <param name="changeTracking">If <c>true</c>, Entity Framework change tracking will be enabled. <br/></param>
        /// <returns>A collection of <see cref="Document"/> entities associated with the specified course.</returns>
        Task<IEnumerable<Document>> GetAllByCourseIdAsync(Guid courseId, bool changeTracking = false);

        /// <summary>
        /// Retrieves all documents attached to a specific module.
        /// </summary>
        /// <param name="moduleId">The unique identifier of the module.</param>
        /// <param name="changeTracking">If <c>true</c>, Entity Framework change tracking will be enabled. <br/></param>
        /// <returns>A collection of <see cref="Document"/> entities associated with the specified module.</returns>
        Task<IEnumerable<Document>> GetAllByModuleIdAsync(Guid moduleId, bool changeTracking = false);

        /// <summary>
        /// Retrieves all documents attached to a specific LMS activity.
        /// </summary>
        /// <param name="activityId">The unique identifier of the activity.</param>
        /// <param name="changeTracking">If <c>true</c>, Entity Framework change tracking will be enabled. <br/></param>
        /// <returns>A collection of <see cref="Document"/> entities associated with the specified activity.</returns>
        Task<IEnumerable<Document>> GetAllByActivityIdAsync(Guid activityId, bool changeTracking = false);
    }
}
