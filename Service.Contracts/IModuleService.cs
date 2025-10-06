using LMS.Shared.DTOs.ModuleDtos;
using LMS.Shared.DTOs.PaginationDtos;
using LMS.Shared.Pagination;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Contracts
{
    /// <summary>
    /// Service contract for managing modules.
    /// This interface defines the operations available for interacting with module data.
    /// </summary>
    public interface IModuleService
    {
        /// <summary>
        /// Retrieves a module by its unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the module.</param>
        /// <param name="include">Related entities to include (e.g., "lmsactivitie", "participants", "documents").</param>
        /// <returns>A <see cref="ModuleDto"/> representing the module.</returns>
        Task<ModuleExtendedDto> GetByIdAsync(Guid id, string? include);

        /// <summary>
        /// Retrieves a paginated list of all modules.
        /// </summary>
        /// <param name="pageNumber">The page number to retrieve.</param>
        /// <param name="pageSize">The number of items per page.</param>
        /// <param name="include">Optional fields to include (e.g., "progress").</param>
        /// <returns>A <see cref="PaginatedResultDto{ModuleDto}"/> containing the paginated list of modules.</returns>
        Task<PaginatedResultDto<ModulePreviewDto>> GetAllAsync(int pageNumber, int pageSize, string? include = null);

        /// <summary>
        /// Retrieves a paginated list of modules associated with a specific course.
        /// </summary>
        /// <param name="courseId">The unique identifier of the course.</param>
        /// <param name="pageNumber">The page number to retrieve.</param>
        /// <param name="pageSize">The number of items per page.</param>
        /// <param name="include">Optional fields to include (e.g., "progress").</param>
        /// <returns>A <see cref="PaginatedResultDto{ModuleDto}"/> containing the paginated list of modules for the specified course.</returns>
        Task<PaginatedResultDto<ModulePreviewDto>> GetAllByCourseIdAsync(Guid courseId, int pageNumber, int pageSize, string? include = null);

        /// <summary>
        /// Creates a new module.
        /// </summary>
        /// <param name="module">The data for the module to create.</param>
        /// <returns>A <see cref="ModuleExtendedDto"/> representing the created module.</returns>
        Task<ModuleExtendedDto> CreateAsync(CreateModuleDto module);

        /// <summary>
        /// Updates an existing module.
        /// </summary>
        /// <param name="id">The unique identifier of the module to update.</param>
        /// <param name="module">The updated data for the module.</param>
        Task UpdateAsync(Guid id, UpdateModuleDto module);

        /// <summary>
        /// Deletes a module by its unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the module to delete.</param>
        Task DeleteAsync(Guid id);

        /// <summary>
        /// Checks if a module name is unique within a specific course.
        /// </summary>
        /// <param name="title">The name of the module to check.</param>
        /// <param name="courseId">The unique identifier of the course.</param>
        /// <param name="excludedModuleId">Optional. The unique identifier of a module to exclude from the check (useful when updating).</param>
        /// <returns><c>true</c> if the module name is unique; otherwise, <c>false</c>.</returns>
        Task<bool> IsUniqueNameAsync(string title, Guid courseId, Guid excludedModuleId = default);
    }
}
