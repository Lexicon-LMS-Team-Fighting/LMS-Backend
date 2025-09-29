using LMS.Shared.DTOs.DocumentDtos;
using LMS.Shared.DTOs.LMSActivityDtos;
using LMS.Shared.DTOs.PaginationDtos;

namespace Service.Contracts
{
    /// <summary>
    /// Service contract for managing documents.
    /// This interface defines the operations available for interacting with document data.
    /// </summary>
    public interface IDocumentService
    {
        /// <summary>
        /// Retrieves an activity by its unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the activity.</param>
        /// <returns>A <see cref="DocumentDto"/> representing the document.</returns>
        Task<DocumentDto> GetByIdAsync(Guid id);

        /// <summary>
        /// Retrieves a paginated list of all documents.
        /// </summary>
        /// <param name="pageNumber">The page number to retrieve.</param>
        /// <param name="pageSize">The number of items per page.</param>
        /// <returns>A <see cref="PaginatedResultDto{DocumentDto}"/> containing the paginated list of documents.</returns>
        Task<PaginatedResultDto<DocumentDto>> GetAllAsync(int pageNumber, int pageSize);
    }
}
