using LMS.Shared.DTOs.ActivityTypeDto;

namespace Service.Contracts
{
    /// <summary>
    /// Service contract for managing activity types.
    /// This interface defines the operations available for interacting with activity type data.
    /// </summary>
    public interface IActivityTypeService
    {
        /// <summary>
        /// Retrieves all activity types.
        /// </summary>
        /// <returns>A collection of activity types.</returns>
        Task<IEnumerable<ActivityTypeDto>> GetAllAsync();
    }
}