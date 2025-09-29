using Domain.Models.Entities;

namespace Domain.Contracts.Repositories
{
    /// <summary>
    /// Defines the contract for document-specific data access operations. <br/>
    /// Inherits common CRUD functionality from <see cref="IRepositoryBase{T}"/>. <br/>
    /// Provides methods to retrieve, add, update, and delete <see cref="Document"/> entities. <br/>
    /// Includes methods for managing modules in relation to their parents <see cref="ApplicationUser"/>, <see cref="Course"/>, <see cref="Module"/>, <see cref="LMSActivity"/>. 
    /// </summary>
    public interface IDocumentRepository : IRepositoryBase<Document>
    {

    }
}
