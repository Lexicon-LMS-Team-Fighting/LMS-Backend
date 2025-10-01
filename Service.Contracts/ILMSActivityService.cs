using LMS.Shared.DTOs.LMSActivityDtos;
using LMS.Shared.DTOs.PaginationDtos;

namespace Service.Contracts
{
    /// <summary>
    /// Service contract for managing <see cref="LMSActivity"/> entities.
    /// This interface defines the operations available for interacting with activity data.
    /// </summary>
    public interface ILMSActivityService
    {
        /// <summary>
        /// Retrieves an activity by its unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the activity.</param>
        /// <param name="include">Related entities to include (e.g., "participants", "feedbacks", "documents").</param>
        /// <returns>A <see cref="LMSActivityExtendedDto"/> representing the activity.</returns>
        Task<LMSActivityExtendedDto> GetByIdAsync(Guid id, string? include);

        /// <summary>
        /// Retrieves a paginated list of all activities.
        /// </summary>
        /// <param name="pageNumber">The page number to retrieve.</param>
        /// <param name="pageSize">The number of items per page.</param>
        /// <returns>A <see cref="PaginatedResultDto{LMSActivityDto}"/> containing the paginated list of activities.</returns>
        Task<PaginatedResultDto<LMSActivityPreviewDto>> GetAllAsync(int pageNumber, int pageSize);

        /// <summary>
        /// Retrieves a paginated list of activities associated with a specific module.
        /// </summary>
        /// <param name="moduleId">The unique identifier of the module.</param>
        /// <param name="pageNumber">The page number to retrieve.</param>
        /// <param name="pageSize">The number of items per page.</param>
        /// <returns>A <see cref="PaginatedResultDto{LMSActivityDto}"/> containing the paginated list of activities for the specified module.</returns>
        Task<PaginatedResultDto<LMSActivityPreviewDto>> GetAllByModuleIdAsync(Guid moduleId, int pageNumber, int pageSize);

        /// <summary>
        /// Creates a new activity.
        /// </summary>
        /// <param name="activity">The data for the activity to create.</param>
        /// <returns>A <see cref="LMSActivityDto"/> representing the created activity.</returns>
        Task<LMSActivityExtendedDto> CreateAsync(CreateLMSActivityDto activity);

        /// <summary>
        /// Updates an existing activity.
        /// </summary>
        /// <param name="id">The unique identifier of the activity to update.</param>
        /// <param name="activity">The updated data for the activity.</param>
        Task UpdateAsync(Guid id, UpdateLMSActivityDto activity);

        /// <summary>
        /// Deletes an activity by its unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the activity to delete.</param>
        Task DeleteAsync(Guid id);

        /// <summary>
        /// Checks if an activity title is unique within a specific module.
        /// </summary>
        /// <param name="title">The title of the activity to check.</param>
        /// <param name="moduleId">The unique identifier of the module.</param>
        /// <param name="excludedActivityId">
        /// Optional. The unique identifier of an activity to exclude from the check (useful when updating an existing activity).
        /// </param>
        /// <returns><c>true</c> if the activity title is unique; otherwise, <c>false</c>.</returns>
        Task<bool> IsUniqueNameAsync(string title, Guid moduleId, Guid excludedActivityId = default);
    }
}