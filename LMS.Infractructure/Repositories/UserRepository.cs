using Domain.Contracts.Repositories;
using Domain.Models.Entities;
using LMS.Infractructure.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace LMS.Infractructure.Repositories;


/// <summary>
/// Provides data access operations for <see cref="ApplicationUser"/> entities. <br/>
/// Inherits common CRUD functionality from <see cref="RepositoryBase{T}"/> 
/// and implements user-specific queries defined in <see cref="IUserRepository"/>. <br/>
/// Serves as the concrete repository for managing application users within the system.
/// </summary>
public class UserRepository : RepositoryBase<ApplicationUser>, IUserRepository
{
    private readonly UserManager<ApplicationUser> _userManager;

    public UserRepository(ApplicationDbContext context, UserManager<ApplicationUser> userManager) : base(context)
    {
        _userManager = userManager;
    }

    /// <summary>
    /// Checks if a user belongs to the "Teacher" role. <br/>
    /// </summary>
    /// <param name="userId">The unique identifier of the user.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains <c>true</c> if the user is a student; otherwise, <c>false</c>.</returns>
    public async Task<bool> IsUserStudentAsync(string userId) =>
        await IsUserInRoleAsync(userId, "Student");

    /// <summary>
    /// Checks if a user belongs to the "Teacher" role. <br/>
    /// </summary>
    /// <param name="userId">The unique identifier of the user.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains <c>true</c> if the user is a teacher; otherwise, <c>false</c>.</returns>
    public async Task<bool> IsUserTeacherAsync(string userId) => 
        await IsUserInRoleAsync(userId, "Teacher");

    /// <summary>
    /// Checks if a user belongs to a specific role. <br/>
    /// </summary>
    /// <param name="userId">The unique identifier of the user.</param>
    /// <param name="roleName">The name of the role to check.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains <c>true</c> if the user is in the specified role; otherwise, <c>false</c>.</returns>
    private async Task<bool> IsUserInRoleAsync(string userId, string roleName)
    {
        var user = await _userManager.FindByIdAsync(userId);

        if (user == null)
            return false;

        return await _userManager.IsInRoleAsync(user, roleName);
    }

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
}
