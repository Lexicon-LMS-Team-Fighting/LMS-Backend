using Domain.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Contracts.Repositories
{
    /// <summary>
    /// Defines the contract for activity type-specific data access operations. <br/>
    /// Inherits common CRUD functionality from <see cref="IRepositoryBase{T}"/>. <br/>
    /// Provides methods to retrieve, add, update, and delete <see cref="ActivityType"/> entities. <br/>
    /// </summary>
    public interface IActivityTypeRepository : IRepositoryBase<ActivityType>
    {
        /// <summary>
        /// Retrieves a single <see cref="ActivityType"/> entity by its name. <br/>
        /// </summary>
        /// <param name="activityTypeName">The name of the activity type.</param>
        /// <param name="changeTracking">
        /// If <c>true</c>, Entity Framework change tracking will be enabled (suitable for updates). <br/>
        /// </param>
        /// <returns>
        /// A task that represents the asynchronous operation. The task result contains the 
        /// matching <see cref="ActivityType"/> or <c>null</c> if not found.
        /// </returns>
        Task<ActivityType?> GetByNameAsync(string activityTypeName, bool changeTracking = false);
    }
}
