using AutoMapper;
using Domain.Contracts.Repositories;
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
    }
}