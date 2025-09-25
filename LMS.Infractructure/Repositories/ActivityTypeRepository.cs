using Domain.Contracts.Repositories;
using Domain.Models.Entities;
using LMS.Infractructure.Data;
using Microsoft.EntityFrameworkCore;

namespace LMS.Infractructure.Repositories
{
    /// <summary>
    /// Repository implementation for <see cref="ActivityType"/> entities.
    /// Provides data access methods for retrieving, adding, updating, and deleting activity types.
    /// Inherits common CRUD operations from <see cref="RepositoryBase{T}"/>.
    /// </summary>
    public class ActivityTypeRepository : RepositoryBase<ActivityType>, IActivityTypeRepository
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ActivityTypeRepository"/> class with the specified <see cref="ApplicationDbContext"/>.
        /// </summary>
        /// <param name="context">The database context used for data access operations.</param>
        public ActivityTypeRepository(ApplicationDbContext context) : base(context)
        {
        }

        /// <summary>
        /// Retrieves a single <see cref="ActivityType"/> entity by its name.
        /// </summary>
        /// <param name="activityTypeName">The name of the activity type to retrieve.</param>
        /// <param name="changeTracking">
        /// If <c>true</c>, Entity Framework change tracking will be enabled (useful for updates); otherwise, no tracking is applied.
        /// </param>
        /// <returns>
        /// A task representing the asynchronous operation. The task result contains the matching <see cref="ActivityType"/> 
        /// or <c>null</c> if no entity is found with the specified name.
        /// </returns>
        public async Task<ActivityType?> GetByNameAsync(string activityTypeName, bool changeTracking = false) =>
            await FindByCondition(a => a.Name == activityTypeName, trackChanges: changeTracking)
                .FirstOrDefaultAsync();

        /// <summary>
        /// Retrieves all <see cref="ActivityType"/> entities.
        /// </summary>
        /// <param name="changeTracking">If true, enables change tracking.</param>
        /// <returns>A collection of activity types.</returns>
        public async Task<IEnumerable<ActivityType>> GetAllAsync(bool changeTracking = false) =>
            await FindAll(trackChanges: changeTracking)
                .ToListAsync();
    }

}
