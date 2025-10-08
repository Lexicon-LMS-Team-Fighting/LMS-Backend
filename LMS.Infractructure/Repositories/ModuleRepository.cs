using Domain.Contracts.Repositories;
using Domain.Models.Entities;
using LMS.Infractructure.Data;
using LMS.Shared;
using LMS.Shared.DTOs.ModuleDtos;
using LMS.Shared.DTOs.PaginationDtos;
using LMS.Shared.Extensions;
using LMS.Shared.Pagination;
using Microsoft.EntityFrameworkCore;

namespace LMS.Infractructure.Repositories
{
    /// <summary>
    /// Repository implementation for managing <see cref="Module"/> entities.
    /// Inherits common CRUD functionality from <see cref="RepositoryBase{T}"/>.
    /// </summary>
    public class ModuleRepository : RepositoryBase<Module>, IModuleRepository
    {
        public ModuleRepository(ApplicationDbContext context) : base(context)
        {
        }

        /// <inheritdoc/>
        private IQueryable<Module> BuildModuleQuery(IQueryable<Module> query, string? include, string? userId = null)
        {
            if (!string.IsNullOrEmpty(userId))
            {
                query = query.Where(m => m.Course.UserCourses.Any(uc => uc.UserId == userId));
            }

            if (!string.IsNullOrEmpty(include))
            {
                if (include.Contains(nameof(ModuleExtendedDto.Activities), StringComparison.OrdinalIgnoreCase))
                {
                    query = query
                        .Include(m => m.LMSActivities)
                            .ThenInclude(a => a.ActivityType);
                }

                if (include.Contains(nameof(ModuleExtendedDto.Participants), StringComparison.OrdinalIgnoreCase))
                {
                    query = query
                        .Include(m => m.Course)
                            .ThenInclude(c => c.UserCourses)
                                .ThenInclude(uc => uc.User);
                }

                if (include.Contains(nameof(ModuleExtendedDto.Documents), StringComparison.OrdinalIgnoreCase))
                {
                    query = query.Include(m => m.Documents);
                }
            }

            return query.Include(m => m.Course);
        }

        /// <inheritdoc/>
        public async Task<Module?> GetByIdAsync(Guid moduleId, string? include, bool changeTracking = false)
        {
            var query = FindByCondition(m => m.Id == moduleId, trackChanges: changeTracking);
            return await BuildModuleQuery(query, include).FirstOrDefaultAsync();
        }

        /// <inheritdoc/>
        public async Task<Module?> GetByIdAsync(Guid moduleId, string userId, string? include, bool changeTracking = false)
        {
            var query = FindByCondition(m => m.Id == moduleId, trackChanges: changeTracking);
            return await BuildModuleQuery(query, include, userId).FirstOrDefaultAsync();
        }

        /// <inheritdoc/>
        public async Task<decimal> CalculateProgressAsync(Guid moduleId, string? userId = null)
        {
            var activities = await FindByCondition(m => m.Id == moduleId)
                    .Include(m => m.LMSActivities)
                        .ThenInclude(a => a.LMSActivityFeedbacks)
                .SelectMany(m => m.LMSActivities)
                .Where(a => string.IsNullOrEmpty(userId)
                            || a.Module.Course.UserCourses.Any(uc => uc.UserId == userId))
                .ToListAsync();

            if (!activities.Any())
                return 0m;

            var completedCount = activities.Count(a =>
                a.LMSActivityFeedbacks.All(f => f.Status == LMSActivityFeedbackStatus.Approved.ToDbString() ||
                                                f.Status == LMSActivityFeedbackStatus.Completed.ToDbString())
            );

            return Math.Round((decimal)completedCount / activities.Count, 4);
        }

        /// <inheritdoc />
        public async Task ClearDocumentRelationsAsync(Guid moduleId)
        {
            var module = await FindByCondition(c => c.Id == moduleId, true)
                .Include(c => c.Documents)
                .Include(m => m.LMSActivities)
                    .ThenInclude(a => a.Documents)
                .FirstOrDefaultAsync();

            if (module is null)
                return;

            var moduleDocuments = module.Documents.ToList();
            var activityDocuments = module.LMSActivities.SelectMany(m => m.Documents).ToList();

            moduleDocuments.ForEach(d => d.ModuleId = null);
            activityDocuments.ForEach(d => d.ActivityId = null);
        }

        public async Task<bool> IsUserEnrolledInModuleAsync(Guid moduleId, string userId) =>
            await FindByCondition(m => m.Id == moduleId)
                .AnyAsync(m => m.Course.UserCourses.Any(uc => uc.UserId == userId));

        /// <summary>
        /// Retrieves a paginated list of modules based on the specified query and pagination parameters.
        /// </summary>
        /// <param name="query">The base query for modules.</param>
        /// <param name="queryDto">Pagination, filtering, and sorting parameters.</param>
        /// <returns>A <see cref="PaginatedResult{T}"/> with modules and pagination metadata.</returns>
        private async Task<PaginatedResult<Module>> GetPaginatedModulesAsync(IQueryable<Module> query, PaginatedQueryDto queryDto)
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

            return new PaginatedResult<Module>(items, new PaginationMetadata(totalCount, queryDto.Page, queryDto.PageSize));
        }

        /// <inheritdoc/>
        public Task<PaginatedResult<Module>> GetAllAsync(PaginatedQueryDto queryDto, bool changeTracking = false)
        {
            var query = FindAll(changeTracking).AsQueryable();
            return GetPaginatedModulesAsync(query, queryDto);
        }

        /// <inheritdoc/>
        public Task<PaginatedResult<Module>> GetAllAsync(string userId, PaginatedQueryDto queryDto, bool changeTracking = false)
        {
            var query = FindByCondition(m => m.Course.UserCourses.Any(uc => uc.UserId == userId), changeTracking)
                .Include(m => m.Course)
                .AsQueryable();
            return GetPaginatedModulesAsync(query, queryDto);
        }

        /// <inheritdoc/>
        public Task<PaginatedResult<Module>> GetByCourseIdAsync(Guid courseId, PaginatedQueryDto queryDto, bool changeTracking = false)
        {
            var query = FindByCondition(m => m.CourseId == courseId, changeTracking)
                .AsQueryable();
            return GetPaginatedModulesAsync(query, queryDto);
        }

        /// <inheritdoc/>
        public Task<PaginatedResult<Module>> GetByCourseIdAsync(Guid courseId, string userId, PaginatedQueryDto queryDto, bool changeTracking = false)
        {
            var query = FindByCondition(m => m.CourseId == courseId && m.Course.UserCourses.Any(uc => uc.UserId == userId), changeTracking)
                .Include(m => m.Course)
                .AsQueryable();
            return GetPaginatedModulesAsync(query, queryDto);
        }

        /// <inheritdoc/>
        public async Task<bool> IsUniqueNameAsync(string name, Guid courseId, Guid excludeModuleId) =>
            !await FindByCondition(m => m.Name.ToUpper().Equals(name.ToUpper()) && m.CourseId == courseId && m.Id != excludeModuleId)
                .AnyAsync();
    }
}
