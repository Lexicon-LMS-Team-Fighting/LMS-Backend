using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Models.Entities;

namespace Domain.Contracts.Repositories
{
    /// <summary>
    /// Defines the contract for module-specific data access operations. <br/>
    /// Inherits common CRUD functionality from <see cref="IRepositoryBase{T}"/>. <br/>
    /// Provides methods to retrieve, add, update, and delete <see cref="LMSActivity"/> entities. <br/>
    /// Includes methods for managing modules in relation to their parent <see cref="LMSActivity"/>. 
    /// </summary>
    public interface ILMSActivityRepository : IRepositoryBase<LMSActivity>
    {
        /// <summary>
        /// Retrieves a single <see cref="LMSActivity"/> entity by its unique identifier. <br/>
        /// </summary>
        /// <param name="moduleId">The unique identifier of the module.</param>
        /// <param name="changeTracking">
        /// If <c>true</c>, Entity Framework change tracking will be enabled (suitable for updates). <br/>
        /// </param>
        /// <returns>
        /// A task that represents the asynchronous operation. The task result contains the 
        /// matching <see cref="LMSActivity"/> or <c>null</c> if not found.
        /// </returns>
        Task<LMSActivity?> GetByIdAsync(Guid moduleId, bool changeTracking = false);

        /// <summary>
        /// Retrieves all <see cref="LMSActivity"/> entities from the data source. <br/>
        /// </summary>
        /// <param name="changeTracking">If <c>true</c>, Entity Framework change tracking will be enabled. <br/></param>
        /// <returns>
        /// A task that represents the asynchronous operation. The task result contains a collection of <see cref="LMSActivity"/> entities.
        /// </returns>
        Task<IEnumerable<LMSActivity>> GetAllAsync(bool changeTracking = false);

        /// <summary>
        /// Retrieves all <see cref="LMSActivity"/> entities associated with a specific <see cref="Course"/>. <br/>
        /// </summary>
        /// <param name="moduleId">The unique identifier of the course.</param>
        /// <param name="changeTracking">If <c>true</c>, Entity Framework change tracking will be enabled. <br/></param>
        /// <returns>
        /// A task that represents the asynchronous operation. The task result contains a collection of <see cref="LMSActivity"/> entities.
        /// </returns>
        Task<IEnumerable<LMSActivity>> GetByModuleIdAsync(Guid moduleId, bool changeTracking = false);
    }
}
