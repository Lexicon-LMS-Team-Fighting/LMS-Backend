using Domain.Contracts.Repositories;
using Domain.Models.Entities;
using LMS.Infractructure.Data;

namespace LMS.Infractructure.Repositories
{
    /// <summary>
    /// Repository implementation for managing <see cref="Document"/> entities.
    /// Inherits common CRUD functionality from <see cref="RepositoryBase{T}"/>.
    /// </summary>
    public class DocumentRepository : RepositoryBase<Document>, IDocumentRepository
    {
        public DocumentRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}