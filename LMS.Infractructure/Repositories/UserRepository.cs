using Domain.Contracts.Repositories;
using Domain.Models.Entities;
using LMS.Infractructure.Data;
using Microsoft.EntityFrameworkCore;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace LMS.Infractructure.Repositories;


/// <summary>
/// Provides data access operations for <see cref="ApplicationUser"/> entities. <br/>
/// Inherits common CRUD functionality from <see cref="RepositoryBase{T}"/> 
/// and implements user-specific queries defined in <see cref="IUserRepository"/>. <br/>
/// Serves as the concrete repository for managing application users within the system.
/// </summary>
public class UserRepository : RepositoryBase<ApplicationUser>, IUserRepository
{
	public UserRepository(ApplicationDbContext context): base(context)
	{}

	// Not used yet. Parameters set to change.
	void IRepositoryBase<ApplicationUser>.Create(ApplicationUser entity)
	{
		throw new NotImplementedException();
	}

	// Not used yet. Parameters set to change.
	void IRepositoryBase<ApplicationUser>.Delete(ApplicationUser entity)
	{
		throw new NotImplementedException();
	}

	/// <inheritdoc/>
	public async Task<ApplicationUser?> GetUserAsync(string userId, bool changeTracking) => 
		await FindByCondition(u => u.Id.Equals(userId), changeTracking)
		.Include(u => u.UserCourses)
            .ThenInclude(uc => uc.Course)
        .FirstOrDefaultAsync();

	/// <inheritdoc/>
	public async Task<List<ApplicationUser>> GetUsersAsync(bool changeTracking) => 
		await FindAll(changeTracking)
        .ToListAsync();
	

	// Not used yet. Parameters set to change.
	void IRepositoryBase<ApplicationUser>.Update(ApplicationUser entity)
	{
		throw new NotImplementedException();
	}

    /// <summary>
    /// Retrieves all participants of a specific course.
    /// </summary>
    /// <param name="courseId">The unique identifier of the course.</param>
    /// <param name="changeTracking">
    /// If <c>true</c>, Entity Framework change tracking will be enabled. <br/>
    /// </param>
    public async Task<IEnumerable<ApplicationUser>> GetCourseParticipantsAsync(Guid courseId, bool changeTracking = false) =>
        await FindAll(changeTracking)
            .Where(u => u.UserCourses.Any(uc => uc.CourseId == courseId))
            .ToListAsync();
}
