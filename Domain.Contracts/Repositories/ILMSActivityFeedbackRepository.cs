using Domain.Models.Entities;

namespace Domain.Contracts.Repositories
{
    /// <summary>
    /// Repository contract for managing <see cref="LMSActivityFeedback"/> entities.
    /// Inherits basic CRUD operations from <see cref="IRepositoryBase{T}"/>.
    /// Provides methods to retrieve, add, update, and delete feedbacks
    /// </summary>
    public interface ILMSActivityFeedbackRepository : IRepositoryBase<LMSActivityFeedback>
    {
        /// <summary>
        /// Deletes all <see cref="LMSActivityFeedback"/> entries associated with a specific user in a specific course.
        /// </summary>
        /// <param name="userId">The unique identifier of the user whose feedbacks are being deleted.</param>
        /// <param name="courseId">The unique identifier of the course whose feedbacks are being deleted.</param>
        /// <returns>A task that represents the asynchronous delete operation.</returns>
        Task DeleteAllInCourseByUserId(string userId, Guid courseId);

        /// <summary>
        /// Retrieves a single <see cref="LMSActivityFeedback"/> entity by its unique identifier
        /// </summary>
        /// <param name="feedbackId">The uniqueidentifier of the activity.</param>
        /// <param name="changeTracking">If <c>true</c>, Entity Framework change tracking will be enabled (suitable for updates). <br/></param>
        /// <returns>Matching <see cref="LMSActivityFeedback"/> with all feedbacks or <c>null</c> if not found.</returns>
        Task<LMSActivityFeedback?> GetByIdAsync(Guid feedbackId, bool changeTracking = false);
    }
}
