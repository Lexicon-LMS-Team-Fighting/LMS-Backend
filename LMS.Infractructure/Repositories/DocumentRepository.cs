using Domain.Contracts.Repositories;
using Domain.Models.Entities;
using LMS.Infractructure.Data;
using Microsoft.EntityFrameworkCore;

namespace LMS.Infractructure.Repositories
{
    /// <summary>
    /// Repository implementation for managing <see cref="Document"/> entities.
    /// Inherits common CRUD functionality from <see cref="RepositoryBase{T}"/>.
    /// </summary>
    public class DocumentRepository : RepositoryBase<Document>, IDocumentRepository
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LMSActivityRepository"/> class with the specified database context.
        /// </summary>
        /// <param name="context">The database context to be used by the repository.</param>
        public DocumentRepository(ApplicationDbContext context) : base(context)
        {
        }

        /// <summary>
        /// Retrieves a single <see cref="Document"/> entity by its unique identifier.
        /// </summary>
        /// <param name="documentId">The unique identifier of the activity.</param>
        /// <param name="changeTracking">If true, enables change tracking.</param>
        /// <returns>The activity entity if found; otherwise, null.</returns>
        public async Task<Document?> GetByIdAsync(Guid documentId, bool changeTracking = false) =>
            await FindByCondition(a => a.Id == documentId, trackChanges: changeTracking)
                .FirstOrDefaultAsync();

        /// <summary>
        /// Retrieves all <see cref="Document"/> entities.
        /// </summary>
        /// <param name="changeTracking">If true, enables change tracking.</param>
        /// <returns>A collection of all documents.</returns>
        public async Task<IEnumerable<Document>> GetAllAsync(bool changeTracking = false) =>
            await FindAll(trackChanges: changeTracking)
                .ToListAsync();
    }
}