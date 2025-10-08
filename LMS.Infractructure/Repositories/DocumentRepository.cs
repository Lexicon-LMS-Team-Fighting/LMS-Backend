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
        /// Initializes a new instance of the <see cref="DocumentRepository"/> class with the specified <see cref="ApplicationDbContext"/>.
        /// </summary>
        public DocumentRepository(ApplicationDbContext context) : base(context)
        {
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<Document>> GetAllAsync(bool changeTracking = false) =>
            await FindAll(changeTracking)
                .ToListAsync();

        /// <inheritdoc/>
        public async Task<IEnumerable<Document>> GetAllAsync(string userId, bool changeTracking = false) =>
            await FindByCondition(d => d.UserId == userId, changeTracking)
                .ToListAsync();

        /// <inheritdoc/>
        public async Task<IEnumerable<Document>> GetAllByCourseIdAsync(Guid courseId, bool changeTracking = false) =>
            await FindByCondition(d => d.CourseId == courseId, changeTracking)
                .ToListAsync();

        /// <inheritdoc/>
        public async Task<IEnumerable<Document>> GetAllByModuleIdAsync(Guid moduleId, bool changeTracking = false) =>
            await FindByCondition(d => d.ModuleId == moduleId, changeTracking)
                .ToListAsync();

        /// <inheritdoc/>
        public async Task<IEnumerable<Document>> GetAllByActivityIdAsync(Guid activityId, bool changeTracking = false) =>
            await FindByCondition(d => d.ActivityId == activityId, changeTracking)
                .ToListAsync();

        /// <inheritdoc/>
        public async Task<Document?> GetByIdAsync(Guid documentId, bool changeTracking = false) =>
            await FindByCondition(d => d.Id == documentId, changeTracking)
                .Include(d => d.User)
                .FirstOrDefaultAsync();

        /// <inheritdoc/>
        public async Task<Document?> GetByIdAsync(Guid documentId, string userId, bool changeTracking = false) =>
            await FindByCondition(d => d.Id == documentId && d.UserId == userId, changeTracking)
                .Include(d => d.User)
                .FirstOrDefaultAsync();
    }
}