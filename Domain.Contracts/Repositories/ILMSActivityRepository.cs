using Domain.Models.Entities;
using LMS.Shared.DTOs.PaginationDtos;
using LMS.Shared.Pagination;

namespace Domain.Contracts.Repositories
{
    /// <summary>
    /// Defines the contract for data access operations related to <see cref="LMSActivity"/> entities. <br/>
    /// Inherits common CRUD functionality from <see cref="IRepositoryBase{T}"/>. <br/>
    /// Provides methods to retrieve, add, update, and delete <see cref="LMSActivity"/> entities. <br/>
    /// Includes methods for managing activities in relation to their parent module.
    /// </summary>
    public interface ILMSActivityRepository : IRepositoryBase<LMSActivity>
    {
        /// <summary>
        /// Retrieves a single <see cref="LMSActivity"/> entity by its unique identifier
        /// </summary>
        /// <param name="activityId">The uniqueidentifier of the activity.</param>
        /// <param name="include">Related entities to include (e.g., "participants", "feedbacks", "documents").</param>
        /// <param name="changeTracking">If <c>true</c>, Entity Framework change tracking will be enabled (suitable for updates). <br/></param>
        /// <returns>Matching <see cref="LMSActivity"/> with all feedbacks or <c>null</c> if not found.</returns>
        Task<LMSActivity?> GetByIdAsync(Guid activityId, string? include, bool changeTracking = false);

        /// <summary>
        /// Retrieves a single <see cref="LMSActivity"/> entity by its unique identifier from the perspective of a specific user<br/>
        /// </summary>
        /// <param name="activityId">The unique identifier of the activity.</param>
        /// <param name="userId">The unique identifier of the user whose feedbacks to include.</param>
        /// <param name="include">Related entities to include (e.g., "participants", "feedbacks", "documents").</param>
        /// <param name="changeTracking">If <c>true</c>, Entity Framework change tracking will be enabled (suitable for updates). <br/></param>
        /// <returns>Matching <see cref="LMSActivity"/> with user's feedbacks or <c>null</c> if not found.</returns>
        Task<LMSActivity?> GetByIdAsync(Guid activityId, string userId, string? include, bool changeTracking = false);

        /// <summary>
        /// Retrieves all <see cref="LMSActivity"/> entities from the data source. <br/>
        /// </summary>
        /// <param name="query">Pagination and filtering parameters.</param>
        /// <param name="changeTracking">If <c>true</c>, Entity Framework change tracking will be enabled. <br/></param>
        /// <returns>
        /// A task representing the asynchronous operation. The task result contains a collection of <see cref="LMSActivity"/> entities.
        /// </returns>
        Task<PaginatedResult<LMSActivity>> GetAllAsync(PaginatedQueryDto query, bool changeTracking = false);

        /// <summary>
        /// Retrieves all <see cref="LMSActivity"/> entities from the data source from the perspective of a specific user. <br/>
        /// </summary>
        /// <param name="userId">The unique identifier of the user.</param>
        /// <param name="query">Pagination and filtering parameters.</param>
        /// <param name="changeTracking">If <c>true</c>, Entity Framework change tracking will be enabled. <br/></param>
        /// <returns>
        /// A task representing the asynchronous operation. The task result contains a collection of <see cref="LMSActivity"/> entities.
        /// </returns>
        Task<PaginatedResult<LMSActivity>> GetAllAsync(string userId, PaginatedQueryDto query, bool changeTracking = false);

        /// <summary>
        /// Retrieves all <see cref="LMSActivity"/> entities associated with a specific module. <br/>
        /// </summary>
        /// <param name="moduleId">The unique identifier of the parent module.</param>
        /// <param name="query">Pagination and filtering parameters.</param>
        /// <param name="changeTracking">If <c>true</c>, Entity Framework change tracking will be enabled. <br/></param>
        /// <returns>
        /// A task representing the asynchronous operation. The task result contains a collection of <see cref="LMSActivity"/> entities.
        /// </returns>
        Task<PaginatedResult<LMSActivity>> GetByModuleIdAsync(Guid moduleId, PaginatedQueryDto query, bool changeTracking = false);

        /// <summary>
        /// Retrieves all <see cref="LMSActivity"/> entities associated with a specific module from the perspective of a specific user. <br/>
        /// </summary>
        /// <param name="moduleId">The unique identifier of the parent module.</param>
        /// <param name="userId">The unique identifier of the user.</param>
        /// <param name="query">Pagination and filtering parameters.</param>
        /// <param name="changeTracking">If <c>true</c>, Entity Framework change tracking will be enabled. <br/></param>
        /// <returns>
        /// A task representing the asynchronous operation. The task result contains a collection of <see cref="LMSActivity"/> entities.
        /// </returns>
        Task<PaginatedResult<LMSActivity>> GetByModuleIdAsync(Guid moduleId, string userId, PaginatedQueryDto query, bool changeTracking = false);

        /// <summary>
        /// Checks if a user is enrolled in a specific activity. <br/>
        /// </summary>
        /// <param name="activityId">The unique identifier of the activity.</param>
        /// <param name="userId">The unique identifier of the user.</param>
        /// <returns>A task representing the asynchronous operation. The task result contains <c>true</c> if the user is enrolled, otherwise <c>false</c>.</returns>
        Task<bool> IsUserEnrolledInActivityAsync(Guid activityId, string userId);

        /// <summary>
        /// Retrieves all documents associated with an activity.
        /// </summary>
        /// <param name="activityId">The unique identifier of the activity.</param>
        Task ClearDocumentRelationsAsync(Guid activityId);

        /// <summary>
        /// Determines whether the specified name is unique within the given module, excluding a specific activity.
        /// </summary>
        /// <param name="name">The name to check for uniqueness.</param>
        /// <param name="moduleId">The identifier of the module in which to check for uniqueness.</param>
        /// <param name="excludeActivityId">The identifier of the activity to exclude from the uniqueness check.</param>
        /// <returns>A task that represents the asynchronous operation. The task result is <see langword="true"/> if the name is
        /// unique; otherwise, <see langword="false"/>.</returns>
        Task<bool> IsUniqueNameAsync(string name, Guid moduleId, Guid excludeActivityId = default);
    }
}
