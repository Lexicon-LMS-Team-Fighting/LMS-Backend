using Domain.Contracts.Repositories;
using Domain.Models.Entities;
using LMS.Infractructure.Data;
using LMS.Shared.DTOs.LMSActivityDtos;
using LMS.Shared.DTOs.PaginationDtos;
using LMS.Shared.Extensions;
using LMS.Shared.Pagination;
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
        public async Task<bool> IsUserEnrolledInActivityAsync(Guid activityId, string userId) =>
            await FindByCondition(a => a.Id == activityId && a.Module.Course.UserCourses.Any(uc => uc.UserId == userId), trackChanges: false)
                .AnyAsync();

        /// <inheritdoc />
        public async Task ClearDocumentRelationsAsync(Guid activityId)
        {
            var activity = await FindByCondition(c => c.Id == activityId, true)
                .Include(c => c.Documents)
                .FirstOrDefaultAsync();

            if (activity is null)
                return;

            var activityDocuments = activity.Documents.ToList();
        }

        /// <summary>
        /// Retrieves a paginated list of LMS activities based on the specified query and pagination parameters.
        /// </summary>
        /// <param name="query">The base query for activities.</param>
        /// <param name="queryDto">Pagination, filtering, and sorting parameters.</param>
        /// <returns>A <see cref="PaginatedResult{T}"/> with activities and pagination metadata.</returns>
        private async Task<PaginatedResult<LMSActivity>> GetPaginatedActivitiesAsync(IQueryable<LMSActivity> query, PaginatedQueryDto queryDto)
        {
            if (!string.IsNullOrEmpty(queryDto.FilterBy))
                query = query.WhereContains(queryDto.FilterBy, queryDto.Filter);

            if (!string.IsNullOrEmpty(queryDto.SortBy))
                query = query.OrderByField(queryDto.SortBy, queryDto.SortDirection);

            var totalCount = await query.CountAsync();

            var items = await query
                .Skip((queryDto.Page - 1) * queryDto.PageSize)
                .Take(queryDto.PageSize)
                .ToListAsync();

            return new PaginatedResult<LMSActivity>(items, new PaginationMetadata(totalCount, queryDto.Page, queryDto.PageSize));
        }

        /// <inheritdoc/>
        public Task<PaginatedResult<LMSActivity>> GetAllAsync(PaginatedQueryDto queryDto, bool changeTracking = false)
        {
            var query = FindAll(changeTracking)
                .Include(a => a.ActivityType)
                .Include(a => a.Module)
                    .ThenInclude(m => m.Course)
                .AsQueryable();

            return GetPaginatedActivitiesAsync(query, queryDto);
        }

        /// <inheritdoc/>
        public Task<PaginatedResult<LMSActivity>> GetAllAsync(string userId, PaginatedQueryDto queryDto, bool changeTracking = false)
        {
            var query = FindAll(changeTracking)
                .Where(a => a.Module.Course.UserCourses.Any(uc => uc.UserId == userId))
                .Include(a => a.ActivityType)
                .Include(a => a.Module)
                    .ThenInclude(m => m.Course)
                .AsQueryable();

            return GetPaginatedActivitiesAsync(query, queryDto);
        }

        /// <inheritdoc/>
        public Task<PaginatedResult<LMSActivity>> GetByModuleIdAsync(Guid moduleId, PaginatedQueryDto queryDto, bool changeTracking = false)
        {
            var query = FindByCondition(a => a.ModuleId == moduleId, changeTracking)
                .Include(a => a.ActivityType)
                .Include(a => a.Module)
                    .ThenInclude(m => m.Course)
                .AsQueryable();

            return GetPaginatedActivitiesAsync(query, queryDto);
        }

        /// <inheritdoc/>
        public Task<PaginatedResult<LMSActivity>> GetByModuleIdAsync(Guid moduleId, string userId, PaginatedQueryDto queryDto, bool changeTracking = false)
        {
            var query = FindByCondition(a => a.ModuleId == moduleId && a.Module.Course.UserCourses.Any(uc => uc.UserId == userId), changeTracking)
                .Include(a => a.ActivityType)
                .Include(a => a.Module)
                    .ThenInclude(m => m.Course)
                .AsQueryable();

            return GetPaginatedActivitiesAsync(query, queryDto);
        }

        /// <inheritdoc/>
        public async Task<bool> IsUniqueNameAsync(string name, Guid moduleId, Guid excludeActivityId) =>
            await FindByCondition(a => a.Name == name && a.ModuleId == moduleId && a.Id != excludeActivityId, false)
                .AnyAsync();
    }
}