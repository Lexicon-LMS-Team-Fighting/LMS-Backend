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
                    .ThenInclude(m => m.Course)
                .Include(a => a.ActivityType);
        }

        /// <inheritdoc />
        public async Task<LMSActivity?> GetByIdAsync(Guid activityId, string? include, bool changeTracking = false)
        {
            var query = FindByCondition(a => a.Id == activityId, trackChanges: changeTracking);
            return await BuildActivityQuery(query, include).FirstOrDefaultAsync();
        }

        /// <inheritdoc />
        public async Task<LMSActivity?> GetByIdAsync(Guid activityId, string userId, string? include, bool changeTracking = false)
        {
            var query = FindByCondition(a => a.Id == activityId, trackChanges: changeTracking);
            return await BuildActivityQuery(query, include, userId).FirstOrDefaultAsync();
        }

        /// <inheritdoc />
        public async Task<IEnumerable<LMSActivity>> GetAllAsync(bool changeTracking = false) =>
            await FindAll(trackChanges: changeTracking)
                .Include(a => a.ActivityType)
                .Include(a => a.Module)
                    .ThenInclude(m => m.Course)
                .ToListAsync();

        /// <inheritdoc />
        public async Task<IEnumerable<LMSActivity>> GetAllAsync(string userId, bool changeTracking = false) =>
            await FindAll(trackChanges: changeTracking)
                .Where(a => a.Module.Course.UserCourses.Any(uc => uc.UserId == userId))
                .Include(a => a.ActivityType)
                .Include(a => a.Module)
                    .ThenInclude(m => m.Course)
                .ToListAsync();


        /// <inheritdoc />
        public async Task<IEnumerable<LMSActivity>> GetByModuleIdAsync(Guid moduleId, bool changeTracking = false) =>
            await FindByCondition(a => a.ModuleId == moduleId, trackChanges: changeTracking)
                .Include(a => a.ActivityType)
                .ToListAsync();

        /// <inheritdoc />
        public async Task<IEnumerable<LMSActivity>> GetByModuleIdAsync(Guid moduleId, string userId, bool changeTracking = false) =>
            await FindByCondition(a => a.ModuleId == moduleId, trackChanges: changeTracking)
                .Where(a => a.Module.Course.UserCourses.Any(uc => uc.UserId == userId))
                .Include(a => a.ActivityType)
                .ToListAsync();

        /// <inheritdoc />
        public async Task<bool> IsUserEnrolledInActivityAsync(Guid activityId, string userId) =>
            await FindByCondition(a => a.Id == activityId && a.Module.Course.UserCourses.Any(uc => uc.UserId == userId), trackChanges: false)
                .AnyAsync();
    }
}