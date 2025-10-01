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

    /// <summary>
    /// Retrieves a single <see cref="Course"/> entity by its unique identifier from the perspective of a specific user. <br/>
    /// </summary>
    /// <param name="courseId">The unique identifier of the user.</param>
    /// <param name="include">Related entities to include (e.g., "participants", "modules", "documents").</param>
    /// <param name="changeTracking">If <c>true</c>, Entity Framework change tracking will be enabled (suitable for updates). <br/></param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the matching <see cref="Course"/> or <c>null</c> if not found.</returns>
    public async Task<Course?> GetCourseAsync(Guid courseId, string? include, bool changeTracking = false)
    {
        var query = FindByCondition(c => c.Id == courseId, changeTracking);
        return await BuildCourseQuery(query, include).FirstOrDefaultAsync();
    }

    /// <summary>
    /// Retrieves a single <see cref="Course"/> entity by its unique identifier from the perspective of a specific user. <br/>
    /// </summary>
    /// <param name="courseId">The unique identifier of the user.</param>
    /// <param name="userId">The unique identifier of the user whose courses to include.</param>
    /// <param name="include">Related entities to include (e.g., "participants", "modules", "documents").</param>
    /// <param name="changeTracking">If <c>true</c>, Entity Framework change tracking will be enabled (suitable for updates). <br/></param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the matching <see cref="Course"/> or <c>null</c> if not found.</returns>
    public async Task<Course?> GetCourseAsync(Guid courseId, string userId, string? include, bool changeTracking = false)
    {
        var query = FindByCondition(c => c.Id == courseId, changeTracking);
        return await BuildCourseQuery(query, include, userId).FirstOrDefaultAsync();
    }

    public async Task<IEnumerable<Course>> GetCoursesAsync(bool changeTracking = false) => 
		await FindAll(changeTracking)
            .ToListAsync();

    /// <summary>
    /// Retrieves all <see cref="Course"/> entities from the data source from the perspective of a specific user. <br/>
    /// </summary>
    /// <param name="userId">The unique identifier of the user.</param>
    /// <param name="changeTracking">If <c>true</c>, Entity Framework change tracking will be enabled. <br/></param>
    /// <returns>A task that represents the asynchronous operation. The task result contains a list of <see cref="Course"/> entities.</returns>
    public async Task<IEnumerable<Course>> GetCoursesAsync(string userId, bool changeTracking = false) =>
        await FindAll(changeTracking)
            .Where(c => c.UserCourses.Any(uc => uc.UserId == userId))
            .ToListAsync();

    /// <summary>
    /// Checks if a course name is unique, excluding a specific course if provided.
    /// </summary>
    /// <param name="name">The name of the course to check.</param>
    /// <param name="excludedCourseId">
    /// The unique identifier of a course to exclude from the uniqueness check (optional).
    /// Use this parameter when updating a course to avoid conflicts with its current name.
    /// </param>
    /// <returns>
    /// <c>true</c> if the course name is unique within the course; otherwise, <c>false</c>.
    /// </returns>
    public async Task<bool> IsUniqueNameAsync(string name, Guid excludedCourseId = default)
    {
        return !await FindByCondition(m => m.Name.ToUpper().Equals(name.ToUpper()) && m.Id != excludedCourseId)
            .AnyAsync();
    }
}
