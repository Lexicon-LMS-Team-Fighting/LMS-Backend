using Domain.Models.Entities;

namespace Domain.Contracts.Repositories;

/// <summary>
/// Defines the contract for user-specific data access operations. <br/>
/// Inherits common CRUD functionality from <see cref="IRepositoryBase{T}"/>. <br/>
/// Provides methods to retrieve single or multiple <see cref="ApplicationUser"/> entities. 
/// </summary>
public interface IUserRepository: IRepositoryBase<ApplicationUser>
{
    /// <summary>
    /// Checks if a user belongs to the "Teacher" role. <br/>
    /// </summary>
    /// <param name="userId">The unique identifier of the user.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains <c>true</c> if the user is a student; otherwise, <c>false</c>.</returns>
    Task<bool> IsUserStudentAsync(string userId);

    /// <summary>
    /// Checks if a user belongs to the "Teacher" role. <br/>
    /// </summary>
    /// <param name="userId">The unique identifier of the user.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains <c>true</c> if the user is a teacher; otherwise, <c>false</c>.</returns>
    Task<bool> IsUserTeacherAsync(string userId);

    /// <summary>
    /// Retrieves a single <see cref="ApplicationUser"/> entity by its unique identifier. <br/>
    /// </summary>
    /// <param name="userId">The unique identifier of the user.</param>
    /// <param name="changeTracking">
    /// If <c>true</c>, Entity Framework change tracking will be enabled (suitable for updates). <br/>
    /// </param>
    /// <returns>
    /// A task that represents the asynchronous operation. The task result contains the 
    /// matching <see cref="User"/> or <c>null</c> if not found.
    /// </returns>
    public Task<ApplicationUser?> GetUserAsync(string userId, bool changeTracking = false);

	/// <summary>
	/// Retrieves all <see cref="User"/> entities from the data source. <br/>
	/// </summary>
	/// <param name="changeTracking">If <c>true</c>, Entity Framework change tracking will be enabled. <br/></param>
	/// <returns>
	/// A task that represents the asynchronous operation. The task result contains a list of <see cref="ApplicationUser"/> entities.
	/// </returns>
	public Task<List<ApplicationUser>> GetUsersAsync(bool changeTracking = false);

    /// <summary>
    /// Retrieves all participants of a specific course.
    /// </summary>
    /// <param name="courseId">The unique identifier of the course.</param>
    /// <param name="changeTracking">
    /// If <c>true</c>, Entity Framework change tracking will be enabled. <br/>
    /// </param>
    public Task<IEnumerable<ApplicationUser>> GetCourseParticipantsAsync(Guid courseId, bool changeTracking = false);

    /// <summary>
    /// Checks if the provided email is unique across all users, optionally excluding a specific user by their ID. <br/>
    /// </summary>
    /// <param name="email">The email address to check for uniqueness.</param>
    /// <param name="excludingUserId">Optional user ID to exclude from the uniqueness check (useful when updating a user's own email).</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains <c>true</c> if the email is unique; otherwise, <c>false</c>.</returns>
    public Task<bool> IsUniqueEmailAsync(string email, string? excludingUserId = null);

    /// <summary>
    /// Checks if the combination of first name and last name is unique across all users, optionally excluding a specific user by their ID. <br/>
    /// </summary>
    /// <param name="username">The username to check for uniqueness.</param>
    /// <param name="excludingUserId">Optional user ID to exclude from the uniqueness check (useful when updating a user's own name).</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains <c>true</c> if the name combination is unique; otherwise, <c>false</c>.</returns>
    public Task<bool> IsUniqueUsernameAsync(string username, string? excludingUserId = null);
}
