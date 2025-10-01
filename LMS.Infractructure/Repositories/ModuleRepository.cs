using Domain.Contracts.Repositories;
using Domain.Models.Entities;
using LMS.Infractructure.Data;
using LMS.Shared.DTOs.ModuleDtos;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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

        /// <summary>
        /// Builds a query for retrieving <see cref="Module"/> entities with optional related data and user filtering.
        /// </summary>
        /// <param name="query">The base query to build upon.</param>
        /// <param name="include">Related entities to include</param>
        /// <param name="userId">Optional user ID to filter by user participation.</param>
        /// <returns>The modified query with the specified includes and filters applied.</returns>
        private IQueryable<Module> BuildModuleQuery(IQueryable<Module> query, string? include, string? userId = null)
        {
            if (!string.IsNullOrEmpty(userId))
            {
                query = query.Where(m => m.Course.UserCourses.Any(uc => uc.UserId == userId));
            }

            if (!string.IsNullOrEmpty(include))
            {
                if (include.Contains(nameof(ModuleExtendedDto.LMSActivities), StringComparison.OrdinalIgnoreCase))
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

        /// <summary>
        /// Retrieves a single <see cref="Module"/> entity by its unique identifier.
        /// </summary>
        /// <param name="moduleId">The unique identifier of the module.</param>
        /// <param name="include">Related entities to include (e.g., "lmsactivities", "participants", "documents").</param>
        /// <param name="changeTracking">If true, enables change tracking.</param>
        /// <returns>The module if found, otherwise null.</returns>
        public async Task<Module?> GetByIdAsync(Guid moduleId, string? include, bool changeTracking = false)
        {
            var query = FindByCondition(m => m.Id == moduleId, trackChanges: changeTracking);
            return await BuildModuleQuery(query, include).FirstOrDefaultAsync();
        }

        /// <summary>
        /// Retrieves a single <see cref="Module"/> entity by its unique identifier from the perspective of a specific user.
        /// </summary>
        /// <param name="moduleId">The unique identifier of the module.</param>
        /// <param name="userId">The unique identifier of the user whose perspective to consider.</param>
        /// <param name="include">Related entities to include (e.g., "lmsactivities", "participants", "documents").</param>
        /// <param name="changeTracking">If true, enables change tracking.</param>
        /// <returns>The module if found, otherwise null.</returns>
        public async Task<Module?> GetByIdAsync(Guid moduleId, string userId, string? include, bool changeTracking = false)
        {
            var query = FindByCondition(m => m.Id == moduleId, trackChanges: changeTracking);
            return await BuildModuleQuery(query, include, userId).FirstOrDefaultAsync();
        }

        /// <summary>
        /// Retrieves all <see cref="Module"/> entities.
        /// </summary>
        /// <param name="changeTracking">If true, enables change tracking.</param>
        /// <returns>A collection of modules.</returns>
        public async Task<IEnumerable<Module>> GetAllAsync(bool changeTracking = false) =>
            await FindAll(trackChanges: changeTracking)
                .ToListAsync();

        /// <summary>
        /// Retrieves all <see cref="Module"/> entities from the perspective of a specific user.
        /// </summary>
        /// <param name="userId">The unique identifier of the user whose perspective to consider.</param>
        /// <param name="changeTracking">If true, enables change tracking.</param>
        /// <returns>A collection of modules.</returns>
        public async Task<IEnumerable<Module>> GetAllAsync(string userId, bool changeTracking = false) =>
            await FindAll(trackChanges: changeTracking)
                .Where(m => m.Course.UserCourses.Any(uc => uc.UserId == userId))
                .ToListAsync();

        /// <summary>
        /// Retrieves all <see cref="Module"/> entities associated with a specific <see cref="Course"/>.
        /// </summary>
        /// <param name="courseId">The unique identifier of the course.</param>
        /// <param name="changeTracking">If true, enables change tracking.</param>
        /// <returns>A collection of modules.</returns>
        public async Task<IEnumerable<Module>> GetByCourseIdAsync(Guid courseId, bool changeTracking = false) =>
            await FindByCondition(m => m.CourseId == courseId, trackChanges: changeTracking)
                .ToListAsync();

        /// <summary>
        /// Retrieves all <see cref="Module"/> entities associated with a specific <see cref="Course"/> from the perspective of a specific user.
        /// </summary>
        /// <param name="courseId">The unique identifier of the course.</param>
        /// <param name="userId">The unique identifier of the user whose perspective to consider.</param>
        /// <param name="changeTracking">If true, enables change tracking.</param>
        /// <returns>A collection of modules.</returns>
        public async Task<IEnumerable<Module>> GetByCourseIdAsync(Guid courseId, string userId, bool changeTracking = false) =>
            await FindByCondition(m => m.CourseId == courseId, trackChanges: changeTracking)
                .Include(m => m.Course)
                    .ThenInclude(c => c.UserCourses)
                .Where(m => m.Course.UserCourses.Any(uc => uc.UserId == userId))
                .ToListAsync();
    }
}
