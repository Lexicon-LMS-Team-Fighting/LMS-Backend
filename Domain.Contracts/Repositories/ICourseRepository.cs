using Domain.Models.Entities;
using LMS.Shared.Pagination;

namespace Domain.Contracts.Repositories;

/// <summary>
/// Defines the contract for course-specific data access operations. <br/>
/// Inherits common CRUD functionality from <see cref="IRepositoryBase{T}"/>. <br/>
/// Provides methods to retrieve single or multiple <see cref="Course"/> entities. 
/// </summary>
public interface ICourseRepository : IRepositoryBase<Course>
{
    /// <summary>
    /// Retrieves a single <see cref="Course"/> entity by its unique identifier.
    /// </summary>
    /// <param name="courseId">The unique identifier of the course.</param>
    /// <param name="include">Related entities to include (e.g., "participants", "modules", "documents").</param>
    /// <param name="changeTracking">If <c>true</c>, Entity Framework change tracking will be enabled (suitable for updates).</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the matching <see cref="Course"/> or <c>null</c> if not found.</returns>
    Task<Course?> GetCourseAsync(Guid courseId, string? include, bool changeTracking = false);

    /// <summary>
    /// Retrieves a single <see cref="Course"/> entity by its unique identifier from the perspective of a specific user.
    /// </summary>
    /// <param name="courseId">The unique identifier of the course.</param>
    /// <param name="userId">The unique identifier of the user whose courses to include.</param>
    /// <param name="include">Related entities to include (e.g., "participants", "modules", "documents").</param>
    /// <param name="changeTracking">If <c>true</c>, Entity Framework change tracking will be enabled (suitable for updates).</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the matching <see cref="Course"/> or <c>null</c> if not found.</returns>
    Task<Course?> GetCourseAsync(Guid courseId, string userId, string? include, bool changeTracking = false);

    /// <summary>
    /// Retrieves all <see cref="Course"/> entities from the data source.
    /// </summary>
    /// <param name="changeTracking">If <c>true</c>, Entity Framework change tracking will be enabled.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains a list of <see cref="Course"/> entities.</returns>
    Task<IEnumerable<Course>> GetCoursesAsync(bool changeTracking = false);

    /// <summary>
    /// Retrieves all <see cref="Course"/> entities from the data source for a specific user.
    /// </summary>
    /// <param name="userId">The unique identifier of the user.</param>
    /// <param name="changeTracking">If <c>true</c>, Entity Framework change tracking will be enabled.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains a list of <see cref="Course"/> entities.</returns>
    Task<IEnumerable<Course>> GetCoursesAsync(string userId, bool changeTracking = false);

    /// <summary>
    /// Checks if a course name is unique, excluding a specific course if provided.
    /// </summary>
    /// <param name="name">The name of the course to check.</param>
    /// <param name="excludedCourseId">Optional: The unique identifier of a course to exclude from the uniqueness check.</param>
    /// <returns><c>true</c> if the course name is unique; otherwise, <c>false</c>.</returns>
    Task<bool> IsUniqueNameAsync(string name, Guid excludedCourseId = default);

    /// <summary>
    /// Calculates the normalized progress for a course.
    /// </summary>
    /// <param name="courseId">The course ID.</param>
    /// <param name="userId">
    /// Optional user ID. 
    /// If provided, calculates progress only for that user (student view). 
    /// If null, can be used for teacher view (aggregate or max per student logic can be added later).
    /// </param>
    /// <returns>A decimal value between 0 and 1 representing course progress.</returns>
    Task<decimal> CalculateProgressAsync(Guid courseId, string? userId = null);

    /// <summary>
    /// Retrieves all documents associated with a course and its modules and activities.
    /// </summary>
    /// <param name="courseId">The unique identifier of the course.</param>
    Task ClearDocumentRelationsAsync(Guid courseId);

    /// <summary>
    /// Determines whether the specified user is enrolled in the given course.
    /// </summary>
    /// <param name="courseId">The unique identifier of the course to check.</param>
    /// <param name="userId">The unique identifier of the user to check.</param>
    /// <returns>A task that represents the asynchronous operation. The task result is <see langword="true"/> if the user is
    /// enrolled in the course; otherwise, <see langword="false"/>.</returns>
    Task<bool> IsUserEnrolledInCourseAsync(Guid courseId, string userId);
}
