using Domain.Models.Entities;

namespace Domain.Contracts.Repositories;

/// <summary>
/// Defines the contract for course-specific data access operations. <br/>
/// Inherits common CRUD functionality from <see cref="IRepositoryBase{T}"/>. <br/>
/// Provides methods to retrieve single or multiple <see cref="Course"/> entities. 
/// </summary>
public interface ICourseRepository: IRepositoryBase<Course>
{
	/// <summary>
	/// Checks if any <see cref="Course"/> entity exists with the specified name. <br/>
	/// </summary>
	/// <param name="name">Name to search for.</param>
	/// <returns>Boolean indicating if the name already exist.</returns>
	Task<bool> AnyAsync(string name);

	/// <summary>
	/// Retrieves a single <see cref="Course"/> entity by its unique identifier. <br/>
	/// </summary>
	/// <param name="courseId">The unique identifier of the user.</param>
	/// <param name="changeTracking">
	/// If <c>true</c>, Entity Framework change tracking will be enabled (suitable for updates). <br/>
	/// </param>
	/// <returns>
	/// A task that represents the asynchronous operation. The task result contains the 
	/// matching <see cref="Course"/> or <c>null</c> if not found.
	/// </returns>
	public Task<Course?> GetCourseAsync(Guid courseId, bool changeTracking = false);

	/// <summary>
	/// Retrieves all <see cref="Course"/> entities from the data source. <br/>
	/// </summary>
	/// <param name="changeTracking">If <c>true</c>, Entity Framework change tracking will be enabled. <br/></param>
	/// <returns>
	/// A task that represents the asynchronous operation. The task result contains a list of <see cref="Course"/> entities.
	/// </returns>
	public Task<List<Course>> GetCoursesAsync(bool changeTracking = false);

}
