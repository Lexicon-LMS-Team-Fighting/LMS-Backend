using Domain.Models.Entities;

namespace Domain.Contracts.Repositories;

/// <summary>
/// Defines the contract for user-specific data access operations. <br/>
/// Inherits common CRUD functionality from <see cref="IRepositoryBase{T}"/>. <br/>
/// Provides methods to retrieve single or multiple <see cref="User"/> entities. 
/// </summary>
public interface IUserRepository: IRepositoryBase<User>
{
	/// <summary>
	/// Retrieves a single <see cref="User"/> entity by its unique identifier. <br/>
	/// </summary>
	/// <param name="userId">The unique identifier of the user.</param>
	/// <param name="changeTracking">
	/// If <c>true</c>, Entity Framework change tracking will be enabled (suitable for updates). <br/>
	/// </param>
	/// <returns>
	/// A task that represents the asynchronous operation. The task result contains the 
	/// matching <see cref="User"/> or <c>null</c> if not found.
	/// </returns>
	Task<User?> GetUserAsync(Guid userId, bool changeTracking = false);

	/// <summary>
	/// Retrieves all <see cref="User"/> entities from the data source. <br/>
	/// </summary>
	/// <param name="changeTracking">If <c>true</c>, Entity Framework change tracking will be enabled. <br/></param>
	/// <returns>
	/// A task that represents the asynchronous operation. The task result contains a list of <see cref="User"/> entities.
	/// </returns>
	Task<List<User>> GetUsersAsync(bool changeTracking = false);
}
