using Domain.Models.Entities;
using LMS.Shared.DTOs.PaginationDtos;
using LMS.Shared.Pagination;

namespace Domain.Contracts.Repositories
{
    /// <summary>
    /// Defines the contract for module-specific data access operations. <br/>
    /// Inherits common CRUD functionality from <see cref="IRepositoryBase{T}"/>. <br/>
    /// Provides methods to retrieve, add, update, and delete <see cref="Module"/> entities. <br/>
    /// Includes methods for managing modules in relation to their parent <see cref="Course"/>. 
    /// </summary>
    public interface IModuleRepository : IRepositoryBase<Module>
    {
        /// <summary>
        /// Retrieves a single <see cref="Module"/> entity by its unique identifier. <br/>
        /// </summary>
        /// <param name="moduleId">The unique identifier of the module.</param>
        /// <param name="include">Related entities to include (e.g., "lmsactivities", "participants", "documents").</param>
        /// <param name="changeTracking">
        /// If <c>true</c>, Entity Framework change tracking will be enabled (suitable for updates). <br/>
        /// </param>
        /// <returns>
        /// A task that represents the asynchronous operation. The task result contains the 
        /// matching <see cref="Module"/> or <c>null</c> if not found.
        /// </returns>
        Task<Module?> GetByIdAsync(Guid moduleId, string? include, bool changeTracking = false);

        /// <summary>
        /// Retrieves a single <see cref="Module"/> entity by its unique identifier from the perspective of a specific user. <br/>
        /// </summary>
        /// <param name="moduleId">The unique identifier of the module.</param>
        /// <param name="userId">The unique identifier of the user whose perspective to consider.</param>
        /// <param name="include">Related entities to include (e.g., "lmsactivities", "participants", "documents").</param>
        /// <param name="changeTracking">
        /// If <c>true</c>, Entity Framework change tracking will be enabled (suitable for updates). <br/>
        /// </param>
        /// <returns>
        /// A task that represents the asynchronous operation. The task result contains the 
        /// matching <see cref="Module"/> or <c>null</c> if not found.
        /// </returns>
        Task<Module?> GetByIdAsync(Guid moduleId, string userId, string? include, bool changeTracking = false);

        /// <summary>
        /// Retrieves all <see cref="Module"/> entities from the data source. <br/>
        /// </summary>
        /// <param name="queryDto">Pagination and filtering parameters.</param>
        /// <param name="changeTracking">If <c>true</c>, Entity Framework change tracking will be enabled. <br/></param>
        /// <returns>
        /// A task that represents the asynchronous operation. The task result contains a collection of <see cref="Module"/> entities.
        /// </returns>
        Task<PaginatedResult<Module>> GetAllAsync(PaginatedQueryDto queryDto, bool changeTracking = false);

        /// <summary>
        /// Retrieves all <see cref="Module"/> entities from the data source from the perspective of a specific user. <br/>
        /// </summary>
        /// <param name="queryDto">Pagination and filtering parameters.</param>
        /// <param name="userId">The unique identifier of the user whose perspective to consider.</param>
        /// <param name="changeTracking">If <c>true</c>, Entity Framework change tracking will be enabled. <br/></param>
        /// <returns>
        /// A task that represents the asynchronous operation. The task result contains a collection of <see cref="Module"/> entities.
        /// </returns>
        Task<PaginatedResult<Module>> GetAllAsync(string userId, PaginatedQueryDto queryDto, bool changeTracking = false);

        /// <summary>
        /// Retrieves all <see cref="Module"/> entities associated with a specific <see cref="Course"/>. <br/>
        /// </summary>
        /// <param name="queryDto">Pagination and filtering parameters.</param>
        /// <param name="courseId">The unique identifier of the course.</param>
        /// <param name="changeTracking">If <c>true</c>, Entity Framework change tracking will be enabled. <br/></param>
        /// <returns>
        /// A task that represents the asynchronous operation. The task result contains a collection of <see cref="Module"/> entities.
        /// </returns>
        Task<PaginatedResult<Module>> GetByCourseIdAsync(Guid courseId, PaginatedQueryDto queryDto, bool changeTracking = false);

        /// <summary>
        /// Retrieves all <see cref="Module"/> entities associated with a specific <see cref="Course"/> from the perspective of a specific user. <br/>
        /// </summary>
        /// <param name="queryDto">Pagination and filtering parameters.</param>
        /// <param name="courseId">The unique identifier of the course.</param>
        /// <param name="userId">The unique identifier of the user whose perspective to consider.</param>
        /// <param name="changeTracking">If <c>true</c>, Entity Framework change tracking will be enabled. <br/></param>
        /// <returns>
        /// A task that represents the asynchronous operation. The task result contains a collection of <see cref="Module"/> entities.
        /// </returns>
        Task<PaginatedResult<Module>> GetByCourseIdAsync(Guid courseId, string userId, PaginatedQueryDto queryDto, bool changeTracking = false);

        /// <summary>
        /// Calculates the normalized progress for a module.
        /// </summary>
        /// <param name="moduleId">The module ID.</param>
        /// <param name="userId">
        /// Optional user ID. 
        /// If provided, calculates progress only for that user (student view). 
        /// If null, can be used for teacher view (aggregate or max per student logic can be added later).
        /// </param>
        /// <returns>A decimal value between 0 and 1 representing module progress.</returns>
        Task<decimal> CalculateProgressAsync(Guid moduleId, string? userId = null);

        /// <summary>
        /// Retrieves all documents associated with a module and its activities.
        /// </summary>
        /// <param name="moduleId">The unique identifier of the module.</param>
        Task ClearDocumentRelationsAsync(Guid moduleId);

        /// <summary>
        /// Determines whether the specified user is enrolled in the given module.
        /// </summary>
        /// <param name="moduleId">The unique identifier of the module to check.</param>
        /// <param name="userId">The unique identifier of the user to check enrollment for.</param>
        /// <returns>A task that represents the asynchronous operation. The task result is <see langword="true"/> if the user is
        /// enrolled in the module; otherwise, <see langword="false"/>.</returns>
        Task<bool> IsUserEnrolledInModuleAsync(Guid moduleId, string userId);

        /// <summary>
        /// Determines whether the specified module name is unique within the context of a given course,  excluding a
        /// specific module by its identifier.
        /// </summary>
        /// <remarks>This method is typically used to ensure that a module name does not conflict with
        /// existing  module names in the same course, except for the module specified by <paramref
        /// name="excludeModuleId"/>.</remarks>
        /// <param name="name">The name of the module to check for uniqueness.</param>
        /// <param name="courseId">The identifier of the course in which to check for uniqueness.</param>
        /// <param name="excludeModuleId">The identifier of the module to exclude from the uniqueness check.</param>
        /// <returns>A task that represents the asynchronous operation. The task result is <see langword="true"/>  if the
        /// specified module name is unique within the course; otherwise, <see langword="false"/>.</returns>
        Task<bool> IsUniqueNameAsync(string name, Guid courseId, Guid excludeModuleId = default);
    }
}