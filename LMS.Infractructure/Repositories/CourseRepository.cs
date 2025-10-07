using Domain.Contracts.Repositories;
using Domain.Models.Entities;
using LMS.Infractructure.Data;
using LMS.Shared;
using LMS.Shared.DTOs.CourseDtos;
using LMS.Shared.DTOs.PaginationDtos;
using LMS.Shared.Extensions;
using LMS.Shared.Pagination;
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

    /// <summary>
    /// Retrieves a paginated list of courses based on the specified query and pagination parameters.
    /// </summary>
    /// <remarks>The method applies filtering and sorting to the query based on the values provided in
    /// <paramref name="queryDto"/>. It then calculates the total number of items and retrieves the appropriate subset
    /// of courses for the requested page.</remarks>
    /// <param name="query">The <see cref="IQueryable{T}"/> representing the base query for courses.</param>
    /// <param name="queryDto">The pagination and filtering parameters, including page number, page size, sorting, and filtering options.</param>
    /// <returns>A <see cref="PaginatedResultDto{T}"/> containing the paginated list of courses and associated pagination
    /// metadata.</returns>
    private async Task<PaginatedResult<Course>> GetPaginatedCoursesAsync(IQueryable<Course> query, PaginatedQueryDto queryDto)
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

        return new PaginatedResult<Course>(items, new PaginationMetadata(totalCount, queryDto.Page, queryDto.PageSize));
    }

    /// <inheritdoc/>
    public Task<PaginatedResult<Course>> GetCoursesAsync(PaginatedQueryDto queryDto, bool changeTracking = false)
    {
        var query = FindAll(changeTracking).AsQueryable();
        return GetPaginatedCoursesAsync(query, queryDto);
    }

    /// <inheritdoc/>
    public Task<PaginatedResult<Course>> GetCoursesAsync(string userId, PaginatedQueryDto queryDto, bool changeTracking = false)
    {
        var query = FindByCondition(c => c.UserCourses.Any(uc => uc.UserId == userId), changeTracking)
            .AsQueryable();
        return GetPaginatedCoursesAsync(query, queryDto);
    }

    /// <inheritdoc/>
    public async Task<bool> IsUniqueNameAsync(string name, Guid excludedCourseId = default)
    {
        return !await FindByCondition(m => m.Name.ToUpper().Equals(name.ToUpper()) && m.Id != excludedCourseId)
            .AnyAsync();
    }

    /// <inheritdoc/>
    public async Task<decimal> CalculateProgressAsync(Guid courseId, string? userId = null)
    {
        var activities = await FindByCondition(c => c.Id == courseId)
            .Include(c => c.Modules)
                .ThenInclude(m => m.LMSActivities)
                    .ThenInclude(a => a.LMSActivityFeedbacks)
            .SelectMany(c => c.Modules)
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

        return Math.Round((decimal)completedCount / activities.Count, 4); ;
    }

    /// <inheritdoc />
    public async Task ClearDocumentRelationsAsync(Guid courseId)
    {
        var course = await FindByCondition(c => c.Id == courseId, true)
            .Include(c => c.Documents)
            .Include(c => c.Modules)
                .ThenInclude(m => m.Documents)
            .Include(c => c.Modules)
                .ThenInclude(m => m.LMSActivities)
                    .ThenInclude(a => a.Documents)
            .FirstOrDefaultAsync();

        if (course is null)
            return;

        var courseDocuments = course.Documents.ToList();
        var moduleDocuments = course.Modules.SelectMany(m => m.Documents).ToList();
        var activityDocuments = course.Modules.SelectMany(m => m.LMSActivities.SelectMany(a => a.Documents)).ToList();

        courseDocuments.ForEach(d => d.CourseId = null);
        moduleDocuments.ForEach(d => d.ModuleId = null);
        activityDocuments.ForEach(d => d.ActivityId = null);
    }

    // / <inheritdoc />
    public async Task<bool> IsUserEnrolledInCourseAsync(Guid courseId, string userId) =>
        await FindByCondition(c => c.Id == courseId)
            .AnyAsync(c => c.UserCourses.Any(uc => uc.UserId == userId));
}
