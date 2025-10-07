using Domain.Contracts.Repositories;
using Domain.Models.Entities;
using LMS.Infractructure.Data;
using Microsoft.EntityFrameworkCore;

namespace LMS.Infractructure.Repositories
{
    /// <summary>
    /// Repository implementation for managing <see cref="Document"/> entities.
    /// Inherits common CRUD functionality from <see cref="RepositoryBase{T}"/>.
    /// </summary>
    public class DocumentRepository : RepositoryBase<Document>, IDocumentRepository
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DocumentRepository"/> class with the specified <see cref="ApplicationDbContext"/>.
        /// </summary>
        public DocumentRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}