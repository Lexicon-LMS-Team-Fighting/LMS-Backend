using Domain.Contracts.Repositories;
using Domain.Models.Entities;
using LMS.Infractructure.Data;
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
	
	public async Task<bool> AnyAsync(string name) => 
		await FindAnyAsync(a => a.Name.Equals(name, StringComparison.OrdinalIgnoreCase));

	public async Task<Course?> GetCourseAsync(Guid courseId, bool changeTracking = false) => 
		await FindByCondition(c => c.Id.Equals(courseId), changeTracking)
                .Include(c => c.UserCourses)
                .Include(c => c.Documents)
                .FirstOrDefaultAsync();

    /// <summary>
    /// Retrieves a single <see cref="Course"/> entity by its unique identifier from the perspective of a specific user. <br/>
    /// </summary>
    /// <param name="courseId">The unique identifier of the user.</param>
    /// <param name="userId">The unique identifier of the user whose courses to include.</param>
    /// <param name="changeTracking">If <c>true</c>, Entity Framework change tracking will be enabled (suitable for updates). <br/></param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the matching <see cref="Course"/> or <c>null</c> if not found.</returns>
    public async Task<Course?> GetCourseAsync(Guid courseId, string userId, bool changeTracking = false) =>
        await FindByCondition(c => c.Id.Equals(courseId), changeTracking)
                .Include(c => c.UserCourses)
                .Where(c => c.UserCourses.Any(uc => uc.UserId == userId))
                .Include(c => c.Documents)
                .FirstOrDefaultAsync();

    public async Task<List<Course>> GetCoursesAsync(bool changeTracking = false) => 
		await FindAll(changeTracking)
            .Include(c => c.UserCourses)
            .Include(c => c.Documents)
            .ToListAsync();

    /// <summary>
    /// Retrieves all <see cref="Course"/> entities from the data source from the perspective of a specific user. <br/>
    /// </summary>
    /// <param name="userId">The unique identifier of the user.</param>
    /// <param name="changeTracking">If <c>true</c>, Entity Framework change tracking will be enabled. <br/></param>
    /// <returns>A task that represents the asynchronous operation. The task result contains a list of <see cref="Course"/> entities.</returns>
    public async Task<List<Course>> GetCoursesAsync(string userId, bool changeTracking = false) =>
        await FindAll(changeTracking)
            .Include(c => c.UserCourses)
            .Where(c => c.UserCourses.Any(uc => uc.UserId == userId))
            .Include(c => c.Documents)
            .ToListAsync();
}
