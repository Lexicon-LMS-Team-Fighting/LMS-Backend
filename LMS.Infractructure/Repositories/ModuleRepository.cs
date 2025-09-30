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
    /// Repository implementation for managing <see cref="Module"/> entities.
    /// Inherits common CRUD functionality from <see cref="RepositoryBase{T}"/>.
    /// </summary>
    public class ModuleRepository : RepositoryBase<Module>, IModuleRepository
    {
        public ModuleRepository(ApplicationDbContext context) : base(context)
        {
        }

        /// <summary>
        /// Retrieves a single <see cref="Module"/> entity by its unique identifier.
        /// </summary>
        /// <param name="moduleId">The unique identifier of the module.</param>
        /// <param name="changeTracking">If true, enables change tracking.</param>
        /// <returns>The module if found, otherwise null.</returns>
        public async Task<Module?> GetByIdAsync(Guid moduleId, bool changeTracking = false) =>
            await FindByCondition(m => m.Id == moduleId, trackChanges: changeTracking)
                .Include(m => m.Documents)
                .FirstOrDefaultAsync();

        /// <summary>
        /// Retrieves a single <see cref="Module"/> entity by its unique identifier from the perspective of a specific user.
        /// </summary>
        /// <param name="moduleId">The unique identifier of the module.</param>
        /// <param name="userId">The unique identifier of the user whose perspective to consider.</param>
        /// <param name="changeTracking">If true, enables change tracking.</param>
        /// <returns>The module if found, otherwise null.</returns>
        public async Task<Module?> GetByIdAsync(Guid moduleId, string userId, bool changeTracking = false) =>
            await FindByCondition(m => m.Id == moduleId, trackChanges: changeTracking)
                .Include(m => m.Course)
                    .ThenInclude(c => c.UserCourses)
                .Where(m => m.Course.UserCourses.Any(uc => uc.UserId == userId))
                .Include(m => m.Documents)
                .FirstOrDefaultAsync();

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
                .Include(m => m.Course)
                    .ThenInclude(c => c.UserCourses)
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
