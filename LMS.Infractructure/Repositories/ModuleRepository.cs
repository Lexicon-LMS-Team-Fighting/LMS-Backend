using Domain.Contracts.Repositories;
using Domain.Models.Entities;
using LMS.Infractructure.Data;
using LMS.Shared;
using LMS.Shared.DTOs.ModuleDtos;
using LMS.Shared.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
        public async Task<IEnumerable<Module>> GetAllAsync(bool changeTracking = false) =>
            await FindAll(trackChanges: changeTracking)
                .ToListAsync();

        /// <inheritdoc/>
        public async Task<IEnumerable<Module>> GetAllAsync(string userId, bool changeTracking = false) =>
            await FindAll(trackChanges: changeTracking)
                .Where(m => m.Course.UserCourses.Any(uc => uc.UserId == userId))
                .ToListAsync();

        /// <inheritdoc/>
        public async Task<IEnumerable<Module>> GetByCourseIdAsync(Guid courseId, bool changeTracking = false) =>
            await FindByCondition(m => m.CourseId == courseId, trackChanges: changeTracking)
                .ToListAsync();

        /// <inheritdoc/>
        public async Task<IEnumerable<Module>> GetByCourseIdAsync(Guid courseId, string userId, bool changeTracking = false) =>
            await FindByCondition(m => m.CourseId == courseId, trackChanges: changeTracking)
                .Include(m => m.Course)
                    .ThenInclude(c => c.UserCourses)
                .Where(m => m.Course.UserCourses.Any(uc => uc.UserId == userId))
                .ToListAsync();

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
    }
}
