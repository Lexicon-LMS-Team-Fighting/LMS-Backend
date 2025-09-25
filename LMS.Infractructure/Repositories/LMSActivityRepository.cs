using Domain.Contracts.Repositories;
using Domain.Models.Entities;
using LMS.Infractructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LMS.Infractructure.Repositories
{
    /// <summary>
    /// Repository implementation for managing <see cref="LMSActivity"/> entities.
    /// Inherits common CRUD functionality from <see cref="RepositoryBase{T}"/>.
    /// </summary>
    public class LMSActivityRepository : RepositoryBase<LMSActivity>, ILMSActivityRepository
    {
        public LMSActivityRepository(ApplicationDbContext context) : base(context)
        {
        }

        /// <summary>
        /// Retrieves a single <see cref="LMSActivity"/> entity by its unique identifier.
        /// </summary>
        /// <param name="activityId">The unique identifier of the module.</param>
        /// <param name="changeTracking">If true, enables change tracking.</param>
        /// <returns>The module if found, otherwise null.</returns>
        public async Task<LMSActivity?> GetByIdAsync(Guid activityId, bool changeTracking = false) =>
            await FindByCondition(m => m.Id == activityId, trackChanges: changeTracking)
                .FirstOrDefaultAsync();

        /// <summary>
        /// Retrieves all <see cref="LMSActivity"/> entities.
        /// </summary>
        /// <param name="changeTracking">If true, enables change tracking.</param>
        /// <returns>A collection of modules.</returns>
        public async Task<IEnumerable<LMSActivity>> GetAllAsync(bool changeTracking = false) =>
            await FindAll(trackChanges: changeTracking)
                .ToListAsync();

        /// <summary>
        /// Retrieves all <see cref="LMSActivity"/> entities associated with a specific <see cref="LMSActivity"/>.
        /// </summary>
        /// <param name="moduleId">The unique identifier of the course.</param>
        /// <param name="changeTracking">If true, enables change tracking.</param>
        /// <returns>A collection of modules.</returns>
        public async Task<IEnumerable<LMSActivity>> GetByModuleIdAsync(Guid moduleId, bool changeTracking = false) =>
            await FindByCondition(m => m.ModuleId == moduleId, trackChanges: changeTracking)
                .ToListAsync();
    }
}
