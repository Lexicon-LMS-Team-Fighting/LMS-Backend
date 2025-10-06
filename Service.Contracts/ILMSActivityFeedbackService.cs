using LMS.Shared.DTOs.LMSActivityFeedbackDtos;
using LMS.Shared.DTOs.ModuleDtos;

namespace Service.Contracts
{
    /// <summary>
    /// Service contract for managing feedbacks.
    /// This interface defines the operations available for interacting with feedback data.
    /// </summary>
    public interface ILMSActivityFeedbackService
    {
        /// <summary>
        /// Retrieves a feedback by its unique identifier.
        /// </summary>
        /// <param name="activityId">The unique identifier of the activity.</param>
        /// <param name="userId">The unique identifier of the user to whom the feedback is provided.</param>
        /// <returns>A <see cref="LMSActivityFeedbackExtendedDto"/> representing the feedback.</returns>
        Task<LMSActivityFeedbackExtendedDto> GetByActivityAndUserIdAsync(Guid activityId, string userId);

        /// <summary>
        /// Creates a new feedback.
        /// </summary>
        /// <param name="activityId">The unique identifier of the activity.</param>
        /// <param name="userId">The unique identifier of the user to whom the feedback is provided.</param>
        /// <param name="createDto">The data for the feedback to create.</param>
        /// <returns>A <see cref="LMSActivityFeedbackExtendedDto"/> representing the created feedback.</returns>
        Task<LMSActivityFeedbackExtendedDto> CreateAsync(Guid activityId, string userId, CreateLMSActivityFeedbackDto createDto);

        /// <summary>
        /// Updates an existing feedback.
        /// </summary>
        /// <param name="activityId">The unique identifier of the activity.</param>
        /// <param name="userId">The unique identifier of the user to whom the feedback is provided.</param>
        /// <param name="updateDto">The updated data for the feedback.</param>
        Task UpdateAsync(Guid activityId, string userId, UpdateLMSActivityFeedbackDto updateDto);

        /// <summary>
        /// Deletes a feedback by its unique identifier.
        /// </summary>
        /// <param name="activityId">The unique identifier of the activity.</param>
        /// <param name="userId">The unique identifier of the user to whom the feedback is provided.</param>
        Task DeleteAsync(Guid activityId, string userId);
    }
}
