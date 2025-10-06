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
        /// <param name="id">The unique identifier of the feedback.</param>
        /// <returns>A <see cref="LMSActivityFeedbackExtendedDto"/> representing the feedback.</returns>
        Task<LMSActivityFeedbackExtendedDto> GetByIdAsync(Guid id);

        /// <summary>
        /// Creates a new feedback.
        /// </summary>
        /// <param name="createDto">The data for the feedback to create.</param>
        /// <returns>A <see cref="LMSActivityFeedbackExtendedDto"/> representing the created feedback.</returns>
        Task<LMSActivityFeedbackExtendedDto> CreateAsync(CreateLMSActivityFeedbackDto createDto);

        /// <summary>
        /// Updates an existing feedback.
        /// </summary>
        /// <param name="id">The unique identifier of the feedback to update.</param>
        /// <param name="updateDto">The updated data for the feedback.</param>
        Task UpdateAsync(Guid id, UpdateLMSActivityFeedbackDto updateDto);

        /// <summary>
        /// Deletes a feedback by its unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the feedback to delete.</param>
        Task DeleteAsync(Guid id);
    }
}
