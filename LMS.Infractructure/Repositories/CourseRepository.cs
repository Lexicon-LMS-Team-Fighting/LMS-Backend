using Domain.Contracts.Repositories;
using Domain.Models.Entities;
using LMS.Infractructure.Data;
using LMS.Shared.DTOs.CourseDtos;
using Microsoft.EntityFrameworkCore;

namespace LMS.Infractructure.Repositories;

/// <summary>
/// Provides data access operations for <see cref="Course"/> entities. <br/>
/// Inherits common CRUD functionality from <see cref="RepositoryBase{T}"/> 
/// and implements course-specific queries defined in <see cref="IUserRepository"/>. <br/>
/// Serves as the concrete repository for managing courses within the system.
/// </summary>
public class CourseRepository : RepositoryBase<Course>, ICourseRepository
{
	public CourseRepository(ApplicationDbContext context) : base(context)
	{}

    /// <summary>
    /// Builds a query for retrieving <see cref="Course"/> entities with optional related data and user filtering.
    /// </summary>
    /// <param name="query">The base query to build upon.</param>
    /// <param name="include">Related entities to include</param>
    /// <param name="userId">Optional user ID to filter by user participation.</param>
    /// <returns>The modified query with the specified includes and filters applied.</returns>
    private IQueryable<Course> BuildCourseQuery(IQueryable<Course> query, string? include, string? userId = null)
    {
        if (!string.IsNullOrEmpty(userId))
        {
            query = query.Where(c => c.UserCourses.Any(uc => uc.UserId == userId));
        }

        if (!string.IsNullOrEmpty(include))
        {
            if (include.Contains(nameof(CourseExtendedDto.Participants), StringComparison.OrdinalIgnoreCase))
            {
                query = query
                    .Include(c => c.UserCourses)
                        .ThenInclude(uc => uc.User);
            }

            if (include.Contains(nameof(CourseExtendedDto.Modules), StringComparison.OrdinalIgnoreCase))
            {
                query = query.Include(c => c.Modules);
            }

            if (include.Contains(nameof(CourseExtendedDto.Documents), StringComparison.OrdinalIgnoreCase))
            {
                query = query.Include(c => c.Documents);
            }
        }

        return query;
    }

    /// <inheritdoc/>
    public async Task<Course?> GetCourseAsync(Guid courseId, string? include, bool changeTracking = false)
    {
        var query = FindByCondition(c => c.Id == courseId, changeTracking);
        return await BuildCourseQuery(query, include).FirstOrDefaultAsync();
    }

    /// <inheritdoc/>
    public async Task<Course?> GetCourseAsync(Guid courseId, string userId, string? include, bool changeTracking = false)
    {
        var query = FindByCondition(c => c.Id == courseId, changeTracking);
        return await BuildCourseQuery(query, include, userId).FirstOrDefaultAsync();
    }

    /// <inheritdoc/>
    public async Task<IEnumerable<Course>> GetCoursesAsync(string include, bool changeTracking = false) => 
		await FindAll(changeTracking)
            .ToListAsync();

    /// <inheritdoc/>
    public async Task<IEnumerable<Course>> GetCoursesAsync(string include, string userId, bool changeTracking = false) =>
        await FindAll(changeTracking)
            .Where(c => c.UserCourses.Any(uc => uc.UserId == userId))
            .ToListAsync();

    /// <inheritdoc/>
    public async Task<bool> IsUniqueNameAsync(string name, Guid excludedCourseId = default)
    {
        return !await FindByCondition(m => m.Name.ToUpper().Equals(name.ToUpper()) && m.Id != excludedCourseId)
            .AnyAsync();
    }

    /// <summary>
    /// Calculates the normalized progress for a course.
    /// </summary>
    /// <param name="course">The course entity to calculate progress for.</param>
    /// <param name="userId">
    /// Optional user ID. 
    /// If provided, calculates progress only for that user (student view). 
    /// If null, can be used for teacher view (aggregate or max per student logic can be added later).
    /// </param>
    /// <returns>A decimal value between 0 and 1 representing course progress.</returns>
    private decimal CalculateProgress(Course course)
    {
        var activities = course.Modules
            .SelectMany(m => m.LMSActivities)
            .ToList();

        if (!activities.Any())
            return 0m;

        var completedCount = activities.Count(a =>
            a.LMSActivityFeedbacks.Any(f => f.Status == LMSActivityFeedback || f.Status == "Genomförd")
        );

        return (decimal)completedCount / activities.Count;
    }
}
