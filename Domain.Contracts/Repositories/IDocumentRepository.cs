using Domain.Models.Entities;

namespace Domain.Contracts.Repositories
{
    /// <summary>
    /// Defines the contract for document-specific data access operations. <br/>
    /// Inherits common CRUD functionality from <see cref="IRepositoryBase{T}"/>. <br/>
    /// Provides methods to retrieve, add, update, and delete <see cref="Document"/> entities. <br/>
    /// Includes methods for managing modules in relation to their parents <see cref="ApplicationUser"/>, <see cref="Course"/>, <see cref="Module"/>, <see cref="LMSActivity"/>. 
    /// </summary>
    public interface IDocumentRepository : IRepositoryBase<Document>
    {
        /// <summary>
        /// Retrieves a single <see cref="Document"/> entity by its unique identifier. <br/>
        /// </summary>
        /// <param name="documentId">The unique identifier of the activity.</param>
        /// <param name="changeTracking">
        /// If <c>true</c>, Entity Framework change tracking will be enabled (suitable for updates). <br/>
        /// </param>
        /// <returns>
        /// A task representing the asynchronous operation. The task result contains the 
        /// matching <see cref="Document"/> or <c>null</c> if not found.
        /// </returns>
        Task<Document?> GetByIdAsync(Guid documentId, bool changeTracking = false);

        /// <summary>
        /// Retrieves all <see cref="Document"/> entities from the data source. <br/>
        /// </summary>
        /// <param name="changeTracking">If <c>true</c>, Entity Framework change tracking will be enabled. <br/></param>
        /// <returns>
        /// A task representing the asynchronous operation. The task result contains a collection of <see cref="Document"/> entities.
        /// </returns>
        Task<IEnumerable<Document>> GetAllAsync(bool changeTracking = false);
    }
}
