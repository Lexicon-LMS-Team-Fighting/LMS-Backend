using Domain.Models.Entities;
using LMS.Shared.Pagination;

namespace Domain.Contracts.Repositories;

/// <summary>
/// Defines the contract for course-specific data access operations. <br/>
/// Inherits common CRUD functionality from <see cref="IRepositoryBase{T}"/>. <br/>
/// Provides methods to retrieve single or multiple <see cref="Course"/> entities. 
/// </summary>
public interface ICourseRepository: IRepositoryBase<Course>
{
    /// <summary>
    /// Retrieves a single <see cref="Course"/> entity by its unique identifier. <br/>
    /// </summary>
    /// <param name="courseId">The unique identifier of the user.</param>
    /// <param name="include">Related entities to include (e.g., "participants", "modules", "documents").</param>
    /// <param name="changeTracking">
    /// If <c>true</c>, Entity Framework change tracking will be enabled (suitable for updates). <br/>
    /// </param>
    /// <returns>
    /// A task that represents the asynchronous operation. The task result contains the 
    /// matching <see cref="Course"/> or <c>null</c> if not found.
    /// </returns>
    public Task<Course?> GetCourseAsync(Guid courseId, string? include, bool changeTracking = false);

    /// <summary>
    /// Retrieves a single <see cref="Course"/> entity by its unique identifier from the perspective of a specific user. <br/>
    /// </summary>
    /// <param name="courseId">The unique identifier of the user.</param>
    /// <param name="userId">The unique identifier of the user whose courses to include.</param>
    /// <param name="include">Related entities to include (e.g., "participants", "module", "document").</param>
    /// <param name="changeTracking">
    /// If <c>true</c>, Entity Framework change tracking will be enabled (suitable for updates). <br/>
    /// </param>
    /// <returns>
    /// A task that represents the asynchronous operation. The task result contains the 
    /// matching <see cref="Course"/> or <c>null</c> if not found.
    /// </returns>
    public Task<Course?> GetCourseAsync(Guid courseId, string userId, string? include, bool changeTracking = false);

    /// <summary>
    /// Retrieves all <see cref="Course"/> entities from the data source. <br/>
    /// </summary>
    /// <param name="changeTracking">If <c>true</c>, Entity Framework change tracking will be enabled. <br/></param>
    /// <returns>
    /// A task that represents the asynchronous operation. The task result contains a list of <see cref="Course"/> entities.
    /// </returns>
    public Task<IEnumerable<Course>> GetCoursesAsync(bool changeTracking = false);

    /// <summary>
    /// Retrieves all <see cref="Course"/> entities from the data source for a specific user. <br/>
    /// </summary>
	/// param name="userId">The unique identifier of the user.</param>
    /// <param name="changeTracking">If <c>true</c>, Entity Framework change tracking will be enabled. <br/></param>
    /// <returns>
    /// A task that represents the asynchronous operation. The task result contains a list of <see cref="Course"/> entities.
    /// </returns>
    public Task<IEnumerable<Course>> GetCoursesAsync(string userId, bool changeTracking = false);

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
    Task<bool> IsUniqueNameAsync(string name, Guid excludedCourseId = default);

}
