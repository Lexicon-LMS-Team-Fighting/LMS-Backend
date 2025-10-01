using Domain.Models.Entities;

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
        /// <param name="changeTracking">If <c>true</c>, Entity Framework change tracking will be enabled. <br/></param>
        /// <returns>
        /// A task representing the asynchronous operation. The task result contains a collection of <see cref="LMSActivity"/> entities.
        /// </returns>
        Task<IEnumerable<LMSActivity>> GetAllAsync(bool changeTracking = false);

        /// <summary>
        /// Retrieves all <see cref="LMSActivity"/> entities from the data source from the perspective of a specific user. <br/>
        /// </summary>
        /// <param name="userId">The unique identifier of the user.</param>
        /// <param name="changeTracking">If <c>true</c>, Entity Framework change tracking will be enabled. <br/></param>
        /// <returns>
        /// A task representing the asynchronous operation. The task result contains a collection of <see cref="LMSActivity"/> entities.
        /// </returns>
        Task<IEnumerable<LMSActivity>> GetAllAsync(string userId, bool changeTracking = false);

        /// <summary>
        /// Retrieves all <see cref="LMSActivity"/> entities associated with a specific module. <br/>
        /// </summary>
        /// <param name="moduleId">The unique identifier of the parent module.</param>
        /// <param name="changeTracking">If <c>true</c>, Entity Framework change tracking will be enabled. <br/></param>
        /// <returns>
        /// A task representing the asynchronous operation. The task result contains a collection of <see cref="LMSActivity"/> entities.
        /// </returns>
        Task<IEnumerable<LMSActivity>> GetByModuleIdAsync(Guid moduleId, bool changeTracking = false);

        /// <summary>
        /// Retrieves all <see cref="LMSActivity"/> entities associated with a specific module from the perspective of a specific user. <br/>
        /// </summary>
        /// <param name="moduleId">The unique identifier of the parent module.</param>
        /// <param name="userId">The unique identifier of the user.</param>
        /// <param name="changeTracking">If <c>true</c>, Entity Framework change tracking will be enabled. <br/></param>
        /// <returns>
        /// A task representing the asynchronous operation. The task result contains a collection of <see cref="LMSActivity"/> entities.
        /// </returns>
        Task<IEnumerable<LMSActivity>> GetByModuleIdAsync(Guid moduleId, string userId, bool changeTracking = false);
    }
}
