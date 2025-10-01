using Domain.Contracts.Repositories;
using Domain.Models.Entities;
using LMS.Infractructure.Data;
using LMS.Shared.DTOs.LMSActivityDtos;
using Microsoft.EntityFrameworkCore;

namespace LMS.Infractructure.Repositories
{
    /// <summary>
    /// Repository for managing <see cref="LMSActivity"/> entities.
    /// Inherits common CRUD functionality from <see cref="RepositoryBase{T}"/>.
    /// </summary>
    public class LMSActivityRepository : RepositoryBase<LMSActivity>, ILMSActivityRepository
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LMSActivityRepository"/> class with the specified database context.
        /// </summary>
        /// <param name="context">The database context to be used by the repository.</param>
        public LMSActivityRepository(ApplicationDbContext context) : base(context)
        {
        }

        /// <summary>
        /// Builds a query for retrieving <see cref="LMSActivity"/> entities with optional related data and user filtering.
        /// </summary>
        /// <param name="query">The base query to build upon.</param>
        /// <param name="include">Related entities to include</param>
        /// <param name="userId">Optional user ID to filter by user participation.</param>
        /// <returns>The modified query with the specified includes and filters applied.</returns>
        private IQueryable<LMSActivity> BuildActivityQuery(IQueryable<LMSActivity> query, string? include, string? userId = null)
        {
            if (!string.IsNullOrEmpty(userId))
            {
                query = query.Where(a => a.Module.Course.UserCourses.Any(uc => uc.UserId == userId));
            }

            if (!string.IsNullOrEmpty(include))
            {
                if (include.Contains(nameof(LMSActivityExtendedDto.Participants), StringComparison.OrdinalIgnoreCase))
                {
                    query = query
                        .Include(a => a.Module)
                            .ThenInclude(m => m.Course)
                                .ThenInclude(c => c.UserCourses)
                                    .ThenInclude(uc => uc.User);
                }

                if (include.Contains(nameof(LMSActivityExtendedDto.Feedbacks), StringComparison.OrdinalIgnoreCase))
                {
                    query = string.IsNullOrEmpty(userId)
                        ? query.Include(a => a.LMSActivityFeedbacks)
                        : query.Include(a => a.LMSActivityFeedbacks.Where(f => f.UserId == userId));
                }

                if (include.Contains(nameof(LMSActivityExtendedDto.Documents), StringComparison.OrdinalIgnoreCase))
                {
                    query = query.Include(a => a.Documents);
                }
            }

            return query
                .Include(a => a.Module)
                .Include(a => a.ActivityType);
        }
        /// <summary>
        /// Retrieves a single <see cref="LMSActivity"/> entity by its unique identifier.
        /// </summary>
        /// <param name="activityId">The unique identifier of the activity.</param>
        /// <param name="include">Related entities to include (e.g., "participants", "feedbacks", "documents").</param>
        /// <param name="changeTracking">If true, enables change tracking.</param>
        /// <returns>Matching <see cref="LMSActivity"/> with all feedbacks or null if not found.</returns>
        public async Task<LMSActivity?> GetByIdAsync(Guid activityId, string? include, bool changeTracking = false)
        {
            var query = FindByCondition(a => a.Id == activityId, trackChanges: changeTracking);
            return await BuildActivityQuery(query, include).FirstOrDefaultAsync();
        }

        /// <summary>
        /// Retrieves a single <see cref="LMSActivity"/> entity by its unique identifier from the perspective of a specific user.
        /// </summary>
        /// <param name="activityId">The unique identifier of the activity.</param>
        /// <param name="userId">The unique identifier of the user whose feedbacks to include.</param>
        /// <param name="include">Related entities to include (e.g., "participants", "feedbacks", "documents").</param>
        /// <param name="changeTracking">If true, enables change tracking.</param>
        /// <returns>Matching <see cref="LMSActivity"/> with user's feedbacks or null if not found.</returns>
        public async Task<LMSActivity?> GetByIdAsync(Guid activityId, string userId, string? include, bool changeTracking = false)
        {
            var query = FindByCondition(a => a.Id == activityId, trackChanges: changeTracking);
            return await BuildActivityQuery(query, include, userId).FirstOrDefaultAsync();
        }

        /// <summary>
        /// Retrieves all <see cref="LMSActivity"/> entities.
        /// </summary>
        /// <param name="changeTracking">If true, enables change tracking.</param>
        /// <returns>A collection of all activities.</returns>
        public async Task<IEnumerable<LMSActivity>> GetAllAsync(bool changeTracking = false) =>
            await FindAll(trackChanges: changeTracking)
                .Include(a => a.ActivityType)
                .ToListAsync();

        /// <summary>
        /// Retrieves all <see cref="LMSActivity"/> entities from the perspective of a specific user.
        /// </summary>
        /// <param name="userId">The unique identifier of the user.</param>
        /// <param name="changeTracking">If true, enables change tracking.</param>
        /// <returns>A collection of all activities.</returns>
        public async Task<IEnumerable<LMSActivity>> GetAllAsync(string userId, bool changeTracking = false) =>
            await FindAll(trackChanges: changeTracking)
                .Where(a => a.Module.Course.UserCourses.Any(uc => uc.UserId == userId))
                .Include(a => a.ActivityType)
                .ToListAsync();


        /// <summary>
        /// Retrieves all <see cref="LMSActivity"/> entities associated with a specific module.
        /// </summary>
        /// <param name="moduleId">The unique identifier of the module.</param>
        /// <param name="changeTracking">If true, enables change tracking.</param>
        /// <returns>A collection of activities for the specified module.</returns>
        public async Task<IEnumerable<LMSActivity>> GetByModuleIdAsync(Guid moduleId, bool changeTracking = false) =>
            await FindByCondition(a => a.ModuleId == moduleId, trackChanges: changeTracking)
                .Include(a => a.ActivityType)
                .ToListAsync();

        /// <summary>
        /// Retrieves all <see cref="LMSActivity"/> entities associated with a specific module from the perspective of a specific user.
        /// </summary>
        /// <param name="moduleId">The unique identifier of the module.</param>
        /// <param name="userId">The unique identifier of the user.</param>
        /// <param name="changeTracking">If true, enables change tracking.</param>
        /// <returns>A collection of activities for the specified module.</returns>
        public async Task<IEnumerable<LMSActivity>> GetByModuleIdAsync(Guid moduleId, string userId, bool changeTracking = false) =>
            await FindByCondition(a => a.ModuleId == moduleId, trackChanges: changeTracking)
                .Where(a => a.Module.Course.UserCourses.Any(uc => uc.UserId == userId))
                .Include(a => a.ActivityType)
                .ToListAsync();
    }
}