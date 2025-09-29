using AutoMapper;
using Domain.Contracts.Repositories;
using Domain.Models.Exceptions.NotFound;
using LMS.Shared.DTOs.DocumentDtos;
using LMS.Shared.DTOs.LMSActivityDtos;
using LMS.Shared.DTOs.PaginationDtos;
using LMS.Shared.Pagination;
using Service.Contracts;

namespace LMS.Services
{
    /// <summary>
    /// Service implementation for managing documents.
    /// Provides methods for creating, retrieving, updating, and deleting documents
    /// </summary>
    public class DocumentService : IDocumentService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        /// <summary>
        /// Initializes a new instance of the <see cref="DocumentService"/> class.
        /// </summary>
        /// <param name="unitOfWork">The unit of work for accessing repositories.</param>
        /// <param name="mapper">The AutoMapper instance for mapping entities and DTOs.</param>
        public DocumentService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        /// <summary>
        /// Retrieves a document by its unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the document.</param>
        /// <returns>A <see cref="DocumentDto"/> representing the activity.</returns>
        /// <exception cref="LMSActivityNotFoundException">Thrown if the document is not found.</exception>
        public async Task<DocumentDto> GetByIdAsync(Guid id)
        {
            var document = await _unitOfWork.Document.GetByIdAsync(id);

            if (document is null)
                throw new DocumentNotFoundException(id);

            return _mapper.Map<DocumentDto>(document);
        }

        /// <summary>
        /// Retrieves a paginated list of all documents.
        /// </summary>
        /// <param name="pageNumber">The page number to retrieve.</param>
        /// <param name="pageSize">The number of items per page.</param>
        /// <returns>A <see cref="PaginatedResultDto{DocumentDto}"/> containing the paginated list of documents.</returns>
        public async Task<PaginatedResultDto<DocumentDto>> GetAllAsync(int pageNumber, int pageSize)
        {
            var documents = await _unitOfWork.Document.GetAllAsync();

            var paginatedDocuments = documents.ToPaginatedResult(new PagingParameters
            {
                PageNumber = pageNumber,
                PageSize = pageSize
            });

            return _mapper.Map<PaginatedResultDto<DocumentDto>>(paginatedDocuments);
        }
    }
}